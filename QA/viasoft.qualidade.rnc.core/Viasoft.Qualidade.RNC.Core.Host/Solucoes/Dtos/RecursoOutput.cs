using System;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.EngenhariaCore.Recursos.Models;

namespace Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

public class RecursoOutput : IRecursoModel
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Descricao { get; set; }
    public int LegacyId { get; set; }
    public decimal Produtividade { get; set; }
    public bool NaoApontar { get; set; }
}