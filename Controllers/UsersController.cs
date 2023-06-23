using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMSSystem.Data;
using LMSSystem.Models;
using AutoMapper;
using LMSSystem.Repositories.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using LMSSystem.Helpers;
using LMSSystem.Prototypes;
using HueFestivalTicketOnline.Helpers;
using Microsoft.AspNetCore.Rewrite;

namespace LMSSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        private readonly IRoleRepository _role;

        public UsersController(IUserRepository repo, IConfiguration configuration, IMapper mapper, ILogger<UsersController> logger, IRoleRepository roleRepo)
        {
            _userRepo = repo;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
            _role = roleRepo;

        }

        // GET: api/Users
        [HttpGet/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetAllUsers(int page = 1, int pageSize = 10)
        {
            try
            {
                var allUsers = await _userRepo.GetAllUsersAsync();
                var paginatedUsers = Pagination.Paginate(allUsers, page, pageSize);

                var totalUsers = allUsers.Count;
                var totalPages = Pagination.CalculateTotalPages(totalUsers, pageSize);

                var paginationInfo = new
                {
                    TotalUsers = totalUsers,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Users = paginatedUsers, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepo.GetUserAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string email, string username, string password)
        {
            _logger.LogInformation("Creating a new User");

            try
            {
                var newUser = new UserDTO
                {
                    Email = email,
                    Username = username,
                    Password = password,
                    VerificationToken = jwtHandler.CreateRandomToken()
                };


                if (await _userRepo.AddUserAsync(newUser) != -1)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Register success",
                        Data = CreatedAtAction(nameof(GetUserById), new { id = newUser.UserID }, newUser)
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Register fail:The user already exists  ",
                        Data = null,
                    });
                }
            }
            catch (System.Exception e)
            {

                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Register fail: " + e.GetBaseException(),
                    Data = null,
                });
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string userName, string password)
        {
            GenerateToken tokenGenerator = new GenerateToken(_configuration,_role);

            if (!await _userRepo.CheckUserName(userName))
            {
                return NotFound("username not found");
            }
            try
            {

                var user = await _userRepo.Login(userName, password);


                var passwordHash = HashMD5.GetMD5Hash(password);
                if (passwordHash != user.Password)
                {
                    return BadRequest("Wrong password.");
                }

                if (user.VerifyAt == null)
                {
                    return BadRequest("Not verify");
                }

                if (user != null)
                {
                    string token = await tokenGenerator.CreateToken(user);

                    var refreshToken = GenerateRefreshToken.CreateRefreshToken();
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = refreshToken.Expires
                    };
                    Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
                    /*                    SetRefreshToken(refreshToken,user);*/
                    await _userRepo.SetRefreshToken(user.UserID, refreshToken);

                    return Ok(token);
                }

                return Ok(user);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO model)
        {
            if (id != model.UserID)
            {
                return NotFound();
            }
            await _userRepo.UpdateUserAsync(id, model);
            return Ok();
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string token)
        {

            if (await _userRepo.VerifyEmail(token) != -1)
            {
                return Ok("User verify");
            }
            else
            {
                return BadRequest("Invalid token");
            }

        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            GenerateToken tokenGenerator = new GenerateToken(_configuration, _role);
            var refreshToken = Request.Cookies["refreshToken"];

            var user = await _userRepo.GetUserByRefreshToken(refreshToken);
            if (user == null)
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (user.RefreshTokenExpries < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = await tokenGenerator.CreateToken(user);
            var newRefreshToken = GenerateRefreshToken.CreateRefreshToken();
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            await _userRepo.SetRefreshToken(user.UserID, newRefreshToken);

            return Ok(token);
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (await _userRepo.ForgotPassword(email) != -1)
            {
                return Ok("success");

            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {

            if (await _userRepo.ResetPassword(request.Token, request.Password) != -1)
            {
                return Ok("Password successfully reset.");

            }
            else
            {
                return BadRequest();
            }

        }
    }
}
