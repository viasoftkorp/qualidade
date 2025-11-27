using System;
using Newtonsoft.Json;

namespace Viasoft.Qualidade.RNC.Core.Domain.ExternalContracts.ProducaoApontamento.Apontamentos.Dtos
{
     public class ApontamentoOperacaoEstornadaEventDto
     {
         [JsonProperty("OrdemFabricacao")]
         public int NumeroOdf { get; set; }
         [JsonProperty("Operacao")]
         public string NumeroOperacao { get; set; }
     }
}