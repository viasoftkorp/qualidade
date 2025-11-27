using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.EngenhariaCore.Recursos.Models;

public interface IRecursoModel
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Descricao { get; set; }
    
}