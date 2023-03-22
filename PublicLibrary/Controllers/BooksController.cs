using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PublicLibrary.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PublicLibrary.Controllers
{
  [Authorize]
  public class BooksController : Controller
  {
    private readonly PublicLibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public BooksController(UserManager<ApplicationUser> userManager, PublicLibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    [AllowAnonymous] 
    public ActionResult Index()
    {
      //string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      //ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      // List<Book> userRecipe = _db.Books.Where(book => book.User.Id == currentUser.Id.ToString()).ToList();

      // List<Book> userRecipes = _db.Books
      //                     .ToList();
      // userRecipes.Sort(Book.CompareRecipeByRating);
      // return View(userRecipes);

      return View(_db.Books.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.Authors = _db.Authors.ToList();
      return View();
    }

    [HttpPost]
    public ActionResult Create(Book book)
    {
      _db.Books.Add(book);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {

      Book thisBook = _db.Books
          .Include(book => book.JoinEntities)
          .ThenInclude(join => join.Author)
          // .Include(book => book.User)
          .FirstOrDefault(book => book.BookId == id);
      
      // if(thisBook.User.Id == currentUser.Id)  HIDE THIS FROM ANYONE NOT THE OWNER
      // {
      //   return View(thisBook);
      // }
      // else
      // {
      //   return RedirectToAction("Index", "Home");
      // }
      return View(thisBook);
    }

    public ActionResult AddAuthor(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(books => books.BookId == id);
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "AuthorName");
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult AddAuthor(Book book, int authorId)
    {
#nullable enable
      AuthorBook? joinEntity = _db.AuthorBooks.FirstOrDefault(join => (join.AuthorId == authorId && join.BookId == book.BookId));
#nullable disable
      if (joinEntity == null && authorId != 0)
      {
        _db.AuthorBooks.Add(new AuthorBook() { AuthorId = authorId, BookId = book.BookId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = book.BookId });
    }


    public ActionResult Edit(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult Edit(Book book)
    {
        _db.Books.Update(book);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      _db.Books.Remove(thisBook);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      AuthorBook joinEntry = _db.AuthorBooks.FirstOrDefault(entry => entry.AuthorBookId == joinId);
      _db.AuthorBooks.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}