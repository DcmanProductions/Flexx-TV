using com.drewchaseproject.net.Flexx.Web.Service.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace com.drewchaseproject.net.Flexx.Web.Service.Data
{
    public class Context : IdentityDbContext<FlexxUser>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
