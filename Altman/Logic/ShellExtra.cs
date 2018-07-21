using System.Net;
using System.Xml;

namespace Altman.Logic
{
    /// <summary>
    /// 保存和提取shell扩展字符串（包括指定http头部字段和数据库连接字符串格式）的相关信息（调用了ShellExtraHandle类的方法）
    /// </summary>
    internal class ShellExtra
    {
        private string _shellExtraString;
        public ShellExtra(string shellExtraString)
        {
            this._shellExtraString = shellExtraString;
        }

        /// <summary>
        /// 提取shell扩展字符串中的http头部字段信息，并生成xml节点
        /// </summary>
        public XmlNode HttpHeaderXml
        {
            get { return ShellExtraHandle.GetHttpHeaderXml(_shellExtraString);}
        }

        /// <summary>
        /// 提取shell扩展字符串中的数据库连接信息，并生成xml节点
        /// </summary>
        public XmlNode SqlConnectionXml
        {
            get { return ShellExtraHandle.GetSqlConnectionXml(_shellExtraString); }
        }

        /// <summary>
        /// shell扩展字符串中http头部字段信息
        /// </summary>
        public WebHeaderCollection HttpHeader
        {
            get
            {
                WebHeaderCollection header = new WebHeaderCollection();
                foreach (var i in ShellExtraHandle.GetHttpHeaderList(_shellExtraString))
                {
                    header.Add(i.Key, i.Value);
                }
                return header;
            }
        }
    }
}