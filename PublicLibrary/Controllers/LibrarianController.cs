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
  public class LibrariansController : Controller
  {
    private readonly PublicLibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public LibrariansController(UserManager<ApplicationUser> userManager, PublicLibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public ActionResult Index()
    {
      List<Librarian> userLibrarians = _db.Librarians
                          .ToList();
      return View(userLibrarians);
    }

    public async Task<ActionResult> Create()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(PublicLibrary.Models.Librarian librarian)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      librarian.User = currentUser;
      _db.Librarians.Add(librarian);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public async Task<ActionResult> Details(int id)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

      Librarian thisLibrarian = _db.Librarians
          .Include(librarian => librarian.JoinEntities)
          .ThenInclude(join => join.Copy)
          .Include(librarian => librarian.User)
          .FirstOrDefault(librarian => librarian.LibrarianId == id);
      
      if(thisLibrarian.User.Id == currentUser.Id)
      {
        return View(thisLibrarian);
      }
      else
      {
        return RedirectToAction("Index", "Home");
      }

    }

    public ActionResult AddBook(int id)
    {
      Librarian thisLibrarian = _db.Librarians.FirstOrDefault(librarians => librarians.LibrarianId == id);
      ViewBag.BookId = new SelectList(_db.Copies, "BookId", "Title");
      return View(thisLibrarian);
    }

    [HttpPost]
    public ActionResult AddBook(PublicLibrary.Models.Author author, int bookId)
    {
#nullable enable
      AuthorBook? joinEntity = _db.AuthorBooks.FirstOrDefault(join => (join.BookId == bookId && join.AuthorId == author.AuthorId));
#nullable disable
      if (joinEntity == null && bookId != 0)
      {
        _db.AuthorBooks.Add(new AuthorBook() { BookId = bookId, AuthorId = author.AuthorId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = author.AuthorId });
    }


    public ActionResult Edit(int id)
    {
      Librarian thisLibrarian = _db.Librarians.FirstOrDefault(librarian => librarian.LibrarianId == id);
      return View(thisLibrarian);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(Librarian librarian)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      if(currentUser.Id == librarian.User.Id)
      {
        _db.Librarians.Update(librarian);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
      else
      {
        return RedirectToAction("Index");
      }


    }

    public ActionResult Delete(int id)
    {
      Librarian thisLibrarian = _db.Librarians.FirstOrDefault(librarian => librarian.LibrarianId == id);
      return View(thisLibrarian);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Librarian thisLibrarian = _db.Librarians.FirstOrDefault(librarian => librarian.LibrarianId == id);
      _db.Librarians.Remove(thisLibrarian);
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