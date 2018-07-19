namespace Altman.Model
{
    /// <summary>
    /// 一句话木马类
    /// </summary>
    public class Shell
    {
        public string Id { get; set; }//保留字段
        public string TargetId { get; set; }//一句话木马的ID
        public string TargetLevel { get; set; }//一句话木马的目标等级
        public string Status { get; set; }//一句话木马的http状态码

        public string ShellUrl { get; set; }//一句话木马的地址
        public string ShellPwd { get; set; }//一句话木马的密码
        public string ShellType { get; set; }//一句话木马的类型
        public string ShellExtraString { get; set; }//一句话木马的扩展字符串，即数据库连接字符串格式

        public string ServerCoding { get; set; }
        public string WebCoding { get; set; }

        public int TimeOut { get; set; }

        public string Area { get; set; }//一句话木马的区域（国别）
        public string Remark { get; set; }//一句话木马的备注
        public string AddTime { get; set; }//一句话木马的添加时间
    }
}
