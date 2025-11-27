using System;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.LogisticsPreRegistration.UnidadeMedidas.Models;

namespace Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

public class UnidadeMedidaProdutoOutput : IUnidadeMedidaProdutoModel
{
    public Guid Id { get; set; }
    public string UnidadeMedida { get; set; }
    
}