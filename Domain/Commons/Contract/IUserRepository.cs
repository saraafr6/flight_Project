using Domain.Commons.Entities;

namespace Domain.Commons.Contract
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
    }

}
