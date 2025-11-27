using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Produtos;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Recursos;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacaoRetrabalhoNaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Dtos.Acls;

namespace Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OperacoesRetrabalho.Services;

public class OperacaoRetrabalhoAclService : IOperacaoRetrabalhoAclService, ITransientDependency
{
    private readonly IRepository<Produto> _produtos;
    private readonly IRepository<Recurso> _recursos;

    public OperacaoRetrabalhoAclService(IRepository<Produto> produtos, IRepository<Recurso> recursos)
    {
        _produtos = produtos;
        _recursos = recursos;
    }
    public async Task<GerarOperacaoRetrabalhoExternalInput> GetGerarOperacaoRetrabalhoExternalInput(GerarOperacaoRetrabalhoAclInput input,
        List<MaquinaInput> maquinasInput)
    {
        var materiais = maquinasInput.SelectMany(maquina => maquina.Materiais).ToList();
        
        var codigosProdutosDictionary = await GetCodigoProdutosDictionary(materiais);

        var maquinas = await GetMaquinasByIds(maquinasInput.ConvertAll(e => e.IdRecurso));

        var operacoes = maquinasInput.Select(e =>
        {
            var materiaisDaOperacao = e.Materiais.Select(e =>
            {
                return new MaterialRetrabalhoExternalInput
                {
                    Quantidade = e.Quantidade,
                    CodigoProduto = codigosProdutosDictionary[e.IdProduto]
                };
            }).ToList();

            var maquina = maquinas.Find(maquina => maquina.Id == e.IdRecurso);
            
            return new OperacaoRetrabalhoExternalInput
            {
                Maquina = maquina.Codigo,
                Hora = e.Horas,
                Minuto = e.Minutos,
                Segundo = 0,
                DescricaoOperacao = e.Detalhamento,
                Materiais = materiaisDaOperacao
            };
        }).ToList();
        
        var gerarOperacaoRetrabalho = new GerarOperacaoRetrabalhoExternalInput
        {
            IdOrigem = input.OperacaoRetrabalhoNaoConformidade.Id,
            Odf = input.NumeroOdf.Value,
            SaldoRetrabalhar = input.OperacaoRetrabalhoNaoConformidade.Quantidade,
            Operacao = input.OperacaoRetrabalhoNaoConformidade.NumeroOperacaoARetrabalhar,
            Operacoes = operacoes
        };

        return gerarOperacaoRetrabalho;
    }
    
    public async Task<GerarOperacaoRetrabalhoAclOutput> GetGerarOperacaoRetrabalhoAclOutput(GerarOperacaoRetrabalhoExternalOutput gerarOperacaoRetrabalhoExternalOutput)
    {
        var operacoesAdicionadas = new List<OperacaoAclOutput>();
        var validationResult = OperacaoRetrabalhoNaoConformidadeValidationResult.Ok;
        if (gerarOperacaoRetrabalhoExternalOutput.Success)
        {
            var codigosMaquinas = gerarOperacaoRetrabalhoExternalOutput.OperacoesAdicionadas.ConvertAll(e => e.Maquina);
            var maquinas = await GetMaquinasByCodes(codigosMaquinas);

            operacoesAdicionadas = gerarOperacaoRetrabalhoExternalOutput.OperacoesAdicionadas.Select(e =>
            {
                var maquina = maquinas.Find(maquina => maquina.Codigo == e.Maquina);

                return new OperacaoAclOutput
                {
                    NumeroOperacao = e.Operacao,
                    IdMaquina = maquina.Id
                };
            }).ToList();
        }
        else
        {
            switch (gerarOperacaoRetrabalhoExternalOutput.Code)
            {
                case 1049:
                    validationResult = OperacaoRetrabalhoNaoConformidadeValidationResult.GerarRetrabalhoOp999;
                    break;
                case 1050:
                    validationResult = OperacaoRetrabalhoNaoConformidadeValidationResult.GerarRetrabalhoOpRetrabalho;
                    break;
                case 1051:
                    validationResult = OperacaoRetrabalhoNaoConformidadeValidationResult
                        .GerarRetrabalhoOpSecundariaSaldoOdfZerado;
                    break;
                case 1052:
                    validationResult = OperacaoRetrabalhoNaoConformidadeValidationResult
                        .GerarRetrabalhoOpSecundariaTamanhoOperacoesNaoPadronizados;
                    break;
                case 1053:
                    validationResult = OperacaoRetrabalhoNaoConformidadeValidationResult
                        .GerarRetrabalhoOpSecundariaLimiteOperacoesAtingido;
                    break;
                case 1054:
                    validationResult = OperacaoRetrabalhoNaoConformidadeValidationResult
                        .GerarRetrabalhoOpSecundariaSaldoOperacaoIndisponivel;
                    break;
                case 1055:
                    validationResult = OperacaoRetrabalhoNaoConformidadeValidationResult
                        .GerarRetrabalhoOpSecundariaSaldoOperacaoInsuficiente;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        var output = new GerarOperacaoRetrabalhoAclOutput
        {
            OperacoesAdicionadas = operacoesAdicionadas,
            Success = gerarOperacaoRetrabalhoExternalOutput.Success,
            Message = gerarOperacaoRetrabalhoExternalOutput.Message,
            ValidationResult = validationResult
        };
         
        return output;
    }

    private async Task<Dictionary<Guid, string>> GetCodigoProdutosDictionary(List<MaterialInput> materiais)
    {
        var idsProdutos = materiais.ConvertAll(e => e.IdProduto);
        var output = await _produtos
            .Where(e => idsProdutos.Contains(e.Id))
            .ToDictionaryAsync(e => e.Id, e => e.Codigo);
        return output;
    }

    private async Task<List<Recurso>> GetMaquinasByIds(List<Guid> idsMaquinas)
    {
        var output = await _recursos
            .Where(e => idsMaquinas.Contains(e.Id))
            .ToListAsync();        
        return output;
    }
    private async Task<List<Recurso>> GetMaquinasByCodes(List<string> codigosMaquinas)
    {
        var output = await _recursos
            .Where(e => codigosMaquinas.Contains(e.Codigo))
            .ToListAsync();        
        return output;
    }
}