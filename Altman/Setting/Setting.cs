using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Altman.Setting
{
    /// <summary>
    /// 设置类，包含useragent,httpheader,policy,proxy等信息
    /// </summary>
    public class Setting
    {
        #region struct
        public struct UserAgentStruct
        {
            public bool IsRandom;
            public KeyValuePair<string, string> Selected;//选中的UserAgent键值对
            public Dictionary<string, string> UserAgentList;//所有的UserAgent键值对列表
        }
        public struct HttpHeaderStruct
        {
            public Dictionary<string, string> HttpHeaderList;
        }
        public struct PolicyStruct
        {
            public bool IsParamRandom;
            public bool IsShowDisclaimer;
        }
        public struct ProxyStruct
        {
            public int IsNoOrIeOrCustomProxy;//0,1,2
            public string ProxyAddr;
            public string ProxyPort;
            public string ProxyUser;
            public string ProxyPassword;
            public string ProxyDomain;
        }
        #endregion

        private UserAgentStruct _userAgent;
        private HttpHeaderStruct _httpHeader;
        private PolicyStruct _policy;
        private ProxyStruct _proxy;

        public UserAgentStruct GetUserAgentStruct
        {
            get { return _userAgent; }
        }
        public HttpHeaderStruct GetHttpHeaderStruct
        {
            get { return _httpHeader; }
        }
        public PolicyStruct GetPolicyStruct
        {
            get { return _policy; }
        }
        public ProxyStruct GetProxyStruct
        {
            get { return _proxy; }
        }
        /// <summary>
        /// 处理后的UserAgent列表，如果列表项只有一个，表示使用该列表项，如果列表项有多个，表示用户开户了随机UserAgent选项，从列表项中随机选一个使用
        /// </summary>
        public List<string> UserAgent
        {
            get
            {
                //程序最后需要通过判断list的个数，来确认是否开启随机功能
                List<string> list = new List<string>();
                if (_userAgent.IsRandom)
                {
                    list.AddRange(_userAgent.UserAgentList.Select(i => i.Value));
                }
                else
                {
                    list.Add(_userAgent.Selected.Value);
                }
                return list;
            }
        }
        /// <summary>
        /// http头部可选键值对的集合
        /// </summary>
        public WebHeaderCollection HttpHeader
        {
            get
            {
                WebHeaderCollection header = new WebHeaderCollection();
                foreach (KeyValuePair<string, string> i in _httpHeader.HttpHeaderList)
                {
                    header.Add(i.Key,i.Value);
                }
                return header;
            }
        }

        public bool IsParamRandom
        {
            get { return _policy.IsParamRandom; }
            set { _policy.IsParamRandom = value; }
        }
        public bool IsShowDisclaimer
        {
            get { return _policy.IsShowDisclaimer; }
            set { _policy.IsShowDisclaimer = value; }
        }

        public IWebProxy Proxy
        {
            get
            {
                IWebProxy proxy=null;
                int type = _proxy.IsNoOrIeOrCustomProxy;
                switch (type)
                {
                    case 1:
                        proxy = WebRequest.GetSystemWebProxy();
                        break;
                    case 2:
                        Uri uri = new Uri("http://" + _proxy.ProxyAddr + ":" + _proxy.ProxyPort);
                        WebProxy currentWebProxy = new WebProxy(uri, false);
                        if (string.IsNullOrEmpty(_proxy.ProxyUser) && string.IsNullOrEmpty(_proxy.ProxyPassword))
                        {
                            currentWebProxy.Credentials = new System.Net.NetworkCredential(_proxy.ProxyUser,
                                _proxy.ProxyPassword,
                                _proxy.ProxyDomain);
                        }
                        else
                        {
                            currentWebProxy.Credentials = System.Net.CredentialCache.DefaultCredentials;                        
                        }
                        proxy = currentWebProxy;
                        break;
                    default:
                        proxy = null;
                        break;
                }
                return proxy;
            }
        }

        public Setting(UserAgentStruct userAgent,HttpHeaderStruct httpHeader,PolicyStruct policy,ProxyStruct proxy)
        {
            _userAgent = userAgent;
            _httpHeader = httpHeader;
            _policy = policy;
            _proxy = proxy;
        }
    }
}
