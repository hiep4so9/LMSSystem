using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using LMSSystem.Repositories.IRepository;

public class FirebaseStorageRepository : IFirebaseStorageRepository
{
    private readonly IConfiguration _configuration;

    public FirebaseStorageRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<string> UploadFileAsync(IFormFile file, string? documentVersion)
    {

        var projectId = _configuration["Firebase:ProjectId"];
        var bucketName = _configuration["Firebase:BucketName"];
        var credentialFilePath = "..\\LMSSystem\\Credentials\\credentials.json";
        Console.WriteLine(credentialFilePath);
        Console.WriteLine(bucketName);
        Console.WriteLine(projectId);
        //D:\repos\FlightDocsSystem\FlightDocsSystem\service-account.json
        // Thay đổi đường dẫn này để trỏ đến tệp JSON của tài khoản dịch vụ

        // Khởi tạo Firebase Admin SDK với thông tin xác thực
        var credential = GoogleCredential.FromFile(credentialFilePath);
        var storageClient = await StorageClient.CreateAsync(credential);

        // Tạo tên tệp tin duy nhất
        var fileName = Path.GetFileName(file.FileName);
        string objectName = "";
        if (documentVersion == null)
        {
            objectName = $"{Path.GetFileNameWithoutExtension(fileName)}{Path.GetExtension(fileName)}";
        }
        objectName = $"{Path.GetFileNameWithoutExtension(fileName)}_{documentVersion}{Path.GetExtension(fileName)}";
        // Tải lên tệp tin lên Firebase Cloud Storage
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await storageClient.UploadObjectAsync(bucketName, objectName, contentType: file.ContentType, memoryStream);
        }

        // Trả về URL của tệp tin đã tải lên
        return $"https://storage.googleapis.com/{bucketName}/{objectName}?readOnly=true";
    }
}
