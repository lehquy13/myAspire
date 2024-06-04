namespace LibraryManagement.Domain.DomainServices.Exceptions;

public class AddItemFailWhileSavingChangesException : Exception
{
    public override string Message => "Add item fail while saving changes";
}