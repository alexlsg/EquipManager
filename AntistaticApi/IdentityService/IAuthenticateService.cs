using UserManagement;

namespace AntistaticApi.IdentityService
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(User request, out string token);
    }
}
