using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class TodoRepository : GenericRepository<TodoItem>, ITodoRepository
    {
        public TodoRepository(TodoDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TodoItem>> GetUserTodosAsync(int userId)
        {
            return await _dbSet
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<TodoItem?> GetUserTodoByIdAsync(int userId, int todoId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.Id == todoId && t.UserId == userId);
        }

        public async Task<(IEnumerable<TodoItem> items, int totalCount)> GetUserTodosPagedAsync(int userId, int page, int pageSize)
        {
            var query = _dbSet.Where(t => t.UserId == userId);
            
            var totalCount = await query.CountAsync();
            
            var items = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<int> GetUserTodoCountAsync(int userId)
        {
            return await _dbSet.CountAsync(t => t.UserId == userId);
        }

        public async Task<int> GetUserCompletedTodoCountAsync(int userId)
        {
            return await _dbSet.CountAsync(t => t.UserId == userId && t.IsCompleted);
        }
    }
}
