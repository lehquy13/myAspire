using LibraryManagement.Domain.Interfaces;
using MapsterMapper;

namespace LibraryManagement.Application.ServiceImpls;

public abstract class ServiceBase
{
    protected readonly IMapper Mapper;
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IAppLogger<ServiceBase> Logger;

    protected ServiceBase(IMapper mapper,IUnitOfWork unitOfWork, IAppLogger<ServiceBase> logger)
    {
        Logger = logger;
        UnitOfWork = unitOfWork;
        Mapper = mapper;
    }
}