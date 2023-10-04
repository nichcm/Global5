using System;
using System.Threading.Tasks;

namespace Global5.Application.Interfaces
{
    public interface IBlobStorageService : IDisposable
    {
        Task Upload(string blobUrl, byte[] file);
        Task<string> Download(string blobUrl);
    }
}