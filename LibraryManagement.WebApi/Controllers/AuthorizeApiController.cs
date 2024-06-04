using MapsterMapper;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.WebApi.Controllers;

[Authorize]
public class AuthorizeApiController : ApiController
{
    public AuthorizeApiController(IMapper mapper, ILogger<AuthorizeApiController> logger) : base(mapper, logger)
    {
    }
}