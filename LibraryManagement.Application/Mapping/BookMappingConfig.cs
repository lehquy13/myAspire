using LibraryManagement.Application.Contracts.Books.BookDtos;
using LibraryManagement.Application.Contracts.Books.ReviewDtos;
using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using Mapster;

namespace LibraryManagement.Application.Mapping;

public class BookMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Author, AuthorDto>()
            .Map(des => des, src => src);

        config.NewConfig<Review, ReviewForListDto>()
            .Map(des => des.Username, src => src.User.Name)
            .Map(des => des.BookTitle, src => src.Book.Title)
            .Map(des => des.ImageUrl, src => src.Image)
            .Map(des => des, src => src);

        config.NewConfig<ReviewForCreateDto, Review>()
            .ConstructUsing(src =>
                new Review(src.Title, src.Content, src.Rating, src.ImageUrl, src.IsLike,
                    IdentityGuid.Create(src.CustomerId)));

        config.NewConfig<Book, BookForListDto>()
            .Map(des => des.AuthorNames, src => src.Authors.Select(x => x.Name).ToList())
            .Map(des => des.Genre, src => src.Genre.ToString())
            .Map(des => des.Image, src => src.Image)
            .Map(des => des, src => src);

        config.NewConfig<Book, BookForDetailDto>()
            .Map(des => des.Id, src => src.Id)
            .Map(des => des.AuthorDtos, src => src.Authors)
            .Map(des => des.ReviewForListDtos, src => src.Reviews)
            .Map(des => des, src => src);

        config.NewConfig<BookForUpsertDto, Book>()
            .MapToConstructor(true);
    }
}