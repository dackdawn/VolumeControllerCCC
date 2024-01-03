using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using CLRNativeCPP;
using Microsoft.Win32;
using MouseHook;
using ScreenHelper;


namespace VolumeController
{
    internal class Hook
    {
        private SystemVolumeHelper systemVolumeHelper;
        private MouseHookHelper mouseHookHelper;
        private ScreenHelper.ScreenHelper screenHelper;

        private readonly object _taskBarListLock = new object(); // 更新taskBarList的线程锁

        private readonly VolumeBoxVertical _volumeBoxVertical; // 音量条的Form
        private readonly VolumeBoxHorizon _volumeBoxHorizon; // 水平音量条的Form

        readonly private Form _parent; // 存储主Form对象，用来获取主线程更新UI

        private int lastClick; // 存储上一次左键单击的时间戳
        private const int TimeInterval = 300; // 两次单击之间间隔n毫秒视为双击
        private bool isDrawing; // 记录当前是否双击并按住
        private Point lastPoint; // 记录双击并按住时的鼠标坐标
        private int lastVolume; // 记录双击并按住时的音量

        public Hook(Form parent)
        {
            _parent = parent;
            systemVolumeHelper = new SystemVolumeHelper();
            mouseHookHelper = new MouseHookHelper();
            screenHelper = new ScreenHelper.ScreenHelper();
            _volumeBoxVertical = new VolumeBoxVertical(parent);
            _volumeBoxHorizon = new VolumeBoxHorizon(parent);
            lastClick = 0;
            isDrawing = false;
            lastPoint = new Point(0, 0);
            lastVolume = 0;
            Init();
        }

        ~Hook()
        {
            StopMouseHook();
            StopSystemPropertyHook();
        }

        private void Init()
        {
            // 添加监听分辨率调整事件
            StartSystemPropertyHook();

            // 监听鼠标事件
            StartHook();
        }

        /// <summary>
        /// 更新Screen信息
        /// </summary>
        public void ManualUpdateScreen()
        {
            GetTaskBarArea();
        }

        /// <summary>
        /// 鼠标事件回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">鼠标事件参数，包括坐标、按键类型</param>
        private void MouseEventCallBack(object sender, MouseData e)
        {
            new Action<MouseData>(ResponseMouseEvent).BeginInvoke(e, null, null);
        }

        /// <summary>
        /// 处理鼠标事件
        /// </summary>
        /// <param name="e">鼠标信息</param>
        private void ResponseMouseEvent(MouseData e)
        {
            var screenIndex = -1;
            if (CheckInTaskBar(e.X, e.Y, ref screenIndex)) {
                switch (e.Button) {
                    case MouseClickEvents.Wheel:
                        // 滚轮
                        var delta = (short) (e.Delta >> 16);
                        var step = delta / 120;
                        systemVolumeHelper.set_volume(systemVolumeHelper.get_volume() + step * 2); // step有正负，所以直接加即可
                        // 显示音量条
                        ShowVolumeBox(e.X, e.Y, screenIndex);
                        break;
                    case MouseClickEvents.LeftDown:
                        if (e.Time - lastClick <= TimeInterval) {
                            // 视为双击事件
                            isDrawing = true;
                            lastPoint.X = e.X;
                            lastPoint.Y = e.Y;
                            lastVolume = systemVolumeHelper.get_volume();
                        }
                        break;
                    case MouseClickEvents.LeftUp:
                        isDrawing = false;
                        lastClick = e.Time;
                        break;
                    case MouseClickEvents.Move:
                        if (isDrawing) {
                            // todo: 节流执行此函数
                            SetVolumeByDraw(e.X, e.Y, screenIndex);
                        }
                        break;
                }
            }
        }

        private void SetVolumeByDraw(int X, int Y, int screenIndex)
        {
            // 计算拖动距离，半个屏幕的长度（宽度）为全过程
            int total, curDelta;

            lock (_taskBarListLock) {
                var screenItem = screenHelper.ScreenInfoList[screenIndex];
                switch (screenItem.TaskBarDirection) {
                    case DirectionEnum.Top:
                    case DirectionEnum.Bottom:
                        total = (screenItem.TaskBarPhysics.right - screenItem.TaskBarPhysics.left) / 2;
                        curDelta = X - lastPoint.X;
                        break;
                    case DirectionEnum.Left:
                    case DirectionEnum.Right:
                        total = (screenItem.TaskBarPhysics.bottom - screenItem.TaskBarPhysics.top) / 2;
                        curDelta = lastPoint.Y - Y;
                        break;
                    default:
                        return;
                }
            }
            systemVolumeHelper.set_volume(lastVolume + (100 * curDelta / total));

            // 显示音量条
            ShowVolumeBox(lastPoint.X, lastPoint.Y, screenIndex);

        }

        /// <summary>
        /// 显示音量条
        /// </summary>
        /// <param name="X">物理坐标系下的X坐标</param>
        /// <param name="Y">物理坐标系下的Y坐标</param>
        /// <param name="screenIndex">显示屏下标</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void ShowVolumeBox(int X, int Y, int screenIndex)
        {
            bool offset = false, horizon = false;
            var screenList = screenHelper.ScreenInfoList;
            lock (_taskBarListLock) {
                switch (screenList[screenIndex].TaskBarDirection) {
                    case DirectionEnum.None:
                        break;
                    case DirectionEnum.Top:
                        Y = screenList[screenIndex].TaskBarPhysics.bottom;
                        horizon = true;
                        break;
                    case DirectionEnum.Right:
                        X = screenList[screenIndex].TaskBarPhysics.left;
                        offset = true; // X的值应该为left - 音量条宽度
                        break;
                    case DirectionEnum.Bottom:
                        Y = screenList[screenIndex].TaskBarPhysics.top;
                        offset = true; // Y的值应该为top - 音量条宽度
                        horizon = true;
                        break;
                    case DirectionEnum.Left:
                        X = screenList[screenIndex].TaskBarPhysics.right;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                screenHelper.TransformPhysicsToLogic(X, Y, ref X, ref Y, screenIndex);
            }

            if (horizon) {
                void UpdateVolumeBox() => _volumeBoxHorizon.UpdateVolume(X, Y, offset);
                _parent.Invoke((Action)UpdateVolumeBox, null);
            }
            else {
                void UpdateVolumeBox() => _volumeBoxVertical.UpdateVolume(X, Y, offset);
                _parent.Invoke((Action)UpdateVolumeBox, null);
            }
        }

        /// <summary>
        /// 检查点是否位于任务栏内
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        private bool CheckInTaskBar(int X, int Y, ref int screenIndex)
        {
            lock (_taskBarListLock) {
                var screenList = screenHelper.ScreenInfoList;
                for (int i = screenList.Count - 1; i >= 0; i--) {
                    if (ScreenHelper.ScreenHelper.RectContains(screenList[i].TaskBarPhysics, X, Y)) {
                        screenIndex = i;
                        return true;
                    }
                }
            }

            screenIndex = -1;
            return false;
        }
        
        /// <summary>
        /// 给回调事件链增加回调函数
        /// </summary>
        private void StartHook()
        {
            mouseHookHelper.RegisterEvent(MouseEventCallBack);
            mouseHookHelper.Start();
        }

        /// <summary>
        /// 停止鼠标监听
        /// </summary>
        private void StopMouseHook()
        {
            mouseHookHelper.UnRegisterEvent(MouseEventCallBack);
            mouseHookHelper.Stop();
        }

        private void StartSystemPropertyHook()
        {
            SystemEvents.DisplaySettingsChanged += UpdateTaskBarArea;
        }

        private void StopSystemPropertyHook()
        {
            SystemEvents.DisplaySettingsChanged -= UpdateTaskBarArea;
        }

        /// <summary>
        /// 系统调整参数的回调，用以更新WorkArea
        /// </summary>
        private void UpdateTaskBarArea(object sender, EventArgs e)
        {
            Action action = GetTaskBarArea;
            action.BeginInvoke(null, null);
        }

        /// <summary>
        /// 获取所有屏幕的任务栏区域
        /// </summary>
        private void GetTaskBarArea()
        {
            lock (_taskBarListLock) {
                screenHelper.UpdateScreenInfo();
            }
        }
        
    }
}
