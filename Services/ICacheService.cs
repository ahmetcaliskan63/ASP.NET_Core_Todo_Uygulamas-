using System;

namespace ToDoUygulaması.Services
{
    public interface ICacheService
    {
        T Get<T>(string key);
        bool Set<T>(string key, T value, TimeSpan? expiry = null);
        bool Remove(string key);
    }
}