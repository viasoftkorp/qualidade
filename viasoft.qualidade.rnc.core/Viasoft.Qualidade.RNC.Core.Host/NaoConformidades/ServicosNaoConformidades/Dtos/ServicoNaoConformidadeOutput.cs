using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.ServicoNaoConformidades.Models;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ServicosNaoConformidades.Dtos;

public class ServicoNaoConformidadeOutput : ServicoNaoConformidadeModel
{
    public ServicoNaoConformidadeOutput(ServicoNaoConformidade servico)
    {
        Id = servico.Id;
        IdProduto = servico.IdProduto;
        IdNaoConformidade = servico.IdNaoConformidade;
        Quantidade = servico.Quantidade;
        Detalhamento = servico.Detalhamento;
        Horas = servico.Horas;
        Minutos = servico.Minutos;
        IdRecurso = servico.IdRecurso;
        OperacaoEngenharia = servico.OperacaoEngenharia;
    }
    
}