namespace SCM.API.Models.Images
{
    public class UsersResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ImageResponseDto> Images { get; set; }

    }
}
