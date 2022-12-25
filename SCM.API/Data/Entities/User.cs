namespace SCM.API.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Image> Images { get; set; }

    }
}
