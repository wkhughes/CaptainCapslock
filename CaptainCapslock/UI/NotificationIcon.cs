namespace CaptainCapslock.UI
{
    internal class NotificationIcon
    {
        private readonly NotifyIcon notifyIcon;
        private Console? console;

        public event Action? ReloadConfig;
        public event Action? OpenConfigFolder;

        public NotificationIcon()
        {
            var icon = new Icon(Path.Combine(AppContext.BaseDirectory, "icon.ico"));

            var contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("Reload Config", null, (sender, e) => ReloadConfig?.Invoke());
            contextMenuStrip.Items.Add("Open Config Folder", null, (sender, e) => OpenConfigFolder?.Invoke());
            contextMenuStrip.Items.Add("-");
            contextMenuStrip.Items.Add("Show Console", null, ShowConsole);
            contextMenuStrip.Items.Add("-");
            contextMenuStrip.Items.Add("Exit", null, Exit);

            notifyIcon = new NotifyIcon
            {
                Text = "Captain Capslock",
                Icon = icon,
                ContextMenuStrip = contextMenuStrip,
                Visible = true
            };

            notifyIcon.DoubleClick += ShowConsole;
        }

        private void ShowConsole(object? sender, EventArgs e)
        {
            if (console == null)
            {
                console = new Console { Visible = true };
                console.FormClosing += (sender, e) => { console = null; };
            }
            else
            {
                console.Activate();
            }
        }

        private void Exit(object? sender, EventArgs e)
        {
            // Clean up resources and exit application
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            Application.Exit();
        }
    }
}