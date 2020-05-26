using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;

namespace baseproject.core.logsList
{
    public class LogsListServicio : ILogsListServicio
    {
        private readonly IHostingEnvironment _env;

        public LogsListServicio(IHostingEnvironment env)
        {
            _env = env;
        }
        public List<object> GetLogsFiles()
        {
            var files = new List<object>();
            string[] filePaths = Directory.GetFiles(_env.ContentRootPath + "/wwwroot/Logs");

            foreach (string myFiles in filePaths)
            {
                string nombre = Path.GetFileName(myFiles);
                files.Add(new { nombre });
            }
            return files;
        }
    }
}
