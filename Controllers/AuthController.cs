using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] Usuario login)
    {
        var usuario = _context.Usuarios
            .FirstOrDefault(u => u.Email == login.Email && u.Senha == login.Senha);

        if (usuario == null)
            return Unauthorized("Email ou senha inválidos");

        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("chave-super-secreta-123"));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: new[]
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email)
            },
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credenciais
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { token = tokenString });
    }
}
