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
        public async Task<ActionResult<IEnumerable<Image>>> GetImage()
        {
            if (_context.Image == null)
            {
                return NotFound();
            }
            return await _context.Image.ToListAsync();
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Image>> GetImage(int id)
        {
            if (_context.Image == null)
            {
                return NotFound();
            }
            var image = await _context.Image.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return image;
        }

        // PUT: api/Images/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage(int id, Image image)
        {
            if (id != image.Id)
            {
                return BadRequest();
            }

            _context.Entry(image).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Images
        [HttpPost]
        public async Task<ActionResult<Image>> PostImage([FromForm] ImageRequestDto image)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{image.File.FileName}";

            string uploadsFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "images");
            Directory.CreateDirectory(uploadsFolder);

            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.File.CopyTo(fileStream);
            }

            var imageForDatabase = new Image
            {
                UserId = image.UserId,
                Name = image.File.FileName,
                Extension = Path.GetExtension(image.File.FileName),
                Location = filePath
            };
            _context.Image.Add(imageForDatabase);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImage", new { id = imageForDatabase.Id }, imageForDatabase);
        }

        // DELETE: api/Images/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            if (_context.Image == null)
            {
                return NotFound();
            }
            var image = await _context.Image.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            _context.Image.Remove(image);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImageExists(int id)
        {
            return (_context.Image?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
