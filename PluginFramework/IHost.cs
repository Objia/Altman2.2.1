namespace PluginFramework
{
    /// <summary>
    /// Host程序的接口，包含数据库操作、界面UI操作、部分核心操作和应用程序的基本信息
    /// </summary>
    public interface IHost
    {
        IHostApp App { get; }
        IHostCore Core { get; }
        IHostUi Ui { get; }
        IHostDb Database { get; }
    }
}