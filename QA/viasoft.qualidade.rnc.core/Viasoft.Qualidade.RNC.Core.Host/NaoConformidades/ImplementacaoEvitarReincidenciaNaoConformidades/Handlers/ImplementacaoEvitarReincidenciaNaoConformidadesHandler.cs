using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Qualidade.RNC.Core.Domain.ImplementacaoEvitarReincidenciaNaoConformidades.Events;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Usuarios.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.ImplementacaoEvitarReincidenciaNaoConformidades.Handlers;

public class ImplementacaoEvitarReincidenciaNaoConformidadesHandler : IHandleMessages<ImplementacaoEvitarReincidenciaInserida>,
    IHandleMessages<ImplementacaoEvitarReincidenciaAtualizada>
{
    private readonly IUsuarioService _usuarioService;

    public ImplementacaoEvitarReincidenciaNaoConformidadesHandler(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }
    public async Task Handle(ImplementacaoEvitarReincidenciaInserida message)
    {
        if (message.Command.ImplementacaoEvitarReincidenciaNaoConformidade.IdResponsavel.HasValue)
        {
            await _usuarioService.InserirSeNaoCadastrado(message.Command.ImplementacaoEvitarReincidenciaNaoConformidade
                .IdResponsavel.Value);
        }
        
        if (message.Command.ImplementacaoEvitarReincidenciaNaoConformidade.IdAuditor.HasValue)
        {
            await _usuarioService.InserirSeNaoCadastrado(message.Command.ImplementacaoEvitarReincidenciaNaoConformidade.IdAuditor.Value);
        }
    }

    public async Task Handle(ImplementacaoEvitarReincidenciaAtualizada message)
    {
        if (message.Command.ImplementacaoEvitarReincidenciaNaoConformidade.IdResponsavel.HasValue)
        {
            await _usuarioService.InserirSeNaoCadastrado(message.Command.ImplementacaoEvitarReincidenciaNaoConformidade.IdResponsavel.Value);
        }
        
        if (message.Command.ImplementacaoEvitarReincidenciaNaoConformidade.IdAuditor.HasValue)
        {
            await _usuarioService.InserirSeNaoCadastrado(message.Command.ImplementacaoEvitarReincidenciaNaoConformidade.IdAuditor.Value);
        }
    }
}