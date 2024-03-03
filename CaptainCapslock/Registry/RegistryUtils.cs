using System.Diagnostics;

namespace CaptainCapslock.Registry
{
    internal static class RegistryUtils
    {
        private const string LaunchOnStartupKeyName = @"Software\\Microsoft\Windows\CurrentVersion\Run";

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static void SetLaunchOnStartup(bool shouldLaunchOnStartup)
        {
            var registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(LaunchOnStartupKeyName, true);
            if (registryKey == null)
            {
                Logger.Error($"Failed to find launch on startup registry key in HKCU {LaunchOnStartupKeyName}");
                return;
            }

            var productName = Application.ProductName;
            Debug.Assert(productName != null, "Product name must be set");

            if (shouldLaunchOnStartup)
            {
                registryKey.SetValue(productName, Application.ExecutablePath);
            }
            else if (registryKey.GetValue(productName) != null)
            {
                registryKey.DeleteValue(productName);
            }
        }
    }
}
