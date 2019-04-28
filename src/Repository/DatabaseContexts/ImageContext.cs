using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Repository.DatabaseContexts
{
    public class ImageContext : DbContext
    {
        public ImageContext(DbContextOptions<ImageContext> options)
            : base(options)
        {}

        public DbSet<Image> Images { get; set; }
    }
}