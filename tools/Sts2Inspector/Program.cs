using System.Reflection;
using System.Runtime.InteropServices;

var sts2Path = args.Length > 0 ? args[0] : GetDefaultSts2Path();
if (!File.Exists(sts2Path))
{
    Console.Error.WriteLine($"STS2 assembly not found: {sts2Path}");
    Console.Error.WriteLine("Usage: dotnet run --project tools/Sts2Inspector/Sts2Inspector.csproj -- /path/to/sts2.dll");
    return 1;
}

var terms = new[] { "Multiplayer", "Save", "RunSave", "Lobby", "HostSubmenu", "LoadGame" };
var assembly = Assembly.LoadFrom(sts2Path);
var types = GetLoadableTypes(assembly);

foreach (var type in types
    .Where(type => terms.Any(term => type.FullName?.Contains(term, StringComparison.OrdinalIgnoreCase) == true))
    .OrderBy(type => type.FullName))
{
    Console.WriteLine($"## {FormatType(type)}");
    foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
        .OrderBy(method => method.Name))
    {
        var parameters = string.Join(", ", method.GetParameters().Select(parameter => $"{FormatType(parameter.ParameterType)} {parameter.Name}"));
        Console.WriteLine($"- method `{method.Name}({parameters})` -> `{FormatType(method.ReturnType)}`");
    }

    foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
        .OrderBy(field => field.Name))
    {
        Console.WriteLine($"- field `{FormatType(field.FieldType)} {field.Name}`");
    }

    Console.WriteLine();
}

return 0;

static string GetDefaultSts2Path()
{
    var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    return Path.Combine(GetDefaultSts2DataPath(home), "sts2.dll");
}

static string GetDefaultSts2DataPath(string home)
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        return Path.Combine(home, ".local/share/Steam/steamapps/common/Slay the Spire 2/data_sts2_linuxbsd_x86_64");
    }

    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
        return Path.Combine(home, "Library/Application Support/Steam/steamapps/common/Slay the Spire 2/data_sts2_macos_universal");
    }

    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        return Path.Combine(programFiles, "Steam/steamapps/common/Slay the Spire 2/data_sts2_windows_x86_64");
    }

    return Path.Combine(home, "Slay the Spire 2");
}

static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
{
    try
    {
        return assembly.GetTypes();
    }
    catch (ReflectionTypeLoadException ex)
    {
        foreach (var loaderException in ex.LoaderExceptions)
        {
            if (!string.IsNullOrWhiteSpace(loaderException?.Message))
            {
                Console.Error.WriteLine(loaderException.Message);
            }
        }

        return ex.Types.Where(type => type is not null).Cast<Type>();
    }
}

static string FormatType(Type type)
{
    if (type.IsGenericParameter)
    {
        return type.Name;
    }

    if (type.IsArray)
    {
        return $"{FormatType(type.GetElementType()!)}[]";
    }

    if (type.IsByRef)
    {
        return $"{FormatType(type.GetElementType()!)}&";
    }

    if (type.IsPointer)
    {
        return $"{FormatType(type.GetElementType()!)}*";
    }

    if (!type.IsGenericType)
    {
        return (type.FullName ?? type.Name).Replace('+', '.');
    }

    var genericTypeDefinition = type.GetGenericTypeDefinition();
    var fullName = genericTypeDefinition.FullName ?? genericTypeDefinition.Name;
    var tickIndex = fullName.IndexOf('`', StringComparison.Ordinal);
    var genericName = tickIndex >= 0 ? fullName[..tickIndex] : fullName;
    var genericArguments = string.Join(", ", type.GetGenericArguments().Select(FormatType));
    return $"{genericName.Replace('+', '.')}<{genericArguments}>";
}
