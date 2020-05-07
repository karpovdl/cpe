# cpe

Chrome proxy extension.
This is an extension of the proxy with data substitution in the authorization form.

## Used packages

- [Microsoft.Extensions.Configuration.CommandLine](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.CommandLine) 3.1.3
- [System.Drawing.Common](https://www.nuget.org/packages/System.Drawing.Common) 4.7.0

### Sample create extension

```
cpe --name="proxy.127.0.0.1.5000" --ip="127.0.0.1" --port="5000" --cu="user" --cp="password"
```
