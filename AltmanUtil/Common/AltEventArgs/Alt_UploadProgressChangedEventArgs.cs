using System;

namespace Altman.Common.AltEventArgs
{
    /// <summary>
    /// 上传数据触发事件的参数
    /// </summary>
    public class AltUploadProgressChangedEventArgs : EventArgs
    {
        private long bytesSent;
        private long totalBytesToSend;
        public long BytesSent
        {
            get
            {
                return this.bytesSent;
            }
        }
        public long TotalBytesToSend
        {
            get
            {
                return this.totalBytesToSend;
            }
        }
        public AltUploadProgressChangedEventArgs(long bytesSent, long totalBytesToSend)
        {
            this.bytesSent = bytesSent;
            this.totalBytesToSend = totalBytesToSend;
        }
    }
}
