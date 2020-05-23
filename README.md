# cpe

[![License][1]][2] [![netcore][10]][11] [![codebeat][20]][21] [![CodeFactor][22]][23] [![Coverity][24]][25] 

[1]: https://img.shields.io/badge/license-MIT-blue.svg?label=License&maxAge=86400 "License"
[2]: ./LICENSE

[10]: https://img.shields.io/badge/.NET%20Core-3.1-blue.svg?style=flat ".NET Core"
[11]: https://dotnet.microsoft.com/download/dotnet-core/3.1

[20]: https://codebeat.co/badges/7d48f2be-145b-4a23-a43d-53dd56d0b2ea "CODEBEAT"
[21]: https://codebeat.co/projects/github-com-karpovdl-cpe-master

[22]: https://www.codefactor.io/repository/github/karpovdl/cpe/badge "CodeFactor"
[23]: https://www.codefactor.io/repository/github/karpovdl/cpe

[24]: https://scan.coverity.com/projects/21197/badge.svg "Coverity Scan Build Status"
[25]: https://scan.coverity.com/projects/karpovdl-cpe

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
