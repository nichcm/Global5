using Global5.Application.Interfaces;
using System;
using System.IO;
using System.IO.Compression;

namespace Global5.Application.Services
{
    public class ZipService : IZipService
    {
        private MemoryStream zipArchiveMemory { get; set; }
        private ZipArchive zipArchive { get; set; }
        public ZipService()
        {
            zipArchiveMemory = new MemoryStream();
            zipArchive = new ZipArchive(zipArchiveMemory, ZipArchiveMode.Create, true);
        }
        public byte[] Compress(string fileName, byte[] file)
        {
            var entry = zipArchive.CreateEntry(fileName);
            using (var entryStream = entry.Open())
            using (var mStream = new MemoryStream(file))
            {
                mStream.CopyTo(entryStream);
            }
            return zipArchiveMemory.ToArray();
        }

        public void Dispose()
        {
            zipArchive.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}