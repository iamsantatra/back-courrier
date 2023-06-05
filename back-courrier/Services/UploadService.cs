namespace back_courrier.Services
{
    public class UploadService: IUploadService
    {
        public string UploadFileAsync(IFormFile file)
        {
            // Generate a unique file name
            string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            // Save the file to a specific directory
            string filePath = Path.Combine("Uploads", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return filePath;
        }
    }
}
