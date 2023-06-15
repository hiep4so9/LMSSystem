using LMSSystem.Data;

namespace LMSSystem.Repositories.IRepository
{
    public interface IUserRepository
    {
        public Task<List<UserDTO>> GetAllUsersAsync();
        public Task<UserDTO> GetUserAsync(int id);
        public Task<int> AddUserAsync(UserDTO model);
        public Task UpdateUserAsync(int id, UserDTO model);
        public Task<UserDTO> Login(string userName, string password);
        Task<bool> CheckUserName(string username);
/*        Task<int> VerifyEmail(string token);
        Task<int> ForgotPassword(string email);
        Task<int> ResetPassword(string token, string password);
        Task<UserDTO> GetUserByRefreshToken(string refreshToken);
        Task SetRefreshToken(int userId, RefreshToken newRefreshToken);*/

    }
}
