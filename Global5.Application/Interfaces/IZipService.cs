using System;

namespace Global5.Application.Interfaces
{
    public interface IZipService : IDisposable
    {
        byte[] Compress(string fileName, byte[] file);
    }
}