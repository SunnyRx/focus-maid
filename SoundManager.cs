using System.Media;
using System.IO;
using System.Windows.Resources;

namespace FocusMaid
{
    public class SoundManager
    {
        private static SoundManager? _instance;
        private readonly Dictionary<string, SoundPlayer> _soundPlayers;
        private const string SOUND_DIRECTORY = "Sounds";
        private const string DEFAULT_SOUND = @"C:\Windows\Media\Windows Notify.wav";

        // 定义声音类型
        public static class SoundType
        {
            public const string FocusComplete = "focus_complete";
            public const string FocusStart = "focus_start";
            // 可以在这里添加更多声音类型
        }

        private SoundManager()
        {
            _soundPlayers = new Dictionary<string, SoundPlayer>();
            InitializeSounds();
        }

        public static SoundManager Instance
        {
            get
            {
                _instance ??= new SoundManager();
                return _instance;
            }
        }

        private void InitializeSounds()
        {
            try
            {
                // 加载所有声音文件
                LoadSound(SoundType.FocusComplete, "focus_complete.wav");
                LoadSound(SoundType.FocusStart, "focus_start.wav");
                // 可以在这里添加更多声音文件的加载
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"初始化声音时出错：{ex.Message}", "声音初始化错误", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }

        private void LoadSound(string soundType, string fileName)
        {
            try
            {
                // 首先尝试从外部 Sounds 文件夹加载
                string externalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", fileName);
                if (File.Exists(externalPath))
                {
                    var player = new SoundPlayer(externalPath);
                    player.LoadAsync();
                    _soundPlayers[soundType] = player;
                    return;
                }

                // 尝试从内嵌资源加载
                try
                {
                    Uri resourceUri = new Uri($"pack://application:,,,/FocusMaid;component/Resources/{fileName}", UriKind.Absolute);
                    StreamResourceInfo resourceStream = System.Windows.Application.GetResourceStream(resourceUri);
                    if (resourceStream != null)
                    {
                        var player = new SoundPlayer(resourceStream.Stream);
                        player.LoadAsync();
                        _soundPlayers[soundType] = player;
                        return;
                    }
                }
                catch
                {
                    // 如果资源加载失败，继续尝试下一个选项
                }

                // 如果都找不到，使用系统默认声音
                var defaultPlayer = new SoundPlayer();
                defaultPlayer.SoundLocation = DEFAULT_SOUND;
                defaultPlayer.LoadAsync();
                _soundPlayers[soundType] = defaultPlayer;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"加载声音文件失败：{fileName}\n错误：{ex.Message}", "声音加载错误",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }

        public void PlaySound(string soundType)
        {
            try
            {
                if (_soundPlayers.TryGetValue(soundType, out var player))
                {
                    player.Play();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"播放声音时出错：{ex.Message}", "声音播放错误",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }

        public void ReloadSounds()
        {
            _soundPlayers.Clear();
            InitializeSounds();
        }

        public bool IsSoundAvailable(string soundType)
        {
            return _soundPlayers.ContainsKey(soundType);
        }
    }
}
