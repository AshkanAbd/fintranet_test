using Application.Common.Validation;
using FluentValidation;

namespace Application.TodoList.Commands;

public class CreateTodoListValidator : Validator<CreateTodoListCommand>
{
    public CreateTodoListValidator()
    {
        Title();
        Color();
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

    private void Color()
    {
        RuleFor(x => x.Color)
            .NotNull()
            .WithDefaultMessage("");
        RuleFor(x => x.Color)
            .Color()
            .WhenNotNull()
            .WithDefaultMessage("");
    }
}