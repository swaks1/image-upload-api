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

        public DbSet<User> Users { get; set; } = default!;

        public DbSet<Image> Images { get; set; } = default!;
    }
}
