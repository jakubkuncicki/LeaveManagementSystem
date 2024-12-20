using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
using LeaveManagementSystem.Web.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public class LeaveAllocationsService(
        ApplicationDbContext _context,
        IMapper _mapper,
        ICommonUserService _commonUserService,
        ICommonPeriodService _commonPeriodService
        ) : ILeaveAllocationsService
    {
        public async Task AllocateLeave(string employeeId)
        {
            var leaveTypes = await _context.LeaveTypes
                .Where(q => !q.LeaveAllocations.Any(x => x.EmployeeId == employeeId))
                .ToListAsync();

            var currentDate = _commonPeriodService.GetCurrentDate();
            var currentPeriod = await _commonPeriodService.GetPeriodContainingTheDate(currentDate);
            var monthsRemaining = _commonPeriodService.CalculatePeriodRemainingMonthsAtDate(currentPeriod, currentDate);

            foreach (var leaveType in leaveTypes)
            {
                //var allocationExists = await AllocationExists(employeeId, currentPeriod.Id, leaveType.Id);
                //if (allocationExists)
                //{
                //    continue;
                //}
                var accuralRate = decimal.Divide(leaveType.NumberOfDays, 12);
                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.Id,
                    PeriodId = currentPeriod.Id,
                    Days = (int)Math.Ceiling(accuralRate * monthsRemaining)
                };

                _context.Add(leaveAllocation);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId)
        {
            var user = string.IsNullOrEmpty(userId)
                ? await _commonUserService.GetCurrentUser()
                : await _commonUserService.GetUserById(userId);

            var allocations = await GetAllocations(user.Id);
            var allocationVmList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations);
            var leaveTypesCount = await _context.LeaveTypes.CountAsync();
            var employeeVm = new EmployeeAllocationVM
            {
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                LeaveAllocations = allocationVmList,
                IsCompletedAllocation = leaveTypesCount == allocations.Count()
            };

            return employeeVm;
        }

        public async Task<List<EmployeeListVM>> GetEmployees()
        {
            var employees = await _commonUserService.GetEmployees();
            var employeeListVms = _mapper.Map<List<ApplicationUser>, List<EmployeeListVM>>(employees);

            return employeeListVms;
        }

        public async Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId)
        {
            var allocation = await _context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .FirstOrDefaultAsync(q => q.Id == allocationId);

            var model = _mapper.Map<LeaveAllocationEditVM>(allocation);

            return model;
        }

        public async Task EditAllocation(LeaveAllocationEditVM allocationEditVM)
        {
            var leaveAllocation = await GetEmployeeAllocation(allocationEditVM.Id);

            if (leaveAllocation == null)
            {
                throw new Exception("Leave allocation record does not exist.");
            }

            leaveAllocation.Days = allocationEditVM.Days;
            // option 1 _context.Update(leaveAllocation);
            // option 2 _context.Entry(leaveAllocation).State = EntityState.Modified;
            // await _context.SaveChangesAsync();

            await _context.LeaveAllocations
                .Where(q => q.Id == allocationEditVM.Id)
                .ExecuteUpdateAsync(s => s.SetProperty(e => e.Days, allocationEditVM.Days));
        }

        public async Task<LeaveAllocation> GetCurrentAllocation(int leabeTypeId, string employeeId)
        {
            var period = await _commonPeriodService.GetCurrentPeriod();
            var allocation = await _context.LeaveAllocations
                .FirstAsync(q =>
                    q.LeaveTypeId == leabeTypeId &&
                    q.EmployeeId == employeeId &&
                    q.PeriodId == period.Id);

            return allocation;
        }

        #region Private Methods
        private async Task<List<LeaveAllocation>> GetAllocations(string? userId)
        {
            var currentDate = _commonPeriodService.GetCurrentDate();
            var leaveAllocations = await _context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Period)
                .Where(q => q.EmployeeId == userId && q.Period.StartDate <= currentDate && currentDate <= q.Period.EndDate)
                .ToListAsync();

            return leaveAllocations;
        }

        private async Task<bool> AllocationExists(string userId, int periodId, int leaveTypeId)
        {
            var exists = await _context.LeaveAllocations.AnyAsync(q =>
                q.EmployeeId == userId
                && q.LeaveTypeId == leaveTypeId
                && q.PeriodId == periodId
            );

            return exists;
        }
        #endregion
    }
}
