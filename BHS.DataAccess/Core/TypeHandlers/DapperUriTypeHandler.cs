using Dapper;
using System;
using System.Data;

namespace BHS.DataAccess.Core.TypeHandlers
{
    public class DapperUriTypeHandler : SqlMapper.TypeHandler<Uri>
    {
        public override Uri Parse(object value)
        {
            Uri.TryCreate(value.ToString(), UriKind.RelativeOrAbsolute, out Uri uri);
            return uri;
        }

        public override void SetValue(IDbDataParameter parameter, Uri value)
        {
            parameter.Value = value.ToString();
        }
    }
}
