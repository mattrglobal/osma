using System.Threading.Tasks;

namespace Osma.Mobile.App.Services.Interfaces
{
    public enum Serializer
    {
        Xml
    }

    public interface IKeyValueStoreService
    {
        bool IsInitialized();

        Task SetDataAsync(string key, string data);

        Task SetDataAsync<T>(string key, T data, Serializer type = Serializer.Xml);

        void SetData(string key, string data);

        void SetData<T>(string key, T data, Serializer type = Serializer.Xml);

        string GetData(string key);

        T GetData<T>(string key, Serializer type = Serializer.Xml);

        Task<bool> DeleteDataAsync(string key);

        bool DeleteData(string key);

        bool KeyExists(string key);
    }
}
