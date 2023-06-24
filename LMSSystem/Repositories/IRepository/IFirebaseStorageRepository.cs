namespace LMSSystem.Repositories.IRepository
{
    public interface IFirebaseStorageRepository
    {
        Task<string> UploadFileAsync(IFormFile fileStream, string objectName, string controllerName);
        Task<byte[]> DownloadFileAsync(string bucketName, string objectName);
    }
}
