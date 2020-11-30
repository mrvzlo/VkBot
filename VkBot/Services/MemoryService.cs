using System.IO;
using Microsoft.Extensions.Configuration;

namespace VkBot
{
    public class MemoryService : IMemoryService
    {
        private readonly string _path;
        
        public MemoryService(IConfiguration configuration)
        {
            _path = configuration["Config:MemoryFile"];
        }

        public void Save(string line)
        {
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            using var sw = File.Exists(_path) ? File.AppendText(_path) : File.CreateText(_path);
            sw.WriteLine(line);
        }

        public int GetMemorizedCount() => File.Exists(_path) ? File.ReadAllLines(_path).Length : 0;
    }
}
