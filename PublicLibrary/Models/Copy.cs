

namespace PublicLibrary.Models
{
  public class Copy
  {
    public int CopyId { get; set; }
    public int BookId { get; set; }
    public int Stock { get; set; }
    public Book Book { get; set; }
  }
}