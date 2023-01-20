using Application.Common.Validation;
using Application.TodoList.Commands.CreateTodoList;
using FluentValidation;

namespace Application.TodoList.Commands.UpdateTodoList;

public class UpdateTodoListValidator : Validator<UpdateTodoListCommand>
{
    public UpdateTodoListValidator()
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