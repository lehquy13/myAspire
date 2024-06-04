namespace LibraryManagement.Domain.DomainServices.Exceptions;

public class CreateOrderFailWhileSavingChangesException : Exception
{
    public override string Message => "Create order fail while saving changes";

    public const string Code = "CreateOrderFailWhileSavingChanges";
}