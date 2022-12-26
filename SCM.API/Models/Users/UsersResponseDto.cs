using SCM.API.Models.Images;

namespace SCM.API.Models.Users
{
    public class UsersResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ImageResponseDto> Images { get; set; }

    }
}
