using System;
using System.Collections.Generic;

namespace Altman.Logic
{
    internal class CustomShellType
    {
        #region class
        /// <summary>
        /// 表示.tree文件中<mainCodeParam>和<funcCodeParam>标签，以及.func文件中<funcParam>标签的类
        /// </summary>
        public struct ParamStruct
        {
            public string Name;
            public string Location;
            public EncryMode EncryMode;
            public ParamStruct(string name, string location, EncryMode encryMode)
            {
                Name = name;
                Location = location;
                EncryMode = encryMode;
            }
        }
        /// <summary>
        /// 表示.type文件中<basicSetting>标签的类
        /// </summary>
        public class Basic
        {
            public string ShellTypeName;
            public string ServiceExample;
            public ParamStruct MainCodeParam;

            /// <summary>
            /// 该构造函数什么都不做，对象的字段没有初始化
            /// </summary>
            public Basic()
            {
            }

            public Basic(string shellTypeName, string serviceExample, ParamStruct mainCodeParam)
            {
                this.ShellTypeName = shellTypeName;
                this.ServiceExample = serviceExample;
                this.MainCodeParam = mainCodeParam;
            }
        }
        /// <summary>
        /// 表示.type文件中<mainCodeSetting>标签的类
        /// </summary>
        public class MainCode
        {
            public string Item;
            public ParamStruct FuncCodeParam;

            public MainCode()
            {
            }

            public MainCode(string item, ParamStruct funcCodeParam)
            {
                this.Item = item;
                this.FuncCodeParam = funcCodeParam;
            }
        }
        /// <summary>
        /// ShellType目录树末结点类(表示.func文件中<func>标签的类)
        /// </summary>
        public class FuncCode
        {
            public string Name;
            public string Type;
            public string Path;
            public string Item;
            public List<ParamStruct> FuncParams;

            public FuncCode()
            {

            }
            public FuncCode(string funcName, string funcType, string funcPath, string funcItem, List<ParamStruct> funcParams)
            {
                this.Name = funcName;
                this.Type = funcType;
                this.Path = funcPath;
                this.Item = funcItem;
                this.FuncParams = funcParams;
            }
        }
        /// <summary>
        /// ShellType目录树分枝结点类(表示.tree文件中<node>标签的类)
        /// </summary>
        public class TreeInfo
        {
            public string Path;
            public string Type;
            public string Info;

            public TreeInfo()
            {
                
            }
        }
        #endregion

        #region 字段
        private string _shellTypeName;
        /// <summary>
        /// ShellType的名字
        /// </summary>
        public string ShellTypeName
        {
            get { return _shellTypeName; }
            set { _shellTypeName = value; }
        }

        private Basic _basicSetting;
        private MainCode _mainCodeSetting;

        private FuncTreeNode _funcTreeRoot;
        public FuncTreeNode FuncTreeRoot
        {
            get { return _funcTreeRoot; }
        }

        public Basic BasicSetting
        {
            get { return _basicSetting; }
        }
        public MainCode MainCodeSetting
        {
            get { return _mainCodeSetting; }
        }
        #endregion

        #region 构造函数
        public CustomShellType(Basic basicSetting,
                               MainCode mainCodeSetting)
        {
            _shellTypeName = basicSetting.ShellTypeName;
            _basicSetting = basicSetting;
            _mainCodeSetting = mainCodeSetting;

            //初始化方法树
            _funcTreeRoot = new FuncTreeNode(_shellTypeName);
        }
        #endregion

        /// <summary>
        /// 添加新的ShellType目录结点（如phpeval）
        /// </summary>
        /// <param name="nodeXpath"></param>
        /// <returns></returns>
        public FuncTreeNode AddFuncTreeNode(string nodeXpath)
        {
            FuncTreeNode tmp = _funcTreeRoot.FindNodes(nodeXpath);
            //如果节点不存在
            if (tmp == null)
            {
                return _funcTreeRoot.AddNodes(nodeXpath);
            }
            return tmp;
        }
        /// <summary>
        /// 在ShellType类型下添加操作末结点（如WwwRootPathCode）
        /// </summary>
        /// <param name="nodeXpath">指定要在哪个ShellType目录下添加末结点</param>
        /// <param name="funcCode">要添加的操作末结点</param>
        public void AddFuncCode(string nodeXpath, FuncCode funcCode)
        {
            FuncTreeNode tmp = _funcTreeRoot.FindNodes(nodeXpath);
            //如果节点不存在
            if (tmp == null)
            {
                throw new Exception(string.Format("FuncTreeNode:[{0}/{1}] has't been defined", _shellTypeName, nodeXpath));
            }
            //如果操作末结点funcCode已经注册
            if (tmp.Funcs.ContainsKey(funcCode.Name))
            {
                throw new Exception(string.Format("FuncCode:[{0}/{1}/{2}] has been registered", _shellTypeName, nodeXpath, funcCode.Name));
            }
            else
            {
                tmp.Funcs.Add(funcCode.Name, funcCode);
            }
        }
        /// <summary>
        /// 获取指定目录下的末结点
        /// </summary>
        /// <param name="nodeXpath">指定的目录</param>
        /// <param name="funcCodeName">指定的结点名称</param>
        /// <returns></returns>
        public FuncCode GetFuncCode(string nodeXpath, string funcCodeName)
        {
            FuncTreeNode tmp = _funcTreeRoot.FindNodes(nodeXpath);
            //如果节点存在
            if (tmp != null)
            {
                //如果funcCode已经注册
                if (tmp.Funcs.ContainsKey(funcCodeName))
                {
                    return tmp.Funcs[funcCodeName];
                }
            }
            //其他情况均为未注册
            throw new Exception(string.Format("FuncCode:[{0}/{1}/{2}] hasn't been registered", _shellTypeName, nodeXpath, funcCodeName));
        }
    }
}
