namespace cpe
{
    using System;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Linq;

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
        private static string Version => "1.0.0.3";

        /// <summary>Directory of available keys</summary>
        internal readonly IReadOnlyDictionary<string, string> Keys = new Dictionary<string, string>
            {
                { "-n", "name" },
                { "-i", "ip" },
                { "-p", "port" },
                { "--cu", "credentials_user" },
                { "--cp", "credentials_pass" },
                { "--bl", "bypass_list" },
            };

        /// <summary>Set of required keys.</summary>
        private readonly IReadOnlyList<string> _keysMandatory = new List<string>() { "name", "ip", "port" };

        /// <summary></summary>
        private readonly IConfigurationRoot Configuration;

        /// <summary></summary>
        internal readonly bool IsHelpFlag;

        /// <summary></summary>
        internal readonly bool IsVersionFlag;

        /// <summary>If [true], then not all necessary keys are entered, otherwise [false].</summary>
        internal readonly bool IsNotAllMandatoryKeysEntered;

        /// <summary></summary>
        private readonly bool _isHideVersion;

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
            _isHideVersion = isHideVersion;

            if ((args.Length == 0) ||
                (args.Length == 1 && args.Any(a => a.Equals("-h", StringComparison.InvariantCultureIgnoreCase) || a.Equals("--help", StringComparison.InvariantCultureIgnoreCase))))
            {
                IsHelpFlag = true;
                ShowHelp();
                return;
            }

            if (args.Length == 1 && args.Any(a => a.Equals("-v", StringComparison.InvariantCultureIgnoreCase) || a.Equals("--version", StringComparison.InvariantCultureIgnoreCase)))
            {
                IsVersionFlag = true;
                ShowVersion();
                return;
            }

            Configuration = ReadConfiguration(args);

            IsNotAllMandatoryKeysEntered = !TryCheckMandatoryKeys();
        }

        /// <summary></summary>
        public void Dispose()
        {
        }

        /// <summary></summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private IConfigurationRoot ReadConfiguration(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder();
                builder.AddCommandLine(args, (IDictionary<string, string>)Keys);
                return builder.Build();
            }
            catch(FormatException fe)
            {
                ShowHelp();

                Console.WriteLine($"Error parsing key: {fe.Message}");
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
        private bool TryCheckMandatoryKeys()
        {
            var allMandataryKeysEntered = true;
            foreach(var key in _keysMandatory)
            {
                if (GetValue(key) != null)
                {
                    continue;
                }

                Console.WriteLine($"Key '{key}' is mandatory.");
                allMandataryKeysEntered = false;
            }

            if (!allMandataryKeysEntered)
            {
                Console.WriteLine(Properties.Resources.ForMoreInformationCallHelp);
            }

            return allMandataryKeysEntered;
        }

        /// <summary></summary>
        private static void ShowName() => Console.WriteLine($"App - {Name}");

        /// <summary></summary>
        private static void ShowFullName() => Console.WriteLine($"{FullName} - {Description}");

        /// <summary></summary>
        internal void ShowVersion()
        {
            if (_isHideVersion)
            {
                return;
            }

            Console.WriteLine($"Version {Version}.");
        }

        /// <summary></summary>
        internal void ShowHelp()
        {
            ShowName();
            ShowFullName();
            ShowVersion();
            ShowKeys();
        }

        /// <summary></summary>
        internal void ShowKeys()
        {
            Console.WriteLine(Properties.Resources.ListOfAvailableKeys);
            foreach (var key in Keys)
            {
                var mandatory = string.Empty;
                if (_keysMandatory.Any(k => k.Equals(key.Value, StringComparison.OrdinalIgnoreCase)))
                {
                    mandatory = $"Key '{key.Value}' is mandatory.";
                }

                Console.WriteLine($"--{key.Value}, {key.Key}. {mandatory}".TrimEnd());
            }
        }

        /// <summary></summary>
        internal void ShowValues()
        {
            foreach (var key in Keys)
            {
                var value = Configuration[key.Value];
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }
                Console.WriteLine($"{key.Value}: '{value}'.");
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