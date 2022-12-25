﻿namespace SCM.API.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }

        public List<Image> Images { get; set; }

    }
}
