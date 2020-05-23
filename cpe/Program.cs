namespace cpe
{
    class Program
    {
        /// <summary></summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var config = Config.Init(args);

            if (config.IsNull() ||
                config.IsHelpFlag ||
                config.IsVersionFlag ||
                config.IsNotAllMandatoryKeysEntered)
            {
                return;
            }

            config.ShowValues();

            Make.Extension(config);
        }
    }
}