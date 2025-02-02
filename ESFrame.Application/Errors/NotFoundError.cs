using FluentResults;

namespace ESFrame.Application.Errors;

public class NotFoundError : IError
{
    public NotFoundError(string entityName, string entityId)
    {
        Message = $"Entity {entityName} with id {entityId} not found";

        Metadata = new Dictionary<string, object>
        {
            { "EntityName", entityName },
            { "EntityId", entityId }
        };
    }

    public List<IError> Reasons => [this];

    public string Message { get; }

    public Dictionary<string, object> Metadata { get; }
}
