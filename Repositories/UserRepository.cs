using IOT.DBContext;
using Microsoft.EntityFrameworkCore;

namespace IOT.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<Usersmaster>> GetAllUsersAsync();
    Task<Usersmaster?> GetByIdAsync(int id);
    Task AddAsync(Usersmaster user);
    Task<Usersmaster?> GetByNameAsync(string name);
}


public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Usersmaster>> GetAllUsersAsync()
    {
        return await _context.Usersmasters.ToAsyncEnumerable().ToListAsync();
    }

    public async Task<Usersmaster?> GetByIdAsync(int id)
    {
        return await _context.Usersmasters.FindAsync(id);
    }

    public async Task AddAsync(Usersmaster user)
    {
        await _context.Usersmasters.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<Usersmaster?> GetByNameAsync(string name)
    {
        return await _context.Usersmasters.FirstOrDefaultAsync(u => u.UserName == name);
    }
}