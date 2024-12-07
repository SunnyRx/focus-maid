using System.Windows;
using System.Windows.Input;

namespace FocusMaid
{
    public partial class MiniWindow : Window
    {
        private MainWindow _mainWindow;

        public MiniWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            
            // 设置窗口位置为屏幕右上角
            Left = SystemParameters.WorkArea.Width - Width - 20;
            Top = 20;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            // 显示主窗口
            _mainWindow.Show();
            _mainWindow.WindowState = WindowState.Normal;
            // 关闭迷你窗口
            Close();
        }

        public void UpdateCountdown(string time)
        {
            CountdownText.Text = time;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
