﻿using System;
using System.Collections.Generic;
using System.Xml;

namespace Altman.Logic
{
    /// <summary>
    /// 用于提取shell扩展字符串中信息的类
    /// </summary>
    internal class ShellExtraHandle
    {
        /// <summary>
        /// 创建一个xmlNode
        /// </summary>
        /// <param name="shellExtraString">节点的innerText值</param>
        /// <returns></returns>
        private static XmlNode GetShellExtraXml(string shellExtraString)
        {
            if (string.IsNullOrEmpty(shellExtraString)) return null;
            XmlNode node = null;
            try
            {
                node = new XmlDocument().CreateElement("ExtraSetting");
                node.InnerXml = shellExtraString;
            }
            catch
            {
                throw new Exception("Sorry,the ini is not legal xml.Please edit the ini firstly.");
            }
            return node;
        }
        /// <summary>
        /// 根据shell扩展字符串中的http头部信息生成xml节点类
        /// </summary>
        /// <param name="shellExtraString">shell扩展字符串</param>
        /// <returns></returns>
        public static XmlNode GetHttpHeaderXml(string shellExtraString)
        {
            XmlNode root = GetShellExtraXml(shellExtraString);
            if (root == null) return null;
            return root.SelectSingleNode("/httpHeader") ?? null;
        }
        /// <summary>
        /// 根据shell扩展字符串中的数据库连接字符串标准格式信息生成xml节点类
        /// </summary>
        /// <param name="shellExtraString">shell扩展字符串</param>
        /// <returns></returns>
        public static XmlNode GetSqlConnectionXml(string shellExtraString)
        {
            XmlNode root = GetShellExtraXml(shellExtraString);
            if (root == null) return null;
            return root.SelectSingleNode("/sqlConnection") ?? null;
        }

        /// <summary>
        /// 根据shell扩展字符串中的http头部信息，提取http头部各字段内容，并生成字典
        /// </summary>
        /// <param name="shellExtraString">shell扩展字符串</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetHttpHeaderList(string shellExtraString)
        {
            XmlNode node = GetHttpHeaderXml(shellExtraString);
            Dictionary<string, string> headers = new Dictionary<string, string>();

            if (node == null) return headers;
            XmlNodeList keys = node.SelectNodes("key");
            foreach (XmlNode key in keys)
            {
                if (key.Attributes["name"] == null) continue;
                string name = key.Attributes["name"].Value;
                string value = key.InnerText;
                if (!string.IsNullOrWhiteSpace(name) && !headers.ContainsKey(name))
                {
                    headers.Add(name, value);
                }             
            }
            return headers;
        }
    }
}
