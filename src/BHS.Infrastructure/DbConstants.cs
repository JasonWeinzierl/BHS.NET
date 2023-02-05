using MongoDB.Driver;

namespace BHS.Infrastructure;

public static class DbConstants
{
    public const string BhsMongoDatabase = "bhs";

    /// <summary>
    /// Necessary to use <c>substrCP</c> instead of <c>substr</c>.
    /// </summary>
    internal static ExpressionTranslationOptions TranslationOptions { get; } = new() { StringTranslationMode = AggregateStringTranslationMode.CodePoints };
}
