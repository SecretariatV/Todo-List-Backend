using Microsoft.EntityFrameworkCore;
using TodoBackend.Data;
using TodoBackend.Data.Entities;

namespace TodoBackend.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        // Constructor to initialize the DbContext for database interactions
        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Creates a new user and saves it to the database, returning the created user
        public async Task<UserEntity> CreateUser(UserEntity user)
        {
            await _dbContext.users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        // Retrieves a user by their email address
        // Throws an exception if no user is found with the provided email
        public async Task<UserEntity> GetOneUserByEmail(string email)
        {
            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new InvalidOperationException($"User with email '{email}' not found.");
            }

            return user;
        }
    }
}
