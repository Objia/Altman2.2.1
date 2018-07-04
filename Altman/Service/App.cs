﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using PluginFramework;

namespace Altman.Service
{
    /// <summary>
    /// 应用程序类，主要包含应用程序的基本信息（继承IHostApp接口）
    /// </summary>
    public class App:IHostApp
    {
        public Version AppVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public string AppCurrentDir
        {
            get { return Application.StartupPath; }
        }

        public string AppPluginDir
        {
            get { return "Plugins"; }
        }

        public string AppPluginConfigDir
        {
            get { return "Plugins/Config"; }
        }
        public string AppBinDir
        {
            get { return "Bin"; }
        }

        public string AppLanguageDir
        {
            get { return "Languages"; }
        }
    }
}
