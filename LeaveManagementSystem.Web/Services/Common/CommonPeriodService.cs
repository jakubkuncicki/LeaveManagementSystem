using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.Common
{
    public class CommonPeriodService(ApplicationDbContext _context) : ICommonPeriodService
    {
        public async Task<Period> GetCurrentPeriod()
        {
            return await GetPeriodContainingTheDate(GetCurrentDate());
        }

        public DateOnly GetCurrentDate()
        {
            var currentDateTime = DateTime.Now;

            return new DateOnly(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day);
        }

        public async Task<Period> GetPeriodContainingTheDate(DateOnly date)
        {
            return await _context.Periods.SingleAsync(q => q.StartDate <= date && date <= q.EndDate);
        }

        public int CalculatePeriodRemainingMonthsAtDate(Period? period, DateOnly date)
        {
            int remainingMonthsCount = 0;

            if (period != null && period.StartDate <= date && date <= period.EndDate)
            {
                remainingMonthsCount = (period.EndDate.Month - date.Month) % 12;
            }

            return remainingMonthsCount;
        }
    }
}
