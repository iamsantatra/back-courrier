namespace back_courrier.Services
{
    public interface IUploadService
    {
        string UploadFileAsync(IFormFile file);
    }
}
