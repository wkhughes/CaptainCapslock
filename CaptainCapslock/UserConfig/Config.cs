using Microsoft.Extensions.Configuration;

namespace CaptainCapslock.UserConfig
{
    internal class Config
    {
        public IEnumerable<ApplicationToggleConfig> Applications { get; set; } = [];
        public bool LaunchOnStartup { get; set; } = false;

        public static Config LoadFromFile(string path)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(path, optional: false)
                .Build();

            var config = new Config();
            configuration.Bind(config);

            return config;
        }
    }
}