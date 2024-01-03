namespace VolumeController
{
    partial class VolumeBoxHorizon
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
            this.VolumeBoxCur.Location = new System.Drawing.Point(12, 2);
            this.VolumeBoxCur.Name = "VolumeBoxCur";
            this.VolumeBoxCur.Size = new System.Drawing.Size(231, 33);
            this.VolumeBoxCur.TabIndex = 0;
            this.VolumeBoxCur.TabStop = false;
            // 
            // VolumeBoxHorizon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(263, 37);
            this.Controls.Add(this.VolumeBoxCur);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VolumeBoxHorizon";
            this.Text = "VolumeBoxHorizon";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.White;
            ((System.ComponentModel.ISupportInitialize)(this.VolumeBoxCur)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox VolumeBoxCur;
    }
}