# 3D打印进丝监控插件
为Repetier-Host开发的，用C#编写的3D打印进丝监控插件。下位机为 Intel Genuino 101 和 旋转编码器的组合。

编译要求：
1. Repetier-Host 2.0.5 或更新版本.
2. Opentk library (VS里的NuGet可以下载).
3. Visual Studio 2013 或更新版本，并有.NET 4.0支持.

准备工作：
下载源码，将plugin工程添加进VS中，设置引用项为"RepetierHostExtender.dll"（在 RepetierHost安装目录中），并把输出目录改至"RepetierHost安装目录/plugin/插件名"下。

Arduino 部分：
根据版型和引脚进行适当的修改，上传即可。




3D printer feeding moniter plugin.

A 3D printer feeding moniter plugin for Repetier-Host, writed in C#, using Arduino and rotary encoder to check feeding status.
Buliding requirements:
1. Repetier-Host 2.0.5 or newer
2. Opentk library (can be found on NuGet)
3. Visual Studio 2013 or newer with at least .NET 4.0

Preparation:
Add plugin project to Visual Studio and add reference to "RepetierHostExtender.dll" (should be found in RepetierHost installation directory), do not forget to set your own output path to "RepetierHost-installation-directory\plugin\$PLUGIN_NANE$"

Arduino part:
Make some change to Arduino project based on your Arduino borad and upload it to Arduino.
