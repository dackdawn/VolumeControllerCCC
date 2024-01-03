using System;
using System.Windows.Forms;
using CLRNativeCPP;

namespace VolumeController
{
    public partial class Notify : Form
    {
        private Hook hookHandle;

        public Notify()
        {
            InitializeComponent();
            hookHandle = new Hook(this);
        }

        protected override CreateParams CreateParams {
            get {
                Visible = false;
                const int WS_EX_APPWINDOW = 0x40000;
                const int WS_EX_TOOLWINDOW = 0x80;
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // 双缓冲
                cp.ExStyle &= ~WS_EX_APPWINDOW; // 不显示在TaskBar
                cp.ExStyle |= WS_EX_TOOLWINDOW; // 不显示在Alt-Tab
                return cp;
            }
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            SystemVolumeHelper svh = new SystemVolumeHelper();
            var tarVolume = svh.get_volume() + 2;
            svh.set_volume(tarVolume);
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }

        private void notifyExitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ResizeTaskBar_Click(object sender, EventArgs e)
        {
            hookHandle.ManualUpdateScreen();
        }
    }
}
