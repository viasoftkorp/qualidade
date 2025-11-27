using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades.Events;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Locais;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Handlers;

public class OrdemRetrabalhoHandler : IHandleMessages<OrdemRetrabalhoNaoConformidadeInserida>
{
    private readonly ILocalService _localService;

    public OrdemRetrabalhoHandler(ILocalService localService)
    {
        _localService = localService;
    }
    public async Task Handle(OrdemRetrabalhoNaoConformidadeInserida message)
    {
        var idsToInsert = new List<Guid>
        {
            message.OrdemRetrabalhoNaoConformidade.IdLocalOrigem,
            message.OrdemRetrabalhoNaoConformidade.IdLocalDestino
        };
        
        await _localService.BatchInserirNaoCadastrados(idsToInsert);
    }
}