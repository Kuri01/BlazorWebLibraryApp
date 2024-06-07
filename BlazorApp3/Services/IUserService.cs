using System.Security.Claims;
using System.Threading.Tasks;

public interface IUserService
{
    string GetUserId(ClaimsPrincipal user);
}