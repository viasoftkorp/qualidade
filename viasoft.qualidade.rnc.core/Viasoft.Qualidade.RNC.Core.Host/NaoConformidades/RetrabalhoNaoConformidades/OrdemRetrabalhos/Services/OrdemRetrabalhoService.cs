using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.ServiceBus.Abstractions;
using Viasoft.PushNotifications.Abstractions.Contracts;
using Viasoft.PushNotifications.Abstractions.Notification;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Locais;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Repositories;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades.Events;
using Viasoft.Qualidade.RNC.Core.Host.EstoqueLocais;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services.Estornos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services.Gerar;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos.Acls;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OrdemRetrabalhos.Services;

public class OrdemRetrabalhoService : IOrdemRetrabalhoService, ITransientDependency
{
    private readonly IRepository<OrdemRetrabalhoNaoConformidade> _repository;
    private readonly INaoConformidadeRepository _naoConformidadeRepository;
    private readonly IGerarOrdemRetrabalhoValidatorService _gerarOrdemRetrabalhoValidatorService;
    private readonly IEstornarOrdemRetrabalhoValidatorService _estornarOrdemRetrabalhoValidatorService;
    private readonly ILocalProvider _localProvider;
    private readonly IPushNotification _pushNotification;
    private readonly IEstoqueLocalAclService _estoqueLocalAclService;
    private readonly IOrdemRetrabalhoAclService _ordemRetrabalhoAclService;
    private readonly IExternalOrdemRetrabalhoService _externalOrdemRetrabalhoService;
    private readonly IRepository<Local> _locais;
    private readonly IServiceBus _serviceBus;

    public OrdemRetrabalhoService(IRepository<OrdemRetrabalhoNaoConformidade> repository,
        INaoConformidadeRepository naoConformidadeRepository,
        IGerarOrdemRetrabalhoValidatorService gerarOrdemRetrabalhoValidatorService,
        IEstornarOrdemRetrabalhoValidatorService estornarOrdemRetrabalhoValidatorService,
        ILocalProvider localProvider, IPushNotification pushNotification, IEstoqueLocalAclService estoqueLocalAclService, 
        IOrdemRetrabalhoAclService ordemRetrabalhoAclService, IExternalOrdemRetrabalhoService externalOrdemRetrabalhoService,
        IRepository<Local> locais, IServiceBus serviceBus)
    {
        _repository = repository;
        _naoConformidadeRepository = naoConformidadeRepository;
        _gerarOrdemRetrabalhoValidatorService = gerarOrdemRetrabalhoValidatorService;
        _estornarOrdemRetrabalhoValidatorService = estornarOrdemRetrabalhoValidatorService;
        _localProvider = localProvider;
        _pushNotification = pushNotification;
        _estoqueLocalAclService = estoqueLocalAclService;
        _ordemRetrabalhoAclService = ordemRetrabalhoAclService;
        _externalOrdemRetrabalhoService = externalOrdemRetrabalhoService;
        _locais = locais;
        _serviceBus = serviceBus;
    }

    public async Task<OrdemRetrabalhoNaoConformidadeOutput> GerarOrdemRetrabalho(AgregacaoNaoConformidade agregacaoNaoConformidade, OrdemRetrabalhoInput input)
    {
        var gerarOrdemRetrabalhoInput = new GerarOrdemRetrabalhoInput
        {
            Quantidade = input.Quantidade,
            NumeroPedido = agregacaoNaoConformidade.NaoConformidade.NumeroPedido,
            IdLocalDestino = input.IdLocalDestino,
            IdProduto = agregacaoNaoConformidade.NaoConformidade.IdProduto,
            IdPessoa = agregacaoNaoConformidade.NaoConformidade.IdPessoa,
            NumeroLote = agregacaoNaoConformidade.NaoConformidade.NumeroLote,
            NumeroOdfOrigem = agregacaoNaoConformidade.NaoConformidade.NumeroOdf.Value,
            MateriaisInput = agregacaoNaoConformidade.ProdutoNaoConformidades.Select(e => new GerarOrdemRetrabalhoMaterialInput
            {
                Quantidade = e.Quantidade,
                Operacao = e.OperacaoEngenharia,
                IdProduto = e.IdProduto
            }).ToList(),
            MaquinasInput = agregacaoNaoConformidade.ServicoNaoConformidades.Select(e => new GerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = e.OperacaoEngenharia,
                Detalhamento = e.Detalhamento,
                Horas = e.Horas,
                Minutos = e.Minutos,
                IdRecurso = e.IdRecurso,
                ControlarApontamento = e.ControlarApontamento,
            }).ToList()
        };
        
        var externalGerarOrdemRetrabalhoInput = await _ordemRetrabalhoAclService.GetExternalGerarOrdemRetrabalhoInput(gerarOrdemRetrabalhoInput);
        
        var result = await _externalOrdemRetrabalhoService.GerarOrdemRetrabalho(externalGerarOrdemRetrabalhoInput);
        
        if (result.Success)
        {
            var estoqueLocalOrigem = await _estoqueLocalAclService.GetById(input.IdEstoqueLocalOrigem);
            var localOrigem = await _localProvider.GetByCode(estoqueLocalOrigem.CodigoLocal);
            
            var ordemRetrabalhoNaoConformidade = new OrdemRetrabalhoNaoConformidade
            {
                IdNaoConformidade = agregacaoNaoConformidade.NaoConformidade.Id,
                NumeroOdfRetrabalho = result.OdfGerada,
                Quantidade = input.Quantidade,
                IdLocalDestino = input.IdLocalDestino,
                IdLocalOrigem = localOrigem.Id,
                CodigoArmazem = estoqueLocalOrigem.CodigoArmazem,
                DataFabricacao = estoqueLocalOrigem.DataFabricacao,
                DataValidade = estoqueLocalOrigem.DataValidade
            };

            await _repository.InsertAsync(ordemRetrabalhoNaoConformidade, true);
            await _pushNotification.SendAsync(new Payload
            {
                Header = "Odf retrabalho gerada com sucesso",
                Body = $"Número odf retrabalho: {result.OdfGerada}"
            }, agregacaoNaoConformidade.NaoConformidade.CreatorId.Value, true);
            await _serviceBus.Publish(new OrdemRetrabalhoNaoConformidadeInserida
            {
                OrdemRetrabalhoNaoConformidade = ordemRetrabalhoNaoConformidade
            });
        }
        
        return new OrdemRetrabalhoNaoConformidadeOutput
        {
            NumeroOdfRetrabalho = result.OdfGerada,
            Message = result.Message,
            Success = result.Success
        };
    }

    public async Task<OrdemRetrabalhoNaoConformidadeOutput> EstornarOrdemRetrabalho(AgregacaoNaoConformidade agregacaoNaoConformidade,
        OrdemRetrabalhoNaoConformidade ordemRetrabalhoNaoConformidade, bool notificarUsuario)
    {
        var externalInput = await _ordemRetrabalhoAclService.GetExternalEstornarOrdemRetrabalhoInput(agregacaoNaoConformidade.NaoConformidade.NumeroOdf.Value, 
            ordemRetrabalhoNaoConformidade);
        
        var result = await _externalOrdemRetrabalhoService.EstornarOrdemRetrabalho(externalInput);
        
        if (result.Success && notificarUsuario)
        {
            await _pushNotification.SendAsync(new Payload
            {
                Header = "Odf retrabalho estornada com sucesso",
                Body = $"Número odf retrabalho: {ordemRetrabalhoNaoConformidade.NumeroOdfRetrabalho}"
            }, agregacaoNaoConformidade.NaoConformidade.CreatorId.Value, true);
        }

        return result;
    }

    public async Task<OrdemRetrabalhoNaoConformidadeOutput> Get(Guid idNaoConformidade)
    {
        var ordemProducaoRetrabalho = await _repository
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.IdNaoConformidade == idNaoConformidade);

        if (ordemProducaoRetrabalho == null) return null;
        
        var output = new OrdemRetrabalhoNaoConformidadeOutput(ordemProducaoRetrabalho);
        return output;

    }

    public async Task<OrdemRetrabalhoNaoConformidadeViewOutput> GetView(Guid idNaoConformidade)
    {
        var query = from ordemRetrabalho in _repository.AsNoTracking()
            join localDestino in _locais.AsNoTracking()
                on ordemRetrabalho.IdLocalDestino equals localDestino.Id
            join localOrigem in _locais.AsNoTracking()
                on ordemRetrabalho.IdLocalOrigem equals localOrigem.Id
            select new OrdemRetrabalhoNaoConformidadeViewOutput
            {
                Quantidade = ordemRetrabalho.Quantidade,
                IdLocalDestino = ordemRetrabalho.IdLocalDestino,
                IdLocalOrigem = ordemRetrabalho.IdLocalOrigem,
                CodigoArmazem = ordemRetrabalho.CodigoArmazem,
                DataFabricacao = ordemRetrabalho.DataFabricacao,
                DataValidade = ordemRetrabalho.DataValidade,
                NumeroOdfRetrabalho = ordemRetrabalho.NumeroOdfRetrabalho,
                Status = ordemRetrabalho.Status,
                IdNaoConformidade = ordemRetrabalho.IdNaoConformidade,
                IdEstoqueLocalDestino = ordemRetrabalho.IdEstoqueLocalDestino,
                DescricaoLocalDestino = localDestino.Descricao,
                DescricaoLocalOrigem = localOrigem.Descricao,
                CodigoLocalDestino = localDestino.Codigo,
                CodigoLocalOrigem = localOrigem.Codigo
            };
        var ordemProducaoRetrabalho = await query
            .FirstOrDefaultAsync(e => e.IdNaoConformidade == idNaoConformidade);

        return ordemProducaoRetrabalho;
    }

    public async Task<GerarOrdemRetrabalhoValidationResult> CanGenerate(Guid idNaoConformidade,
        OrdemRetrabalhoInput input,
        bool isFullValidation)
    {
        var agregacao = await _naoConformidadeRepository.Get(idNaoConformidade);

        GerarOrdemRetrabalhoValidationResult result;
        if (isFullValidation)
        {
            result = await _gerarOrdemRetrabalhoValidatorService
                .ValidateStatusRnc()
                .ValidateOperacaoEngenhariaFinal()
                .ValidateOperacaoEngenhariaDuplicada()
                .ValidateOdf()
                .ValidateLote(input) 
                .ValidateAsync(agregacao);
        }
        else
        {
            result = await _gerarOrdemRetrabalhoValidatorService
                .ValidateStatusRnc()
                .ValidateOdf()
                .ValidateAsync(agregacao);
        }

        return result;
    }

    public async Task<EstornarOrdemRetrabalhoValidationResult> CanEstornar(Guid idNaoConformidade)
    {
        var agregacao = await _naoConformidadeRepository.Get(idNaoConformidade);

        var result = await _estornarOrdemRetrabalhoValidatorService
            .ValidateStatusRnc()
            .ValidateOdfRetrabalho()
            .ValidateHistoricoApontamento()
            .ValidateOrigemInspecaoSaida()
            .ValidateAsync(agregacao);

        return result;
    }
}