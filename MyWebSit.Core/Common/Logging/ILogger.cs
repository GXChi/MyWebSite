using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSit.Core.Common.Logging
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILogger
    {
        ILogger CreateChildLogger(string name);

        void Debug(object message);

        void Debug(object message, Exception exception);

        void Error(object message);

        void Info(object message);

        bool IsDebugEnabled { get; }

        bool IsErrorEnabled { get; }

        bool IsInfoEnabled { get; }

        bool IsWarnEnabled { get; }
    }
}
