using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMinIOService
    {
        Task PutObjectAsync(string bucketName, string fileName, byte[] bytes);
        Task<byte[]> GetObjectAsync(string bucketName, string fileName);
        Task<string> GetImageUrlAsync(string bucketName, string fileName);
        Task RemoveObjectAsync(string bucketName, string fileName);
    }
}