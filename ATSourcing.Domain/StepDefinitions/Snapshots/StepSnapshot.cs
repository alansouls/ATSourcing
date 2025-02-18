using ATSourcing.Domain.StepDefinitions.Enums;
using FluentResults;
using System.Text.Json;

namespace ATSourcing.Domain.StepDefinitions.Snapshots;

public class StepSnapshot
{
    public required string Name { get; set; }

    public required string Description { get; set; }

    public required StepState State { get; set; }

    public Dictionary<string, string?> Fields { get; set; } = [];

    public Result<Step> ToStep()
    {
        return Name switch
        {
            "File Upload" => FileUploadStep.Restore(
                State, 
                GetStringEnumerableField(nameof(FileUploadStep.RequiredFiles)),
                GetStringEnumerableField(nameof(FileUploadStep.ReturnObservations)),
                GetDictionaryField<string, Guid>(nameof(FileUploadStep.UploadedFiles))
                ).Bind<Step>(s => s),
            "Conversation" => ConversationStep.Restore(
                State,
                Fields.GetValueOrDefault(nameof(ConversationStep.Question)) ?? "",
                GetStringEnumerableField(nameof(ConversationStep.CandidateAnswers)),
                GetStringEnumerableField(nameof(ConversationStep.RecruiterAnswers))
                ).Bind<Step>(v => v),
            _ => Result.Fail("Unknown step type")
        };
    }

    private IEnumerable<string> GetStringEnumerableField(string fieldName)
    {
        var value = Fields.GetValueOrDefault(fieldName);

        if (value is null)
        {
            return [];
        }

        return JsonSerializer.Deserialize<IEnumerable<string>>(value) ?? [];
    }

    private Dictionary<TKey, TValue> GetDictionaryField<TKey, TValue>(string fieldName) where TKey : notnull
    {
        var value = Fields.GetValueOrDefault(fieldName);
        if (value is null)
        {
            return [];
        }
        return JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(value) ?? [];
    }
}
