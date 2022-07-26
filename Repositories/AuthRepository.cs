using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RBG_API.Data;

namespace RBG_API.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;

    public AuthRepository(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<ServiceResponse<int>> Register(User user, string password)
    {
        var response = new ServiceResponse<int>();

        if(await UserExists(user.UserName))
        {
            response.Sucess = false;
            response.Message = "User already exists!";
            return response;
        }

        CreatePasswordHash( password, out byte[] passwordHash, out byte[] passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        response.Data = user.Id;
        return response;
    }

    public async Task<ServiceResponse<string>> Login(string userName, string password)
    {
        var response = new ServiceResponse<string>();
        var user = await _context.Users.FirstOrDefaultAsync(u =>
         u.UserName.ToLower() == userName.ToLower());

        if(user is null)
        {
            response.Sucess = false;
            response.Message = "No matching data";
            return response;
        }

        if(!VerifyPasswordHash(password,user.PasswordHash,user.PasswordSalt))
        {
            response.Sucess = false;
            response.Message = "No matching data";
            return response;
        }

        response.Data = CreateToken(user);
        return response;
    }

    public async Task<bool> UserExists(string userName)
    {
        if(await _context.Users.AnyAsync(u => u.UserName.ToLower() == userName.ToLower()))
        {
            return true;
        }
        return false;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        {
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computeHash.SequenceEqual(passwordHash);
        }
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(System.Text.Encoding.
            UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

        SigningCredentials credentials = 
            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        SecurityTokenDescriptor tokenDescriptor= new SecurityTokenDescriptor
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
