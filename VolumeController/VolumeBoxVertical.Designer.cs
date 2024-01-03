namespace VolumeController
{
    partial class VolumeBoxVertical
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
            if (disposing && (components != null)) {
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
            this.VolumeBoxCur = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeBoxCur)).BeginInit();
            this.SuspendLayout();
            // 
            // VolumeBoxCur
            // 
            this.VolumeBoxCur.BackColor = System.Drawing.Color.White;
            this.VolumeBoxCur.Location = new System.Drawing.Point(2, 12);
            this.VolumeBoxCur.Name = "VolumeBoxCur";
            this.VolumeBoxCur.Size = new System.Drawing.Size(33, 231);
            this.VolumeBoxCur.TabIndex = 2;
            this.VolumeBoxCur.TabStop = false;
            // 
            // VolumeBoxVertical
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(37, 263);
            this.Controls.Add(this.VolumeBoxCur);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VolumeBoxVertical";
            this.Text = "VolumeBox";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.VolumeBoxCur)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox VolumeBoxCur;
    }
}