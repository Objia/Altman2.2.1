using System.Windows.Forms;
using Altman.Forms;
using PluginFramework;

namespace Altman.Service
{
    /// <summary>
    /// 界面UI类，主要用于在界面显示部分信息（继承IHostUI接口）
    /// </summary>
    public class Ui : IHostUi
    {
        private FormMain _mainForm;
        public Ui(FormMain mainForm)
        {
            _mainForm = mainForm;
        }

        public void ShowMsgInAppDialog(string msg)
        {
            MessageBox.Show(msg);
        }
        public void ShowMsgInStatusBar(string msg)
        {
            _mainForm.MsgInStatusBar = msg;
        }
        public void CreateNewTabPage(string tabPageName, UserControl control)
        {
            TabCore.CreateNewTabPage(tabPageName, control);
        }
        public ContextMenuStrip GetRightMenu()
        {
            return _mainForm.RightMenu;
        }
    }
}
