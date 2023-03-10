using Application.Common;
using Application.Common.Response;
using Application.TodoItem.Queries.GetTodoItemList;
using Infrastructure.Repositories.TodoList;
using MediatR;

namespace Application.TodoList.Queries.GetTodoList;

public class GetTodoListQueryHandler : AbstractRequestHandler<GetTodoListQuery, StdResponse<GetTodoListDto>>
{
    private ITodoListRepository TodoListRepository { get; }
    private IMediator Mediator { get; }

    public GetTodoListQueryHandler(ITodoListRepository todoListRepository, IMediator mediator)
    {
        TodoListRepository = todoListRepository;
        Mediator = mediator;
    }

    public override async Task<StdResponse<GetTodoListDto>> Handle(GetTodoListQuery request, CancellationToken _)
    {
        if (!await TodoListRepository.Exists(request.Id, _)) {
            return NotFoundMsg<GetTodoListDto>("Todo list notfound.");
        }

        var todoList = (await TodoListRepository.Get(request.Id, _))!;

        var todoItemList = await Mediator.Send(new GetTodoItemListQuery {
            TodoListId = request.Id,
            Page = request.Page,
            PageSize = request.PageSize,
        }, _);

        return Ok(new GetTodoListDto {
            Id = todoList.Id,
            Title = todoList.Title,
            Color = todoList.Color,
            TodoItemList = todoItemList.DataAsDataStruct()!
        });
    }
}