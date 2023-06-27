using AutoMapper;
using HueFestivalTicketOnline.Helpers;
using LMSSystem.Data;
using LMSSystem.Models;
using LMSSystem.Prototypes;
using LMSSystem.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace LMSSystem.Repositories
{

    public class UserRepository : IUserRepository
    {
        private readonly SchoolContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailRepository _emailRepository;
        public UserRepository(SchoolContext context, IMapper mapper, IEmailRepository emailRepository)
        {
            _context = context;
            _mapper = mapper;
            _emailRepository = emailRepository;
        }

        public async Task<int> AddUserAsync(UserDTO model)
        {
            var newUser = _mapper.Map<User>(model);
            if (await _context.Users.CountAsync(x => x.Email == model.Email) > 0)
            {
                return -1;
            }
            if (await _context.Users.CountAsync(x => x.Username == model.Username) > 0)
            {
                return -1;
            }
            var passwordHash = HashMD5.GetMD5Hash(model.Password);
            newUser.Password = passwordHash;
            newUser.IsActive = true;
            newUser.RoleID = 1;
            newUser.VerificationToken = jwtHandler.CreateRandomToken();
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser.UserID;
        }

        public async Task<bool> CheckUserName(string Username)
        {
            var result = await _context.Users.CountAsync(x => x.Username == Username) > 0;
            Console.WriteLine(result);
            return result;
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users!.ToListAsync();
            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<List<UserDTO>> GetAlLUsersByRoleAsync(int id)
        {
            var users = await _context.Users
                .Where(m => m.RoleID == id)
                .ToListAsync();
            return _mapper.Map<List<UserDTO>>(users);
        }

/*        public async Task<List<UserDTO>> GetAllStudentsAsync(int id)
        {
            var users = await _context.Users
                .Where(m => m.RoleID == id)
                .ToListAsync();
            return _mapper.Map<List<UserDTO>>(users);
        }*/

        public async Task<UserDTO> GetUserAsync(int id)
        {
            var users = await _context.Users!.FindAsync(id);
            return _mapper.Map<UserDTO>(users);
        }

        public async Task<UserDTO> Login(string userName, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == userName);
            if (user == null)
            {
                return null;
            }

            if (password == null)
            {
                return null;
            }

            return _mapper.Map<UserDTO>(user);
        }

        public async Task UpdateUserAsync(int id, UserDTO model)
        {
            if (id == model.UserID)
            {
                var updateUser = _mapper.Map<User>(model);
                _context.Users!.Update(updateUser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> VerifyEmail(string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);

            if (user == null)
            {
                return -1;
            }
            user.VerifyAt = DateTime.Now;
            try
            {
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }

        public async Task<UserDTO> GetUserByRefreshToken(string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserDTO>(user);
        }

        public async Task SetRefreshToken(int userId, RefreshToken newRefreshToken)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user != null)
            {
                user.RefreshToken = newRefreshToken.Token;
                user.RefreshTokenCreated = newRefreshToken.Created;
                user.RefreshTokenExpries = newRefreshToken.Expires;
                await _context.SaveChangesAsync();
            }
        }



        public async Task<int> ForgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return -1;
            }

            user.PasswordResetToken = jwtHandler.CreateRandomToken();
            user.ResetTokenExpries = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();
            var sendEmail = new EmailDTO
            {
                To = email,
                Subject = "Reset password link",
                Body = "<a target=" + "_blank" + " href=" + "http://localhost:7254/api/Users/reset-password/" + user.PasswordResetToken + ">CLICK HERE</a>"

            };
            await _emailRepository.SendEmailAsync(sendEmail);

            return 1;
        }

        public async Task<int> ResetPassword(string token, string password)
        {
            var u = await _context.Users.FirstOrDefaultAsync(u => u.Username == "hiep");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == token);
            if (user == null || user.ResetTokenExpries < DateTime.Now)
            {
                return -1;
            }
            string passwordHash = HashMD5.GetMD5Hash(password);

            user.Password = passwordHash;
            user.PasswordResetToken = token;
            user.ResetTokenExpries = null;

            await _context.SaveChangesAsync();
            return 1;
        }
    }
}