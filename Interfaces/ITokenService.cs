using DATINGAPP.Entities;

namespace DATINGAPP.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
