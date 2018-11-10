namespace AppSettingsConfigurationPlugin.Interfaces
{
    interface IAppSettingsConfiguration
    {
        string GetValue(string key);

        void GetSectionAndBind<T>(string key, T entity);
    }
}
