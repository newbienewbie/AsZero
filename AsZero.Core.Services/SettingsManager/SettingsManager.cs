using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace AsZero.Core.Services
{
    public class SettingsManager<T> where T : class
    {
        private readonly IHostEnvironment _env;

        public SettingsManager(IHostEnvironment env)
        {
            this._env = env;
        }


        protected string GetSettingFileName()
        {
            return $"{typeof(T).Name}.json";
        }

        protected string GetSettingFileDir()
        {
            string basedir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            basedir = Path.Combine(basedir, this._env.ApplicationName);
            return basedir;
        }

        public T LoadSettings()
        {
            var dir = this.GetSettingFileDir();
            var filename = this.GetSettingFileName();
            var filepath = Path.Combine(dir, filename);
            return File.Exists(filepath) ?
                JsonConvert.DeserializeObject<T>(File.ReadAllText(filepath)) :
                null;
        }

        public void SaveSettings(T settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var filename = this.GetSettingFileName();
            var dir= this.GetSettingFileDir();
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var filepath = Path.Combine(dir, filename);
            string json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(filepath, json);
        }
    }
}
