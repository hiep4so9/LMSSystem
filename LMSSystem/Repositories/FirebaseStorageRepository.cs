using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using LMSSystem.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;

public class FirebaseStorageRepository : IFirebaseStorageRepository
{
    private readonly IConfiguration _configuration;

    public FirebaseStorageRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<string> UploadFileAsync(IFormFile file, string? documentVersion, string dataName)
    {

        var projectId = _configuration["Firebase:ProjectId"];
        var bucketName = _configuration["Firebase:BucketName"];
        var credentialFilePath = "..\\LMSSystem\\Credentials\\credentials.json";
        Console.WriteLine(credentialFilePath);
        Console.WriteLine(bucketName);
        Console.WriteLine(projectId);
        //F:\LMSSystem\Credentials\Credentials\credentials.json
        // Thay đổi đường dẫn này để trỏ đến tệp JSON của tài khoản dịch vụ

        // Khởi tạo Firebase Admin SDK với thông tin xác thực
        var credential = GoogleCredential.FromFile(credentialFilePath);
        var storageClient = await StorageClient.CreateAsync(credential);

        // Tạo tên tệp tin duy nhất
        var fileName = Path.GetFileName(file.FileName);
        string objectName = "";
        if (documentVersion == null)
        {
            objectName = $"{dataName}/{Path.GetFileNameWithoutExtension(fileName)}{Path.GetExtension(fileName)}";
        }
        else
        {
            objectName = $"{dataName}/{Path.GetFileNameWithoutExtension(fileName)}_{documentVersion}{Path.GetExtension(fileName)}";
        }
        // Tải lên tệp tin lên Firebase Cloud Storage
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await storageClient.UploadObjectAsync(bucketName, objectName, contentType: file.ContentType, memoryStream);
        }

        // Trả về URL của tệp tin đã tải lên
        return $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o/{Uri.EscapeDataString(objectName)}?alt=media";
    }

    public async Task<byte[]> DownloadFileAsync(string bucketName, string objectName)
    {
        try
        {
            // Đường dẫn tới tệp tin credentials.json
            var credentialFilePath = "..\\LMSSystem\\Credentials\\credentials.json";

            // Khởi tạo Firebase Admin SDK với thông tin xác thực
            var credential = GoogleCredential.FromFile(credentialFilePath);
            var storageClient = await StorageClient.CreateAsync(credential);

            // Tải xuống tệp tin từ Firebase Cloud Storage
            using (var memoryStream = new MemoryStream())
            {
                await storageClient.DownloadObjectAsync(bucketName, objectName, memoryStream);
                return memoryStream.ToArray();
            }
        }
        catch (Exception ex)
        {
            // Xử lý lỗi và ném ngoại lệ để được xử lý ở lớp gọi
            throw new Exception("Failed to download file.", ex);
        }
    }

    public async Task DeleteFileAsync(string bucketName, string objectName)
    {
        // Đường dẫn đến tệp JSON của tài khoản dịch vụ
        string credentialFilePath = "..\\LMSSystem\\Credentials\\credentials.json";

        // Khởi tạo Firebase Admin SDK với thông tin xác thực
        GoogleCredential credential = GoogleCredential.FromFile(credentialFilePath);
        StorageClient storageClient = await StorageClient.CreateAsync(credential);

        // Xóa tệp tin từ Firebase Cloud Storage
        await storageClient.DeleteObjectAsync(bucketName, objectName);
    }

}
