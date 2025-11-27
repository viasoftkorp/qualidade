using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events.AcoesPreventivasNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Usuarios.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.AcoesPreventivasNaoConformidades.Handler;

public class AcoesPreventivasNaoConformidadesHandler : IHandleMessages<AcaoPreventivaNaoConformidadeInserida>, IHandleMessages<AcaoPreventivaNaoConformidadeAtualizada>
{
    private readonly IUsuarioService _usuarioService;

    public AcoesPreventivasNaoConformidadesHandler(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }
    public async Task Handle(AcaoPreventivaNaoConformidadeInserida message)
    {
        if (message.Command.AcaoPreventivaNaoConformidade.IdResponsavel.HasValue)
        {
            await _usuarioService.InserirSeNaoCadastrado(message.Command.AcaoPreventivaNaoConformidade.IdResponsavel.Value);
        }
        
        if (message.Command.AcaoPreventivaNaoConformidade.IdAuditor.HasValue)
        {
            await _usuarioService.InserirSeNaoCadastrado(message.Command.AcaoPreventivaNaoConformidade.IdAuditor.Value);
        }
    }

    public async Task Handle(AcaoPreventivaNaoConformidadeAtualizada message)
    {
        if (message.Command.AcaoPreventivaNaoConformidade.IdResponsavel.HasValue)
        {
            await _usuarioService.InserirSeNaoCadastrado(message.Command.AcaoPreventivaNaoConformidade.IdResponsavel.Value);
        }
        
        if (message.Command.AcaoPreventivaNaoConformidade.IdAuditor.HasValue)
        {
            await _usuarioService.InserirSeNaoCadastrado(message.Command.AcaoPreventivaNaoConformidade.IdAuditor.Value);
        }
    }
}