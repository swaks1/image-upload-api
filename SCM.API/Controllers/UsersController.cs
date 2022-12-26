using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCM.API.Data;
using SCM.API.Data.Entities;
using SCM.API.Models.Images;
using SCM.API.Models.Users;

namespace SCM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SCMContext _context;

        public UsersController(SCMContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersResponseDto>>> GetUsers()
        {
            var users = await _context.Users
                .Include(x => x.Images)
                .ToListAsync();

            var result = users.Select(user => new UsersResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Images = user.Images.Select(image => new ImageResponseDto
                {
                    Id = image.Id,
                    UserId = image.UserId,
                    Url = $"{Request.Scheme}://{Request.Host.Value}/{image.Location}"
                }).ToList()
            });

            return result.ToList();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsersResponseDto>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return new UsersResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Images = user.Images.Select(image => new ImageResponseDto
                {
                    Id = image.Id,
                    UserId = image.UserId,
                    Url = $"{Request.Scheme}://{Request.Host.Value}/{image.Location}"
                }).ToList()
            };
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UsersRequestDto user)
        {
            var userFromDatabase = await _context.Users.FindAsync(id);

            if (userFromDatabase == null)
            {
                return NotFound();
            }

            userFromDatabase.Name = user.Name;

            _context.Entry(userFromDatabase).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UsersResponseDto>> PostUser(UsersRequestDto user)
        {
            var userForDatabase = new User
            {
                Name = user.Name
            };
            _context.Users.Add(userForDatabase);
            await _context.SaveChangesAsync();

            var response = new UsersResponseDto
            {
                Id = userForDatabase.Id,
                Name = userForDatabase.Name,
                Images = new List<ImageResponseDto>()
            };

            return Created("PostUser", response);
        }
    }
}
