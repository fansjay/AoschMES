using System;
using System.IO;
using System.Web;
using System.Text;

namespace Aosch.MES.Common
{
    public static class LoggerHelper
    {
        public static void Log(string FilePath,LogType logType,string Message)
        {
           
            Message = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")} \t {logType}--->" + Message+"\n";
            try
            {              
                    File.WriteAllText(FilePath, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

    }
}
