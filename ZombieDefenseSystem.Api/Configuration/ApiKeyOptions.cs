namespace ZombieDefenseSystem.Api.Configuration
{
    public class ApiKeyOptions
    {
        public const string SectionName = "ApiKeySettings";

        public string HeaderName { get; set; } = "X-API-KEY";
        public string Key { get; set; } = string.Empty;
    }
}
