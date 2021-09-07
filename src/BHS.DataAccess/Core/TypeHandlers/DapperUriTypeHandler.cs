using Dapper;

namespace BHS.DataAccess.Core.TypeHandlers
{
    public class DapperUriTypeHandler : SqlMapper.StringTypeHandler<Uri>
    {
        public static DapperUriTypeHandler Default => new();

        protected override string? Format(Uri? xml)
        {
            return xml?.ToString();
        }

        // TODO: Remove this when Dapper implements nullable.
#pragma warning disable CS8764 // Nullability of return type doesn't match overridden member (possibly because of nullability attributes).
        protected override Uri? Parse(string? xml)
#pragma warning restore CS8764 // Nullability of return type doesn't match overridden member (possibly because of nullability attributes).
        {
            Uri.TryCreate(xml, UriKind.RelativeOrAbsolute, out Uri? uri);
            return uri;
        }
    }
}
