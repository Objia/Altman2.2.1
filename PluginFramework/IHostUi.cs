using System.Windows.Forms;

namespace PluginFramework
{
    /// <summary>
    /// 针对界面UI的一些操作（显示信息，获取右键菜单，创建TabPage等）
    /// </summary>
    public interface IHostUi
    {
        //ui
        /// <summary>
        /// 在状态栏中显示信息
        /// </summary>
        /// <param name="msg"></param>
        void ShowMsgInStatusBar(string msg);
        /// <summary>
        /// 在对话框中显示信息
        /// </summary>
        /// <param name="msg"></param>
        void ShowMsgInAppDialog(string msg);
        /// <summary>
        /// 创建新的选项卡页
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="control"></param>
        void CreateNewTabPage(string tabName, UserControl control);
        /// <summary>
        /// 获取右键菜单
        /// </summary>
        /// <returns></returns>
        ContextMenuStrip GetRightMenu();
    }
}
