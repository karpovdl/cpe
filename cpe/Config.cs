namespace cpe
{
    using System;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary></summary>
    sealed internal class Config : IDisposable
    {
        /// <summary>Application name.</summary>
        private static string Name => "cpe";

        /// <summary>The full name of the application.</summary>
        private static string FullName => "Chrome proxy extension";

        /// <summary>Description of the application.</summary>
        private static string Description => "the application creates an proxy extension chrome based on the specified parameters";

        /// <summary>Application version.</summary>
        private static string Version => "1.0.0.4";

        /// <summary>Directory of available keys</summary>
        internal readonly IReadOnlyDictionary<string, string> Keys = new Dictionary<string, string>
            {
                { "-n", "name" },
                { "-i", "ip" },
                { "-p", "port" },
                { "--cu", "credentials_user" },
                { "--cp", "credentials_pass" },
                { "--bl", "bypass_list" },
                { "--da", "delete_artifactory"},
            };

        /// <summary>Set of required keys.</summary>
        private readonly IReadOnlyList<string> _keysMandatory = new List<string>() { "name", "ip", "port" };

        /// <summary></summary>
        private IConfigurationRoot Configuration { get; set; }

        /// <summary></summary>
        internal bool IsHelpFlag { get; set; }

        /// <summary></summary>
        internal bool IsVersionFlag { get; set; }

        /// <summary>If [true], then not all necessary keys are entered, otherwise [false].</summary>
        internal bool IsNotAllMandatoryKeysEntered { get; set; }

        /// <summary></summary>
        private bool _isHideVersion;

        /// <summary></summary>
        private static Config _instance;

        /// <summary></summary>
        /// <param name="args"></param>
        /// <param name="isHideVersion"></param>
        /// <returns></returns>
        public static Config Init(string[] args, bool isHideVersion = false) => _instance ??= new Config(args, isHideVersion);

        /// <summary></summary>
        /// <param name="args"></param>
        /// <param name="isHideVersion"></param>
        private Config(string[] args, bool isHideVersion = false)
        {
            ConfigAsync(args, isHideVersion).GetAwaiter().GetResult();
        }

        /// <summary></summary>
        /// <param name="args"></param>
        /// <param name="isHideVersion"></param>
        private async Task ConfigAsync(string[] args, bool isHideVersion = false)
        {
            _isHideVersion = isHideVersion;

            if ((args.Length == 0) ||
                (args.Length == 1 && args.Any(a => a.Equals("-h", StringComparison.InvariantCultureIgnoreCase) || a.Equals("--help", StringComparison.InvariantCultureIgnoreCase))))
            {
                IsHelpFlag = true;
                await ShowHelpAsync().ConfigureAwait(false);
                return;
            }

            if (args.Length == 1 && args.Any(a => a.Equals("-v", StringComparison.InvariantCultureIgnoreCase) || a.Equals("--version", StringComparison.InvariantCultureIgnoreCase)))
            {
                IsVersionFlag = true;
                await ShowVersionAsync().ConfigureAwait(false);
                return;
            }

            Configuration = await ReadConfigurationAsync(args).ConfigureAwait(false);

            IsNotAllMandatoryKeysEntered = !await TryCheckMandatoryKeysAsync().ConfigureAwait(false);
        }

        /// <summary></summary>
        public void Dispose()
        {
        }

        /// <summary></summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task<IConfigurationRoot> ReadConfigurationAsync(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder();
                builder.AddCommandLine(args, (IDictionary<string, string>)Keys);
                return builder.Build();
            }
            catch(FormatException fe)
            {
                await ShowHelpAsync().ConfigureAwait(false);

                await Console.Out.WriteLineAsync($"Error parsing key: {fe.Message}").ConfigureAwait(false);
            }

            return null;
        }

        /// <summary></summary>
        /// <returns></returns>
        internal bool IsNull()
        {
            return Configuration is null;
        }

        /// <summary></summary>
        private async Task<bool> TryCheckMandatoryKeysAsync()
        {
            var allMandataryKeysEntered = true;
            foreach(var key in _keysMandatory)
            {
                if (GetValue(key) != null)
                {
                    continue;
                }

                await Console.Out.WriteLineAsync($"Key '{key}' is mandatory.").ConfigureAwait(false);
                allMandataryKeysEntered = false;
            }

            if (!allMandataryKeysEntered)
            {
                await Console.Out.WriteLineAsync(Properties.Resources.ForMoreInformationCallHelp).ConfigureAwait(false);
            }

            return allMandataryKeysEntered;
        }

        /// <summary></summary>
        private static async Task ShowNameAsync() => await Console.Out.WriteLineAsync($"App - {Name}").ConfigureAwait(false);

        /// <summary></summary>
        private static async Task ShowFullNameAsync() => await Console.Out.WriteLineAsync($"{FullName} - {Description}").ConfigureAwait(false);

        /// <summary></summary>
        internal async Task ShowVersionAsync()
        {
            if (_isHideVersion)
            {
                return;
            }

            await Console.Out.WriteLineAsync($"Version {Version}.").ConfigureAwait(false);
        }

        /// <summary></summary>
        internal async Task ShowHelpAsync()
        {
            await ShowNameAsync().ConfigureAwait(false);
            await ShowFullNameAsync().ConfigureAwait(false);
            await ShowVersionAsync().ConfigureAwait(false);
            await ShowKeysAsync().ConfigureAwait(false);
        }

        /// <summary></summary>
        internal async Task ShowKeysAsync()
        {
            await Console.Out.WriteLineAsync(Properties.Resources.ListOfAvailableKeys).ConfigureAwait(false);

            foreach (var key in Keys)
            {
                var mandatory = string.Empty;
                if (_keysMandatory.Any(k => k.Equals(key.Value, StringComparison.OrdinalIgnoreCase)))
                {
                    mandatory = $"Key '{key.Value}' is mandatory.";
                }

                await Console.Out.WriteLineAsync($"--{key.Value}, {key.Key}. {mandatory}".TrimEnd()).ConfigureAwait(false);
            }
        }

        /// <summary></summary>
        internal async Task ShowValuesAsync()
        {
            foreach (var key in Keys)
            {
                var value = Configuration[key.Value];
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                await Console.Out.WriteLineAsync($"{key.Value}: '{value}'.").ConfigureAwait(false);
            }
        }

        /// <summary></summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal string GetValue(string name)
        {
            return Configuration[name];
        }
    }
}