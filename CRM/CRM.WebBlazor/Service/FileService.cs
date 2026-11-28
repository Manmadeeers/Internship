using Microsoft.AspNetCore.Components.Forms;

namespace CRM.WebBlazor.Service;

public class FileService(IWebHostEnvironment webHostEnvironment):IFileService
{
    private readonly string[] _allowedImageTypes = {"image/jpeg", "image/png", "image/gif"};
    private const long MaxAllowedFileSize = 5 * 1024 * 1024;
    
    public async Task<string> ReadFileAsync(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"File not found: {path}");
        }
        
        return await File.ReadAllTextAsync(path);
    }

    public async Task<string> UploadImageAsync(IBrowserFile file, string container)
    {
        try
        {
            if (file.Size > MaxAllowedFileSize)
            {
                throw new InvalidOperationException($"File too large: {file.Name}");
            }

            if (!_allowedImageTypes.Contains(Path.GetExtension(file.Name)))
            {
                throw new InvalidOperationException($"File type not supported: {Path.GetExtension(file.Name)}");
            }
            IBrowserFile fileToUpload = file;
            if (file.Size > 1024 * 1024)
            {
                fileToUpload = await file.RequestImageFileAsync(file.ContentType, 500, 500);
            }

            using var stream = new MemoryStream();
            await fileToUpload.OpenReadStream().CopyToAsync(stream);
            
            var fileExtension = Path.GetExtension(file.Name);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";

            if (fileName.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
            {
                throw new InvalidOperationException($"Invalid file extension: {Path.GetExtension(fileName)}");
            }
            
            var folderPath = Path.Combine(webHostEnvironment.ContentRootPath,"images", container);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            var filePath = Path.Combine(folderPath, fileName);
            await File.WriteAllBytesAsync(filePath, stream.ToArray());
            
            return fileName;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error uploading file: {ex.Message}", ex);
        }
    }

    public async Task DeleteImageAsync(string container, string fileName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileName) || fileName.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
            {
                throw new InvalidOperationException($"Invalid file name: {fileName}");
            }
            var  folderPath = Path.Combine(webHostEnvironment.ContentRootPath,"images", container);
            var filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
            else
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error deleting file: {ex.Message}", ex);
        }
    }
}