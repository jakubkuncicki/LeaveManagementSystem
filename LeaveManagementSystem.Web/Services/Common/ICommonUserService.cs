
namespace LeaveManagementSystem.Web.Services.Common
{
    public interface ICommonUserService
    {
        Task<ApplicationUser?> GetCurrentUser();
        Task<List<ApplicationUser>> GetEmployees();
        Task<ApplicationUser> GetUserById(string userId);
    }
}