namespace Viasoft.Qualidade.RNC.Gateway.Domain.Authorizations.Policies;

public class RetrabalhoPolicies
{
    // Ordem retrabalho
    public const string GerarOdfRetrabalhoPolicy = "NaoConformidades.Retrabalho.OrdemRetrabalho.Create";
    public const string EstornarOdfRetrabalhoPolicy = "NaoConformidades.Retrabalho.OrdemRetrabalho.Delete";
    
    // Operacao Retrabalho
    public const string GerarOperacaoRetrabalhoPolicy = "NaoConformidades.Retrabalho.OperacaoRetrabalho.Create";
    
}