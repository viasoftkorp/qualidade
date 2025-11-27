using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.EngenhariaCore.Recursos;

[Endpoint("Viasoft.Engenharia.Core.RecursoUpdated")]

public class RecursoUpdated : IMessage
{
    public string Codigo { get; set; }
    public string Descricao { get; set; }
    public Guid IdRecurso { get; set; }
}