namespace cpe
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Text;

    static class Make
    {
        /// <summary></summary>
        private static readonly Dictionary<string, string> Resources = new Dictionary<string, string>()
            {
                { "background", "background.js" },
                { "manifest", "manifest.json" },
            };

        /// <summary></summary>
        private static readonly Dictionary<string, string> Images = new Dictionary<string, string>()
            {
                { "cpe16", "cpe16.png" },
                { "cpe48", "cpe48.png" },
                { "cpe128", "cpe128.png" },
            };

        /// <summary></summary>
        /// <param name="config"></param>
        static internal void Extension(Config config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            foreach(var resource in Resources)
            {
                var res = GetResource(resource.Key);
                ChangeResource(config, ref res);
                SaveResource($"{config.GetValue("name")}\\{resource.Value}", ref res);
            }

            foreach (var image in Images)
            {
                using var res = GetImage(image.Key);
                SaveImage($"{config.GetValue("name")}\\{image.Value}", res);
            }

            Create($"{config.GetValue("name")}");
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static private string GetResource(string name)
        {
            if (Properties.Resources.ResourceManager.GetObject(name, CultureInfo.InvariantCulture) is byte[] dataByte)
            {
                return Encoding.UTF8.GetString(dataByte, 0, dataByte.Length);
            }

            string dataString = Properties.Resources.ResourceManager.GetObject(name, CultureInfo.InvariantCulture) as string;
            if (!string.IsNullOrEmpty(dataString))
            {
                return dataString;
            }

            return string.Empty;
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static private Bitmap GetImage(string name)
        {
            if (Properties.Resources.ResourceManager.GetObject(name, CultureInfo.InvariantCulture) is byte[] dataByte)
            {
                using MemoryStream stream = new MemoryStream(dataByte);
                return new Bitmap(stream);
            }

            return null;
        }

        /// <summary></summary>
        /// <param name="config"></param>
        /// <param name="resource"></param>
        static private void ChangeResource(Config config, ref string resource)
        {
            foreach (var key in config.Keys)
            {
                resource = resource.Replace(
                    string.Join(null, "{", $"{key.Value}", "}"),
                    config.GetValue(key.Value),
                    StringComparison.InvariantCultureIgnoreCase);
            }
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="resource"></param>
        static private void SaveResource(string name, ref string resource)
        {
            var path = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)}\\{name}";
            var dir = $"{Path.GetDirectoryName(path)}";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllText(path, resource);
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="resource"></param>
        static private void SaveImage(string name, Bitmap resource)
        {
            var path = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)}\\{name}";
            var dir = $"{Path.GetDirectoryName(path)}";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            resource.Save(path);
        }

        static private void Create(string name)
        {
            var path = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)}\\{name}";

            var chrome = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\Google\Chrome\Application\chrome.exe";

            //Create *.pem and *.crx files
            Process.Start(chrome, $"--pack-extension=\"{path}\" --no-message-box").WaitForExit();
        }
    }
}