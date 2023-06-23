using AutoMapper;
using LMSSystem.Data;
using LMSSystem.Models;
using LMSSystem.Repositories;
using LMSSystem.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LMSSystem.Helpers
{
    public class GenerateToken
    {
        private readonly IConfiguration _configuration;
        private readonly IRoleRepository _role;

        public GenerateToken(IConfiguration configuration, IRoleRepository repo)
        {
            _configuration = configuration;
            _role = repo;
        }


        public async Task<string> CreateToken(UserDTO user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("RoleId", user.RoleID.ToString()),
            };


            RoleDTO role = await _role.GetRoleNameFromRoleId(user.RoleID);
            string roleName = role.RoleName;
            if (!string.IsNullOrEmpty(roleName))
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
