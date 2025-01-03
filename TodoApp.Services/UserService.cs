using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;

namespace TodoApp.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private IUserService _userServiceImplementation;

    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task AddUserAsync(User user)
    {
        await _userRepository.AddAsync(user);
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return (await _userRepository.FindAsync(u => u.Email == email)).FirstOrDefault();
    }
}