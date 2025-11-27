namespace Viasoft.Qualidade.RNC.Core.Domain.Extensions;

public static class ErpNumeroPedidoConventions
{
    public static string GetNumeroPedido(string numeroPedido, bool isMovimentacaoEstoque)
    {
        if (string.IsNullOrWhiteSpace(numeroPedido) 
            || numeroPedido == "0"
            || numeroPedido == "991")
        {
            if (isMovimentacaoEstoque)
            {
                return "ESTOQUE";
            }

            return "991";
        }

        return numeroPedido;
    }
}