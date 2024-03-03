using System.Diagnostics;

namespace CaptainCapslock.Remote
{
    internal class RemoteApplication(string processName)
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public void Toggle()
        {
            // There may be multiple instances of the process, get all main windows of all instances
            var processesIds = Process.GetProcessesByName(processName)
                .Select(process => Convert.ToUInt32(process.Id))
                .ToHashSet();
            var windows = RemoteWindow
                .EnumerateMainWindows()
                .Where(window => processesIds.Contains(window.GetProcessId()));

            if (!windows.Any())
            {
                Logger.Debug($"Application {processName} has no windows, nothing to toggle");
            }

            if (windows.Any(window => window.IsFocused))
            {
                // At least 1 of the application's windows are currently focused, unfocus
                // Unfocus in reverse order of how we focus, so that windows focus/unfocus in same z order
                Logger.Debug($"Unfocusing windows of application {processName}");

                foreach (var window in windows.Reverse())
                {
                    window.Unfocus();
                }
            }
            else
            {
                // None of the application's windows are currenly focused, bring them into focus
                Logger.Debug($"Focusing windows of application {processName}");

                foreach (var window in windows)
                {
                    window.Focus();
                }
            }
        }
    }
}