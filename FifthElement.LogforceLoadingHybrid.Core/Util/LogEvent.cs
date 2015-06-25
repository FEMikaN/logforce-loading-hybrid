using System;


namespace FifthElement.LogforceLoadingHybrid.Core.Util
{
    public class LogEvent : EventArgs
    {
        public enum ServiceLogType : int { Request = 0, Response, CommandStart, CommandEnd };

        public string Message { get; set; }
        public string ServiceUrl { get; set; }
        public ServiceLogType LogType { get; set; }
        public int ManagedThreadId { get; set; }
        public string CommandName { get; set; }
        public int ExecutionTime { get; set; }
    }
}
