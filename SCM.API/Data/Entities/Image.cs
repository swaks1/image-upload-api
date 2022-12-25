namespace SCM.API.Data.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Location { get; set; }

        public int UserId { get; set; }
    }
}
