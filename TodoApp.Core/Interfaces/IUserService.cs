using TodoApp.Core.Entities;

namespace TodoApp.Core.Interfaces;

public interface IUserService
{
    public Task AddUserAsync(User user);
    public Task<User> GetUserByEmailAsync(string email);
}