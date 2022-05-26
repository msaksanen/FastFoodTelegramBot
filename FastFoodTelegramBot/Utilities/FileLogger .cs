using FastFoodTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodTelegramBot.Utilities
{
    class FileLogger
    {
        private static object locker = new object();
        public static void WriteLog (string logmessage) 
        {
            string countstr=string.Empty;
            string filename = string.Empty;
            int counter = 0, tempcount = 0, index = 0;
            bool isInt=false;
            try
            {
                string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
               // string pathToLog = ".. / .. /.. /Log";
                if (!Directory.Exists(pathToLog))
                    Directory.CreateDirectory(pathToLog);
                DirectoryInfo logdir = new(pathToLog);
                FileInfo?[] logfiles = logdir.GetFiles("log*.log");
              
                if (logfiles.Length != 0)
                {
                    for (int i = 0; i < logfiles.Length; i++)
                    {  //msg.Substring(msg.IndexOf('*')+1,msg.Length-msg.IndexOf('*')-1);  ("-5" .log for point and extension)
                        countstr = logfiles[i].Name.Substring(logfiles[i].Name.LastIndexOf('_') + 1, logfiles[i].Name.Length - logfiles[i].Name.LastIndexOf('_')-5);
                        if (countstr.Equals(string.Empty, StringComparison.OrdinalIgnoreCase) || countstr.Equals("0", StringComparison.OrdinalIgnoreCase))
                            counter = 0;
                        else isInt = int.TryParse(countstr, out tempcount);
                        
                        if (!isInt) counter = 0;

                        if (tempcount > counter)
                        {
                            counter = tempcount;
                            index = i;
                        }
                    }
                    if (counter == 0)
                    {
                        filename = Path.Combine(pathToLog, string.Format($"log_{DateTime.Now:yyyy.MM.dd.HH.mm}_1.log"));
                    }
                    else if (logfiles[index].Length < 30_720)
                    {
                        filename = Path.Combine(pathToLog, logfiles[index].Name);
                    }
                    else if (logfiles[index].Length >= 30_720)
                    {
                        filename = Path.Combine(pathToLog, string.Format($"log_{DateTime.Now:yyyy.MM.dd.HH.mm}_{counter + 1}.log"));
                    }
                }
                else filename = Path.Combine(pathToLog, string.Format($"log_{DateTime.Now:yyyy.MM.dd.HH.mm}_1.log"));
            
                lock (locker)
                {   if (filename != string.Empty && logmessage != string.Empty)
                        //File.AppendAllText(filename, fullText, Encoding.GetEncoding("Windows-1251"));
                        File.AppendAllTextAsync(filename, logmessage).ConfigureAwait(false);
                    else throw new LoggerFailureException($"Logging to file failed.");
                }
            }
            catch (LoggerFailureException e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                Console.WriteLine($"Exception: {e.StackTrace}");
                Console.WriteLine($"Exception: {e.TargetSite.Name}");
                Console.WriteLine($"Exception: {e.TargetSite.DeclaringType}");

            }
          
        }
    }
}
