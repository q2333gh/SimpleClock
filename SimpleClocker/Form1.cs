using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleClocker {

  public partial class Form1 : Form {

    // 声明一个定时器对象
    private readonly Timer timer;

    public Form1() {
      InitializeComponent();
      //this.Opacity = 0.9;
      ControlBox = false;
      //不显示最上面最小化按钮的一行
      //FormBorderStyle = FormBorderStyle.None;
      AutoScaleMode = AutoScaleMode.Font;
      // 初始化定时器对象
      timer = new Timer();
      // 设置定时器的间隔为1000毫秒（即一秒）
      timer.Interval = 1000;
      // 设置定时器的触发事件为Tick方法
      timer.Tick += Tick;
      // 启动定时器
      timer.Start();
      FormBorderStyle = FormBorderStyle.None;
      TopMost = true;
      //EnableBlur();
    }

    private void Tick(object sender, EventArgs e) {
      label3.Text = DateTime.Now.ToString("HH:mm:ss\ndddd\nMM/dd/yyyy");

    }

    private void Label1_Click(object sender, EventArgs e) {

    }


    private void Form1_Load_1(object sender, EventArgs e) { 
    }

    private void Label3_Click(object sender, EventArgs e) {

    }

    #region 无边框窗体移动

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();
    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    protected override void OnMouseMove(MouseEventArgs e) {
      base.OnMouseMove(e);
      if (e.Button == MouseButtons.Left) {
        //这里一定要判断鼠标左键按下状态，否则会出现一个很奇葩的BUG，不信邪可以试一下~~
        ReleaseCapture();
        SendMessage(Handle, 0x00A1, 2, 0);
      }
    }


    #endregion


    #region 无边框窗体移动
    [DllImport("user32.dll")]
    internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData {
      public WindowCompositionAttribute Attribute;
      public IntPtr Data;
      public int SizeOfData;
    }

    internal enum WindowCompositionAttribute {
      WCA_ACCENT_POLICY = 19
    }

    internal enum AccentState {
      ACCENT_DISABLED = 0,
      ACCENT_ENABLE_GRADIENT = 1,
      ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
      ACCENT_ENABLE_BLURBEHIND = 3,
      ACCENT_INVALID_STATE = 4
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy {
      public AccentState AccentState;
      public int AccentFlags;
      public int GradientColor;
      public int AnimationId;
    }

    internal void EnableBlur() {
      var accent = new AccentPolicy();
      var accentStructSize = Marshal.SizeOf(accent);
      accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

      var accentPtr = Marshal.AllocHGlobal(accentStructSize);
      Marshal.StructureToPtr(accent, accentPtr, false);

      var data = new WindowCompositionAttributeData {
        Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
        SizeOfData = accentStructSize,
        Data = accentPtr
      };

      SetWindowCompositionAttribute(Handle, ref data);

      Marshal.FreeHGlobal(accentPtr);
    }

    #endregion
    //毛玻璃效果





  }

}
