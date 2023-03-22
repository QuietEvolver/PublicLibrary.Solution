using Microsoft.AspNetCore.Mvc;
using PublicLibrary.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PublicLibrary.Controllers
{
  [Authorize]
  public class HomeController : Controller
  {
    private readonly PublicLibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(UserManager<ApplicationUser> userManager, PublicLibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    [AllowAnonymous]
    [HttpGet("/")]
    public ActionResult Index()
    {

      Dictionary<string, object[]> model = new Dictionary<string, object[]>();

        List<Book> books = _db.Books.ToList();
        List<Author> authors = _db.Authors.ToList();

        model.Add("books", books.ToArray());
        model.Add("authors", authors.ToArray());

      return View(model);
    }
  }
}