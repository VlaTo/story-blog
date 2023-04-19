namespace SlimMessageBus.Host.NamedPipe.Core;

internal static class NamedPipeNamePartition
{
    public static string GetPipeName(string? path)
    {
        return path ?? "(default)";
    }
}