using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FifthElement.LogforceLoadingHybrid.Core.Util
{
    /// <summary>
    /// A Logging class implementing the Singleton pattern and an internal Queue to be flushed perdiodically
    /// </summary>
    public class LogWriter
    {
        private static LogWriter _instance;
        private static Queue<Log> _logQueue;
        private static readonly int MaxLogAge = int.Parse("10");
        private static readonly int QueueSize = int.Parse("10");
        private static DateTime _lastFlushed = DateTime.Now;

        /// <summary>
        /// Private constructor to prevent instance creation
        /// </summary>
        private LogWriter()
        {
            if (!File.Exists(LogWriter.LogFilename()))
            {
                File.WriteAllText(LogWriter.LogFilename(), "");
            }
        }

        /// <summary>
        /// An LogWriter instance that exposes a single instance
        /// </summary>
        public static LogWriter Instance
        {
            get
            {
                // If the instance is null then create one and init the Queue
                if (_instance == null)
                {
                    _instance = new LogWriter();
                    _logQueue = new Queue<Log>();
                }
                return _instance;
            }
        }

        private static string LogPath()
        {
            return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + Path.DirectorySeparatorChar;
        }
        public static string LogFilename()
        {
            var fileName = String.Format("{0}__Kotka.log",DateTime.Now.ToString("yyyy-MM-dd"));
            return Path.Combine(LogPath(),fileName);
        }

        /// <summary>
        /// The single instance method that writes to the log file
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        /// <param name="doFlush">Force to write to file immediately, if true</param>
        public void WriteToLog(string message, bool doFlush = true)
        {
            // Lock the queue while writing to prevent contention for the log file
            lock (_logQueue)
            {
                // Create the entry and push to the Queue
                Log logEntry = new Log(message);
                _logQueue.Enqueue(logEntry);

                // If we have reached the Queue Size then flush the Queue
                if (_logQueue.Count >= QueueSize || DoPeriodicFlush() || doFlush)
                {
                    FlushLog();
                }
            }
        }

        public Stream GetStream()
        {
            return File.Open(LogFilename(), FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
        }

        private bool DoPeriodicFlush()
        {
            TimeSpan logAge = DateTime.Now - _lastFlushed;
            if (logAge.TotalSeconds >= MaxLogAge)
            {
                _lastFlushed = DateTime.Now;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Flushes the Queue to the physical log file
        /// </summary>
        private void FlushLog()
        {
            while (_logQueue.Count > 0)
            {
                Log entry = _logQueue.Dequeue();
                Trace.WriteLine(String.Format("{0}\t{1}", entry.LogTime, entry.Message));
            }
        }
    }

    /// <summary>
    /// A Log class to store the message and the Date and Time the log entry was created
    /// </summary>
    public class Log
    {
        public string Message { get; set; }
        public string LogTime { get; set; }
        public string LogDate { get; set; }

        public Log(string message)
        {
            Message = message;
            LogDate = DateTime.Now.ToString("yyyy-MM-dd");
            LogTime = DateTime.Now.ToString("HH:mm:ss.fff tt");
        }
    }


}