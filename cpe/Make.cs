namespace cpe
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary></summary>
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
        internal static async Task ExtensionAsync(Config config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var name = $"{config.GetValue("name")}";

            Delete(name);

            Resources.AsParallel().ForAll(async resource =>
            {
                var res = GetResource(resource.Key);
                res = ChangeResource(config, res);
                await SaveResourceAsync($"{name}\\{resource.Value}", res).ConfigureAwait(false);
            });

            Images.AsParallel().ForAll(image =>
            {
                using var res = GetImage(image.Key);
                SaveImage($"{name}\\{image.Value}", res);
            });

            _ = bool.TryParse($"{config.GetValue("delete_artifactory")}", out bool isDeleteArtifactory);
            await CreateAsync(name, isDeleteArtifactory).ConfigureAwait(false);
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetResource(string name)
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
        private static Bitmap GetImage(string name)
        {
            if (Properties.Resources.ResourceManager.GetObject(name, CultureInfo.InvariantCulture) is byte[] dataByte)
            {
                using MemoryStream stream = new MemoryStream(dataByte);
                return new Bitmap(stream);
            }

            return null;
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetPath(string name)
        {
            var path = $"{Directory.GetCurrentDirectory()}\\{name}";
            var dir = $"{Path.GetDirectoryName(path)}";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return path;
        }

        /// <summary></summary>
        /// <param name="config"></param>
        /// <param name="resource"></param>
        private static string ChangeResource(Config config, string resource)
        {
            foreach (var key in config.Keys)
            {
                var keyValue = config.GetValue(key.Value);
                resource = resource.Replace(
                    string.Join(null, "{", $"{key.Value}", "}"),
                    keyValue,
                    StringComparison.InvariantCultureIgnoreCase);
            }

            return resource;
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="resource"></param>
        private static async Task SaveResourceAsync(string name, string resource) => await File.WriteAllTextAsync(GetPath(name), resource).ConfigureAwait(false);

        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="resource"></param>
        private static void SaveImage(string name, Bitmap resource) => resource.Save(GetPath(name));

        /// <summary></summary>
        /// <param name="name"></param>
        private static void Delete(string name)
        {
            var path = $"{Directory.GetCurrentDirectory()}\\{name}";

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            if (File.Exists($"{path}.pem"))
            {
                File.Delete($"{path}.pem");
            }

            if (File.Exists($"{path}.crx"))
            {
                File.Delete($"{path}.crx");
            }
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <param name="isDeleteArtifactory"></param>
        /// <returns></returns>
        private static async Task CreateAsync(string name, bool isDeleteArtifactory)
        {
            var path = GetPath(name);
            await Console.Out.WriteLineAsync($"Path extension: {path}").ConfigureAwait(false);

            var chrome = string.Concat(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), @"\Google\Chrome\Application\chrome.exe");
            if (!File.Exists(chrome))
            {
                await Console.Out.WriteLineAsync(Properties.Resources.ChromeAppNotFound).ConfigureAwait(false);
                return;
            }

            // Create *.pem and *.crx files
            await Task.Run(() => Process.Start(chrome, $"--pack-extension=\"{path}\" --no-message-box").WaitForExit()).ConfigureAwait(false);

            // Delete unpacked extension
            if (isDeleteArtifactory && Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            await Console.Out.WriteLineAsync(Properties.Resources.ChromeExtensionWasCreatedSuccessfully).ConfigureAwait(false);
        }
    }
}