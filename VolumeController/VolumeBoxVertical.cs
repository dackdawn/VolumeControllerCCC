using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using CLRNativeCPP;
using Timer = System.Timers.Timer;

namespace VolumeController
{
    public partial class VolumeBoxVertical : Form
    {
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);

        private SystemVolumeHelper volumeHelper;
        private Timer hideTimer; // 计时器，用来超时自动隐藏音量条

        private Form _parent; // 保存父Form，用来获取主线程更新UI

        public int Offset = 20;

        protected override CreateParams CreateParams {
            get {
                const int WS_EX_APPWINDOW = 0x40000;
                const int WS_EX_TOOLWINDOW = 0x80;
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // 双缓冲
                cp.ExStyle &= ~WS_EX_APPWINDOW; // 不显示在TaskBar
                cp.ExStyle |= WS_EX_TOOLWINDOW; // 不显示在Alt-Tab
                return cp;
            }
        }

        public VolumeBoxVertical()
        {
            _parent = this;
            InitializeComponent();
            Init();
        }

        public VolumeBoxVertical(Form parent)
        {
            _parent = parent;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            volumeHelper = new SystemVolumeHelper();
            hideTimer = new Timer();

            // 设置定时器
            hideTimer.Interval = 3000;
            hideTimer.Enabled = true;
            hideTimer.Elapsed += TimerCallBack;
            hideTimer.AutoReset = false;
            // 绘制音量条背景色
            DrawVolumeBoxCur();
        }

        private void TimerCallBack(object sender, System.Timers.ElapsedEventArgs e)
        {
            Action hideThisForm = this.Hide;
            _parent.Invoke(hideThisForm, null);
        }

        public void UpdateVolume(int X, int Y, bool offset = false)
        {
            // 设置位置
            // todo: 不让音量条超出显示器
            if (offset) {
                SetBounds(X - Bounds.Width - Offset, Y, Bounds.Width, Bounds.Height);
            }
            else {
                SetBounds(X + Offset, Y, Bounds.Width, Bounds.Height);
            }
            DrawVolumeBoxCur();
            Show();
            hideTimer.Stop();
            hideTimer.Start();
        }


        /// <summary>
        /// 填充音量条的前景
        /// </summary>
        private void DrawVolumeBoxCur()
        {
            int widthMax = VolumeBoxCur.Bounds.Width;
            int heightMax = VolumeBoxCur.Bounds.Height;
            int curVolume = GetVolume();
            int width = widthMax;
            int height = (int) (heightMax * (curVolume * 1.0f / 100.0f));
            Bitmap bitmap = new Bitmap(widthMax, heightMax);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.Default;
            // 减1是因为防止越界
            Rectangle rectangleBg = new Rectangle(0, 0, widthMax - 1, heightMax - 1);
            Rectangle rectangleCur = new Rectangle(0, heightMax - height, width - 1, height - 1);
            // 先绘画音量条背景
            FillRoundRectangle(g, rectangleBg, Color.FromArgb(240, 248, 255), (width - 1) / 2);
            // 然后再绘制当前音量
            FillRoundRectangle(g, rectangleCur, Color.FromArgb(64, 158, 255), (width - 1) / 2);
            // 最后绘制边框
            g.DrawPath(new Pen(Color.FromArgb(242, 246, 252)), GetRoundRectangle(rectangleBg, (width - 1) / 2));
            g.Dispose();
            VolumeBoxCur.Image = bitmap;
        }
        private int GetVolume()
        {
            return volumeHelper.get_volume();
        }

        /// <summary>
        /// 填充圆角矩形
        /// </summary>
        /// <param name="g">Graphics 绘图对象</param>
        /// <param name="rectangle">圆角矩形位置</param>
        /// <param name="backColor">颜色</param>
        /// <param name="r">半径</param>
        public static void FillRoundRectangle(Graphics g, Rectangle rectangle, Color backColor, int r)
        {
            Brush b = new SolidBrush(backColor);
            g.FillPath(b, GetRoundRectangle(rectangle, r));
        }

        /// <summary> 
        /// 根据普通矩形得到圆角矩形的路径 
        /// </summary> 
        /// <param name="rectangle">原始矩形</param> 
        /// <param name="r">半径</param> 
        /// <returns>图形路径</returns> 
        private static GraphicsPath GetRoundRectangle(Rectangle rectangle, int r)
        {
            int l = 2 * r;
            // 把圆角矩形分成八段直线、弧的组合，依次加到路径中 
            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(new Point(rectangle.X + r, rectangle.Y), new Point(rectangle.Right - r, rectangle.Y));
            gp.AddArc(new Rectangle(rectangle.Right - l, rectangle.Y, l, l), 270F, 90F);

            gp.AddLine(new Point(rectangle.Right, rectangle.Y + r), new Point(rectangle.Right, rectangle.Bottom - r));
            gp.AddArc(new Rectangle(rectangle.Right - l, rectangle.Bottom - l, l, l), 0F, 90F);

            gp.AddLine(new Point(rectangle.Right - r, rectangle.Bottom), new Point(rectangle.X + r, rectangle.Bottom));
            gp.AddArc(new Rectangle(rectangle.X, rectangle.Bottom - l, l, l), 90F, 90F);

            gp.AddLine(new Point(rectangle.X, rectangle.Bottom - r), new Point(rectangle.X, rectangle.Y + r));
            gp.AddArc(new Rectangle(rectangle.X, rectangle.Y, l, l), 180F, 90F);
            return gp;
        }
    }
}
