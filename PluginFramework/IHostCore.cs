using System.Collections.Generic;
using System.Xml;
using Altman.Model;

namespace PluginFramework
{
    /// <summary>
    /// 一些核心操作，如提交命令、关于ShellType的相关操作、获取Http头、获取ShellSql连接等
    /// </summary>
    public interface IHostCore
    {
        //core
        byte[] SubmitCommand(Shell data, string funcNameXpath, string[] param);
        string GetCustomShellTypeServerCode(string shellTypeName);
        string[] GetCustomShellTypeNameList();
        string[] GetDbNodeFuncCodeNameList(string shellTypeName);
        IEnumerable<IPlugin> GetPlugins();
        XmlNode GetShellHttpHeader(Shell data);
        XmlNode GetShellSqlConnection(Shell data);
    }
}
