using ATSourcing.Domain.StepDefinitions.Enums;
using ATSourcing.Domain.StepDefinitions.Snapshots;
using FluentResults;
using System.Text.Json;

namespace ATSourcing.Domain.StepDefinitions;

public class FileUploadStep : Step
{
    public override string Name => "File Upload";

    public override string Description => "Upload the required files";

    private readonly List<string> _requiredFiles = [];
    public List<string> RequiredFiles => _requiredFiles;

    private readonly List<string> _returnObservations = [];
    public List<string> ReturnObservations => _returnObservations;

    private readonly Dictionary<string, Guid> _uploadedFiles = [];
    public Dictionary<string, Guid> UploadedFiles => _uploadedFiles;

    public override string Title => $"Upload the following files: {string.Join(", ", RequiredFiles)}";

    private FileUploadStep(IEnumerable<string> requiredFiles)
    {
        State = StepState.PendingCandidate;
        _requiredFiles.AddRange(requiredFiles);
    }

    public static Result<FileUploadStep> Create(IEnumerable<string> requiredFiles)
    {
        if (requiredFiles is null || !requiredFiles.Any())
        {
            return Result.Fail<FileUploadStep>("Required files cannot be empty");
        }

        return Result.Ok(new FileUploadStep(requiredFiles));
    }

    public static Result<FileUploadStep> Restore(StepState state,
        IEnumerable<string> requiredFiles, 
        IEnumerable<string> returnObservations, 
        Dictionary<string, Guid> uploadedFiles,
        string? finalObservations)
    {
        if (requiredFiles is null || !requiredFiles.Any())
        {
            return Result.Fail<FileUploadStep>("Required files cannot be empty");
        }
        var step = new FileUploadStep(requiredFiles);
        step._returnObservations.AddRange(returnObservations);

        foreach (var (fileName, fileId) in uploadedFiles)
        {
            step._uploadedFiles[fileName] = fileId;
        }

        step.State = state;
        step.FinalObservations = finalObservations;
        return Result.Ok(step);
    }

    public Result AddReturnObservation(string observation)
    {
        if (string.IsNullOrWhiteSpace(observation))
        {
            return Result.Fail("Observation cannot be empty");
        }

        if (State != StepState.PendingRecruiter)
        {
            return Result.Fail("Cannot add observation to a step that is not pending recruiter");
        }

        _returnObservations.Add(observation.Trim());

        State = StepState.PendingCandidate;

        return Result.Ok();
    }

    public Result AddFile(string fileName, Guid fileId)
    {
        if (State != StepState.PendingCandidate)
        {
            return Result.Fail("Cannot add file to a step that is not pending candidate");
        }
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return Result.Fail("File name cannot be empty");
        }
        if (!_requiredFiles.Contains(fileName))
        {
            return Result.Fail($"File '{fileName}' is not required");
        }
        _uploadedFiles[fileName] = fileId;
        return Result.Ok();
    }

    public Result FinalizeFileUpload(bool ignoreMissing)
    {
        if (State != StepState.PendingCandidate)
        {
            return Result.Fail("Cannot add file to a step that is not pending candidate");
        }
        if (!ignoreMissing && _requiredFiles.Any(f => !_uploadedFiles.ContainsKey(f)))
        {
            return Result.Fail("Required files missing, either upload the missing files or make sure you want to ignore them.");
        }

        State = StepState.PendingRecruiter;

        return Result.Ok();
    }

    public override StepSnapshot ToSnapshot()
    {
        return new StepSnapshot
        {
            Name = Name,
            Description = Description,
            Title = Title,
            State = State,
            FinalObservations = FinalObservations,
            Fields = new Dictionary<string, string?>
            {
                { nameof(RequiredFiles), JsonSerializer.Serialize(RequiredFiles) },
                { nameof(ReturnObservations), JsonSerializer.Serialize(ReturnObservations) },
                { nameof(UploadedFiles), JsonSerializer.Serialize(UploadedFiles) }
            }
        };
    }
}
