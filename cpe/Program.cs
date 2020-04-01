namespace cpe
{
    class Program
    {
        static void Main(string[] args)
        {
            Config config = Config.Init(args);

            if (config.IsNull() ||
                config.IsHelpFlag ||
                config.IsVersionFlag ||
                config.IsNotAllMandataryKeysEntered)
            {
                return;
            }

            config.ShowValues();

            Make.Extension(config);
        }
    }
}