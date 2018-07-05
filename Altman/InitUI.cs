using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Altman.Forms;
using Altman.Logic;
using Altman.Setting;

namespace Altman
{
    /// <summary>
    /// 界面UI初始化类
    /// </summary>
    internal class InitUi
    {
        public static void InitCustomShellType()
        {
            try
            {
                //初始化CustomShellType
                InitWorker.RegisterCustomShellType();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void InitGlobalSetting()
        {
            try
            {
                //初始化GlobalSetting
                InitWorker.InitGlobalSetting();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void InitWelcome()
        {
            try
            {
                //获取setting
                Setting.Setting tmp = (Setting.Setting)GlobalSetting.Setting;
                if (tmp.IsShowDisclaimer)
                {
                    Welcome w = new Welcome(tmp);
                    w.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /*********************************************
         * 关于自定义CustomShellType文件的分析
         * CustomShellType文件主要用于自定义服务器端的一句话木马、相应的服务器端执行的Web语句、相应的数据库连接和操作语句。
         * 分为三类文件：.tree文件、.type文件、.func文件
         * 
         * .tree文件：_func.tree
         * 文件指定ShellType的目录结构
         * 例如：          phpeval         
         *         ___________|_________________  
         *        |           |                 |
         *      Cmder       FileManager     DBManager
         *     _________________________________|________________________________________________________
         *    |       |           |             |                |                |           |          |
         * access   mssql   access_oledb    mssql_oledb     mysql_oledb     oracle_oledb    mysql   mssql_sqlsrv
         * 
         * 
         * .func文件：phpEval_BuiltIn.func、phpEval_DB.func
         * phpEval_BuiltIn.func文件指定Cmder和FileManager目录下的末节点，包含执行命令行和文件操作的相关语句
         * phpEval_DB.func文件指定access、mssql、access_oledb、mssql_oledb、mysql_oledb、oracle_oledb、mysql、mssql_sqlsrv目录下的末节点，包含执行数据库操作的相关语句
         * 
         * 
         * .type文件：phpEval.type
         * 文件指定服务器端的一句话木马类型 
         * ********************************************/
        /// <summary>
        /// 获取CustomShellType名字列表
        /// </summary>
        public static string[] GetCustomShellTypeNameList()
        {
            return CustomShellTypeProvider.ShellTypeStyleContainer.Keys.ToArray();
        }
        /// <summary>
        /// 获取CustomShellType节点DbManager的子节点名字列表
        /// </summary>
        /// <param name="shellTypeName"></param>
        /// <returns></returns>
        public static string[] GetDbNodeFuncCodeNameList(string shellTypeName)
        {
            List<string> funcCodeNameList = new List<string>();
            if (CustomShellTypeProvider.ShellTypeStyleContainer.ContainsKey(shellTypeName))
            {
                CustomShellType shelltype = CustomShellTypeProvider.ShellTypeStyleContainer[shellTypeName];
                FuncTreeNode dbNode = shelltype.FuncTreeRoot.FindNodes("/DbManager");
                if (dbNode != null)
                {
                    foreach (var child in dbNode.Nodes)
                    {
                        funcCodeNameList.Add(child.Value.Info);
                    }
                }
            }
            return funcCodeNameList.ToArray();
        }
        /// <summary>
        /// 获取指定ShellType的serviceExample一句话木马语句
        /// </summary>
        /// <param name="shellTypeName"></param>
        /// <returns></returns>
        public static string GetCustomShellTypeServerCode(string shellTypeName)
        {
            if (CustomShellTypeProvider.ShellTypeStyleContainer.ContainsKey(shellTypeName))
            {
                CustomShellType shelltype = CustomShellTypeProvider.ShellTypeStyleContainer[shellTypeName];
                return shelltype.BasicSetting.ServiceExample;
            }
            return null;
        }

        /// <summary>
        /// 将节点添加到目录树中
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static TreeNode GetTreeNodes(FuncTreeNode node)
        {
            TreeNode tree = new TreeNode(node.Name);
            foreach (var func in node.Funcs.Keys)
            {
                tree.Nodes.Add(func);
            }
            if (node.Nodes.Count > 0)
            {
                foreach (var child in node.Nodes)
                {
                    tree.Nodes.Add(GetTreeNodes(child.Value));
                }
            }
            return tree;
        }
        /// <summary>
        /// 绘制CustomShellType目录树
        /// </summary>
        /// <returns></returns>
        public static TreeNode GetCustomShellTypeTree()
        {
            TreeNode tree = new TreeNode();
            foreach (var shelltype in CustomShellTypeProvider.ShellTypeStyleContainer)
            {
                tree.Nodes.Add(GetTreeNodes(shelltype.Value.FuncTreeRoot));
            }
            return tree;
        }
    }
}
