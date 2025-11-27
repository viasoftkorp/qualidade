using System;
using System.Threading.Tasks;
using Rebus.Handlers;
using Viasoft.Core.DDD.Repositories;
using Viasoft.PushNotifications.Abstractions.Notification;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Enums;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.Events;
using Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.UpdateNotifications;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Pessoas.Services;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Produtos.Services;
using Viasoft.Qualidade.RNC.Core.Host.ExternalEntities.Usuarios.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyCompras.NotasFiscaisEntrada.Services;
using Viasoft.Qualidade.RNC.Core.Host.Proxies.LegacyFaturamentos.NotasFiscaisSaida.Services;

namespace Viasoft.Qualidade.RNC.Core.Host.NaoConformidades.Handlers;

public class NaoConformidadeViewHandler : IHandleMessages<NaoConformidadeInserida>, IHandleMessages<NaoConformidadeAtualizada>
{
    private readonly INotaFiscalEntradaProvider _notaFiscalEntradaProvider;
    private readonly IRepository<NaoConformidade> _naoConformidades;
    private readonly IPushNotification _pushNotification;
    private readonly IUsuarioService _usuarioService;
    private readonly IProdutoService _produtoService;
    private readonly IPessoaService _pessoaService;
    private readonly INotaFiscalSaidaProvider _notaFiscalSaidaProvider;

    public NaoConformidadeViewHandler(INotaFiscalEntradaProvider notaFiscalEntradaProvider, IRepository<NaoConformidade> naoConformidades,
        INotaFiscalSaidaProvider notaFiscalSaidaProvider, IPushNotification pushNotification, 
        IUsuarioService usuarioService, IProdutoService produtoService, IPessoaService pessoaService)
    {
        _notaFiscalEntradaProvider = notaFiscalEntradaProvider;
        _naoConformidades = naoConformidades;
        _pushNotification = pushNotification;
        _usuarioService = usuarioService;
        _produtoService = produtoService;
        _pessoaService = pessoaService;
        _notaFiscalSaidaProvider = notaFiscalSaidaProvider;
    }

    public async Task Handle(NaoConformidadeInserida message)
    {
        if (message.NaoConformidade.IdPessoa.HasValue)
        {
            await _pessoaService.InserirSeNaoCadastrado(message.NaoConformidade.IdPessoa.Value);
        }

        await _produtoService.InserirSeNaoCadastrado(message.NaoConformidade.IdProduto);

        await _usuarioService.InserirSeNaoCadastrado(message.NaoConformidade.IdCriador);
        
        if (message.NaoConformidade.IdNotaFiscal.HasValue)
        {
            var naoconformidade = await _naoConformidades.FindAsync(message.NaoConformidade.Id);
            
            if (message.NaoConformidade.Origem == OrigemNaoConformidade.InspecaoEntrada )
            {
                naoconformidade.NumeroNotaFiscal = await GetNumeroNotaFiscalEntrada(message.NaoConformidade.IdNotaFiscal.Value);
            }
            else
            {
                naoconformidade.NumeroNotaFiscal = await GetNumeroNotaFiscalSaida(message.NaoConformidade.IdNotaFiscal.Value);
            }
            
            await _naoConformidades.UpdateAsync(naoconformidade, true);
        }
        var naoConformidadeViewInseridaNotificationUpdate = new NaoConformidadeViewInseridaNotificationUpdate();

        await _pushNotification.SendUpdateAsync(naoConformidadeViewInseridaNotificationUpdate);
    }
    public async Task Handle(NaoConformidadeAtualizada message)
    {
        if (message.NaoConformidade.IdPessoa.HasValue)
        {
            await _pessoaService.InserirSeNaoCadastrado(message.NaoConformidade.IdPessoa.Value);
        }
        
        await _produtoService.InserirSeNaoCadastrado(message.NaoConformidade.IdProduto);
        
        if ( message.NaoConformidade.IdNotaFiscal.HasValue)
        {
            var naoConformidade = await _naoConformidades.FindAsync(message.NaoConformidade.Id);
            
            if (message.NaoConformidade.Origem == OrigemNaoConformidade.InspecaoEntrada )
            {
                naoConformidade.NumeroNotaFiscal = await GetNumeroNotaFiscalEntrada(message.NaoConformidade.IdNotaFiscal.Value);
            }
            else
            {
                naoConformidade.NumeroNotaFiscal = await GetNumeroNotaFiscalSaida(message.NaoConformidade.IdNotaFiscal.Value);
            }
            
            await _naoConformidades.UpdateAsync(naoConformidade, true);
        }
        
        var naoConformidadeViewAtualizadaNotificationUpdate = new NaoConformidadeViewAtualizadaNotificationUpdate
        {
            IdNaoConformidade = message.NaoConformidade.Id
        };
        await _pushNotification.SendUpdateAsync(naoConformidadeViewAtualizadaNotificationUpdate);

    }
    private async Task<string> GetNumeroNotaFiscalEntrada(Guid idNotaFiscal)
    {
        var notaFiscal = await _notaFiscalEntradaProvider.GetById(idNotaFiscal);
        return notaFiscal.NumeroNotaFiscal.ToString();
    }
    private async Task<string> GetNumeroNotaFiscalSaida(Guid idNotaFiscal)
    {
        var notaFiscal = await _notaFiscalSaidaProvider.GetById(idNotaFiscal);
        return notaFiscal.NumeroNotaFiscal.ToString();
    }
}