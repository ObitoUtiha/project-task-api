using Microsoft.EntityFrameworkCore;

namespace ProjectTaskApi.Data
{
    public class ApplicationContext : DbContext
    {

        public ApplicationContext(DbContextOptions<ApplicationContext> options):
            base(options) { } 

    }
}
