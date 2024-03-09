using CaptainCapslock.Keyboard;
using CaptainCapslock.Logging;
using CaptainCapslock.Registry;
using CaptainCapslock.Remote;
using CaptainCapslock.UI;
using CaptainCapslock.UserConfig;
using System.Diagnostics;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace CaptainCapslock
{
    internal static class CaptainCapslock
    {
        private const string LogFilePath = "LastRun.log";
        private const string ConfigFilePath = "Config.json";

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        [STAThread]
        static void Main(string[] args)
        {
            using var mutex = new Mutex(true, "CaptainCapslockSingleInstanceMutex", out bool isSingleInstance);
            if (isSingleInstance)
            {
                RunProgram(args);
            }
            else
            {
                MessageBox.Show("Captain Capslock is already running.", "Captain Capslock", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void RunProgram(string[] args)
        {
            LoggingUtils.ConfigureLogging(LogFilePath, args.Contains("-debug"));
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            InitializeFromConfig();

            var icon = new NotificationIcon();
            icon.ReloadConfig += InitializeFromConfig;
            icon.OpenConfigFolder += () => Process.Start("explorer.exe", AppContext.BaseDirectory);
            Application.Run();
            Logger.Info("Closed by user");
        }

        private static void InitializeFromConfig()
        {
            Logger.Info($"Loading config from file {ConfigFilePath}");
            var config = Config.LoadFromFile(ConfigFilePath);

            // Capslock will be used as a modifier key and its usual functionality disabled,
            // so make sure it's off if currently on
            if (KeyboardUtils.TurnOffCapslockIfOn())
            {
                Logger.Info("Capslock was on and has been turned off");
            }

            var modifierKeyState = ModifierKeyState.FromCurrentCapslockKeyState();
            var keyHooks = CreateApplicationToggleKeyHooksFromConfig(config, modifierKeyState);
            keyHooks[(uint)VIRTUAL_KEY.VK_CAPITAL] = new ModifierToggleKeyHook(modifierKeyState);

            Logger.Info($"Installing keyboard hook");
            KeyboardHook.Install(keyHooks);

            Logger.Info($"Setting launch on startup to {config.LaunchOnStartup}");
            RegistryUtils.SetLaunchOnStartup(config.LaunchOnStartup);

            Logger.Info("Initialization complete! Waiting for hotkeys...");
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                Logger.Fatal(exception, "Unhandled exception");
            }
            else
            {
                Logger.Fatal("Unhandled exception");
            }
        }

        private static Dictionary<uint, IKeyHook> CreateApplicationToggleKeyHooksFromConfig(Config config, ModifierKeyState modifierKeyState)
        {
            var applicationToggleKeyHooks = new Dictionary<uint, IKeyHook>();
            foreach (var applicationConfig in config.Applications)
            {
                var processName = applicationConfig.Process;
                foreach (var key in applicationConfig.Keys)
                {
                    Logger.Info($"Key {key} set to application {processName}");
                    applicationToggleKeyHooks[key] = new ApplicationToggleKeyHook(modifierKeyState, new RemoteApplication(processName));
                }
            }

            return applicationToggleKeyHooks;
        }
    }
}