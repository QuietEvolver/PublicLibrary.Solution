using System.Collections.Generic;
using PublicLibrary.Models;

namespace PublicLibrary.Models
{
  public class Author
  {
    public int AuthorId { get; set;}
    public string AuthorName { get; set; }
    public List<AuthorBook> JoinEntities { get; set; }
  }
}