using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Qualidade.RNC.Core.Domain.Extensions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Clientes;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.ProdutosEmpresas;
using Viasoft.Qualidade.RNC.Core.Domain.OrdemRetrabalhoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyLogisticas.Locais.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyParametros.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Dtos.Acls;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticsPreRegistrations.CategoriaProdutos.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Recursos;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.LogisticaServices.ExternalOrdemRetrabalho.Services;

public class OrdemRetrabalhoAclService : IOrdemRetrabalhoAclService, ITransientDependency
{
    private readonly ICurrentCompany _currentCompany;
    private readonly ICategoriaProdutoProvider _categoriaProdutoProvider;
    private readonly IOrdemProducaoProvider _ordemProducaoProvider;
    private readonly IRecursosProxyService _recursosProxyService;
    private readonly IRepository<Cliente> _clientesRepository;
    private readonly IRepository<Produto> _produtosRepository;
    private readonly IRepository<ProdutoEmpresa> _produtosEmpresasRepository;
    private readonly ILocalProvider _localProvider;
    private readonly ILegacyParametrosProvider _legacyParametrosProvider;

    public OrdemRetrabalhoAclService(ICurrentCompany currentCompany, 
        ICategoriaProdutoProvider categoriaProdutoProvider, IOrdemProducaoProvider ordemProducaoProvider,
        IRecursosProxyService recursosProxyService, IRepository<Cliente> clientesRepository, IRepository<Produto> produtosRepository, 
        IRepository<ProdutoEmpresa> produtosEmpresasRepository, ILocalProvider localProvider, ILegacyParametrosProvider legacyParametrosProvider)
    {
        _currentCompany = currentCompany;
        _categoriaProdutoProvider = categoriaProdutoProvider;
        _ordemProducaoProvider = ordemProducaoProvider;
        _recursosProxyService = recursosProxyService;
        _clientesRepository = clientesRepository;
        _produtosRepository = produtosRepository;
        _produtosEmpresasRepository = produtosEmpresasRepository;
        _localProvider = localProvider;
        _legacyParametrosProvider = legacyParametrosProvider;
    }

    public async Task<ExternalGerarOrdemRetrabalhoInput> GetExternalGerarOrdemRetrabalhoInput(GerarOrdemRetrabalhoInput input)
    {
        var codigoProduto = await GetCodigoProduto(input.IdProduto);
        var materias = await GetMaterias(input.MateriaisInput, codigoProduto);
        var maquinas = await GetMaquinas(input.MaquinasInput, materias, codigoProduto);

        var numeroPedido =
            ErpNumeroPedidoConventions.GetNumeroPedido(input.NumeroPedido, false);

        var ordemProducao = await _ordemProducaoProvider.GetByNumeroOdf(input.NumeroOdfOrigem, false);

        var numeroOdfOrigem = ordemProducao.NumeroOdf;

        var utilizarReservaDePedidoNaLocalizacaoDeEstoque = await _legacyParametrosProvider.GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque();
        if (utilizarReservaDePedidoNaLocalizacaoDeEstoque)
        {
            numeroOdfOrigem = ordemProducao.NumeroOdfDestino.Value;
        }
        var localDestino = await _localProvider.GetById(input.IdLocalDestino);
        
        var externalInput = new ExternalGerarOrdemRetrabalhoInput
        {
            IdEmpresa = _currentCompany.LegacyId,
            Quantidade = input.Quantidade,
            CodigoProduto = codigoProduto,
            CodigoCliente = await GetCodigoCliente(input.IdPessoa),
            DataEntrega = ordemProducao.DataEntrega.AddDateMask(),
            Pedido = numeroPedido,
            OdfOrigem = numeroOdfOrigem,
            Servico = false,
            Projetar = false,
            Retrabalho = true,
            AnalisarReversa = false,
            Lote = input.NumeroLote,
            LocalDestino = localDestino.Codigo,
            Materias = materias,
            Maquinas = maquinas
        };

        return externalInput;
    }

    public async Task<ExternalEstornarOrdemRetrabalhoInput> GetExternalEstornarOrdemRetrabalhoInput(int numeroOdfOrigem,
        OrdemRetrabalhoNaoConformidade ordemRetrabalhoNaoConformidade)
    {
        var odfRetrabalho = await _ordemProducaoProvider.GetByNumeroOdf(ordemRetrabalhoNaoConformidade.NumeroOdfRetrabalho, false);
        
        var numeroOdf = numeroOdfOrigem;
        
        var utilizarReservaDePedidoNaLocalizacaoDeEstoque = await _legacyParametrosProvider.GetUtilizarReservaDePedidoNaLocalizacaoDeEstoque();
        if (utilizarReservaDePedidoNaLocalizacaoDeEstoque)
        {
            numeroOdf = odfRetrabalho.NumeroOdfDestino.Value;
        }

        var externalInput = new ExternalEstornarOrdemRetrabalhoInput
        {
            Odf = odfRetrabalho.NumeroOdf,
            OdfVenda = numeroOdf,
            Quantidade = ordemRetrabalhoNaoConformidade.Quantidade,
            SaldoOdf = odfRetrabalho.Quantidade,
            Motivo = "Estornada Ordem de Retrabalho pelo RNC",
            Situacao = "991",
            CodigoProduto = await GetCodigoProduto(odfRetrabalho.IdProduto),
            PedidoVenda = ErpNumeroPedidoConventions.GetNumeroPedido(odfRetrabalho.NumeroPedido, false)
        };
        return externalInput;
    }
    private async Task<List<ExternalGerarOrdemRetrabalhoMaterialInput>> GetMaterias(List<GerarOrdemRetrabalhoMaterialInput> input,
        string codigoProduto)
    {
        var idsProdutos = input.ConvertAll(e => e.IdProduto);
        var produtos = await _produtosRepository.AsNoTracking()
            .Where(e => idsProdutos.Contains(e.Id))
            .Join(_produtosEmpresasRepository.AsNoTracking()
                    .Where(produtoEmpresa => produtoEmpresa.IdEmpresa == _currentCompany.Id),
                produto => produto.Id,
                produtoEmpresa => produtoEmpresa.Id,
                (produto, produtoEmpresa) => new
                {
                    produto.Id,
                    produto.Codigo,
                    produtoEmpresa.IdCategoria
                })
            .ToListAsync();

        var idsCategorias = produtos
            .Select(e => e.IdCategoria)
            .ToList();

        var categorias = await _categoriaProdutoProvider.GetAllCategoriasPaginando(idsCategorias);
        var materiais = input
            .GroupBy(e => e.Operacao)
            .SelectMany(agrupamento => agrupamento.Select((e, index) =>
            {
                var produto = produtos.Find(produto => produto.Id == e.IdProduto);
                var categoria = categorias.Find(categoria => categoria.Id == produto.IdCategoria);
                var materia = new ExternalGerarOrdemRetrabalhoMaterialInput
                {
                    Quantidade = e.Quantidade,
                    CodigoProduto = produto.Codigo,
                    Categoria = categoria.Codigo,
                    Operacao = e.Operacao,
                    Sequencia = (index + 1).ToString(),
                    Revisao = "",
                    IdRoteiro = 0,
                    Peca = codigoProduto,
                    PecaPai = "",
                    Nivel = 0,
                    Posicao = "",
                    Expedicao = "N",
                    NaoGerarCusto = "N",
                    Reaproveitado = "N",
                    AjustarQtdApontamento = "N",
                    LocalConsumo = 0
                };
                return materia;
            })).ToList();
        return materiais;
    }

    private async Task<List<ExternalGerarOrdemRetrabalhoMaquinaInput>> GetMaquinas(
        List<GerarOrdemRetrabalhoMaquinaInput> input,
        List<ExternalGerarOrdemRetrabalhoMaterialInput> materiais,
        string codigoProduto)
    {
        var idsRecursos = input
            .Select(e => e.IdRecurso)
            .ToList();
        var recursos = await _recursosProxyService.GetAllByIdsPaginando(idsRecursos);

        var maquinas = input.Select(e =>
        {
            var materiaisDestaOperacaoOrdenadosPelaSequencia = materiais
                .Where(material => material.Operacao == e.Operacao)
                .OrderBy(material => material.Sequencia)
                .ToList();
            var ultimaSequenciaDestaOperacao = 0;
            if (materiaisDestaOperacaoOrdenadosPelaSequencia.Any())
            {
                var ultimoMaterialEmSequencia = materiaisDestaOperacaoOrdenadosPelaSequencia.Last();
                ultimaSequenciaDestaOperacao = int.TryParse(ultimoMaterialEmSequencia.Sequencia, out var outInt)
                    ? outInt
                    : 0;
            }

            var maquina = recursos.Find(recurso => recurso.Id == e.IdRecurso);
            
            return new ExternalGerarOrdemRetrabalhoMaquinaInput
            {
                Operacao = e.Operacao,
                IdMaquina = maquina.LegacyId,
                DescricaoOperacao = e.Detalhamento,
                QuantidadeHoras = e.TempoTotal,
                Sequencia = (ultimaSequenciaDestaOperacao + 1).ToString(),
                ProdutividadeMaquina = maquina.Produtividade,
                ApontarMaquina = !maquina.NaoApontar,
                Regula = 0,
                Peca = codigoProduto,
                PecaPai = "",
                Nivel = 0,
                Posicao = "",
                ConfirmarMateriais = "N",
                ControlarApontamento = "N"
            };
        }).ToList();
        return maquinas;
    }

    private async Task<string> GetCodigoCliente(Guid? idCliente)
    {
        if (idCliente.HasValue)
        {
            var cliente = await _clientesRepository.FirstAsync(e => e.Id == idCliente);
            return cliente.Codigo;
        }

        return "000";
    }

    private async Task<string> GetCodigoProduto(Guid idProduto)
    {
        var produto = await _produtosRepository.FirstAsync(e => e.Id == idProduto);
        return produto.Codigo;
    }
}