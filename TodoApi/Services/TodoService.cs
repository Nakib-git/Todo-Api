using AutoMapper;
using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TodoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<TodoItemDto>> GetUserTodosAsync(int userId, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100; // Limit max page size

            var (items, totalCount) = await _unitOfWork.Todos.GetUserTodosPagedAsync(userId, page, pageSize);
            var todoItems = _mapper.Map<List<TodoItemDto>>(items);

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return new PaginatedResponse<TodoItemDto>
            {
                Data = todoItems,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                HasNextPage = page < totalPages,
                HasPreviousPage = page > 1
            };
        }

        public async Task<TodoItemDto?> GetUserTodoByIdAsync(int userId, int todoId)
        {
            var todoItem = await _unitOfWork.Todos.GetUserTodoByIdAsync(userId, todoId);
            return todoItem != null ? _mapper.Map<TodoItemDto>(todoItem) : null;
        }

        public async Task<TodoItemDto> CreateTodoAsync(int userId, CreateTodoDto createTodoDto)
        {
            var todoItem = _mapper.Map<TodoItem>(createTodoDto);
            todoItem.UserId = userId;

            await _unitOfWork.Todos.AddAsync(todoItem);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public async Task<TodoItemDto?> UpdateTodoAsync(int userId, int todoId, UpdateTodoDto updateTodoDto)
        {
            var existingTodo = await _unitOfWork.Todos.GetUserTodoByIdAsync(userId, todoId);
            if (existingTodo == null)
            {
                return null;
            }

            _mapper.Map(updateTodoDto, existingTodo);
            _unitOfWork.Todos.Update(existingTodo);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TodoItemDto>(existingTodo);
        }

        public async Task<bool> DeleteTodoAsync(int userId, int todoId)
        {
            var todoItem = await _unitOfWork.Todos.GetUserTodoByIdAsync(userId, todoId);
            if (todoItem == null)
            {
                return false;
            }

            _unitOfWork.Todos.Remove(todoItem);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<object> GetUserTodoStatsAsync(int userId)
        {
            var totalCount = await _unitOfWork.Todos.GetUserTodoCountAsync(userId);
            var completedCount = await _unitOfWork.Todos.GetUserCompletedTodoCountAsync(userId);
            var pendingCount = totalCount - completedCount;

            return new
            {
                TotalTodos = totalCount,
                CompletedTodos = completedCount,
                PendingTodos = pendingCount,
                CompletionRate = totalCount > 0 ? Math.Round((double)completedCount / totalCount * 100, 2) : 0
            };
        }
    }
}
