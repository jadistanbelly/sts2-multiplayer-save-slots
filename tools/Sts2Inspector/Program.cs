using System.Reflection;

var sts2Path = args.Length > 0
    ? args[0]
    : Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".local/share/Steam/steamapps/common/Slay the Spire 2/data_sts2_linuxbsd_x86_64/sts2.dll");

var assembly = Assembly.LoadFrom(sts2Path);
var terms = new[] { "Multiplayer", "Save", "RunSave", "Lobby", "HostSubmenu", "LoadGame" };

foreach (var type in assembly.GetTypes()
    .Where(type => terms.Any(term => type.FullName?.Contains(term, StringComparison.OrdinalIgnoreCase) == true))
    .OrderBy(type => type.FullName))
{
    Console.WriteLine($"## {type.FullName}");
    foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
        .OrderBy(method => method.Name))
    {
        var parameters = string.Join(", ", method.GetParameters().Select(parameter => $"{parameter.ParameterType.Name} {parameter.Name}"));
        Console.WriteLine($"- method `{method.Name}({parameters})` -> `{method.ReturnType.Name}`");
    }

    foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
        .OrderBy(field => field.Name))
    {
        Console.WriteLine($"- field `{field.FieldType.Name} {field.Name}`");
    }

    Console.WriteLine();
}
