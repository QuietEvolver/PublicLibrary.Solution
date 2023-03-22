using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using PublicLibrary.Models;
using System;

namespace PublicLibrary.Models
{
  public class Book
  {
    public int BookId { get; set; }
    [Required(ErrorMessage = "The Books Title can't be empty!")]
    public string Title { get; set; }
    // public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Copy Copies { get; set; }
    public List<AuthorBook> JoinEntities { get; set; }
  }
}