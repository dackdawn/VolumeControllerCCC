using System.Drawing;

namespace MouseHook
{
    public class MouseData
    {
        public MouseData(MouseClickEvents button, int x, int y, int delta, int time)
        {
            Button = button;
            X = x;
            Y = y;
            Delta = delta;
            Time = time;
        }

        public MouseClickEvents Button { get; }

        public int X { get; }

        public int Y { get; }

        public int Delta { get; }

        public int Time { get; }

        public Point Location => new Point(this.X, this.Y);
    }
}
