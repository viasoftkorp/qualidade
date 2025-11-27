using System;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ErpPerson.Clientes.Models;

public class PersonModel
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; }
    public string Code { get; set; }

    public PersonModel()
    {
    }
}