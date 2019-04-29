using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Osma.Mobile.App.Services.Interfaces;
using Xamarin.Forms;

namespace Osma.Mobile.App.Services
{
    public class KeyValueStoreService : IKeyValueStoreService
    {
        public bool IsInitialized() => Application.Current != null;

        public Task SetDataAsync<T>(string key, T data, Serializer type = Serializer.Xml)
        {
            switch (type)
            {
                case Serializer.Xml:
                    return SetDataAsyncXml(key, data);
            }
            return Task.FromResult(false);
        }

        private async Task SetDataAsyncXml<T>(string key, T data)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (TextWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, data);
                    await SetDataAsync(key, writer.ToString());
                }
            }
            catch (Exception) { }
        }

        public Task SetDataAsync(string key, string data)
        {
            Application.Current.Properties[key] = data;
            return Application.Current.SavePropertiesAsync();
        }

        public void SetData(string key, string data) => Application.Current.Properties[key] = data;

        public void SetData<T>(string key, T data, Serializer type = Serializer.Xml)
        {
            switch (type)
            {
                case Serializer.Xml:
                    SetDataAsyncXml(key, data).Wait();
                    break;
            }
        }

        public string GetData(string key) => Application.Current.Properties[key] as string;

        public T GetData<T>(string key, Serializer type = Serializer.Xml)
        {
            switch (type)
            {
                case Serializer.Xml:
                    return GetDataXml<T>(key);
            }
            return default(T);
        }

        private T GetDataXml<T>(string key)
        {
            try
            {
                string userXml = GetData(key);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (TextReader reader = new StringReader(userXml))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public bool DeleteData(string key)
        {
            if (!KeyExists(key))
                return false;

            return Application.Current.Properties.Remove(key);
        }

        public async Task<bool> DeleteDataAsync(string key)
        {
            if (!KeyExists(key))
                return false;

            if (Application.Current.Properties.Remove(key))
            {
                await Application.Current.SavePropertiesAsync();
                return true;
            }
            return false;
        }

        public bool KeyExists(string key) => Application.Current.Properties.ContainsKey(key);
    }
}
