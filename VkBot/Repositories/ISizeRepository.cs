﻿namespace VkBot.Repositories
{
    public interface ISizeRepository
    {
        public void Save(int size);
        public int GetAverage();
    }
}
