using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCM.API.Data;
using SCM.API.Data.Entities;
using SCM.API.Models.Images;

namespace SCM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly SCMContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImagesController(IWebHostEnvironment webHostEnvironment, SCMContext context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        // GET: api/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageResponseDto>>> GetImages()
        {
            var images = await _context.Images.ToListAsync();

            var response = images.Select(image => new ImageResponseDto
            {
                Id = image.Id,
                UserId = image.UserId,
                Url = $"{Request.Scheme}://{Request.Host.Value}/{image.Location}"
            });

            return response.ToList();
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageResponseDto>> GetImage(int id)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return new ImageResponseDto
            {
                Id = image.Id,
                UserId = image.UserId,
                Url = $"{Request.Scheme}://{Request.Host.Value}/{image.Location}"
            };
        }


        // POST: api/Images
        [HttpPost]
        public async Task<ActionResult<ImageResponseDto>> PostImage([FromForm] ImageRequestDto image)
        {
            if (image.UserId <= 0)
                return BadRequest("Enter User Id for the image");

            if (image.File == null || image.File.Length <= 0 || !image.File.ContentType.Contains("image"))
                return BadRequest("Please upload valid image");

            var user = await _context.Users.FindAsync(image.UserId);
            if (user == null)
                return BadRequest("User doesnt't exists");

            var uploadFolder = Path.Combine(_webHostEnvironment.ContentRootPath, $"Images", image.UserId.ToString());
            Directory.CreateDirectory(uploadFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{image.File.FileName}";
            var fullFilePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var fileStream = new FileStream(fullFilePath, FileMode.Create))
            {
                image.File.CopyTo(fileStream);
            }

            var imageForDatabase = new Image
            {
                UserId = image.UserId,
                Name = image.File.FileName,
                Extension = Path.GetExtension(image.File.FileName),
                Location = $"Images/{image.UserId}/{uniqueFileName}"
            };

            _context.Images.Add(imageForDatabase);
            await _context.SaveChangesAsync();

            var response = new ImageResponseDto
            {
                Id = imageForDatabase.Id,
                UserId = imageForDatabase.UserId,
                Url = $"{Request.Scheme}://{Request.Host.Value}/{imageForDatabase.Location}"
            };

            return Created("PostImage", response);
        }

        // DELETE: api/Images/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, image.Location);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
