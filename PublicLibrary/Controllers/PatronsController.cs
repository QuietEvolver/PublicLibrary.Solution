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

namespace Patron.Controllers
{
  [Authorize]
  public class PatronsController : Controller
  {
    private readonly PublicLibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public PatronsController(UserManager<ApplicationUser> userManager, PublicLibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    [AllowAnonymous] 
    public ActionResult Index()
    {
      //string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      //ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      // List<Patron> userPatron = _db.Patrons.Where(patron => patron.User.Id == currentUser.Id.ToString()).ToList();

      List<Patron> userPatrons = _db.Patrons
                          .ToList();
      // userPatrons.Sort(Patron.ComparePatronByRating);
      return View(userPatrons);
    }

    public async Task<ActionResult> Create()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

      //ViewBag.Copies = _db.Copies.ToList();
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Patron patron)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      patron.User = currentUser;
      _db.Patrons.Add(patron);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public async Task<ActionResult> Details(int id)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

      Patron thisPatron = _db.Patrons
          .Include(patron => patron.JoinEntities)
          .ThenInclude(join => join.Copy)
          .Include(patron => patron.User)
          .FirstOrDefault(patron => patron.PatronId == id);
      
      if(thisPatron.User.Id == currentUser.Id)
      {
        return View(thisPatron);
      }
      else
      {
        return RedirectToAction("Index", "Home");
      }

    }

    public ActionResult AddCopy(int id)
    {
      Patron thisPatron = _db.Patrons.FirstOrDefault(patrons => patrons.PatronId == id);
      ViewBag.CopyId = new SelectList(_db.Copies, "CopyId", "Title");
      return View(thisPatron);
    }

    [HttpPost]
    public ActionResult AddCopy(Patron patron, int copyId)
    {
#nullable enable
      Checkout? joinEntity = _db.Checkouts.FirstOrDefault(join => (join.CopyId == copyId && join.PatronId == patron.PatronId));
#nullable disable
      if (joinEntity == null && copyId != 0)
      {
        _db.Checkouts.Add(new Checkout() { CopyId = copyId, PatronId = patron.PatronId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = patron.PatronId });
    }


    public ActionResult Edit(int id)
    {
      Patron thisPatron = _db.Patrons.FirstOrDefault(patron => patron.PatronId == id);
      return View(thisPatron);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(Patron patron)
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      if(currentUser.Id == patron.User.Id)
      {
        _db.Patrons.Update(patron);
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
      Patron thisPatron = _db.Patrons.FirstOrDefault(patron => patron.PatronId == id);
      return View(thisPatron);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Patron thisPatron = _db.Patron.FirstOrDefault(patron => patron.PatronId == id);
      _db.Patrons.Remove(thisPatron);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      Checkout joinEntry = _db.Checkouts.FirstOrDefault(entry => entry.CheckoutId == joinId);
      _db.Checkouts.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}