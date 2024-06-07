using System.Security.Claims;

public class UserService : IUserService
{
    public string GetUserId(ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}