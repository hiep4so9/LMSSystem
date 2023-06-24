using LMSSystem.Data;
using LMSSystem.Helpers;
using LMSSystem.Models;
using LMSSystem.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace LMSSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly IMaterialRepository _MaterialRepo;
        private readonly IFirebaseStorageRepository _firebaseStorageService;

        public MaterialsController(IMaterialRepository repo, IFirebaseStorageRepository firebaseStorageService)
        {
            _MaterialRepo = repo;
            _firebaseStorageService = firebaseStorageService;
        }

        [HttpGet/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetAllMaterials(int page = 1, int pageSize = 10)
        {
            try
            {
                var allMaterials = await _MaterialRepo.GetAllMaterialsAsync();
                var paginatedMaterials = Pagination.Paginate(allMaterials, page, pageSize);

                var totalMaterials = allMaterials.Count;
                var totalPages = Pagination.CalculateTotalPages(totalMaterials, pageSize);

                var paginationInfo = new
                {
                    TotalMaterials = totalMaterials,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Ok(new { Materials = paginatedMaterials, Pagination = paginationInfo });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> GetMaterialById(int id)
        {
            var Material = await _MaterialRepo.GetMaterialAsync(id);
            return Material == null ? NotFound() : Ok(Material);
        }

        /*        [HttpPost, Authorize]
                public async Task<IActionResult> AddNewMaterial(MaterialDTO model)
                {
                    try
                    {
                        var newMaterialId = await _MaterialRepo.AddMaterialAsync(model);
                        var Material = await _MaterialRepo.GetMaterialAsync(newMaterialId);
                        return Material == null ? NotFound() : Ok(Material);
                    }
                    catch
                    {
                        return BadRequest();
                    }
                }*/

        [HttpGet("download/{materialId}")]
        public async Task<IActionResult> DownloadMaterial(int materialId)
        {
            try
            {
                var material = await _MaterialRepo.GetMaterialAsync(materialId);

                if (material == null)
                {
                    return NotFound();
                }

                // Lấy tên tệp tin và tên bucket từ URL của tệp tin trên Firebase
                var fileUrl = material.MaterialFile;
                var bucketName = fileUrl.Split('/')[3];
                var objectName = fileUrl.Substring(fileUrl.IndexOf(bucketName) + bucketName.Length + 1);

                // Tải xuống tệp tin từ Firebase
                var fileData = await _firebaseStorageService.DownloadFileAsync(bucketName, objectName);

                // Trả về tệp tin đã tải xuống
                return File(fileData, "application/octet-stream", material.MaterialTitle);
            }
            catch
            {
                return BadRequest();
            }
        }




        [HttpPost]
        public async Task<IActionResult> AddNewMaterial(IFormFile file, int courseId)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file selected.");
                }

                var fileName = Path.GetFileName(file.FileName);
                var fileUrl = await _firebaseStorageService.UploadFileAsync(file, null, "Material");

                var model = new MaterialDTO
                {
                    MaterialTitle = fileName,
                    MaterialFile = fileUrl,
                    CourseID = courseId,
                };

                var newMaterialId = await _MaterialRepo.AddMaterialAsync(model);
                var material = await _MaterialRepo.GetMaterialAsync(newMaterialId);

                return material == null ? NotFound() : Ok(material);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaterial(int id, IFormFile updatedFile, int courseId)
        {
            try
            {
                // Kiểm tra xem vật liệu có tồn tại trong cơ sở dữ liệu hay không
                var existingMaterial = await _MaterialRepo.GetMaterialAsync(id);
                if (existingMaterial == null)
                {
                    return NotFound();
                }

                // Nếu có tệp tin được gửi lên từ client, thực hiện quá trình tải lên và cập nhật URL tệp tin
                string fileUrl = existingMaterial.MaterialFile;
                if (updatedFile != null && updatedFile.Length > 0)
                {
                    var fileName = Path.GetFileName(updatedFile.FileName);
                    fileUrl = await _firebaseStorageService.UploadFileAsync(updatedFile, null, "Material");

                    // Cập nhật thông tin vật liệu
                    existingMaterial.MaterialTitle = fileName;
                    existingMaterial.MaterialFile = fileUrl;
                }

                existingMaterial.CourseID = courseId;

                await _MaterialRepo.UpdateMaterialAsync(id, existingMaterial);

                return Ok(existingMaterial);
            }
            catch
            {
                return BadRequest();
            }
        }





        [HttpDelete("{id}")/*, Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> DeleteMaterial([FromRoute] int id)
        {
            await _MaterialRepo.DeleteMaterialAsync(id);
            return Ok();
        }
    }
}
