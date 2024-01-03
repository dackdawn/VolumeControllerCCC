using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScreenHelper
{
    public partial class ScreenHelper
    {
        public List<ScreenInfo> ScreenInfoList;

        private readonly EnumMonitorsDelegate _monitorEnumProcedure;
        public ScreenHelper()
        {
            _monitorEnumProcedure = HandleEnumMonitor;
            UpdateScreenInfo();
        }

        public void UpdateScreenInfo()
        {
            ScreenInfoList = new List<ScreenInfo>();

            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, _monitorEnumProcedure, IntPtr.Zero);
        }

        /// <summary>
        /// EnumDisplayMonitors的回调函数，接收处理hMonitor
        /// </summary>
        /// <param name="hMonitor"></param>
        /// <param name="hdcMonitor"></param>
        /// <param name="lprcMonitor"></param>
        /// <param name="dwData"></param>
        /// <returns></returns>
        private bool HandleEnumMonitor(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
        {
            HandleMonitorInfo(hMonitor);
            return true;
        }

        /// <summary>
        /// 处理枚举出来的hMonitor，从中提取出显示器的信息
        /// </summary>
        /// <param name="hMonitor"></param>
        private void HandleMonitorInfo(IntPtr hMonitor)
        {
            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(dm);
            MONITORINFOEX monitorinfo = new MONITORINFOEX();
            monitorinfo.cbSize = Marshal.SizeOf(monitorinfo);
            double scale;

            GetMonitorInfo(hMonitor, ref monitorinfo);
            // 显示器逻辑坐标系 工作区和整个区域的RECT
            // monitorinfo.rcWork;
            // monitorinfo.rcMonitor;

            EnumDisplaySettings(monitorinfo.DeviceName, EnumDisplaySettingsMode.ENUM_CURRENT_SETTINGS, ref dm);
            // 显示器物理坐标系 坐标、宽高
            // dm.dmPositionX;
            // dm.dmPositionY;
            // dm.dmPelsWidth;
            // dm.dmPelsHeight;

            // 构建ScreenInfo
            ScreenInfo si = new ScreenInfo();
            // 缩放比例
            scale = (double)dm.dmPelsWidth / (monitorinfo.rcMonitor.right - monitorinfo.rcMonitor.left);
            // 物理显示区域
            si.MonitorPhysics.left = dm.dmPositionX;
            si.MonitorPhysics.top = dm.dmPositionY;
            si.MonitorPhysics.right = dm.dmPositionX + dm.dmPelsWidth;
            si.MonitorPhysics.bottom = dm.dmPositionY + dm.dmPelsHeight;

            // 逻辑显示区域
            si.MonitorLogic = monitorinfo.rcMonitor;

            // 逻辑任务栏区域
            if (RectContains(monitorinfo.rcWork, monitorinfo.rcMonitor.left, monitorinfo.rcWork.top)) {
                // 任务栏在底部、右边、不存在
                if (RectContains(monitorinfo.rcWork, monitorinfo.rcMonitor.right, monitorinfo.rcMonitor.bottom)) {
                    // 任务栏不存在
                    si.TaskBarLogic.left = monitorinfo.rcMonitor.left;
                    si.TaskBarLogic.top = monitorinfo.rcMonitor.top;
                    si.TaskBarLogic.right = monitorinfo.rcMonitor.left;
                    si.TaskBarLogic.bottom = monitorinfo.rcMonitor.top;
                    si.TaskBarDirection = DirectionEnum.None;
                }
                else if (RectContains(monitorinfo.rcWork, monitorinfo.rcMonitor.left, monitorinfo.rcMonitor.bottom)) {
                    // 任务栏在右边
                    si.TaskBarLogic.left = monitorinfo.rcWork.right;
                    si.TaskBarLogic.top = monitorinfo.rcWork.top;
                    si.TaskBarLogic.right = monitorinfo.rcMonitor.right;
                    si.TaskBarLogic.bottom = monitorinfo.rcMonitor.bottom;
                    si.TaskBarDirection = DirectionEnum.Right;
                }
                else {
                    // 任务栏在底部
                    si.TaskBarLogic.left = monitorinfo.rcWork.left;
                    si.TaskBarLogic.top = monitorinfo.rcWork.bottom;
                    si.TaskBarLogic.right = monitorinfo.rcMonitor.right;
                    si.TaskBarLogic.bottom = monitorinfo.rcMonitor.bottom;
                    si.TaskBarDirection = DirectionEnum.Bottom;
                }
            }
            else {
                // 任务栏在左边、顶部
                if (RectContains(monitorinfo.rcWork, monitorinfo.rcMonitor.right, monitorinfo.rcMonitor.top)) {
                    // 任务栏在左边
                    si.TaskBarLogic.left = monitorinfo.rcMonitor.left;
                    si.TaskBarLogic.top = monitorinfo.rcMonitor.top;
                    si.TaskBarLogic.right = monitorinfo.rcWork.left;
                    si.TaskBarLogic.bottom = monitorinfo.rcWork.bottom;
                    si.TaskBarDirection = DirectionEnum.Left;
                }
                else {
                    // 任务栏在顶部
                    si.TaskBarLogic.left = monitorinfo.rcMonitor.left;
                    si.TaskBarLogic.top = monitorinfo.rcMonitor.top;
                    si.TaskBarLogic.right = monitorinfo.rcWork.right;
                    si.TaskBarLogic.bottom = monitorinfo.rcWork.top;
                    si.TaskBarDirection = DirectionEnum.Top;
                }
            }

            // 物理任务栏区域
            // 基本思想是依据逻辑任务栏位置与逻辑显示器位置的相对位置，缩放推测出物理的任务栏位置
            int _height = (int)(scale * (si.TaskBarLogic.bottom - si.TaskBarLogic.top));
            int _width = (int)(scale * (si.TaskBarLogic.right - si.TaskBarLogic.left));
            switch (si.TaskBarDirection) {
                case DirectionEnum.None:
                    si.TaskBarPhysics.left = si.MonitorPhysics.left;
                    si.TaskBarPhysics.top = si.MonitorPhysics.top;
                    si.TaskBarPhysics.right = si.MonitorPhysics.left;
                    si.TaskBarPhysics.bottom = si.MonitorPhysics.top;
                    break;
                case DirectionEnum.Top:
                    si.TaskBarPhysics.left = si.MonitorPhysics.left;
                    si.TaskBarPhysics.top = si.MonitorPhysics.top;
                    si.TaskBarPhysics.right = si.MonitorPhysics.right;
                    si.TaskBarPhysics.bottom = si.MonitorPhysics.top + _height;
                    break;
                case DirectionEnum.Right:
                    si.TaskBarPhysics.left = si.MonitorPhysics.right - _width;
                    si.TaskBarPhysics.top = si.MonitorPhysics.top;
                    si.TaskBarPhysics.right = si.MonitorPhysics.right;
                    si.TaskBarPhysics.bottom = si.MonitorPhysics.bottom;
                    break;
                case DirectionEnum.Bottom:
                    si.TaskBarPhysics.left = si.MonitorPhysics.left;
                    si.TaskBarPhysics.top = si.MonitorPhysics.bottom - _height;
                    si.TaskBarPhysics.right = si.MonitorPhysics.right;
                    si.TaskBarPhysics.bottom = si.MonitorPhysics.bottom;
                    break;
                case DirectionEnum.Left:
                    si.TaskBarPhysics.left = si.MonitorPhysics.left;
                    si.TaskBarPhysics.top = si.MonitorPhysics.top;
                    si.TaskBarPhysics.right = si.MonitorPhysics.left + _width;
                    si.TaskBarPhysics.bottom = si.MonitorPhysics.bottom;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // 创建ScreenInfo完成，插入
            ScreenInfoList.Add(si);
        }
        

        /// <summary>
        /// 判断点是否存在矩形中，包含右下边界
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool RectContains(RECT rect, int x, int y) => rect.left <= x && x <= rect.right && rect.top <= y && y <= rect.bottom;


        /// <summary>
        /// 将位于某个显示器上的一个以物理坐标表示的点转化为逻辑坐标标识
        /// </summary>
        /// <param name="x_in"></param>
        /// <param name="y_in"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int TransformPhysicsToLogic(int x_phy, int y_phy, ref int x_logic, ref int y_logic, int index = -1)
        {
            if (index == -1) {
                foreach (var screenInfoItem in ScreenInfoList) {
                    if (RectContains(screenInfoItem.MonitorPhysics, x_phy, y_phy)) {
                        double scale = (double)(screenInfoItem.MonitorPhysics.right - screenInfoItem.MonitorPhysics.left) /
                                       (screenInfoItem.MonitorLogic.right - screenInfoItem.MonitorLogic.left);
                        int pWidth = x_phy - screenInfoItem.MonitorPhysics.left;
                        int pHeight = y_phy - screenInfoItem.MonitorPhysics.top;
                        x_logic = screenInfoItem.MonitorLogic.left + (int)(pWidth * scale);
                        y_logic = screenInfoItem.MonitorLogic.top + (int)(pHeight * scale);
                        return 0;
                    }
                }
            }
            else {
                var screenInfoItem = ScreenInfoList[index];
                double scale = (double)(screenInfoItem.MonitorLogic.right - screenInfoItem.MonitorLogic.left) /
                               (screenInfoItem.MonitorPhysics.right - screenInfoItem.MonitorPhysics.left);
                int pWidth = x_phy - screenInfoItem.MonitorPhysics.left;
                int pHeight = y_phy - screenInfoItem.MonitorPhysics.top;
                x_logic = screenInfoItem.MonitorLogic.left + (int)(pWidth * scale);
                y_logic = screenInfoItem.MonitorLogic.top + (int)(pHeight * scale);
                return 0;
            }
            
            return 1;
        }
    }
}

