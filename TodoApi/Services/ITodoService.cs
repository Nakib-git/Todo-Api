using TodoApi.DTOs;

namespace TodoApi.Services
{
    public interface ITodoService
    {
        Task<PaginatedResponse<TodoItemDto>> GetUserTodosAsync(int userId, int page = 1, int pageSize = 10);
        Task<TodoItemDto?> GetUserTodoByIdAsync(int userId, int todoId);
        Task<TodoItemDto> CreateTodoAsync(int userId, CreateTodoDto createTodoDto);
        Task<TodoItemDto?> UpdateTodoAsync(int userId, int todoId, UpdateTodoDto updateTodoDto);
        Task<bool> DeleteTodoAsync(int userId, int todoId);
        Task<object> GetUserTodoStatsAsync(int userId);
    }
}
