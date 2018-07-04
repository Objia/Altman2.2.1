using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginFramework
{
    /// <summary>
    /// 包含程序的一些基本信息，如版本、程序目录、插件目录、语言目录等。
    /// </summary>
    public interface IHostApp
    {
        Version AppVersion { get; }

        string AppCurrentDir { get; }
        string AppPluginDir { get; }
        string AppPluginConfigDir { get; }
        string AppBinDir { get; }
        string AppLanguageDir { get; }
    }
}
