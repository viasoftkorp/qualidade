using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Application.Dto.Paged;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Core.MultiTenancy.Abstractions.Company;
using Viasoft.Data.Extensions.Filtering.AdvancedFilter;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Clientes;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.Naturezas;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services;

public class NaoConformidadeViewService : INaoConformidadeViewService, ITransientDependency
{
    private readonly IRepository<NaoConformidade> _naoConformidades;
    private readonly IRepository<Produto> _produtos;
    private readonly IRepository<Cliente> _clientes;
    private readonly IRepository<Natureza> _naturezas;
    private readonly IRepository<Usuario> _usuarios;
    private readonly ICurrentCompany _currentCompany;

    public NaoConformidadeViewService(IRepository<NaoConformidade> naoConformidades, IRepository<Produto> produtos,
        IRepository<Cliente> clientes, IRepository<Natureza> naturezas, IRepository<Usuario> usuarios,
        ICurrentCompany currentCompany)
    {
        _naoConformidades = naoConformidades;
        _produtos = produtos;
        _clientes = clientes;
        _naturezas = naturezas;
        _usuarios = usuarios;
        _currentCompany = currentCompany;
    }

    public async Task<PagedResultDto<NaoConformidadeViewOutput>> GetListView(PagedFilteredAndSortedRequestInput input)
    {
        var query = GetQuery()
            .Select(result => new NaoConformidadeViewOutput(result.Natureza, result.Produto, result.NaoConformidade,
                result.Usuario)
            {
                Id = result.NaoConformidade.Id,
                Codigo = result.NaoConformidade.Codigo,
                Origem = result.NaoConformidade.Origem,
                Status = result.NaoConformidade.Status,
                IdNotaFiscal = result.NaoConformidade.IdNotaFiscal,
                NumeroNotaFiscal = result.NaoConformidade.NumeroNotaFiscal,
                IdNatureza = result.NaoConformidade.Id,
                DescricaoNatureza = result.Natureza.Descricao,
                CodigoNatureza = result.Natureza.Codigo,
                Natureza = result.Natureza.Codigo + " - " + result.Natureza.Descricao,
                NumeroOdf = result.NaoConformidade.NumeroOdf,
                IdProduto = result.Produto.Id,
                DescricaoProduto = result.Produto.Descricao,
                CodigoProduto = result.Produto.Codigo,
                Produto = result.Produto.Codigo + " - " + result.Produto.Descricao,
                Revisao = result.NaoConformidade.Revisao,
                Equipe = result.NaoConformidade.Equipe,
                IdLote = result.NaoConformidade.IdLote,
                NumeroLote = result.NaoConformidade.NumeroLote,
                DataFabricacaoLote = result.NaoConformidade.DataFabricacaoLote,
                CampoNf = result.NaoConformidade.CampoNf,
                IdCriador = result.Usuario.Id,
                LoteTotal = result.NaoConformidade.LoteTotal,
                LoteParcial = result.NaoConformidade.LoteParcial,
                Rejeitado = result.NaoConformidade.Rejeitado,
                AceitoConcessao = result.NaoConformidade.AceitoConcessao,
                RetrabalhoPeloCliente = result.NaoConformidade.RetrabalhoPeloCliente,
                RetrabalhoNoCliente = result.NaoConformidade.RetrabalhoNoCliente,
                NaoConformidadeEmPotencial = result.NaoConformidade.NaoConformidadeEmPotencial,
                RelatoNaoConformidade = result.NaoConformidade.RelatoNaoConformidade,
                MelhoriaEmPotencial = result.NaoConformidade.MelhoriaEmPotencial,
                Descricao = result.NaoConformidade.Descricao,
                Incompleta = result.NaoConformidade.Incompleta,
                
                IdFornecedor = result.NaoConformidade.Origem == OrigemNaoConformidade.InspecaoEntrada ? result.Cliente.Id : null,
                NomeFornecedor = result.NaoConformidade.Origem == OrigemNaoConformidade.InspecaoEntrada ? result.Cliente.RazaoSocial : "",
                CodigoFornecedor = result.NaoConformidade.Origem == OrigemNaoConformidade.InspecaoEntrada ? result.Cliente.Codigo : "",
                Fornecedor = result.NaoConformidade.Origem == OrigemNaoConformidade.InspecaoEntrada && result.Cliente.Codigo != null 
                    ? result.Cliente.Codigo + " - " + result.Cliente.RazaoSocial 
                    : "",
                IdCliente = result.NaoConformidade.Origem != OrigemNaoConformidade.InspecaoEntrada ? result.Cliente.Id : null,
                NomeCliente = result.NaoConformidade.Origem != OrigemNaoConformidade.InspecaoEntrada ? result.Cliente.RazaoSocial : "",
                CodigoCliente = result.NaoConformidade.Origem != OrigemNaoConformidade.InspecaoEntrada ? result.Cliente.Codigo : "",
                Cliente = result.NaoConformidade.Origem != OrigemNaoConformidade.InspecaoEntrada && result.Cliente.Codigo != null 
                    ? result.Cliente.Codigo + " - " + result.Cliente.RazaoSocial 
                    : "",
                
                
            })
            .ApplyAdvancedFilter(input.AdvancedFilter, input.Sorting);

        var totalCount = await query.CountAsync();
        
        var itens = await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .ToListAsync();

        var output = new PagedResultDto<NaoConformidadeViewOutput>(totalCount, itens);
        return output;
    }

    public async Task<NaoConformidadeViewOutput> GetView(Guid idNaoConformidade)
    {
        var query = GetQuery()
            .Where(result => result.NaoConformidade.Id == idNaoConformidade)
            .Select(result => new NaoConformidadeViewOutput(result.Natureza, result.Produto, result.NaoConformidade,
                result.Usuario)
            {
                Id = result.NaoConformidade.Id,
                Codigo = result.NaoConformidade.Codigo,
                Origem = result.NaoConformidade.Origem,
                Status = result.NaoConformidade.Status,
                IdNotaFiscal = result.NaoConformidade.IdNotaFiscal,
                NumeroNotaFiscal = result.NaoConformidade.NumeroNotaFiscal,
                IdNatureza = result.NaoConformidade.Id,
                DescricaoNatureza = result.Natureza.Descricao,
                CodigoNatureza = result.Natureza.Codigo,
                Natureza = $"{result.Natureza.Codigo} - {result.Natureza.Descricao}",
                NumeroOdf = result.NaoConformidade.NumeroOdf,
                IdProduto = result.Produto.Id,
                DescricaoProduto = result.Produto.Descricao,
                CodigoProduto = result.Produto.Codigo,
                Produto = $"{result.Produto.Codigo} - {result.Produto.Descricao}",
                Revisao = result.NaoConformidade.Revisao,
                Equipe = result.NaoConformidade.Equipe,
                IdLote = result.NaoConformidade.IdLote,
                NumeroLote = result.NaoConformidade.NumeroLote,
                DataFabricacaoLote = result.NaoConformidade.DataFabricacaoLote,
                CampoNf = result.NaoConformidade.CampoNf,
                IdCriador = result.Usuario.Id,
                LoteTotal = result.NaoConformidade.LoteTotal,
                LoteParcial = result.NaoConformidade.LoteParcial,
                Rejeitado = result.NaoConformidade.Rejeitado,
                AceitoConcessao = result.NaoConformidade.AceitoConcessao,
                RetrabalhoPeloCliente = result.NaoConformidade.RetrabalhoPeloCliente,
                RetrabalhoNoCliente = result.NaoConformidade.RetrabalhoNoCliente,
                NaoConformidadeEmPotencial = result.NaoConformidade.NaoConformidadeEmPotencial,
                RelatoNaoConformidade = result.NaoConformidade.RelatoNaoConformidade,
                MelhoriaEmPotencial = result.NaoConformidade.MelhoriaEmPotencial,
                Descricao = result.NaoConformidade.Descricao,
                NomeUsuarioCriador = result.Usuario.Nome,
                SobrenomeUsuarioCriador = result.Usuario.Sobrenome,
                IdFornecedor = result.NaoConformidade.Origem == OrigemNaoConformidade.InspecaoEntrada ? result.Cliente.Id : null,
                NomeFornecedor = result.NaoConformidade.Origem == OrigemNaoConformidade.InspecaoEntrada ? result.Cliente.RazaoSocial : "",
                CodigoFornecedor = result.NaoConformidade.Origem == OrigemNaoConformidade.InspecaoEntrada ? result.Cliente.Codigo : "",
                Fornecedor = result.NaoConformidade.Origem == OrigemNaoConformidade.InspecaoEntrada && result.Cliente.Codigo != null 
                    ? result.Cliente.Codigo + " - " + result.Cliente.RazaoSocial 
                    : "",
                IdCliente = result.NaoConformidade.Origem != OrigemNaoConformidade.InspecaoEntrada ? result.Cliente.Id : null,
                NomeCliente = result.NaoConformidade.Origem != OrigemNaoConformidade.InspecaoEntrada ? result.Cliente.RazaoSocial : "",
                CodigoCliente = result.NaoConformidade.Origem != OrigemNaoConformidade.InspecaoEntrada ? result.Cliente.Codigo : "",
                Cliente = result.NaoConformidade.Origem != OrigemNaoConformidade.InspecaoEntrada && result.Cliente.Codigo != null 
                    ? result.Cliente.Codigo + " - " + result.Cliente.RazaoSocial 
                    : "",
            });

        var output = await query.FirstAsync();
        return output;
    }

    private IQueryable<ReturnQuery> GetQuery()
    {
        var query = from naoConformidade in _naoConformidades.AsNoTracking()
                .Where(e => e.CompanyId == _currentCompany.Id)
            join clientes in _clientes.AsNoTracking()
                on naoConformidade.IdPessoa equals clientes.Id
                into clientesJoinedTable
            from cliente in clientesJoinedTable.DefaultIfEmpty()
            join produtos in _produtos.AsNoTracking()
                on naoConformidade.IdProduto equals produtos.Id
                into produtoJoinedTable
            from produto in produtoJoinedTable.DefaultIfEmpty()
            join naturezas in _naturezas.AsNoTracking()
                on naoConformidade.IdNatureza equals naturezas.Id
                into naturezaJoinedTable
            from natureza in naturezaJoinedTable.DefaultIfEmpty()
            join usuarios in _usuarios.AsNoTracking()
                on naoConformidade.IdCriador equals usuarios.Id
                into usuarioJoinedTable
            from usuario in usuarioJoinedTable.DefaultIfEmpty()
            select new ReturnQuery
            {
                NaoConformidade = naoConformidade,
                Cliente = cliente,
                Produto = produto,
                Natureza = natureza,
                Usuario = usuario
            };
        return query;
    }

    private class ReturnQuery
    {
        public NaoConformidade NaoConformidade { get; set; }
        public Cliente Cliente { get; set; }
        public Produto Produto { get; set; }
        public Natureza Natureza { get; set; }
        public Usuario Usuario { get; set; }
    }
}