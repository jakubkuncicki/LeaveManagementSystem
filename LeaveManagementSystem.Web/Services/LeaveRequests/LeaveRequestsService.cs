using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveRequests;
using LeaveManagementSystem.Web.Services.Common;
using LeaveManagementSystem.Web.Services.LeaveAllocations;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveRequests;

public partial class LeaveRequestsService(
    IMapper _mapper,
    ICommonUserService _commonUserService,
    ApplicationDbContext _context,
    ICommonPeriodService _commonPeriodService,
    ILeaveAllocationsService _leaveAllocationsService
    ) : ILeaveRequestsService
{
    public async Task CancelLeaveRequest(int id)
    {
        var leaveRequest = await _context.LeaveRequests.FindAsync(id);
        leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Canceled;

        await UpdateAllocationDays(leaveRequest, false);

        await _context.SaveChangesAsync();
    }

    public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
    {
        var leaveRequest = _mapper.Map<LeaveRequest>(model);
        var user = await _commonUserService.GetCurrentUser();
        leaveRequest.EmployeeId = user.Id;
        leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Pending;
        _context.Add(leaveRequest);

        await UpdateAllocationDays(leaveRequest, true);

        await _context.SaveChangesAsync();
    }

    public async Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests()
    {
        var user = await _commonUserService.GetCurrentUser();
        var leaveRequests = await _context.LeaveRequests
            .Include(q => q.LeaveType)
            .Where(q => q.EmployeeId == user.Id)
            .ToListAsync();

        var model = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
        {
            StartDate = q.StartDate,
            EndDate = q.EndDate,
            Id = q.Id,
            LeaveType = q.LeaveType.Name,
            LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
            NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber
        }).ToList();

        return model;
    }

    public async Task<EmployeeLeaveRequestListVM> AdminGetAllLeaveRequests()
    {
        var leaveRequests = await _context.LeaveRequests
            .Include(q => q.LeaveType)
            .ToListAsync();

        var leaveRequestModels = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
        {
            StartDate = q.StartDate,
            EndDate = q.EndDate,
            Id = q.Id,
            LeaveType = q.LeaveType.Name,
            LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
            NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber
        }).ToList();

        var model = new EmployeeLeaveRequestListVM
        {
            ApprovedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Approved),
            PendingRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Pending),
            DeclinedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Declined),
            TotalRequests = leaveRequests.Count,
            LeaveRequests = leaveRequestModels
        };

        return model;
    }

    public async Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM model)
    {
        var user = await _commonUserService.GetCurrentUser();
        var period = _commonPeriodService.GetCurrentPeriod();
        var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
        var allocation = await _context.LeaveAllocations
            .FirstAsync(q =>
                q.LeaveTypeId == model.LeaveTypeId &&
                q.EmployeeId == user.Id &&
                q.PeriodId == period.Id);

        return allocation.Days < numberOfDays;
    }

    public async Task ReviewLeaveRequest(int leaveRequestId, bool approved)
    {
        var user = await _commonUserService.GetCurrentUser();
        var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);
        leaveRequest.LeaveRequestStatusId = approved
            ? (int)LeaveRequestStatusEnum.Approved
            : (int)LeaveRequestStatusEnum.Declined;
        leaveRequest.ReviewerId = user.Id;

        if (!approved)
        {
            await UpdateAllocationDays(leaveRequest, false);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int id)
    {
        var leaveRequest = await _context.LeaveRequests
            .Include(q => q.LeaveType)
            .FirstAsync(q => q.Id == id);

        var user = await _commonUserService.GetUserById(leaveRequest.EmployeeId);

        var model = new ReviewLeaveRequestVM
        {
            StartDate = leaveRequest.StartDate,
            EndDate = leaveRequest.EndDate,
            NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber,
            LeaveRequestStatus = (LeaveRequestStatusEnum)leaveRequest.LeaveRequestStatusId,
            Id = leaveRequest.Id,
            LeaveType = leaveRequest.LeaveType.Name,
            RequestComments = leaveRequest.RequestComments,
            Employee = new EmployeeListVM
            {
                Id = leaveRequest.EmployeeId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            }
        };

        return model;
    }

    private async Task UpdateAllocationDays(LeaveRequest leaveRequest, bool dedctDays)
    {
        var allocation = await _leaveAllocationsService.GetCurrentAllocation(leaveRequest.LeaveTypeId, leaveRequest.EmployeeId);
        var numberOfDays = CalculateDays(leaveRequest.StartDate, leaveRequest.EndDate);

        if (dedctDays)
        {
            allocation.Days -= numberOfDays;
        }
        else
        {
            allocation.Days += numberOfDays;
        }

        _context.Entry(allocation).State = EntityState.Modified;
    }

    private int CalculateDays(DateOnly start, DateOnly end)
    {
        return end.DayNumber - start.DayNumber;
    }
}
