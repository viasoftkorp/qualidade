using System;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ErpPerson.Clientes.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ErpPerson.Clientes;

[Endpoint("Viasoft.ERP.Person.PersonUpdated")]

public class PersonUpdated : IMessage
{
    public Guid Id { get; set; }
    public Guid EnvironmentId { get; set; }
    public DateTime AsOfDate { get; set; }
    public PersonModel Person { get; set; }
}