using auth.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace auth.Data
{
    public class DBContext(DbContextOptions<DBContext> options) : IdentityDbContext<User>(options)
    {
    }
}
