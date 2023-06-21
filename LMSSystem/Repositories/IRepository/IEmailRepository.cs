using LMSSystem.Data;

namespace LMSSystem.Repositories.IRepository
{
    public interface IEmailRepository
    {
        Task SendEmailAsync(EmailDTO request, string filepath = null!);
    }
}
