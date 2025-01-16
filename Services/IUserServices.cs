
using TodoBackend.Data.Entities;

namespace TodoBackend.Services
{
    public interface IUserService
    {
        // Creates a new user and returns the created User entity
        Task<UserEntity> CreateUser(UserEntity user);

        // Retrieves a user by their email address
        // Throws an exception if no user is found with the provided email
        Task<UserEntity> GetOneUserByEmail(string email);
    }
}
