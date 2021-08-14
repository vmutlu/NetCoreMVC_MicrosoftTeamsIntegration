using System.Threading.Tasks;

namespace NetCoreMVC_GraphAPI_Integration.Services
{
    public interface IRefreshTokenService
    {
        Task RefleshToken();
    }
}
