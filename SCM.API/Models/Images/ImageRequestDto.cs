namespace SCM.API.Models.Images
{
    public class ImageRequestDto
    {
        public int UserId { get; set; }
        public IFormFile File { get; set; }

    }
}
