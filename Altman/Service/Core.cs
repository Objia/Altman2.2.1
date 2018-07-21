using System.Collections.Generic;
using System.Xml;
using Altman.Forms;
using Altman.Logic;
using Altman.Model;
using Altman.Web;
using PluginFramework;

namespace Altman.Service
{
    /// <summary>
    /// 核心类，主要用于通过Post方法提交Web命令等核心操作（继承IHostCore接口）
    /// </summary>
    public class Core : IHostCore
    {
        private FormMain _mainForm;
        public Core(FormMain mainForm)
        {
            _mainForm = mainForm;
        }

        /// <summary>
        /// 提交命令
        /// </summary>
        /// <param name="data">shell类</param>
        /// <param name="funcNameXpath">需提交命令的路径（/cmder/readfile）</param>
        /// <param name="param">数据库连接参数组</param>
        public byte[] SubmitCommand(Shell data, string funcNameXpath, string[] param)
        {
            CustomShellType shellType = CustomShellTypeProvider.GetShellType(data.ShellType);
            CustomCommandCode customCommandCode = new CustomCommandCode(shellType, data.ShellPwd);
            Dictionary<string, string> commandCode = customCommandCode.GetCode(funcNameXpath, param);

            ShellExtra shellExtra = new ShellExtra(data.ShellExtraString);
            HttpClient httpClient = new HttpClient(shellExtra.HttpHeader);
            return httpClient.SubmitCommandByPost(data.ShellUrl, commandCode);
        }
        public byte[] SubmitCommand(Shell data,
                                string funcNameXpath, string[] param,
                                bool isBindUploadProgressChangedEvent,
                                bool isBindDownloadProgressChangedEvent)
        {
            CustomShellType shellType = CustomShellTypeProvider.GetShellType(data.ShellType);
            CustomCommandCode customCommandCode = new CustomCommandCode(shellType, data.ShellPwd);
            Dictionary<string, string> commandCode = customCommandCode.GetCode(funcNameXpath, param);
            HttpClient httpClient = new HttpClient();
            //if (isBindUploadProgressChangedEvent)
            //    httpClient.UploadFileProgressChangedToDo += httpClient_UploadFileProgressChangedToDo;
            //if (isBindDownloadProgressChangedEvent)
            //    httpClient.DownloadFileProgressChangedToDo += httpClient_DownloadFileProgressChangedToDo;
            return httpClient.SubmitCommandByPost(data.ShellUrl, commandCode);
        }

        /// <summary>
        /// 获取指定ShellType的<serviceExample></serviceExample>一句话木马语句
        /// </summary>
        /// <param name="shellTypeName"></param>
        /// <returns></returns>
        public string GetCustomShellTypeServerCode(string shellTypeName)
        {
            return InitUi.GetCustomShellTypeServerCode(shellTypeName);
        }

        /// <summary>
        /// 获取所有自定义ShellType名称并生成数组
        /// </summary>
        /// <returns></returns>
        public string[] GetCustomShellTypeNameList()
        {
            return InitUi.GetCustomShellTypeNameList();
        }
        /// <summary>
        /// 获取CustomShellType节点DbManager的子节点名字列表
        /// </summary>
        /// <param name="shellTypeName"></param>
        /// <returns></returns>
        public string[] GetDbNodeFuncCodeNameList(string shellTypeName)
        {
            return InitUi.GetDbNodeFuncCodeNameList(shellTypeName);
        }
        /// <summary>
        /// 获取所有插件
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IPlugin> GetPlugins()
        {
            if (_mainForm.PluginsImport == null)
            {
                return null;
            }
            return _mainForm.PluginsImport.Plugins;
        }

        /// <summary>
        /// 根据shell扩展字符串中的http头部信息生成xml节点类
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public XmlNode GetShellHttpHeader(Shell data)
        {
            return ShellExtraHandle.GetHttpHeaderXml(data.ShellExtraString);
        }

        /// <summary>
        /// 根据shell扩展字符串中的数据库连接字符串标准格式信息生成xml节点类
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public XmlNode GetShellSqlConnection(Shell data)
        {
            return ShellExtraHandle.GetSqlConnectionXml(data.ShellExtraString);
        }
    }
}
