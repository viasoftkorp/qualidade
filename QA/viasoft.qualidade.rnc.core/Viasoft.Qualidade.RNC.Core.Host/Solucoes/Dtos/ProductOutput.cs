using System;

namespace Viasoft.Qualidade.RNC.Core.Host.Solucoes.Dtos;

public class ProductOutput
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Code { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid MeasureId { get; set; }
}