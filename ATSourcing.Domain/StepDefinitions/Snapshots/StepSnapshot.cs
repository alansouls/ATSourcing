using ATSourcing.Domain.StepDefinitions.Enums;
using FluentResults;
using System.Text.Json;

namespace ATSourcing.Domain.StepDefinitions.Snapshots;

public class StepSnapshot
{
    public required string Name { get; set; }

    public required string Description { get; set; }

    public required string Title { get; set; }

    public required StepState State { get; set; }

    public string? FinalObservations { get; set; }

    public Dictionary<string, string?> Fields { get; set; } = [];

    public Result<Step> ToStep()
    {
        return Name switch
        {
            "File Upload" => FileUploadStep.Restore(
                State, 
                GetStringEnumerableField(nameof(FileUploadStep.RequiredFiles)),
                GetStringEnumerableField(nameof(FileUploadStep.ReturnObservations)),
                GetDictionaryField<string, Guid>(nameof(FileUploadStep.UploadedFiles)),
                FinalObservations
                ).Bind<Step>(s => s),
            "Conversation" => ConversationStep.Restore(
                State,
                GetValueFromFields(nameof(ConversationStep.Question)) ?? "",
                GetStringEnumerableField(nameof(ConversationStep.CandidateAnswers)),
                GetStringEnumerableField(nameof(ConversationStep.RecruiterAnswers)),
                FinalObservations
                ).Bind<Step>(v => v),
            _ => Result.Fail("Unknown step type")
        };
    }

    private IEnumerable<string> GetStringEnumerableField(string fieldName)
    {
        var value = GetValueFromFields(fieldName);

        if (value is null)
        {
            return [];
        }

        return JsonSerializer.Deserialize<IEnumerable<string>>(value) ?? [];
    }

    private Dictionary<TKey, TValue> GetDictionaryField<TKey, TValue>(string fieldName) where TKey : notnull
    {
        var value = GetValueFromFields(fieldName);
        if (value is null)
        {
            return [];
        }
        return JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(value) ?? [];
    }

    private string? GetValueFromFields(string fieldName)
    {
        var pascalCaseFieldName = char.ToLowerInvariant(fieldName[0]) + fieldName[1..];
        return Fields.GetValueOrDefault(fieldName) ?? Fields.GetValueOrDefault(pascalCaseFieldName);
    }
}
