using Microsoft.EntityFrameworkCore;

namespace VkBot.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<MessagePair> MessagePairs { get; set; }
        public DbSet<MessageSize> MessageSizes { get; set; }
    }
}
