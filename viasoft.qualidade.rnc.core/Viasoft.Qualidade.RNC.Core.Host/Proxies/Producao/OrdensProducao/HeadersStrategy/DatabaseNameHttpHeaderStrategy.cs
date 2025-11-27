using System.Collections.Generic;
using Viasoft.Core.AmbientData.Abstractions;
using Viasoft.Core.ApiClient.HttpHeaderStrategy;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.HeadersStrategy;

public class DatabaseNameHttpHeaderStrategy : AmbientDataCallOptionsHttpHeaderStrategy
{
    private readonly string _databaseName;

    public DatabaseNameHttpHeaderStrategy(AmbientDataCallOptions headerOptions, string databaseName) : base(headerOptions)
    {
        _databaseName = databaseName;
    }

    public DatabaseNameHttpHeaderStrategy(IAmbientDataCallOptionsResolver optionsResolver, string databaseName) : base(optionsResolver)
    {
        _databaseName = databaseName;
    }
    public override Dictionary<string, string> GetHeaders()
    {
        var headers = base.GetHeaders();
        headers["DatabaseName"] = _databaseName;
        return headers;
    }
}