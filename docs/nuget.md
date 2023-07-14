## Manual to locally add NuGet packages

### To create nuget package you can use the next command:

```bash
dotnet pack -c Release
```

### To add assembly to nuget you can use the next command:

```bash
nuget add [file].nupkg -Source [path to nuget repository]
```

### Command with which <code>Figures.SpawnRandomizer</code> was added to nuget:

```bash
.\nuget.exe add 'D:\Asp .Net Core\Projects\Desktop\Figures\src\Figures.SpawnRandomizer\bin\Debug\Figures.SpawnRandomizer.1.0.0.nupkg' -Source "C:\\Program Files (x86)\\Microsoft SDKs\\NuGetPackages\\"
```

### Then you can add nuget package to your project:

```bash
dotnet add package [package name]
```

Or you can use package manager in Visual Studio or your specific IDE.