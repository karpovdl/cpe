# cpe

[![License](https://img.shields.io/badge/license-MIT-blue.svg?label=License&maxAge=86400)](./LICENSE)
[![codebeat][1]][2]

[1]: https://codebeat.co/badges/7d48f2be-145b-4a23-a43d-53dd56d0b2ea "Codebeat badge"
[2]: https://codebeat.co/projects/github-com-karpovdl-cpe-master "Codebeat"

[![](cpe/Resources/cpe48.png)](https://github.com/karpovdl/cpe)

Chrome proxy extension.
This is an extension of the proxy with data substitution in the authorization form.

## Used packages

- [Microsoft.Extensions.Configuration.CommandLine](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.CommandLine) 3.1.4
- [System.Drawing.Common](https://www.nuget.org/packages/System.Drawing.Common) 4.7.0

### Sample create extension

```
cpe --name="proxy.127.0.0.1.5000" --ip="127.0.0.1" --port="5000" --cu="user" --cp="password"
```
