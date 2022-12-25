namespace SCM.API.Models.Images
{
    public class ImageResponseDto
    {
        public int UserId { get; set; }
        public IFormFile File { get; set; }

    }
}
