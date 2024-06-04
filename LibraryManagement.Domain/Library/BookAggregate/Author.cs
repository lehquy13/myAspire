using LibraryManagement.Domain.Primitives;
using LibraryManagement.Domain.Shared.Exceptions.AuthorExceptions;

namespace LibraryManagement.Domain.Library.BookAggregate;

//A book may have multiple authors
public class Author : Entity<int>
{
    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidAuthorNameException();
            }

            _name = value;
        }
    }

    public List<Book> Books { get; private set; } = null!; 

    public Author(string name)
    {
        Name = name;
    }
}