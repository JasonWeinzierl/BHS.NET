namespace BHS.Web;

using System.Reflection;

static class AssemblyUtil
{
    /// <summary>
    /// Indicates whether the current assembly load is for the OpenAPI document generator.
    /// </summary>
    public static bool IsOpenApiGenerator => Assembly.GetEntryAssembly()?.GetName().Name == "GetDocument.Insider";

    /// <summary>
    /// Gets the major version of the current assembly.
    /// </summary>
    public static string MajorVersion => Assembly.GetExecutingAssembly().GetName().Version?.Major.ToString() ?? "next";

    /// <summary>
    /// Gets the semantic version of the current assembly.
    /// </summary>
    public static string SemVer => Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "next";
}
