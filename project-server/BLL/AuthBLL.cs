using BCrypt.Net;
using server.Auth.Jwt;
using server.BLL.Interfaces;
using server.DAL.Interfaces;
using server.DTO;
using System.Security.Claims;
using WebApplication1.Models; 

public class AuthBLL : IAuthBLL
{
    private readonly IAuthDAL authDAL;
    private readonly JwtTokenGenerator jwtTokenGenerator;
    private readonly ILogger<AuthBLL> _logger;

    public AuthBLL(IAuthDAL authDAL, JwtTokenGenerator jwtTokenGenerator, ILogger<AuthBLL> logger)
    {
        this.authDAL = authDAL;
        this.jwtTokenGenerator = jwtTokenGenerator;
        this._logger = logger;
    }

    public async Task Register(CustomerModel customerModel)
    {
        _logger.LogInformation("Registering user: {UserName}", customerModel.UserName);

        customerModel.Password = BCrypt.Net.BCrypt.HashPassword(customerModel.Password);

        await authDAL.Register(customerModel);
    }

    public async Task<AuthDTO> Login(string userName, string password)
    {
        _logger.LogInformation("Login attempt: {UserName}", userName);

        var user = await authDAL.GetUserByName(userName);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            _logger.LogWarning("Invalid login for: {UserName}", userName);
            return null; 
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var token = jwtTokenGenerator.GenerateToken(claims);

        return new AuthDTO
        {
            Token = token,
            Role = user.Role
        };
    }
}