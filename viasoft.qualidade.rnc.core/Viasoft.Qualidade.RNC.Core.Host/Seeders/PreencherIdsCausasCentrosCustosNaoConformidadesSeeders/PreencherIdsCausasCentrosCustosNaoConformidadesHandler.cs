using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Core.Domain.CausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherIdsCausasCentrosCustosNaoConformidadesSeeders.Contracts;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.PreencherIdsCausasCentrosCustosNaoConformidadesSeeders;

public class PreencherIdsCausasCentrosCustosNaoConformidadesHandler : IHandleMessages<SeedPreencherIdsCausasCentrosCustosNaoConformidadesMessage>
{
    private readonly IRepository<CentroCustoCausaNaoConformidade> _centrosCustosCausasNaoConformidades;
    private readonly IRepository<CausaNaoConformidade> _causasNaoConformidades;
    private readonly IRepository<SeederManager> _seederManagers;
    private readonly ICurrentCompany _currentCompany;
    private readonly IUnitOfWork _unitOfWork;
    private const int MaxResultCount = 2000;

    public PreencherIdsCausasCentrosCustosNaoConformidadesHandler(
        IRepository<CentroCustoCausaNaoConformidade> centrosCustosCausasNaoConformidades,
        IRepository<CausaNaoConformidade> causasNaoConformidades,
        IRepository<SeederManager> seederManagers,
        ICurrentCompany currentCompany,
        IUnitOfWork unitOfWork)
    {
        _centrosCustosCausasNaoConformidades = centrosCustosCausasNaoConformidades;
        _causasNaoConformidades = causasNaoConformidades;
        _seederManagers = seederManagers;
        _currentCompany = currentCompany;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SeedPreencherIdsCausasCentrosCustosNaoConformidadesMessage message)
    {
        await AtualizarDados();
        await AtualizarSeederManager();
    }

    private async Task AtualizarDados()
    {
        var skipCount = 0;

        while (true)
        {
            var agrupamentosCentrosCustosNaoConformidades = await _centrosCustosCausasNaoConformidades
                .AsNoTracking()
                .OrderBy(centroCustoNaoConformidade => centroCustoNaoConformidade.IdNaoConformidade)
                .GroupBy(centroCustoNaoConformidade => centroCustoNaoConformidade.IdNaoConformidade,
                    (idNaoConformidade, centrosCustosNaoConformidades) => new
                    {
                        IdNaoConformidade = idNaoConformidade,
                        CentrosCustosNaoConformidades = centrosCustosNaoConformidades.ToList()
                    })
                .Skip(skipCount)
                .Take(MaxResultCount)
                .ToListAsync();

            if (!agrupamentosCentrosCustosNaoConformidades.Any())
            {
                break;
            }

            var idsNaoConformidades = agrupamentosCentrosCustosNaoConformidades
                .Select(agrupamento => agrupamento.IdNaoConformidade)
                .ToList();

            var idsCausasNaoConformidades = await _causasNaoConformidades.AsNoTracking()
                .Where(causaNaoConformidade => idsNaoConformidades.Contains(causaNaoConformidade.IdNaoConformidade))
                .Select(causaNaoConformidade => causaNaoConformidade.Id)
                .ToListAsync();

            using (_unitOfWork.Begin())
            {
                foreach (var agrupamentoCentrosCustosNaoConformidades in agrupamentosCentrosCustosNaoConformidades)
                {
                    foreach (var idCausaNaoConformidade in idsCausasNaoConformidades)
                    {
                        var centrosCustosCausasNaoConformidades = agrupamentoCentrosCustosNaoConformidades.CentrosCustosNaoConformidades
                            .Select(centroCustoNaoConformidade => new CentroCustoCausaNaoConformidade
                            {
                                IdCentroCusto = centroCustoNaoConformidade.IdCentroCusto,
                                IdNaoConformidade = centroCustoNaoConformidade.IdNaoConformidade,
                                IdCausaNaoConformidade = idCausaNaoConformidade,
                                CompanyId = _currentCompany.Id
                            })
                            .ToList();

                        await _centrosCustosCausasNaoConformidades.InsertRangeAsync(centrosCustosCausasNaoConformidades);
                    }

                    foreach (var centroCustoNaoConformidade in agrupamentoCentrosCustosNaoConformidades.CentrosCustosNaoConformidades)
                    {
                        await _centrosCustosCausasNaoConformidades.DeleteAsync(centroCustoNaoConformidade.Id);
                    }
                }

                await _unitOfWork.CompleteAsync();
            }

            skipCount += MaxResultCount;
        }
    }

    private async Task AtualizarSeederManager()
    {
        var seederManager = await _seederManagers.FirstAsync();
        seederManager.PreencherIdsCausasCentrosCustosNaoConformidadesSeederFinalizado = true;

        await _seederManagers.UpdateAsync(seederManager, true);
    }
}
