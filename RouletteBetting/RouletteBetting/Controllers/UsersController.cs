using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RouletteBetting.Dtos;
using RouletteBetting.Models;

namespace RouletteBetting.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<GetUserResponse>> GetUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return BadRequest("Se requiere nombre de usuario.");

            var normalized = username.ToUpperInvariant();

            var user = await _db.Users.AsQueryable().FirstOrDefaultAsync(u => u.Username.ToUpper() == normalized);

            if (user == null) return NotFound();

            return Ok(new GetUserResponse { Username = user.Username, Balance = user.Balance });
        }

        [HttpPost("save")]
        public async Task<ActionResult<GetUserResponse>> SaveUser([FromBody] SaveUserRequest request)
        {
            if (request == null) return BadRequest("Solicitud requerida.");
            if (string.IsNullOrWhiteSpace(request.Username)) return BadRequest("Se requiere nombre de usuario.");

            var normalized = request.Username.ToUpperInvariant();

            var user = await _db.Users.AsQueryable().FirstOrDefaultAsync(u => u.Username.ToUpper() == normalized);

            if (user == null)
            {
                user = new User
                {
                    Username = request.Username,
                    Balance = request.Amount
                };
                _db.Users.Add(user);
            }
            else
            {
                user.Balance += request.Amount;
            }

            await _db.SaveChangesAsync();

            return Ok(new GetUserResponse { Username = user.Username, Balance = user.Balance });
        }
    }
}
