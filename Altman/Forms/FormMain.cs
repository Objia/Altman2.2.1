﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Altman.Model;
using Altman.Service;
using PluginFramework;

namespace Altman.Forms
{
    public partial class FormMain : Form
    {
        private PluginsImport _pluginsImport;
        private DirectoryCatalog _directoryCatalog;
        private CompositionContainer _container;

        private IHost _host;
        public FormMain()
        {
            InitializeComponent();
            //CheckForIllegalCrossThreadCalls = false;

            //目录初始化
            if (!Directory.Exists("Bin"))
                Directory.CreateDirectory("Bin");
            if (!Directory.Exists("Plugins"))
                Directory.CreateDirectory("Plugins");
            if (!Directory.Exists("Plugins/Config"))
                Directory.CreateDirectory("Plugins/Config");
            if (!Directory.Exists("Languages"))
                Directory.CreateDirectory("Languages");

            /*使用MEF导入插件
            *MEF的使用步骤：
            *1.导出标指定对象[Export];
            *2.在宿主代码中设定[Import];
            *3.组合部件(主要是将宿主中Import和导出Export对象进行引用链接);
            *4.使用导入的对象。*/
            //----导入插件----
            _pluginsImport = new PluginsImport();
            _host = new Host(this);//该接口类实例将作为参数传递给MEF导入类的构造函数，方便导入对象使用
            Compose();
            //----导入插件结束----


            //----数据初始化----
            InitUi.InitCustomShellType();//读取指定目录的CustomShellType文件内容到程序中，并进行相关设置
            InitUi.InitGlobalSetting();//读取指定目录的Setting文件内容到程序中，并进行相关设置
            //----数据初始化结束----


            //----UI处理----
            //tabControl事件绑定
            tabControl1.DoubleClick += tabControl1_DoubleClick;//添加双击Tab选项卡标题则关闭选项卡的事件处理函数
            //设置版本号
            toolStripStatusLabel_productVersion.Text = string.Format("Version:{0}@KeePwn", Assembly.GetExecutingAssembly().GetName().Version);
            //tabControl初始化
            TabCore.Init(this, this.tabControl1);//TabCore类的作用主要是在TabControl中添加新的TabPage
           

            //treenode
            TreeNode treeNodeRoot1;
            treeNodeRoot1 = InitUi.GetCustomShellTypeTree();
            treeNodeRoot1.Name = "ShellType";
            treeNodeRoot1.Text = "ShellType";
            this.treeView_func.Nodes.AddRange(new TreeNode[] { treeNodeRoot1 });           

            //加载Plugins菜单项
            LoadPluginsInUi(_pluginsImport.Plugins.OrderBy(p=>p.PluginSetting.IndexInList).ThenBy(p=>p.PluginInfo.Name));
            //----UI处理结束----

            
            //显示免责声明
            InitUi.InitWelcome();

            //auto load plugins
            AutoLoadPlugins(_pluginsImport.Plugins);
        }

        private void pluginRun_Click(object sender, EventArgs e)
        {
            IPlugin plugin = (IPlugin)(sender as ToolStripMenuItem).Tag;
            if (plugin == null)
                return;
            if (plugin is IControlPlugin)
            {
                UserControl view = (plugin as IControlPlugin).GetUi(new Shell());
                //创建新的tab标签
                //设置标题为FileManager|TargetId
                string title = plugin.PluginInfo.Name;
                TabCore.CreateNewTabPage(title, view);
            }
            else if (plugin is IFormPlugin)
            {
                Form form = (plugin as IFormPlugin).GetUi(new Shell());
                form.Show();
            }
        }
        private void pluginAbout_Click(object sender, EventArgs e)
        {
            IPlugin plugin = (IPlugin)(sender as ToolStripMenuItem).Tag;
            if (plugin == null)
                return;
            string msg = string.Format("Author:{0}\nVersion:{1}\nDescription:{2}\n",
                plugin.PluginInfo.Author,
                plugin.PluginInfo.Version,
                plugin.PluginInfo.Description);
            MessageBox.Show(msg, "About "+plugin.PluginInfo.Name, MessageBoxButtons.OK);
        }

        #region MEF处理
        /// <summary>
        /// 组合部件(添加目录到容器中、并将导出对象引用到Import语句中指定的变量)
        /// </summary>
        private void Compose()
        {
            var catalog = new AggregateCatalog();
			_directoryCatalog = new DirectoryCatalog("Plugins");
			catalog.Catalogs.Add(_directoryCatalog);
            _container = new CompositionContainer(catalog);//添加目录到容器中
            try
            {
                _container.ComposeExportedValue("IHost", _host);//为ImportingConstructor传递参数_host(将_host传递给导出类的构造函数，"IHost"是定义构造函数时指定的契约名)
                _container.ComposeParts(_pluginsImport);//组合部件，实质就是将对象导出到指定的变量（_pluginsImport类中Import语句定义的变量）
            }
            catch (CompositionException compositionException)
            {
                MessageBox.Show(compositionException.ToString());
                _container.Dispose();
            }
        }
        /// <summary>
        /// 卸载插件
        /// </summary>
        /// <param name="plugin"></param>
        /// <returns></returns>
        private bool UnLoadPlugin(IPlugin plugin)
        {
            bool isSuccess = false;
            try
            {
                var batch = new CompositionBatch();
                var part = AttributedModelServices.CreatePart(plugin);
                //var part = batch.AddExportedValue<IPlugin>(plugin);
                //var part2 = _container.GetExportedValues<IPlugin>().First();
                //Lazy<IPlugin> part3 = _container.GetExport<IPlugin>();
                //IPlugin tmp = part3.Value;
                //batch.RemovePart(part);
                batch.AddPart(part);
                _container.Compose(batch);
                //_container.ReleaseExport(part3);
                isSuccess = true;
            }
            catch
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        #endregion

        #region Event
        private void tabControl1_DoubleClick(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text != "Index" && tabControl1.SelectedTab.Text != "ShellManager")
            {
                int index = tabControl1.SelectedIndex;
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                if (index - 1 >= 0)
                    tabControl1.SelectedIndex = index - 1;
            }
        }

        private void treeView_func_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //
        }

                private void Tsmi_Setting_Click(object sender, EventArgs e)
        {
            FormGlobalSetting setting = new FormGlobalSetting();
            setting.ShowDialog();
        }

        private void Tsmi_Wizard_Click(object sender, EventArgs e)
        {
            //启动自定义shelltype
            FormCustomShellTypeWizard wiz = new FormCustomShellTypeWizard();
            wiz.Show();
        }

        private void Tsmi_Listening_Click(object sender, EventArgs e)
        {
            FormListening lisenting = new FormListening();
            lisenting.Show();
        }

        private void Tsmi_about_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

        private void Tsmi_developerMode_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = !Tsmi_developerMode.Checked;
        }

        private void Tsmi_ReloadShellType_Click(object sender, EventArgs e)
        {
            InitUi.InitCustomShellType();
        }

        private void Tsmi_ReloadSetting_Click(object sender, EventArgs e)
        {
            InitUi.InitGlobalSetting();
        }

        private void Tsmi_docs_Click(object sender, EventArgs e)
        {
            string chm = Application.StartupPath + "//Docs//HELP.chm";
            if (File.Exists(chm))
            {
                System.Diagnostics.Process.Start(chm);
            }
            else
            {
                MessageBox.Show("Not find /Docs/help.chm");
            }
        }
        #endregion

        public string MsgInStatusBar 
        {
            get { return toolStripStatusLabel_showMsg.Text; }
            set { toolStripStatusLabel_showMsg.Text = value; }
        }

        public ContextMenuStrip RightMenu
        {
            get { return rightMenu; }
        }

        public PluginsImport PluginsImport
        {
            get { return _pluginsImport; }
        }

        public void RefreshPlugins()
        {
            _directoryCatalog.Refresh();
            try
            {
                _container.ComposeParts(_pluginsImport);
            }
            catch (CompositionException compositionException)
            {
                MessageBox.Show(compositionException.ToString());
                _container.Dispose();
            }
            finally
            {
                LoadPluginsInUi(_pluginsImport.Plugins);
            }
        }

        private void LoadPluginsInUi(IEnumerable<IPlugin> plugins)
        {
            //clear Tsmi_Plugins
            Tsmi_Plugins.DropDownItems.Clear();

            foreach (var plugin in plugins)
            {
                string title = plugin.PluginInfo.Name;

                //plugins in Tsmi_Plugins
                ToolStripMenuItem item = new ToolStripMenuItem(title);
                if (!plugin.PluginSetting.IsNeedShellData)
                {
                    ToolStripMenuItem pluginRun = new ToolStripMenuItem("run");
                    pluginRun.Click += pluginRun_Click;
                    pluginRun.Tag = plugin;
                    item.DropDownItems.Add(pluginRun);
                }
                ToolStripMenuItem pluginAbout = new ToolStripMenuItem("about");
                pluginAbout.Click += pluginAbout_Click;
                pluginAbout.Tag = plugin;
                item.DropDownItems.Add(pluginAbout);
                Tsmi_Plugins.DropDownItems.Add(item);
            }
        }

        private void AutoLoadPlugins(IEnumerable<IPlugin> plugins)
        {
            //IsAutoLoad
            foreach (var plugin in plugins)
            {
                //IsAutoLoad
                if (plugin.PluginSetting.IsAutoLoad)
                {
                    string title = plugin.PluginInfo.Name;
                    if (plugin is IControlPlugin)
                    {
                        UserControl view = (plugin as IControlPlugin).GetUi(new Shell());
                        //创建新的tab标签
                        TabCore.CreateNewTabPage(title, view);
                    }
                    else if (plugin is IFormPlugin)
                    {
                        Form form = (plugin as IFormPlugin).GetUi(new Shell());
                        form.Show();
                    }
                }
            }
        }
    }
}
