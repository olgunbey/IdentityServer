namespace IdentityOrnek.Services
{
    public interface IEmailService
    {
        Task SendResetEmail(string resetEmailLink, string tooEmail);
    }
}
