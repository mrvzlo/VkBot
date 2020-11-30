namespace VkBot
{
    public interface IMemoryService
    {
        void Save(string line);
        int GetMemorizedCount();
    }
}
