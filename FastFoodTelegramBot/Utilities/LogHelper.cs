using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FastFoodTelegramBot.Utilities
{
    class LogHelper
    {
        private static object locker = new object();
        public static void Info(string str)
        {
            if (CommandNames.LogLevel.Equals("info", StringComparison.OrdinalIgnoreCase))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"[Log INFO] {DateTime.Now:G}");
                sb.AppendLine("============================");
                sb.AppendLine(str);
                sb.AppendLine("============================");

                lock (locker)
                {
                    sb.AppendLine($"Thread information:");
                    sb.AppendLine($"Thread ID:         {Thread.CurrentThread.ManagedThreadId}");
                    sb.AppendLine($"Thread State:      {Thread.CurrentThread.ThreadState}");
                    sb.AppendLine($"Thread Priority:   {Thread.CurrentThread.Priority}");
                    sb.AppendLine($"Is thread alive:   {Thread.CurrentThread.IsAlive}");
                    sb.AppendLine($"Background:        {Thread.CurrentThread.IsBackground}");
                    sb.AppendLine($"Thread Pool:       {Thread.CurrentThread.IsThreadPoolThread}");
                    sb.AppendLine("============================");
                }
                FileLogger.WriteLog(sb.ToString());
            }

        }
        public static void Debug(string str, string sysinfo)
        {
            if (CommandNames.LogLevel.Equals("info", StringComparison.OrdinalIgnoreCase) || CommandNames.LogLevel.Equals("debug", StringComparison.OrdinalIgnoreCase))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"[Log DEBUG] {DateTime.Now:G}");
                sb.AppendLine($"Additional info. {sysinfo}");
                sb.AppendLine("============================");
                sb.AppendLine(str);
                sb.AppendLine("============================");

                lock (locker)
                {
                    sb.AppendLine($"Thread information:");
                    sb.AppendLine($"Thread ID:         {Thread.CurrentThread.ManagedThreadId}");
                    sb.AppendLine($"Thread State:      {Thread.CurrentThread.ThreadState}");
                    sb.AppendLine($"Thread Priority:   {Thread.CurrentThread.Priority}");
                    sb.AppendLine($"Is thread alive:   {Thread.CurrentThread.IsAlive}");
                    sb.AppendLine($"Background:        {Thread.CurrentThread.IsBackground}");
                    sb.AppendLine($"Thread Pool:       {Thread.CurrentThread.IsThreadPoolThread}");
                    sb.AppendLine("============================");
                }
                FileLogger.WriteLog(sb.ToString());
            }

        }
        public static void Error(Exception ex)
        {
            if (CommandNames.LogLevel.Equals("info", StringComparison.OrdinalIgnoreCase)|| CommandNames.LogLevel.Equals("debug", StringComparison.OrdinalIgnoreCase) || CommandNames.LogLevel.Equals("error", StringComparison.OrdinalIgnoreCase))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"[Log ERROR] {DateTime.Now:G}");
                sb.AppendLine("============================");
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);
                sb.AppendLine(ex.TargetSite.Name);
                sb.AppendLine(ex.TargetSite.DeclaringType.ToString());    
                sb.AppendLine("============================");

                lock (locker)
                {
                    sb.AppendLine($"Thread information:");
                    sb.AppendLine($"Thread ID:         {Thread.CurrentThread.ManagedThreadId}");
                    sb.AppendLine($"Thread State:      {Thread.CurrentThread.ThreadState}");
                    sb.AppendLine($"Thread Priority:   {Thread.CurrentThread.Priority}");
                    sb.AppendLine($"Is thread alive:   {Thread.CurrentThread.IsAlive}");
                    sb.AppendLine($"Background:        {Thread.CurrentThread.IsBackground}");
                    sb.AppendLine($"Thread Pool:       {Thread.CurrentThread.IsThreadPoolThread}");
                    sb.AppendLine("============================");
                }
                FileLogger.WriteLog(sb.ToString());
            }
        }
}   }
