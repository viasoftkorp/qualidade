package main

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/controllers"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	"bitbucket.org/viasoftkorp/korp.sdk/service_info"
	utilsSdk "bitbucket.org/viasoftkorp/korp.sdk/utils"
	"github.com/gofiber/fiber/v2"
)

const GetNotasFiscaisRoute = utils.ApiPrefix + "/notas-fiscais"
const GetNotasFiscaisHistoricoRoute = utils.ApiPrefix + "/inspecoes/historico"
const GetLocaisRoute = utils.ApiPrefix + "/locais/:tipoLocal"
const GetInspecoesEntrada = utils.ApiPrefix + "/inspecoes/:notaFiscal/lotes/:lote"
const GetInspecoesEntradaHistorico = utils.ApiPrefix + "/inspecoes/historico/:notaFiscal/lotes/:lote/itens"
const GetInspecaoEntradaRoute = utils.ApiPrefix + "/inspecoes/:codigoInspecao"
const InspecaoEntradaRoute = utils.ApiPrefix + "/inspecoes"
const GetItensPlanoNovaInspecaoRoute = utils.ApiPrefix + "/planos/:codigoPlano"
const GetInspecaoEntradaItensRoute = utils.ApiPrefix + "/inspecoes/:codigoInspecao/itens"
const GetResultadoInspecaoRoute = utils.ApiPrefix + "/inspecoes/:codigoInspecao/resultado"
const FinalizarInspecaoEntradaRoute = utils.ApiPrefix + "/inspecoes/:codigoInspecao/finalizar"
const ImprimirInspecaoEntradaRoute = utils.ApiPrefix + "/inspecoes/:codigoInspecao/imprimir"
const EstornarInspecaoEntradaRoute = utils.ApiPrefix + "/inspecoes/historico/:recnoInspecao/estornar"
const GetParametroRoute = utils.ApiPrefix + "/parametros/:parametro/secoes/:secao"
const BuscarProcessamentoInspecaoEntradaRoute = utils.ApiPrefix + "/processamentos"
const RemoverProcessamentoInspecaoEntradaRoute = utils.ApiPrefix + "/processamentos/:id"
const ReprocessarProcessamentoInspecaoEntradaRoute = utils.ApiPrefix + "/processamentos/:id/reprocessar"
const InspecaoEntradaAlocacaoPedidoRoute = utils.ApiPrefix + "/inspecoes/:recnoInspecao/pedidos-venda"
const UpdateDeleteGetInspecaoEntradaAlocacaoPedidoRoute = utils.ApiPrefix + "/inspecoes/:recnoInspecao/pedidos-venda/:id"
const GetPedidoVendaLotes = utils.ApiPrefix + "/inspecoes/:recnoInspecao/pedidos-venda/:id"
const InspecaoEntradaAlocacaoPedidoQuantidadeRoute = utils.ApiPrefix + "/inspecoes/:recnoInspecao/pedidos-venda/quantidade-alocada"
const GetProdutosRoute = utils.ApiPrefix + "/produtos"
const GetProdutoRoute = utils.ApiPrefix + "/produtos/:codigo"
const GetAllLocaisRoute = utils.ApiPrefix + "/locais"
const GetLocalRoute = utils.ApiPrefix + "/locais/codigo/:codigo"
const GetInspecaoSaidaRncRoute = utils.ApiPrefix + "/inspecoes/:recnoInspecao/rnc"
const PlanosAmostragemRoute = utils.ApiPrefix + "/planos-amostragem"
const UpdateNotaFiscalDadosAdicionais = utils.ApiPrefix + "/notas-fiscais/:idNotaFiscal/dados-adicionais"
const GerarNumeroLoteRoute = utils.ApiPrefix + "/lotes/gerar-numero"
const fileProviderProxyUploadFileWithSubDomainRoute = utils.ApiPrefix + "/file-provider/app/:appid/domain/:domain/subdomain/:subdomain/file"
const fileProviderProxyDeleteFileRoute = utils.ApiPrefix + "/file-provider/file/:id"
const fileProviderProxyDownloadFileRoute = utils.ApiPrefix + "/file-provider/file/:id/download"
const fileProviderProxyGetFilesByDomainWithFiltersRoute = utils.ApiPrefix + "/file-provider/app/:appid/domains"

func initRoutes(app *fiber.App) {
	routerGroup := app.Group(utilsSdk.GetVersionWithoutBuild(service_info.Version))
	RoutesVersion(routerGroup)
}

func RoutesVersion(routerGroup fiber.Router) {
	routerGroup.Get(GetNotasFiscaisRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetNotasFiscais(ctx)
	})
	routerGroup.Get(GetNotasFiscaisHistoricoRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetInspecoesEntradaHistoricoCabecalho(ctx)
	})
	routerGroup.Get(GetInspecoesEntrada, func(ctx *fiber.Ctx) error {
		return controllers.GetInspecoesEntrada(ctx)
	})
	routerGroup.Get(GetInspecoesEntradaHistorico, func(ctx *fiber.Ctx) error {
		return controllers.GetInspecoesEntradaHistoricoItems(ctx)
	})
	routerGroup.Post(InspecaoEntradaRoute, func(ctx *fiber.Ctx) error {
		return controllers.CriarInspecao(ctx)
	})
	routerGroup.Put(InspecaoEntradaRoute, func(ctx *fiber.Ctx) error {
		return controllers.AtualizarInspecao(ctx)
	})
	routerGroup.Get(GetItensPlanoNovaInspecaoRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetItensPlanoNovaInspecao(ctx)
	})
	routerGroup.Get(GetInspecaoEntradaRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetInspecaoEntrada(ctx)
	})
	routerGroup.Get(GetInspecaoEntradaItensRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetInspecaoEntradaItens(ctx)
	})
	routerGroup.Delete(GetInspecaoEntradaRoute, func(ctx *fiber.Ctx) error {
		return controllers.RemoverInspecaoEntrada(ctx)
	})
	routerGroup.Get(BuscarProcessamentoInspecaoEntradaRoute, func(ctx *fiber.Ctx) error {
		return controllers.BuscarSagas(ctx)
	})
	routerGroup.Put(ReprocessarProcessamentoInspecaoEntradaRoute, func(ctx *fiber.Ctx) error {
		return controllers.ReprocessarSaga(ctx)
	})
	routerGroup.Post(FinalizarInspecaoEntradaRoute, func(ctx *fiber.Ctx) error {
		return controllers.PublicarFinalizarInspecaoEntrada(ctx)
	})
	routerGroup.Get(ImprimirInspecaoEntradaRoute, func(ctx *fiber.Ctx) error {
		return controllers.ImprimirInspecaoEntrada(ctx)
	})
	routerGroup.Put(EstornarInspecaoEntradaRoute, func(ctx *fiber.Ctx) error {
		return controllers.PublicarEstornarInspecaoEntrada(ctx)
	})
	routerGroup.Get(GetResultadoInspecaoRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetResultadoInspecao(ctx)
	})
	routerGroup.Get(GetParametroRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetValorParametro(ctx)
	})
	routerGroup.Get(GetLocaisRoute, func(ctx *fiber.Ctx) error {
		return controllers.BuscarLocaisTipo(ctx)
	})
	routerGroup.Get(InspecaoEntradaAlocacaoPedidoRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetInspecaoEntradaAlocacaoPedido(ctx)
	})
	routerGroup.Get(InspecaoEntradaAlocacaoPedidoQuantidadeRoute, func(ctx *fiber.Ctx) error {
		return controllers.BuscarQuantidadeTotalAlocadaPedidoVenda(ctx)
	})
	routerGroup.Put(UpdateDeleteGetInspecaoEntradaAlocacaoPedidoRoute, func(ctx *fiber.Ctx) error {
		return controllers.AtualizarDistribuicaoInspecaoEstoquePedidoVenda(ctx)
	})
	routerGroup.Get(GetPedidoVendaLotes, func(ctx *fiber.Ctx) error {
		return controllers.BuscarPedidoVendaLotes(ctx)
	})
	routerGroup.Get(GetProdutosRoute, func(ctx *fiber.Ctx) error {
		return controllers.BuscarProdutos(ctx)
	})
	routerGroup.Get(GetProdutoRoute, func(ctx *fiber.Ctx) error {
		return controllers.BuscarProduto(ctx)
	})
	routerGroup.Get(GetAllLocaisRoute, func(ctx *fiber.Ctx) error {
		return controllers.BuscarLocais(ctx)
	})
	routerGroup.Get(GetLocalRoute, func(ctx *fiber.Ctx) error {
		return controllers.BuscarLocal(ctx)
	})
	routerGroup.Delete(RemoverProcessamentoInspecaoEntradaRoute, func(ctx *fiber.Ctx) error {
		return controllers.RemoverSaga(ctx)
	})
	routerGroup.Get(GetInspecaoSaidaRncRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetRncDetails(ctx)
	})
	routerGroup.Get(PlanosAmostragemRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetAllPlanosAmostragem(ctx)
	})
	routerGroup.Post(PlanosAmostragemRoute, func(ctx *fiber.Ctx) error {
		return controllers.CreatePlanoAmostragem(ctx)
	})
	routerGroup.Put(PlanosAmostragemRoute+"/:id", func(ctx *fiber.Ctx) error {
		return controllers.UpdatePlanoAmostragem(ctx)
	})
	routerGroup.Delete(PlanosAmostragemRoute+"/:id", func(ctx *fiber.Ctx) error {
		return controllers.DeletePlanoAmostragem(ctx)
	})
	routerGroup.Get(PlanosAmostragemRoute+"/faixas", func(ctx *fiber.Ctx) error {
		return controllers.GetPlanoAmostragemFaixa(ctx)
	})

	routerGroup.Post(fileProviderProxyUploadFileWithSubDomainRoute, func(ctx *fiber.Ctx) error {
		return controllers.UploadFiles(ctx)
	})
	routerGroup.Post(fileProviderProxyGetFilesByDomainWithFiltersRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetFilesByDomainWithFilters(ctx)
	})
	routerGroup.Delete(fileProviderProxyDeleteFileRoute, func(ctx *fiber.Ctx) error {
		return controllers.DeleteFile(ctx)
	})
	routerGroup.Get(fileProviderProxyDownloadFileRoute, func(ctx *fiber.Ctx) error {
		return controllers.DownloadFile(ctx)
	})
	routerGroup.Put(UpdateNotaFiscalDadosAdicionais, func(ctx *fiber.Ctx) error {
		return controllers.UpdateNotaFiscalDadosAdicionais(ctx)
	})
	routerGroup.Post(GerarNumeroLoteRoute, func(ctx *fiber.Ctx) error {
		return controllers.GerarNumeroLote(ctx)
	})
}
