namespace LMSSystem.Repositories.IRepository
{
    public interface IFirebaseStorageRepository
    {
        Task<string> UploadFileAsync(IFormFile fileStream, string documentVersion, string dataName, string data);
        Task<byte[]> DownloadFileAsync(string bucketName, string objectName);
        Task DeleteFileAsync(string bucketName, string objectName);
    }
}
