namespace LMSSystem.Repositories.IRepository
{
    public interface IFirebaseStorageRepository
    {
        Task<string> UploadFileAsync(IFormFile fileStream, string fileName);
    }
}
