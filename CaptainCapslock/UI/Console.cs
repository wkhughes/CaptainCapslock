namespace CaptainCapslock.UI
{
    public partial class Console : Form
    {
        public Console()
        {
            InitializeComponent();
        }

        private void Console_Load(object sender, EventArgs e)
        {
            NLog.Windows.Forms.RichTextBoxTarget.ReInitializeAllTextboxes(this);
        }
    }
}
