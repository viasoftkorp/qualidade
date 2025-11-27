using System;

namespace Viasoft.Qualidade.RNC.Gateway.Host.NaoConformidades.RetrabalhoNaoConformidades.OperacoesRetrabalhoNaoConformidade.Dtos;

public class MaterialInput
{
    public Guid Id { get; set; }
    public string Detalhamento { get; set; }
    public Guid IdMaquina { get; set; }
    public Guid IdProduto { get; set; }
    public int Quantidade { get; set; }
} 