namespace VolumeController
{
    partial class Notify
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Notify));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notifyExitBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.ResizeTaskBar = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.notifyIconContext;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "音量控制";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // notifyIconContext
            // 
            this.notifyIconContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ResizeTaskBar,
            this.notifyExitBtn});
            this.notifyIconContext.Name = "notifyIconContext";
            this.notifyIconContext.Size = new System.Drawing.Size(181, 70);
            // 
            // notifyExitBtn
            // 
            this.notifyExitBtn.Name = "notifyExitBtn";
            this.notifyExitBtn.Size = new System.Drawing.Size(180, 22);
            this.notifyExitBtn.Text = "退出";
            this.notifyExitBtn.Click += new System.EventHandler(this.notifyExitBtn_Click);
            // 
            // ResizeTaskBar
            // 
            this.ResizeTaskBar.Name = "ResizeTaskBar";
            this.ResizeTaskBar.Size = new System.Drawing.Size(180, 22);
            this.ResizeTaskBar.Text = "重新定位";
            this.ResizeTaskBar.Click += new System.EventHandler(this.ResizeTaskBar_Click);
            // 
            // Notify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(44, 26);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Notify";
            this.Text = "Notify";
            this.TopMost = true;
            this.notifyIconContext.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notifyIconContext;
        private System.Windows.Forms.ToolStripMenuItem notifyExitBtn;
        private System.Windows.Forms.ToolStripMenuItem ResizeTaskBar;
    }
}