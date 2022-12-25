using Microsoft.EntityFrameworkCore;
using SCM.API.Data.Entities;

namespace SCM.API.Data
{
    public class SCMContext : DbContext
    {
        public SCMContext (DbContextOptions<SCMContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = default!;

        public DbSet<Image> Image { get; set; } = default!;
    }
}
