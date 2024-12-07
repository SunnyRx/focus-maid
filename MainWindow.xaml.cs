using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace FocusMaid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer? _timer;
        private DateTime _endTime;
        private bool _isFocusing = false;
        private MiniWindow? _miniWindow;

        public MainWindow()
        {
            InitializeComponent();

            // 加载默认女仆图片
            LoadMaidImage("default");

            // 禁用最小化按钮（只有在专注时才能最小化）
            MinimizeButton.IsEnabled = false;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isFocusing)
            {
                // 创建并显示迷你窗口
                _miniWindow = new MiniWindow(this);
                _miniWindow.Show();
                // 更新迷你窗口的倒计时
                UpdateMiniWindowCountdown();
                // 隐藏主窗口
                Hide();
            }
        }

        private void UpdateMiniWindowCountdown()
        {
            if (_miniWindow != null && _isFocusing)
            {
                TimeSpan remainingTime = _endTime - DateTime.Now;
                if (remainingTime.TotalSeconds > 0)
                {
                    _miniWindow.UpdateCountdown($"{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}");
                }
            }
        }

        private void LoadMaidImage(string state)
        {
            string imagePath = state switch
            {
                "focusing" => "pack://application:,,,/FocusMaid;component/Resources/maid_focusing.png",
                "complete" => "pack://application:,,,/FocusMaid;component/Resources/maid_complete.png",
                _ => "pack://application:,,,/FocusMaid;component/Resources/maid.png"
            };

            try
            {
                MaidImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            }
            catch (Exception)
            {
                MessageBox.Show($"无法加载图片：{imagePath}", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (TimerComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                if (int.TryParse(selectedItem.Tag?.ToString(), out int minutes))
                {
                    StartFocusMode(minutes);
                }
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isFocusing)
            {
                StopFocusMode();
            }
        }

        private void StartFocusMode(int minutes)
        {
            // 设置结束时间
            _endTime = DateTime.Now.AddMinutes(minutes);

            // 初始化计时器
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick!;
            _timer.Start();

            // 更新UI状态
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            MinimizeButton.IsEnabled = true;
            TimerComboBox.IsEnabled = false;
            _isFocusing = true;

            // 更新女仆图片状态
            LoadMaidImage("focusing");

            // 播放开始专注音效
            SoundManager.Instance.PlaySound(SoundManager.SoundType.FocusStart);
        }

        private void StopFocusMode()
        {
            // 停止计时器
            _timer?.Stop();

            // 关闭迷你窗口
            if (_miniWindow != null)
            {
                _miniWindow.Close();
                _miniWindow = null;
                Show();
            }

            // 重置UI
            CountdownText.Text = "00:00";
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            MinimizeButton.IsEnabled = false;
            TimerComboBox.IsEnabled = true;
            _isFocusing = false;

            // 恢复默认女仆图片
            LoadMaidImage("default");
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeSpan remainingTime = _endTime - DateTime.Now;

            if (remainingTime.TotalSeconds > 0)
            {
                CountdownText.Text = $"{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
                // 同时更新迷你窗口的倒计时
                UpdateMiniWindowCountdown();
            }
            else
            {
                // 停止计时器
                _timer?.Stop();

                // 播放完成提示音
                SoundManager.Instance.PlaySound(SoundManager.SoundType.FocusComplete);

                // 关闭迷你窗口
                if (_miniWindow != null)
                {
                    _miniWindow.Close();
                    _miniWindow = null;
                    Show();
                }

                // 重置UI
                CountdownText.Text = "00:00";
                StartButton.IsEnabled = true;
                StopButton.IsEnabled = false;
                MinimizeButton.IsEnabled = false;
                TimerComboBox.IsEnabled = true;
                _isFocusing = false;

                // 显示完成状态的女仆图片
                LoadMaidImage("complete");

                // 显示完成提示
                MessageBox.Show("专注时间已结束！", "专注女仆", MessageBoxButton.OK, MessageBoxImage.Information);

                // 延迟3秒后恢复默认图片
                var delayTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(3)
                };
                delayTimer.Tick += (s, args) =>
                {
                    LoadMaidImage("default");
                    delayTimer.Stop();
                };
                delayTimer.Start();
            }
        }
    }
}