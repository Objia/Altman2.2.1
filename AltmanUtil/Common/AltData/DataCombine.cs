using System;
using System.Collections.Specialized;
using System.Text;

namespace Altman.Common.AltData
{
    public class DataCombine
    {
        private NameValueCollection _items;
        private NameValueCollection _mainCodeItems;
        private NameValueCollection _funcCodeItems;
        private NameValueCollection _funcParmaItems;
        public DataCombine()
        {
            _items = new NameValueCollection();
            _mainCodeItems = new NameValueCollection();
            _funcCodeItems = new NameValueCollection();
            _funcParmaItems = new NameValueCollection();
        }

        public NameValueCollection Items
        {
            get { return _items; }
            set { _items = value; }
        }
        public NameValueCollection MainCodeItems
        {
            get { return _mainCodeItems; }
            set { _mainCodeItems = value; }
        }
        public NameValueCollection FuncCodeItems
        {
            get { return _funcCodeItems; }
            set { _funcCodeItems = value; }
        }
        public NameValueCollection FuncParmaItems
        {
            get { return _funcParmaItems; }
            set { _funcParmaItems = value; }
        }

        public void AddItem(string key,string value)
        {
            try
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _items.Add(key, value);
            }
            catch
            {
                throw;
            }
        }
        public void AddMainCodeItem(string key, string value)
        {
            try
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _mainCodeItems.Add(key, value);
            }
            catch
            {
                throw;
            }
        }
        public void AddFuncCodeItem(string key, string value)
        {
            try
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _funcCodeItems.Add(key, value);
            }
            catch
            {
                throw;
            }
        }
        public void AddFuncParmaItem(string key, string value)
        {
            try
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _funcParmaItems.Add(key, value);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 将Web访问的参数和值拼接起来（例如pass=aaa&amp;funcode=eval(base64_decode($_POST[$funcCode$]));&amp;connstr=@mysql_connect($host,$user,$pwd);等），并转换成字节串
        /// </summary>
        /// <returns>返回编码拼接后的字节串</returns>
        public byte[] Combine()
        {
            string value = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            string[] allKeys = _items.AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                string key = allKeys[i];
                stringBuilder.Append(value);
                stringBuilder.Append(DataConvert.StrToUrlEncode(key));
                stringBuilder.Append("=");
                stringBuilder.Append(DataConvert.StrToUrlEncode(_items[key]));
                value = "&";
            }
            return Encoding.Default.GetBytes(stringBuilder.ToString());
        }
        /// <summary>
        /// 将Web访问的参数和值拼接起来（例如pass=aaa&amp;funcode=eval(base64_decode($_POST[$funcCode$]));&amp;connstr=@mysql_connect($host,$user,$pwd);等），并转换成字符串
        /// </summary>
        /// <param name="items">需要拼接的参数和值的集合（例如{{"pass","aaa"},{"funcode","eval(base64_decode($_POST[$funcCode$]));"},{"connstr","@mysql_connect($host,$user,$pwd);"}}）</param>
        /// <returns>返回编码拼接后的字符串</returns>
        public string CombineToStr(NameValueCollection items)
        {
            string value = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            string[] allKeys = items.AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                string key = allKeys[i];
                stringBuilder.Append(value);
                stringBuilder.Append(DataConvert.StrToUrlEncode(key));
                stringBuilder.Append("=");
                stringBuilder.Append(DataConvert.StrToUrlEncode(items[key]));
                value = "&";                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 将指定的Web访问参数和值的列表拼接起来（例如pass=aaa&amp;funcode=eval(base64_decode($_POST[$funcCode$]));&amp;connstr=@mysql_connect($host,$user,$pwd);等），并转换成字节串
        /// </summary>
        /// <param name="item">需要拼接的参数和值的集合（例如{{"pass","aaa"},{"funcode","eval(base64_decode($_POST[$funcCode$]));"},{"connstr","@mysql_connect($host,$user,$pwd);"}}）</param>
        /// <returns>返回编码拼接后的字节串</returns>
        public byte[] Combine(NameValueCollection item)
        {
            string value = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            string[] allKeys = item.AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                string key = allKeys[i];
                stringBuilder.Append(value);
                stringBuilder.Append(DataConvert.StrToUrlEncode(key));
                stringBuilder.Append("=");
                stringBuilder.Append(DataConvert.StrToUrlEncode(item[key]));
                value = "&";
            }
            return Encoding.Default.GetBytes(stringBuilder.ToString());
        }
    }
}
