using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades.Models;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.CentroCustoCausaNaoConformidades.Commands;

public class InserirCentroCustoCausaNaoConformidadeCommand 
{
    public CentroCustoCausaNaoConformidadeModel CentroCustoCausaNaoConformidade { get; set; }
    public InserirCentroCustoCausaNaoConformidadeCommand()
    {
    }

    public InserirCentroCustoCausaNaoConformidadeCommand(CentroCustoCausaNaoConformidadeModel model)
    {
        CentroCustoCausaNaoConformidade = new CentroCustoCausaNaoConformidadeModel(model);
    }
}
