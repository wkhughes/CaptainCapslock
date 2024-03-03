namespace CaptainCapslock.UI
{
    partial class Console
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Console));
            ConsoleText = new RichTextBox();
            SuspendLayout();
            // 
            // ConsoleText
            // 
            ConsoleText.Location = new Point(12, 12);
            ConsoleText.Name = resources.GetString("TextName");
            ConsoleText.ReadOnly = true;
            ConsoleText.Size = new Size(776, 426);
            ConsoleText.TabIndex = 1;
            ConsoleText.Text = "";
            // 
            // Console
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ConsoleText);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = resources.GetString("Name");
            Text = "Captain Capslock Console";
            Load += Console_Load;
            ResumeLayout(false);
        }

        #endregion
        private RichTextBox ConsoleText;
    }
}