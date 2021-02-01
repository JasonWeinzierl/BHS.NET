using Dapper;
using System;

namespace BHS.DataAccess.Core.TypeHandlers
{
    public class DapperUriTypeHandler : SqlMapper.StringTypeHandler<Uri>
    {
        public static DapperUriTypeHandler Default => new DapperUriTypeHandler();

        protected override string Format(Uri xml)
        {
            return xml.ToString();
        }

        protected override Uri Parse(string xml)
        {
            Uri.TryCreate(xml, UriKind.RelativeOrAbsolute, out Uri uri);
            return uri;
        }
    }
}
