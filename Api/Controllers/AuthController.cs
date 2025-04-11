using DataAccess.Abstraction.Entity;
using DataAccess.Abstraction.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.Swagger.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("private/[controller]")]
[AllowAnonymous]
public class AuthController : Controller
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IConfiguration _config;

    // En mémoire (à remplacer par DB ou Redis plus tard)
    private static readonly Dictionary<string, string> _refreshTokens = new();

    public AuthController(UserManager<UserEntity> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    [HttpPost("register")]
    [SwaggerOperation(OperationId = "RegisterUser")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new UserEntity { UserName = model.Username, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        await _userManager.AddToRoleAsync(user,model.Role.ToString());
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User created");
    }

    [HttpPost("login")]
    [SwaggerOperation(OperationId = "Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return Unauthorized();

        var accessToken = GenerateJwtToken(user);
        var refreshToken = Guid.NewGuid().ToString();

        _refreshTokens[refreshToken] = user.UserName!;

        return Ok(new
        {
            token = accessToken,
            refreshToken
        });
    }

    [HttpPost("refresh")]
    [SwaggerOperation(OperationId = "Refresh")]
    public IActionResult Refresh([FromBody] RefreshRequest model)
    {
        if (!_refreshTokens.TryGetValue(model.RefreshToken, out var username))
            return Unauthorized();

        var token = GenerateJwtToken(new UserEntity { UserName = username });
        var newRefreshToken = Guid.NewGuid().ToString();

        _refreshTokens.Remove(model.RefreshToken);
        _refreshTokens[newRefreshToken] = username;

        return Ok(new
        {
            token,
            refreshToken = newRefreshToken
        });
    }

    private string GenerateJwtToken(UserEntity user)
    {
        // Récupérer les rôles de l'utilisateur
        var roles = _userManager.GetRolesAsync(user).Result;  // Attention, c'est une opération asynchrone

        // Ajouter les rôles dans les claims
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName!),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        // Ajouter chaque rôle comme claim
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:AccessTokenExpirationMinutes"]));

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}

public record RegisterModel(string Username,RoleTypeData Role, string Email, string Password);
public record LoginModel(string Username, string Password);
public record RefreshRequest(string RefreshToken);
