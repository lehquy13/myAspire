using LibraryManagement.Application.Contracts.Commons.Primitives;

namespace LibraryManagement.Application.Contracts.Books.BookDtos;

public class AuthorDto: EntityDto<int>
{
    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Author with Id: {Id} Name: {Name}";
    }
}