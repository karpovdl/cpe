# Sample

## Create an extension named "proxy.127.0.0.1.5000"

```bash
cpe --name="proxy.127.0.0.1.5000" --ip="127.0.0.1" --port="5000" --cu="user" --cp="password"
```

## Create an extension named and delete the folder with the unpacked extension

```bash
cpe --name="proxy.127.0.0.1.5000" --ip="127.0.0.1" --port="5000" --cu="user" --cp="password" --da="true"
```

## Selenium web driver

### Add extension

```c-sharp
    /// <summary>Create proxy extension for browser.</summary>
    /// <param name="name"></param>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    private static void CreateProxyExtension(string name, string ip, string port)
    {
        var cpe = @$"{AppDomain.CurrentDomain.BaseDirectory}\{Settings.AppPathBrowsers}\{Settings.AppPathExtensions}\{Settings.AppCpe}";
        Process.Start(cpe, $"--name=\"{name}\" --ip=\"{ip}\" --port=\"{port}\" --cu=\"{Settings.NetworkCredential.UserName}\" --cp=\"{Settings.NetworkCredential.Password}\"").WaitForExit();
    }

    /// <summary>Get proxy extension for browser.</summary>
    /// <param name="httpProxy"></param>
    /// <returns></returns>
    private static string GetProxyExtension(string httpProxy)
    {
        // Sample: httpProxy = IP:PORT => "IP:PORT".Split(':');
        var ip = "__IP__";
        var port = "__PORT__";
        var name = $"http.{ip}";
        var extension = @$"{AppDomain.CurrentDomain.BaseDirectory}\{name}.crx";

        if (!string.IsNullOrEmpty(ip) && !File.Exists(extension))
        {
            CreateProxyExtension(name, ip, port);
        }

        return extension;
    }

    /// <summary>Get chrome driver.</summary>
    /// <param name="ipProxy">Value of the proxy for the HTTP protocol.</param>
    /// <returns></returns>
    private static IWebDriver GetChromeDriver(string httpProxy)
    {
        ChromeOptions op = new ChromeOptions()
        {
            AcceptInsecureCertificates = true,
            Proxy = new Proxy()
            {
                Kind = ProxyKind.Manual,
                IsAutoDetect = false
            },
        };

        if (!string.IsNullOrEmpty(httpProxy))
        {
            var extension = GetProxyExtension(httpProxy);

            if (File.Exists(extension))
            {
                op.AddExtension(extension);
            }
            else
            {
                op.AddArgument("--disable-extensions");
            }
        }

        var ds = ChromeDriverService.CreateDefaultService(@$"{AppDomain.CurrentDomain.BaseDirectory}\{Settings.AppPathBrowsers}\{Settings.WebDriverVersion}\{Settings.WebDriverOS}\");
        ds.HideCommandPromptWindow = true;

        return new ChromeDriver(ds, op);
    }
```
