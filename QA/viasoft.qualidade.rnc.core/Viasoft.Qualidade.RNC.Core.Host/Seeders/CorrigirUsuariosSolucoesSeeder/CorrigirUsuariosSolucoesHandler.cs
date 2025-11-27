using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rebus.Handlers;
using Viasoft.Core.DDD.Extensions;
using Viasoft.Core.DDD.Repositories;
using Viasoft.Core.DDD.UnitOfWork;
using Viasoft.Qualidade.RNC.Core.Domain.ExternalEntities.Usuarios;
using Viasoft.Qualidade.RNC.Core.Domain.SeederManagers;
using Viasoft.Qualidade.RNC.Core.Domain.SolucaoNaoConformidades;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.Usuarios;

namespace Viasoft.Qualidade.RNC.Core.Host.Seeders.CorrigirUsuariosSolucoesSeeder;

public class CorrigirUsuariosSolucoesHandler :
    IHandleMessages<CorrigirUsuariosSolucoesMessage>
{
    private readonly IRepository<SeederManager> _seederManagers;
    private readonly IRepository<SolucaoNaoConformidade> _solucoes;
    private readonly IRepository<Usuario> _usuarios;
    private readonly IUsuarioProxyService _usuarioProxyService;
    private readonly IUnitOfWork _unitOfWork;
    private const int MaxResultCount = 100;

    public CorrigirUsuariosSolucoesHandler(
        IRepository<SeederManager> seederManagers,
        IRepository<SolucaoNaoConformidade> solucoes,
        IRepository<Usuario> usuarios,
        IUsuarioProxyService usuarioProxyService,
        IUnitOfWork unitOfWork)
    {
        _seederManagers = seederManagers;
        _solucoes = solucoes;
        _usuarios = usuarios;
        _usuarioProxyService = usuarioProxyService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CorrigirUsuariosSolucoesMessage message)
    {
        var skipCount = 0;

        while (true)
        {
            var solucoes = await _solucoes.AsNoTracking()
                .Where(solucao => solucao.IdAuditor.HasValue || solucao.IdResponsavel.HasValue)
                .OrderBy(solucao => solucao.Id)
                .PageBy(skipCount, MaxResultCount)
                .Select(solucao => new
                {
                    solucao.IdAuditor,
                    solucao.IdResponsavel
                })
                .ToListAsync();
            
            if (!solucoes.Any())
            {
                break;
            }
            
            var idsUsuarios = solucoes
                .SelectMany(solucao => new[] { solucao.IdAuditor, solucao.IdResponsavel })
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .Distinct()
                .ToList();

            var idsUsuariosJaInseridos = await _usuarios.AsNoTracking()
                .Where(usuario => idsUsuarios.Contains(usuario.Id))
                .Select(usuario => usuario.Id)
                .ToListAsync();

            var idsUsuariosParaInserir = idsUsuarios.Except(idsUsuariosJaInseridos).ToList();

            if (idsUsuariosParaInserir.Any())
            {
                var usuariosParaInserir = await _usuarioProxyService.GetAllByIdsPaginando(idsUsuariosParaInserir);

                using (_unitOfWork.Begin())
                {
                    var usuarios = usuariosParaInserir
                        .Select(usuario => new Usuario
                        {
                            Id = usuario.Id,
                            Nome = usuario.FirstName,
                            Sobrenome = usuario.SecondName
                        })
                        .ToList();

                    await _usuarios.InsertRangeAsync(usuarios);
                    await _unitOfWork.CompleteAsync();
                }
            }

            skipCount += MaxResultCount;
        }

        var seederManager = await _seederManagers.FirstAsync();
        seederManager.CorrigirUsuariosSolucoesSeederFinalizado = true;
        await _seederManagers.UpdateAsync(seederManager, true);
    }
}