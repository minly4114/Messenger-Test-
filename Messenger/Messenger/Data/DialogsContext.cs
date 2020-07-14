using Messenger.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Data
{
    public class DialogsContext : DbContext
    {
        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DialogsContext(DbContextOptions<DialogsContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Dialog");
            modelBuilder.Entity<Dialog>()
                .HasMany(x => x.Messages)
                .WithOne(x => x.Dialog);
        }
    }
}
