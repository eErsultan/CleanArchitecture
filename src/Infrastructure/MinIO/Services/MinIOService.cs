using Minio;
using Minio.AspNetCore;
using Minio.Exceptions;
using System;
using System.IO;
using System.Threading.Tasks;
using Application.Interfaces;
using System.Collections.Generic;

namespace Infrastructure.MinIO.Service.Services
{
    public class MinIOService : IMinIOService
    {
        private readonly MinioClient _minio;

        public MinIOService(IMinioClientFactory clientFactory)
        {
            _minio = clientFactory.CreateClient();
        }

        public async Task PutObjectAsync(string bucketName, string fileName, byte[] bytes)
        {
            try
            {
                var isFound = await _minio.BucketExistsAsync(bucketName);
                if (isFound)
                {
                    await using (var stream = new MemoryStream(bytes))
                    {
                        await _minio.PutObjectAsync(bucketName, fileName, stream, bytes.Length, "application/octet-stream");
                    }
                }
                else
                {
                    throw new MinioException("Bucket не найден");
                }
            }
            catch (MinioException ex)
            {
                throw new Exception($"Ошибка загрузки файла: {ex.Message}");
            }
        }

        public async Task<byte[]> GetObjectAsync(string bucketName, string fileName)
        {
            try
            {
                byte[] bytes = default;

                var isFound = await _minio.BucketExistsAsync(bucketName);
                if (isFound)
                {
                    await _minio.StatObjectAsync(bucketName, fileName);

                    await _minio.GetObjectAsync(bucketName, fileName, stream =>
                    {
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            bytes = ms.ToArray();
                        }
                    });
                }
                else
                {
                    throw new MinioException("Bucket не найден");
                }

                return bytes;
            }
            catch (MinioException ex)
            {
                throw new Exception($"Ошибка загрузки файла: {ex.Message}");
            }
        }

        public async Task<string> GetImageUrlAsync(string bucketName, string fileName)
        {
            try
            {
                var reqParams = new Dictionary<string, string> { { "response-content-type", "image/jpeg" } };
                string presignedUrl = await _minio.PresignedGetObjectAsync(bucketName, fileName, 86400, reqParams);
                return presignedUrl;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получений файла: {ex.Message}");
            }
        }

        public async Task RemoveObjectAsync(string bucketName, string fileName)
        {
            try
            {
                await _minio.RemoveObjectAsync(bucketName, fileName);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удалений файла: {ex.Message}");
            }
        }
    }
}
