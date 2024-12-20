

namespace LeaveManagementSystem.Web.Services.Common
{
    public interface ICommonPeriodService
    {
        int CalculatePeriodRemainingMonthsAtDate(Period? period, DateOnly date);
        DateOnly GetCurrentDate();
        Task<Period> GetCurrentPeriod();
        Task<Period> GetPeriodContainingTheDate(DateOnly date);
    }
}