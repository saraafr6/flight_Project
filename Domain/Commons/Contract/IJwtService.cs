using Domain.Commons.Entities;

namespace Domain.Commons.Contract
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
    }
}
