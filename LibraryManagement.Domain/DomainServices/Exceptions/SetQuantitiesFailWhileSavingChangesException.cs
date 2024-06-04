namespace LibraryManagement.Domain.DomainServices.Exceptions;

public class SetQuantitiesFailWhileSavingChangesException : Exception
{
    public override string Message => "Set quantities fail while saving changes";
}