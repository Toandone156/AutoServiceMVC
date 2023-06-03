namespace AutoServiceMVC.Services.System
{
    public interface IImageUploadService
    {
        Task<string> UploadImageAsync(IFormFile imageFile);
    }

    public class ImageUploadService : IImageUploadService
    {
        private readonly HttpContext? _httpContext;

        public ImageUploadService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("No image file specified.");
            }

            var fileName = await UploadAsync(imageFile);

            return GetImageUrl(fileName);
        }

        private async Task<string> UploadAsync(IFormFile imageFile)
        {
            string fileExtension = Path.GetExtension(imageFile.FileName);
            string newFileName = $"{Guid.NewGuid()}{fileExtension}";
            string uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads");

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            string filePath = Path.Combine(uploadDirectory, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return newFileName;
        }

        private string GetImageUrl(string fileName)
        {
            string baseUrl = $"{_httpContext.Request.Scheme}://{_httpContext.Request.Host}";

            string relativePath = $"/uploads/{fileName}";

            return relativePath;
        }
    }
}
