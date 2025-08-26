using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITodoRepository : IGenericRepository<TodoItem>
    {
        Task<IEnumerable<TodoItem>> GetUserTodosAsync(int userId);
        Task<TodoItem?> GetUserTodoByIdAsync(int userId, int todoId);
        Task<(IEnumerable<TodoItem> items, int totalCount)> GetUserTodosPagedAsync(int userId, int page, int pageSize);
        Task<int> GetUserTodoCountAsync(int userId);
        Task<int> GetUserCompletedTodoCountAsync(int userId);
    }
}
