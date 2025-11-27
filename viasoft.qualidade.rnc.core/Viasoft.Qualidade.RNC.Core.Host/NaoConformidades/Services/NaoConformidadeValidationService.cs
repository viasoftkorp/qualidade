using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Viasoft.Core.DynamicLinqQueryBuilder;
using Viasoft.Core.IoC.Abstractions;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntrada.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntradaRateioLote.Dtos;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.ItemNotasFiscaisEntradaRateioLote.Providers;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Producao.OrdensProducao.Providers;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Services
{
    public class NaoConformidadeValidationService : INaoConformidadeValidationService, ITransientDependency
    {
        private readonly IItemNotaFiscalEntradaProvider _itemNotaFiscalEntradaProvider;
        private readonly IOrdemProducaoProvider _ordemProducaoProvider;
        private readonly IItemNotaFiscalEntradaRateioLoteProvider _itemNotaFiscalEntradaRateioLoteProvider;
        private List<ItemNotaFiscalEntradaOutput> ItensNotaFiscal { get; set; }
        public NaoConformidadeValidationService(IItemNotaFiscalEntradaProvider itemNotaFiscalEntradaProvider,
            IItemNotaFiscalEntradaRateioLoteProvider itemNotaFiscalEntradaRateioLoteProvider,
            IOrdemProducaoProvider ordemProducaoProvider)
        {
            _itemNotaFiscalEntradaProvider = itemNotaFiscalEntradaProvider;
            _ordemProducaoProvider = ordemProducaoProvider;
            _itemNotaFiscalEntradaRateioLoteProvider = itemNotaFiscalEntradaRateioLoteProvider;
            ItensNotaFiscal = new List<ItemNotaFiscalEntradaOutput>();
        }
        
        public NaoConformidadeValidationResult ValidarChangeStatus(NaoConformidadeInput input)
        {
            if (input.Status == StatusNaoConformidade.Fechado)
            {
                return NaoConformidadeValidationResult.StatusFechado;
            }

            return NaoConformidadeValidationResult.Ok;
        }

        public NaoConformidadeValidationResult ValidarCampoCliente(NaoConformidadeInput input)
        {
            if (!input.IdPessoa.HasValue && input.Origem == OrigemNaoConformidade.Cliente)
            {
                return NaoConformidadeValidationResult.ClienteObrigatorio;
            }

            return NaoConformidadeValidationResult.Ok;
        }

        public NaoConformidadeValidationResult ValidarCampoFornecedor(NaoConformidadeInput input)
        {
            if (!input.IdPessoa.HasValue && input.Origem == OrigemNaoConformidade.InspecaoEntrada)
            {
                return NaoConformidadeValidationResult.FornecedorObrigatorio;
            }
            return NaoConformidadeValidationResult.Ok;
        }

        public async Task<NaoConformidadeValidationResult> ValidarCampoOdf(NaoConformidadeInput input)
        {
            if (input.NumeroOdf.HasValue)
            {
                if (!await OdfExistente(input.NumeroOdf.Value))
                {
                    return NaoConformidadeValidationResult.OdfInexistente;
                }
                return NaoConformidadeValidationResult.Ok;
            }

            if (input.Origem == OrigemNaoConformidade.InpecaoSaida)
            {
                return NaoConformidadeValidationResult.OdfObrigatorio;
            }
            
            return NaoConformidadeValidationResult.Ok;
        }
        
        public NaoConformidadeValidationResult ValidarCampoNotaFiscal(NaoConformidadeInput input)
        {
            if (!input.IdNotaFiscal.HasValue && input.Origem == OrigemNaoConformidade.InspecaoEntrada)
            {
                return NaoConformidadeValidationResult.NotaFiscalObrigatoria;
            }
            return NaoConformidadeValidationResult.Ok;
        }
        public async Task<NaoConformidadeValidationResult> ValidarCampoProduto(NaoConformidadeInput input)
        {
            if (input.Origem != OrigemNaoConformidade.InspecaoEntrada)
            {
                return NaoConformidadeValidationResult.Ok;
            }
            
            if (!ItensNotaFiscal.Any())
            {
                await GetItensNotaFiscal(input);
            }
            var isProdutoValido = ItensNotaFiscal.Any(e => e.IdProduto == input.IdProduto);

            if (!isProdutoValido)
            {
                return NaoConformidadeValidationResult.ProdutoInvalido;
            };
            return NaoConformidadeValidationResult.Ok;
        }
        public async Task<NaoConformidadeValidationResult> ValidarCampoLote(NaoConformidadeInput input)
        {
            if (input.Origem != OrigemNaoConformidade.InspecaoEntrada)
            {
                return NaoConformidadeValidationResult.Ok;
            }
            
            if (!ItensNotaFiscal.Any())
            {
                await GetItensNotaFiscal(input);
            }
            
            if (string.IsNullOrWhiteSpace(input.NumeroLote))
            {
                return NaoConformidadeValidationResult.LoteInvalido;
            }
            
            var hasItemNotaFiscal = ItensNotaFiscal.Any(e => e.Lote == input.NumeroLote);

            if (hasItemNotaFiscal)
            {
                return NaoConformidadeValidationResult.Ok;
            }
            
            var itensNotaFiscalRateioLote = await GetItensNotaFiscalRateioLote(input);
            var hasItemNotaFiscalRateio = itensNotaFiscalRateioLote.Any();

            if (hasItemNotaFiscalRateio)
            {
                return NaoConformidadeValidationResult.Ok;
            }
           
            return NaoConformidadeValidationResult.LoteInvalido;
        }

        private async Task GetItensNotaFiscal(NaoConformidadeInput naoConformidadeInput)
        {
            long totalCount;
            var skipCount = 0;
            var itens = new List<ItemNotaFiscalEntradaOutput>();
            
            do
            {
                var input = new GetListItemNotaFiscalInput
                {
                    IdNotaFiscal = naoConformidadeInput.IdNotaFiscal,
                    MaxResultCount = 50,
                    SkipCount = skipCount
                };
                var result = await _itemNotaFiscalEntradaProvider.GetList(input);
                itens.AddRange(result.Items);
                totalCount = result.TotalCount;
                skipCount += 50;
            } while (itens.Count < totalCount);

            ItensNotaFiscal = itens;
        }

        private async Task<bool> OdfExistente(int numeroOdf)
        {
            var ordemProducao = await _ordemProducaoProvider.GetByNumeroOdf(numeroOdf, true);
            if (ordemProducao != null)
            {
                return true;
            }

            return false;
        }

        private async Task<List<ItemNotaFiscalEntradaRateioLoteOutput>> GetItensNotaFiscalRateioLote(NaoConformidadeInput naoConformidadeInput)
        {
            long totalCount;
            var skipCount = 0;
            var itens = new List<ItemNotaFiscalEntradaRateioLoteOutput>();

            var advancedFilter = new JsonNetFilterRule
            {
                Condition = "AND",
                Rules = new List<JsonNetFilterRule>
                {
                    new JsonNetFilterRule()
                    {
                        Field = "IdNotaFiscal",
                        Operator = "equal",
                        Type = "string",
                        Value = naoConformidadeInput.IdNotaFiscal
                    },
                    new JsonNetFilterRule()
                    {
                        Field = "Lote",
                        Operator = "equal",
                        Type = "string",
                        Value = naoConformidadeInput.NumeroLote
                    }
                }
            };
           
            do
            {
                var input = new GetListItemNotaFiscalRateioLoteInput()
                {
                    AdvancedFilter = JsonConvert.SerializeObject(advancedFilter),
                    MaxResultCount = 50,
                    SkipCount = skipCount
                };
                var result = await _itemNotaFiscalEntradaRateioLoteProvider.GetList(input);
                itens.AddRange(result.Items);
                totalCount = result.TotalCount;
                skipCount += 50;
            } while (itens.Count < totalCount);

            return itens;
        }
    }
}

