using System;
using System.Collections.Generic;
using System.Media;
using System.IO;

namespace FocusMaid
{
    public class SoundManager
    {
        private static SoundManager? _instance;
        private readonly Dictionary<string, SoundPlayer> _soundPlayers;
        private const string SOUND_DIRECTORY = "Sounds";

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
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string soundDirectory = Path.Combine(baseDirectory, SOUND_DIRECTORY);

                // 确保声音目录存在
                if (!Directory.Exists(soundDirectory))
                {
                    Directory.CreateDirectory(soundDirectory);
                }

                // 加载所有声音文件
                LoadSound(SoundType.FocusComplete, Path.Combine(soundDirectory, "focus_complete.wav"));
                LoadSound(SoundType.FocusStart, Path.Combine(soundDirectory, "focus_start.wav"));
                // 可以在这里添加更多声音文件的加载
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"初始化声音时出错：{ex.Message}", "声音初始化错误", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }

        private void LoadSound(string soundType, string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var player = new SoundPlayer(filePath);
                    player.LoadAsync(); // 异步加载声音文件
                    _soundPlayers[soundType] = player;
                }
                else
                {
                    // 如果找不到自定义声音文件，使用系统默认声音
                    var player = new SoundPlayer();
                    player.SoundLocation = @"C:\Windows\Media\Windows Notify.wav";
                    player.LoadAsync();
                    _soundPlayers[soundType] = player;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"加载声音文件失败：{filePath}\n错误：{ex.Message}", "声音加载错误",
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
