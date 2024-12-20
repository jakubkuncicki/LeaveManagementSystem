namespace LeaveManagementSystem.Web.Services.Common
{
    public class CommonUserService(
        IHttpContextAccessor _httpContextAccessor,
        UserManager<ApplicationUser> _userManager) : ICommonUserService
    {
        public async Task<ApplicationUser?> GetCurrentUser()
        {
            ApplicationUser? user = null;

            if (_httpContextAccessor.HttpContext != null)
            {
                user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            }

            return user;
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user;
        }

        public async Task<List<ApplicationUser>> GetEmployees()
        {
            var employees = await _userManager.GetUsersInRoleAsync(Roles.Employee);

            return employees.ToList();
        }
    }
}
