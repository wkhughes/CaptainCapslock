using NLog;
using NLog.Targets;
using NLog.Windows.Forms;
using System.ComponentModel;

namespace CaptainCapslock.Logging
{
    internal static class LoggingUtils
    {
        private const string LogLayout = "${longdate} [${level:uppercase=true}] ${message:withexception=true}";

        public static void ConfigureLogging(string fileName, bool includeDebugLevel)
        {
            var fileTarget = new FileTarget("Last run log file")
            {
                FileName = fileName,
                ArchiveOldFileOnStartup = true,
                MaxArchiveFiles = 1,
                Layout = LogLayout,
            };

            var consoleFormResources = new ComponentResourceManager(typeof(UI.Console));
            var consoleFormTarget = new RichTextBoxTarget
            {
                Name = "Console form",
                FormName = consoleFormResources.GetString("Name"),
                ControlName = consoleFormResources.GetString("TextName"),
                AutoScroll = true,
                MaxLines = 1000,
                AllowAccessoryFormCreation = false,
                MessageRetention = NLog.Windows.Forms.RichTextBoxTargetMessageRetentionStrategy.All,
                UseDefaultRowColoringRules = true,
                Layout = LogLayout,
            };


            var config = new NLog.Config.LoggingConfiguration();
            var minLevel = includeDebugLevel ? LogLevel.Debug : LogLevel.Info;

            config.AddRule(minLevel, LogLevel.Fatal, fileTarget);
            config.AddRule(minLevel, LogLevel.Fatal, consoleFormTarget);

            LogManager.Configuration = config;
        }
    }
}
