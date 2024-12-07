# Focus Maid 专注女仆

![image](https://github.com/user-attachments/assets/5bbce279-19e8-419d-8560-6cae47033995)

[![built with Codeium](https://codeium.com/badges/main)](https://codeium.com)

Focus Maid 是一款简洁而可爱的专注力辅助工具，它能帮助您在工作或学习时保持专注。通过可爱的女仆形象和贴心的功能设计，让您的专注时间更加愉悦和高效。

## ✨ 主要特性

- 🕒 自定义专注时长
- 🪟 迷你窗口模式，随时查看剩余时间
- 🎵 自定义提示音效
- 👀 可爱的女仆形象
- 🖱️ 支持窗口拖拽

## 🚀 快速开始

### 系统要求

- Windows 10 或更高版本
- [.NET 8.0 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)

### 安装步骤

1. 从 [Releases](../../releases) 页面下载最新版本
2. 解压下载的文件到任意位置
3. 运行 `FocusMaid.exe`

### 使用方法

1. 从下拉菜单选择专注时长
2. 点击"开始专注"按钮
3. 专注期间可以点击"最小化"按钮切换到迷你窗口模式
4. 专注结束后会自动播放提示音并显示通知

## 🎨 自定义设置

### 自定义音效

可以通过以下两种方式自定义提示音效：

1. **外部文件方式**（推荐）
   - 在程序运行目录下创建 `Sounds` 文件夹
   - 将自定义音效（WAV格式）放入该文件夹，并按以下命名：
     - `focus_start.wav`: 开始专注时播放
     - `focus_complete.wav`: 专注完成时播放
   - 程序会优先使用外部文件夹中的音效

2. **内置资源方式**（暂不支持）
   - 音效文件会从以下位置按优先级依次查找：
     1. 外部 `Sounds` 文件夹
     2. 内置资源（Resources/）
     3. Windows 默认通知音效您

### 自定义女仆图片

您可以通过以下两种方式自定义女仆图片：

1. **外部文件方式**（暂不支持）
   - 在程序运行目录下创建 `Images` 文件夹
   - 将自定义图片放入该文件夹，并按以下命名：
     - `maid.png`: 默认状态
     - `maid_focusing.png`: 专注状态
     - `maid_complete.png`: 完成状态
   - 程序会优先使用外部文件夹中的图片

2. **内置资源方式**
   - 图片文件会从以下位置按优先级依次查找：
     1. 外部 `Images` 文件夹
     2. 内置资源（Resources/）

## 🛠️ 开发者信息

### 构建环境

- Visual Studio 2022 或 Visual Studio Code
- .NET 8.0 SDK
- Windows 10 或更高版本

### 构建步骤

```bash
git clone https://github.com/SunnyRx/focus-maid.git
cd focus-maid
dotnet restore
dotnet build
```

## 📝 许可证

本项目采用 MIT 许可证 - 详见 [LICENSE](LICENSE) 文件

## 🙏 鸣谢

- [Codeium](https://codeium.com): 本项目在 Codeium AI 的协助下完成开发
- [AivisSpeech](https://aivis-project.com/): 提供了优质的语音合成技术，用于生成女仆语音
