﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Altman.Common;
using Altman.Common.AltData;
using Altman.Setting;

namespace Altman.Logic
{
    /// <summary>
    /// 用于拼接脚本命令的类
    /// </summary>
    internal class CustomCommandCode
    {
        private CustomShellType _customShellType;//自定义Shell类型
        private string _pass;//一句话木马的密码
        private string _coding;

        private Dictionary<string, string> _randomParam;
        public CustomCommandCode(CustomShellType customShellType, string pass)
        {
            _customShellType = customShellType;
            _pass = pass;
            _randomParam = new Dictionary<string, string>();
        }

        /**
         * Dictionary<string1,string2> 
         * string1值为：Body,Cookie,Referer(header头)等
         * string2值为：pass=MMM&z=AAA&z1=BBB ,MMM,等
         */
        private void AddItemToDic(Dictionary<string, string> dic, string key, string value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] += "&" + value;
            }
            else
            {
                dic.Add(key, value);
            }
        }
        /// <summary>
        /// 使用指定编码方式编码指定字符串
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private string EncryItem(EncryMode mode, string item)
        {
            if (mode == EncryMode.None)
            {
                return item;
            }
            else if (mode == EncryMode.Base64)
            {
                return DataConvert.StrToBase64(item, 1);
            }
            else if (mode == EncryMode.Hex)
            {
                return DataConvert.StrToHex(item);
            }
            else
            {
                return item;
            }
        }

        /// <summary>
        /// 获取指定长度的随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string GetRandomStr(int length)
        {
            Random r = new Random();
            string str = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string s = ((char)r.Next(97, 123)).ToString();
                str += s;
            }
            return str;
        }

        /// <summary>
        /// 填充参数，主要用于MainCodeSetting.item和FuncCodeSetting.func.item中的$par$=>par
        /// <funcParam></funcParam>节点指定后序的<item></item>节点内容中哪些是参数变量
        /// </summary>
        /// <param name="item">item节点内容字符串</param>
        /// <param name="pars">funcparam节点</param>
        /// <returns></returns>
        private string FillParams(string item, object pars)
        {
            List<string> parList = new List<string>();
            //将传入参数转换为List<string>
            if (pars is CustomShellType.ParamStruct)
            {
                parList.Add(((CustomShellType.ParamStruct)pars).Name);
            }
            else if (pars is List<CustomShellType.ParamStruct>)
            {
                List<CustomShellType.ParamStruct> tmp = (List<CustomShellType.ParamStruct>)pars;
                foreach (var i in tmp)
                {
                    parList.Add(i.Name);
                }
            }
            foreach (string par in parList)
            {
                if (GlobalSetting.IsParamRandom)
                {
                    //string newguid = Guid.NewGuid().ToString().Substring(0, 2);
                    string newguid = GetRandomStr(1);
                    //新的web发包参数id不能等于pass且，不与已经产生的id相同
                    while (newguid == _pass || _randomParam.ContainsValue(newguid))
                    {
                        newguid = GetRandomStr(1);
                    }
                    _randomParam.Add(par, newguid);
                    
                    item = item.Replace("$" + par + "$", newguid);
                }
                else
                {
                    item = item.Replace("$" + par + "$", par);
                }
            }
            return item;
        }


        /// <summary>
        /// 将指定的操作拼接包装成完整的脚本语言，并存储于字典中
        /// </summary>
        /// <param name="customShellType">自定义的Shell类型</param>
        /// <param name="pass">一句话木马的密码</param>
        /// <param name="funcCode">funcCode代码类型，分为非数据库操作(存储于BuiltIn.func文件内)、数据库操作(存储于Db.func文件内)</param>
        /// <param name="parmas">数据库连接参数组</param>
        /// <returns></returns>
        private Dictionary<string, string> GetCode(CustomShellType customShellType,
                                                   string pass,
                                                   CustomShellType.FuncCode funcCode,
                                                   string[] parmas)
        {
            DataCombine dataCombine = new DataCombine();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //MainCodeSetting
            string mainCodeString =
                FillParams(customShellType.MainCodeSetting.Item, customShellType.MainCodeSetting.FuncCodeParam);

                //NameValueCollection与Dictionary<string,string>比较相似，区别在于NameValueCollection在处理Add时，如果遇到已有的Key会以追加的形式进行修改（以逗号为分隔符）。
            NameValueCollection mainCodeItem = new NameValueCollection
            {
                {pass, EncryItem(customShellType.BasicSetting.MainCodeParam.EncryMode, mainCodeString)}
            };
            AddItemToDic(dic, customShellType.BasicSetting.MainCodeParam.Location, dataCombine.CombineToStr(mainCodeItem));
            
            //FuncCode
            string funcCodeString = "";
            if (funcCode.FuncParams.Count > 0)
            {
                funcCodeString = FillParams(funcCode.Item, funcCode.FuncParams);
            }
            else
            {
                funcCodeString = funcCode.Item;
            }
            //判断是否进行了参数随机化，如果进行了参数随机化，则将funcParamName的随机参数从随机参数列表中取出来代替默认的funcParamName
            //备注：_ramdomParma是程序集中记录随机参数的列表，以<默认参数名,随机参数名>的形式记录哪些默认参数使用的参数随机化
            string funcParamName = customShellType.MainCodeSetting.FuncCodeParam.Name;
            if (GlobalSetting.IsParamRandom)
            {
                string newguid = _randomParam[funcParamName];
                funcParamName = newguid;
            }
            NameValueCollection funcCodeItem = new NameValueCollection
            {
                {funcParamName, EncryItem(customShellType.MainCodeSetting.FuncCodeParam.EncryMode, funcCodeString)}
            };
            AddItemToDic(dic, customShellType.MainCodeSetting.FuncCodeParam.Location, dataCombine.CombineToStr(funcCodeItem));
            //FunParma
            if (parmas != null && parmas.Length > 0)
            {
                if (parmas.Length != funcCode.FuncParams.Count)
                {
                    throw new Exception("调用方法的参数个数与实现代码的参数个数不符合");
                }
                for (int i = 0; i < parmas.Length; i++)
                {
                    string parName = funcCode.FuncParams[i].Name;
                    if (GlobalSetting.IsParamRandom)
                    {
                        string newguid = _randomParam[parName];
                        parName = newguid;
                    }

                    NameValueCollection item = new NameValueCollection
                    {
                        {parName, EncryItem(funcCode.FuncParams[i].EncryMode, parmas[i])}
                    };
                    AddItemToDic(dic, funcCode.FuncParams[i].Location, dataCombine.CombineToStr(item));

                    //dataCombine.AddFuncParmaItem("z" + (i + 1), EncryItem(FuncCode.FuncParmaEncryMode, parmas[i]));                   
                }
                //AddItemToDic(dic, FuncCode.FuncParmaLocation, dataCombine.CombineToStr(dataCombine.FuncParmaItems));
            }
            return dic;
        }


        /// <summary>
        /// 将指定的操作拼接包装成完整的脚本语言，并存储于字典中
        /// </summary>
        /// <param name="funcCodeNameXpath">操作命令所在的节点路径</param>
        /// <param name="parmas">数据库连接参数组</param>
        /// <returns></returns>
        public Dictionary<string, string> GetCode(string funcCodeNameXpath, string[] parmas)
        {
            //分解funcCodeNameXpath，将"cmder/exec"分解为cmder和exec
            List<string> list = new List<string>(funcCodeNameXpath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
            string funcCodeName = list[list.Count-1];

            list.RemoveAt(list.Count-1);
            string path = string.Join("/", list);
            return GetCode(_customShellType, _pass, _customShellType.GetFuncCode(path, funcCodeName), parmas);
        }
    }
}
