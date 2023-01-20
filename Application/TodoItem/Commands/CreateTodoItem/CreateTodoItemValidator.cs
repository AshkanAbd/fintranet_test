using Application.Common.Validation;
using FluentValidation;

namespace Application.TodoItem.Commands.CreateTodoItem;

public class CreateTodoItemValidator : Validator<CreateTodoItemCommand>
{
    public CreateTodoItemValidator()
    {
        Title();
        Note();
        Priority();
    }

    private void Title()
    {
        RuleFor(x => x.Title)
            .NotNull()
            .WithDefaultMessage("");
        RuleFor(x => x.Title)
            .MinimumLength(1)
            .WhenNotNull()
            .WithDefaultMessage("");
        RuleFor(x => x.Title)
            .MaximumLength(30)
            .WhenNotNull()
            .WithDefaultMessage("");
    }

    private void Note()
    {
        RuleFor(x => x.Note)
            .NotNull()
            .WithDefaultMessage("");
        RuleFor(x => x.Note)
            .MinimumLength(1)
            .WhenNotNull()
            .WithDefaultMessage("");
        RuleFor(x => x.Note)
            .MaximumLength(65000)
            .WhenNotNull()
            .WithDefaultMessage("");
    }

    private void Priority()
    {
        RuleFor(x => x.Priority)
            .NotNull()
            .WithDefaultMessage("");
        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(1)
            .WhenNotNull()
            .WithDefaultMessage("");
        RuleFor(x => x.Priority)
            .LessThanOrEqualTo(5)
            .WhenNotNull()
            .WithDefaultMessage("");
    }
}