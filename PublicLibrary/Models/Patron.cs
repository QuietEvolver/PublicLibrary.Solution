using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PublicLibrary.Models
{
  public class Patron
  { 
    public int PatronId { get; set; }
    public string PatronName { get; set; }
    public List<Checkout> JoinEntities { get; set; }
    public ApplicationUser User { get; set; } // TBD: Role
  }
}