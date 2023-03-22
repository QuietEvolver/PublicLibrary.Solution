using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PublicLibrary.Models
{
  public class Librarian
  { 
    public int LibrarianId { get; set; }
    public string LibrarianName { get; set; }
    public List<Checkout> JoinEntities { get; set; }
    public ApplicationUser User { get; set; } // TBD: Role
  }
}