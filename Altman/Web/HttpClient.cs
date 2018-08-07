using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Altman.Common.AltData;
using Altman.Common.AltEventArgs;
using Altman.Setting;

namespace Altman.Web
{
    /// <summary>
    /// 数据发送和接收前的预操作（对header的操作，如设置代理、设置cookie等）
    /// </summary>
    internal class HttpClient
    {
        private WebHeaderCollection _header;
        public HttpClient()
        {
        }
        public HttpClient(WebHeaderCollection header)
        {
            this._header = header;
        }
        private Http GetHttp()
        {
            Http http = new Http();
            //配置proxy，静态字段
            WebRequest.DefaultWebProxy = GlobalSetting.Proxy;
            //配置cookie
            if (GlobalSetting.HttpCookie != null)
            {
                http.Cookies = GlobalSetting.HttpCookie;
            }
            //配置HttpHeader
            //Shell的HttpHeader优先于全局HttpHeader
            if (_header != null)
            {
                foreach (string key in GlobalSetting.HttpHeader.Keys)
                {
                    if (_header[key]==null)
                    {
                        _header.Add(key, GlobalSetting.HttpHeader[key]);
                    }
                }
                //由于传递过去的Headers的值可能发生变化，所以这里需要完全拷贝一份。
                WebHeaderCollection tmpHeader = new WebHeaderCollection { _header };//此处使用的是集合初始化器，实际调用的是集合的add方法进行拷贝
                http.Headers = tmpHeader;
            }
            else
            {
                if (GlobalSetting.HttpHeader != null)
                {                  
                    WebHeaderCollection tmpHeader = new WebHeaderCollection { GlobalSetting.HttpHeader };//此处使用的是集合初始化器，实际调用的是集合的add方法进行拷贝
                    http.Headers = tmpHeader;
                }
            }

            //配置UserAgent
            if (GlobalSetting.UserAgent != null)
            {
                int index = new Random().Next(0, GlobalSetting.UserAgent.Count);
                http.Headers.Add(HttpRequestHeader.UserAgent, GlobalSetting.UserAgent[index]);
            }
            return http;
        }


        #region 提交命令
        /// <summary>
        /// 从存储自定义Shell操作的字典中提交命令代码（将字典中header和cookie的数据写入http类，将字典中body的数据提取并返回）
        /// </summary>
        /// <param name="http">存储header和cookie数据的http类</param>
        /// <param name="commandCode">存储自定义Shell操作代码的字典</param>
        /// <returns>字典中的body数据，即post或get的代码</returns>
        private string ProcessCommandCode(ref Http http, Dictionary<string, string> commandCode)
        {
            string postCode = "";
            //保存位置为Cookie
            if (commandCode.ContainsKey("Cookie"))
            {
                CookieContainer cookies = new CookieContainer();
                //分解之前组合的string，获取参数
                NameValueCollection parmas = DataConvert.GetParma(commandCode["Cookie"]);
                string[] allKeys = parmas.AllKeys;
                for (int i = 0; i < allKeys.Length; i++)
                {
                    string key = allKeys[i];
                    cookies.Add(new Cookie(key, parmas[key]));
                }
                //保存cookie到webclient
                http.Cookies = cookies;
                //移除cookie的Key
                commandCode.Remove("Cookie");
            }
            //保存位置为Body
            if (commandCode.ContainsKey("Body"))
            {
                //准备发送的post内容
                postCode = commandCode["Body"];
                //移除Body的Key
                commandCode.Remove("Body");
            }
            //保存位置为Header
            if (commandCode.Keys.Count > 0)
            {
                //保存自定义header到webclient
                foreach (var a in commandCode)
                {
                    if (http.Headers[a.Key] == null)
                    {
                        http.Headers.Add(a.Key, a.Value);
                    }
                    else
                    {
                        http.Headers[a.Key] = a.Value;
                    }
                }
            }
            return postCode;
        }
        /// <summary>
        /// 
        /// </summary使用post提交命令
        /// <param name="url">一句话木马</param>
        /// <param name="commandCode">存储自定义Shell操作的字典</param>
        /// <param name="isBindUploadProgressChangedEvent">是否绑定了上传进度更改事件（是否显示上传进度条）</param>
        /// <param name="isBindDownloadProgressChangedEvent">是否绑定了下载进度更改事件（是否显示下载进度条）</param>
        /// <returns></returns>
        public byte[] SubmitCommandByPost(string url,
                                          Dictionary<string, string> commandCode,
                                          bool isBindUploadProgressChangedEvent = false,
                                          bool isBindDownloadProgressChangedEvent = false)
        {
            Http http = GetHttp();
            if (isBindUploadProgressChangedEvent)
                http.UploadProgressChanged += http_UploadProgressChanged;
            if (isBindDownloadProgressChangedEvent)
                http.DownloadProgressChanged += http_DownloadProgressChanged;

            string postCode = ProcessCommandCode(ref http, commandCode);
            return http.Post(url, Encoding.Default.GetBytes(postCode));
        }

        private void http_DownloadProgressChanged(object sender, AltDownloadProgressChangedEventArgs e)
        {
            if (DownloadFileProgressChangedToDo != null)
            {
                DownloadFileProgressChangedToDo(null, e);
            }
        }
        private void http_UploadProgressChanged(object sender, AltUploadProgressChangedEventArgs e)
        {
            if (UploadFileProgressChangedToDo != null)
            {
                UploadFileProgressChangedToDo(null, e);
            }
        }
        #endregion

        public event EventHandler<AltUploadProgressChangedEventArgs> UploadFileProgressChangedToDo;
        public event EventHandler<AltDownloadProgressChangedEventArgs> DownloadFileProgressChangedToDo;

    }
}
