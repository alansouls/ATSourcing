using ATSourcing.Domain.StepDefinitions.Enums;
using ATSourcing.Domain.StepDefinitions.Snapshots;
using FluentResults;
using System.Text.Json;

namespace ATSourcing.Domain.StepDefinitions;

public class ConversationStep : Step
{
    public override string Name => "Conversation";

    public override string Description => "Conversation with the candidate";

    public string? Question { get; private set; }

    private readonly List<string> _candidateAnswers = [];
    public List<string> CandidateAnswers => _candidateAnswers;

    private readonly List<string> _recruiterAnswers = [];
    public List<string> RecruiterAnswers => _recruiterAnswers;

    private ConversationStep(string question)
    {
        State = StepState.PendingCandidate;
        Question = question.Trim();
    }

    public static Result<ConversationStep> Create(string question)
    {
        if (string.IsNullOrWhiteSpace(question))
        {
            return Result.Fail<ConversationStep>("Question cannot be empty");
        }

        return Result.Ok(new ConversationStep(question));
    }

    public static Result<ConversationStep> Restore(StepState state, string question, IEnumerable<string> candidateAnswers, IEnumerable<string> recruiterAnswers)
    {
        if (string.IsNullOrWhiteSpace(question))
        {
            return Result.Fail<ConversationStep>("Question cannot be empty");
        }
        var step = new ConversationStep(question);
        step._candidateAnswers.AddRange(candidateAnswers);
        step._recruiterAnswers.AddRange(recruiterAnswers);
        step.State = state;
        return Result.Ok(step);
    }

    public Result AddAnswer(string answer)
    {
        if (string.IsNullOrWhiteSpace(answer))
        {
            return Result.Fail("Answer cannot be empty");
        }

        if (State != StepState.PendingCandidate || State != StepState.PendingRecruiter)
        {
            return Result.Fail("Cannot add answer to a step that is not pending candidate or recruiter");
        }

        var answersToAdd = State == StepState.PendingCandidate ? _candidateAnswers : _recruiterAnswers;

        answersToAdd.Add(answer.Trim());

        State = State == StepState.PendingCandidate ? StepState.PendingRecruiter : StepState.PendingCandidate;

        return Result.Ok();
    }

    public override StepSnapshot ToSnapshot()
    {
        return new StepSnapshot
        {
            Name = Name,
            Description = Description,
            State = State,
            Fields = new Dictionary<string, string?>
            {
                { nameof(Question), Question },
                { nameof(CandidateAnswers), JsonSerializer.Serialize(CandidateAnswers) },
                { nameof(RecruiterAnswers), JsonSerializer.Serialize(RecruiterAnswers) }
            }
        };
    }
}
