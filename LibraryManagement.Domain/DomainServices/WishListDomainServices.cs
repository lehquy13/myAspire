using LibraryManagement.Domain.DomainServices.Interfaces;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Library.BookAggregate;
using LibraryManagement.Domain.Library.UserAggregate;

namespace LibraryManagement.Domain.DomainServices;

public class WishListDomainServices : IWishListDomainServices
{
    private readonly IUserRepository _userRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IAppLogger<WishListDomainServices> _logger;

    public WishListDomainServices(IUserRepository userRepository, IAppLogger<WishListDomainServices> logger,
        IBookRepository bookRepository)
    {
        _userRepository = userRepository;
        _logger = logger;
        _bookRepository = bookRepository;
    }

   
}