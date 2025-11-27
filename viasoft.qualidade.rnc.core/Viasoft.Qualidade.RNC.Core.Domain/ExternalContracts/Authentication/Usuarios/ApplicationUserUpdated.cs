using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.Authentication.Usuarios;

[Endpoint("Authentication.ApplicationUserUpdated")]
public class ApplicationUserUpdated : IMessage
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string SecondName { get; set; }
}