using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Rebus.Handlers;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyFaturamentos.NotasFiscaisSaida.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.CorrigirNumeroNotaFiscalNaoConformidadesSeeder;

public class CorrigirNumeroNotaFiscalNaoConformidadesHandler :
    IHandleMessages<CorrigirNumeroNotaFiscalNaoConformidadesMessage>
{
    private readonly IRepository<SeederManager> _seederManagers;
    private readonly IRepository<NaoConformidade> _naoConformidades;
    private readonly INotaFiscalSaidaProvider _notaFiscalSaidaProvider;
    private readonly IUnitOfWork _unitOfWork;
    private const int MaxQueryCount = 100;
    private const int MaxQueryParamsCount = 70;

    public CorrigirNumeroNotaFiscalNaoConformidadesHandler(IRepository<SeederManager> seederManagers,
        IRepository<NaoConformidade> naoConformidades, INotaFiscalSaidaProvider notaFiscalSaidaProvider,
        IUnitOfWork unitOfWork)
    {
        _seederManagers = seederManagers;
        _naoConformidades = naoConformidades;
        _notaFiscalSaidaProvider = notaFiscalSaidaProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CorrigirNumeroNotaFiscalNaoConformidadesMessage message)
    {
        var skipCount = 0;

        while (true)
        {
            var naoConformidades = await _naoConformidades
                .Where(naoConformidade => naoConformidade.Origem == OrigemNaoConformidade.InpecaoSaida)
                .Where(naoConformidade => naoConformidade.IdNotaFiscal.HasValue)
                .OrderBy(naoConformidade => naoConformidade.Id)
                .PageBy(skipCount, MaxQueryCount)
                .ToListAsync();

            if (!naoConformidades.Any())
            {
                break;
            }

            var idsNotasFiscais = naoConformidades
                .Select(naoConformidade => naoConformidade.IdNotaFiscal.Value)
                .Distinct()
                .ToList();

            var numerosNotasFiscaisPorId = await GetNumerosNotasFiscaisPorId(idsNotasFiscais);

            using (_unitOfWork.Begin())
            {
                foreach (var naoConformidade in naoConformidades)
                {
                    var numeroNotaFiscal = numerosNotasFiscaisPorId[naoConformidade.IdNotaFiscal.Value];
                    naoConformidade.NumeroNotaFiscal = numeroNotaFiscal.ToString();

                    await _naoConformidades.UpdateAsync(naoConformidade);
                }

                await _unitOfWork.CompleteAsync();
            }

            skipCount += MaxQueryCount;
        }

        var seederManager = await _seederManagers.FirstAsync();
        seederManager.CorrigirNumeroNotaFiscalNaoConformidadesSeederFinalizado = true;
        await _seederManagers.UpdateAsync(seederManager, true);
    }

    private async Task<Dictionary<Guid, int?>> GetNumerosNotasFiscaisPorId(IReadOnlyCollection<Guid> idsNotasFiscais)
    {
        var skipCount = 0;
        var numerosNotasFiscaisPorId = new Dictionary<Guid, int?>();

        while (true)
        {
            var idsNotasFiscaisParaBuscar = idsNotasFiscais
                .Skip(skipCount)
                .Take(MaxQueryParamsCount)
                .ToList();

            if (!idsNotasFiscaisParaBuscar.Any())
            {
                break;
            }

            var advancedFilter = new JsonNetFilterRule
            {
                Condition = "and",
                Rules = new List<JsonNetFilterRule>
                {
                    new()
                    {
                        Field = "Id",
                        Type = "string",
                        Operator = "in",
                        Value = idsNotasFiscaisParaBuscar
                    }
                }
            };

            var input = new GetListNotasFiscaisInput
            {
                MaxResultCount = MaxQueryParamsCount,
                AdvancedFilter = JsonConvert.SerializeObject(advancedFilter)
            };

            var pagedResult = await _notaFiscalSaidaProvider.GetList(input);

            foreach (var notaFiscal in pagedResult.Items)
            {
                numerosNotasFiscaisPorId.Add(notaFiscal.Id, notaFiscal.NumeroNotaFiscal);
            }

            skipCount += MaxQueryParamsCount;
        }

        return numerosNotasFiscaisPorId;
    }
}