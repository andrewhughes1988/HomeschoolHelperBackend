using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using HomeschoolHelperApi.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace HomeschoolHelperApi.Data
{
    public class AuthenticationRepo : IAuthenticationRepo
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthenticationRepo(IMapper mapper, DataContext context, IConfiguration configuration)
        {
            this._configuration = configuration;
            this._mapper = mapper;
            this._context = context;

        }


        public async Task<ServerResponse<string>> Login(string email, string password)
        {
            ServerResponse<string> response = new ServerResponse<string>();
            try
            {

                User user = await _context.Users.FirstOrDefaultAsync
                        (user => user.Email.ToLower().Equals(email.ToLower()));

                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Email has not been registered.";
                }

                else if (VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) == false)
                {
                    response.Success = false;
                    response.Message = "Password Incorrect.";
                }

                else
                {
                    response.Data = GenerateJWT(user);
                    response.Message = "Authentication Successful";
                }

            }

            catch (Exception e)
            {
                response.Success = false;
                response.Message = "Error authenticating user.";
            }


            return response;
        }


        public async Task<ServerResponse<int>> Register(User user, string password)
        {
            ServerResponse<int> response = new ServerResponse<int>();

            if (await UserExists(user.Email))
            {
                response.Success = false;
                response.Message = "Email has already been registered.";
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                response.Data = user.Id;
                response.Message = "Registration was successful.";
            }
            catch (Exception e)
            {
                response.Data = -1;
                response.Success = false;
                response.Message = "Failed to register new user.";
            }


            return response;

        }


        public async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(user => user.Email.ToLower() == email.ToLower()))
            {
                return true;
            }

            return false;
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {

            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private string GenerateJWT(User user)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.Name)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey
                (System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:JWTSecret").Value));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);


            return tokenHandler.WriteToken(token);

        }

    }

}