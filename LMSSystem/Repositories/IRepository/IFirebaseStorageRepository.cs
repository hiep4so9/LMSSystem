namespace LMSSystem.Repositories.IRepository
{
    public interface IFirebaseStorageRepository
    {
        Task<string> UploadFileAsync(IFormFile fileStream, string fileName);
        Task<byte[]> DownloadFileAsync(string bucketName, string objectName);
    }
}
