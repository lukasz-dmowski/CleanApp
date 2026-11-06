namespace CleanApp.Interfaces
{
    public interface IAuthenticationService
    {
        bool Authenticate(string username, string password);

        bool IsUsernameValid(string username);
        bool IsPasswordValid(string password);
    }
}
