namespace cpe
{
    class Program
    {
        /// <summary></summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        /// <summary></summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static async System.Threading.Tasks.Task MainAsync(string[] args)
        {
            var config = Config.Init(args);

            if (config.IsNull() ||
                config.IsHelpFlag ||
                config.IsVersionFlag ||
                config.IsNotAllMandatoryKeysEntered)
            {
                return;
            }

            await config.ShowValuesAsync().ConfigureAwait(false);

            await Make.ExtensionAsync(config).ConfigureAwait(false);
        }
    }
}