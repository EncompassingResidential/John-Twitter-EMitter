using Microsoft.EntityFrameworkCore;
using twitter_emitter_server.Models;

namespace twitter_emitter_server.Data {
    public class IssueDbContext : DbContext {
        public IssueDbContext(DbContextOptions<IssueDbContext> options) 
            :base(options)    
        {

        }

        public DbSet<Issue> Issues { get; set; } 
    }
}
