using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PublicLibrary.Models
{
  public class PublicLibraryContext : IdentityDbContext<ApplicationUser>
  {
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<AuthorBook> AuthorBooks { get; set; }
    public DbSet<Copy> Copies { get; set; }
    public DbSet<Patron> Patrons { get; set; }
    public DbSet<Checkout> Checkouts { get; set; }
    public DbSet<Librarian> Librarians { get; set; }
    public PublicLibraryContext(DbContextOptions options) : base(options) { }
  }
}