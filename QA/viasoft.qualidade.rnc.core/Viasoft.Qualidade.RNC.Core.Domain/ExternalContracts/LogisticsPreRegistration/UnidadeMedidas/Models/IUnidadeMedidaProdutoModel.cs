using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.LogisticsPreRegistration.UnidadeMedidas.Models;

public interface IUnidadeMedidaProdutoModel
{
    public Guid Id { get; set; }
    public string UnidadeMedida { get; set; }
}