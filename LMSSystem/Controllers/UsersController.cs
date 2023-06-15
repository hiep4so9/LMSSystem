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

        public UsersController(IUserRepository repo, IConfiguration configuration, IMapper mapper, ILogger<UsersController> logger)
        {
            _userRepo = repo;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;

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
    }
}
