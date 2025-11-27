package main

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/controllers"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	"bitbucket.org/viasoftkorp/korp.sdk/service_info"
	utilsSdk "bitbucket.org/viasoftkorp/korp.sdk/utils"
	"github.com/gofiber/fiber/v2"
)

const GetLocaisRoute = utils.ApiPrefix + "/locais/:tipoLocal"
const GetParametroRoute = utils.ApiPrefix + "/parametros/:parametro/secoes/:secao"
const GetOrdensProducaoRoute = utils.ApiPrefix + "/ordens-producao"
const GetInspecoesSaidaRoute = utils.ApiPrefix + "/inspecoes"
const GetInspecoesSaidaHistoricoCabecalhoRoute = utils.ApiPrefix + "/inspecoes/historico"
const GetInspecoesSaidaHistoricoItensRoute = utils.ApiPrefix + "/inspecoes/historico/:odf/itens"
const GetPutDeleteInspecaoSaidaRoute = utils.ApiPrefix + "/inspecoes/:codInspecao"
const PostInspecaoSaidaRoute = utils.ApiPrefix + "/inspecoes"
const GetPlanosNovaInspecaoRoute = utils.ApiPrefix + "/planos"
const GetInspecaoSaidaItensRoute = utils.ApiPrefix + "/inspecoes/:codInspecao/itens"
const GetResultadoInspecaoRoute = utils.ApiPrefix + "/inspecoes/:codInspecao/resultado"
const FinalizarInspecaoSaidaRoute = utils.ApiPrefix + "/inspecoes/:codInspecao/finalizar"
const ImprimirInspecaoSaidaRoute = utils.ApiPrefix + "/inspecoes/:codInspecao/imprimir"
const EstornarInspecaoSaidaRoute = utils.ApiPrefix + "/inspecoes/historico/:recnoInspecao/estornar"
const BuscarProcessamentoInspecaoSaidaRoute = utils.ApiPrefix + "/processamentos"
const ReprocessarProcessamentoInspecaoSaidaRoute = utils.ApiPrefix + "/processamentos/:id/reprocessar"
const RemoverProcessamentoInspecaoSaidaRoute = utils.ApiPrefix + "/processamentos/:id"
const GetProdutosRoute = utils.ApiPrefix + "/produtos"
const GetProdutoRoute = utils.ApiPrefix + "/produtos/:codigo"
const GetProcessoEngenhariaRoute = utils.ApiPrefix + "/produtos/:codigoProduto/processo"
const GetAllLocaisRoute = utils.ApiPrefix + "/locais"
const GetLocalRoute = utils.ApiPrefix + "/locais/codigo/:codigo"
const GetInspecaoSaidaRncRoute = utils.ApiPrefix + "/inspecoes/:recnoInspecao/rnc"
const FileProviderProxyUploadFileWithSubDomainRoute = utils.ApiPrefix + "/file-provider/app/:appid/domain/:domain/subdomain/:subdomain/file"
const FileProviderProxyDeleteFileRoute = utils.ApiPrefix + "/file-provider/file/:id"
const FileProviderProxyDownloadFileRoute = utils.ApiPrefix + "/file-provider/file/:id/download"
const FileProviderProxyGetFilesByDomainWithFiltersRoute = utils.ApiPrefix + "/file-provider/app/:appid/domains"

func initRoutes(app *fiber.App) {
	// REGION PROTECTED ROUTES
	routerGroup := app.Group(utilsSdk.GetVersionWithoutBuild(service_info.Version))
	RoutesVersion(routerGroup)
	// ENDREGION PROTECTED ROUTES
}

func RoutesVersion(routerGroup fiber.Router) {
	routerGroup.Get(GetInspecoesSaidaHistoricoCabecalhoRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetInspecoesSaidaHistoricoCabecalho(ctx)
	})
	routerGroup.Get(GetInspecoesSaidaHistoricoItensRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetInspecoesSaidaHistoricoItems(ctx)
	})
	routerGroup.Get(GetOrdensProducaoRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetOrdensProducao(ctx)
	})
	routerGroup.Get(GetInspecoesSaidaRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetInspecoesSaida(ctx)
	})
	routerGroup.Post(PostInspecaoSaidaRoute, func(ctx *fiber.Ctx) error {
		return controllers.CriarInspecao(ctx)
	})
	routerGroup.Put(GetPutDeleteInspecaoSaidaRoute, func(ctx *fiber.Ctx) error {
		return controllers.AtualizarInspecao(ctx)
	})
	routerGroup.Get(GetPlanosNovaInspecaoRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetPlanosNovaInspecao(ctx)
	})
	routerGroup.Get(GetPutDeleteInspecaoSaidaRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetInspecaoSaida(ctx)
	})
	routerGroup.Get(GetInspecaoSaidaItensRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetInspecaoSaidaItens(ctx)
	})
	routerGroup.Delete(GetPutDeleteInspecaoSaidaRoute, func(ctx *fiber.Ctx) error {
		return controllers.RemoverInspecaoSaida(ctx)
	})
	routerGroup.Post(FinalizarInspecaoSaidaRoute, func(ctx *fiber.Ctx) error {
		return controllers.PublicarFinalizarInspecaoSaida(ctx)
	})
	routerGroup.Get(ImprimirInspecaoSaidaRoute, func(ctx *fiber.Ctx) error {
		return controllers.ImprimirInspecaoSaida(ctx)
	})
	routerGroup.Put(EstornarInspecaoSaidaRoute, func(ctx *fiber.Ctx) error {
		return controllers.PublicarEstornarInspecaoSaida(ctx)
	})
	routerGroup.Get(BuscarProcessamentoInspecaoSaidaRoute, func(ctx *fiber.Ctx) error {
		return controllers.BuscarSagas(ctx)
	})
	routerGroup.Put(ReprocessarProcessamentoInspecaoSaidaRoute, func(ctx *fiber.Ctx) error {
		return controllers.ReprocessarSaga(ctx)
	})
	routerGroup.Get(GetResultadoInspecaoRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetResultadoInspecao(ctx)
	})
	routerGroup.Get(GetParametroRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetParametroRoute(ctx)
	})
	routerGroup.Get(GetLocaisRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetLocaisRoute(ctx)
	})
	routerGroup.Get(GetProdutosRoute, func(ctx *fiber.Ctx) error {
		return controllers.BuscarProdutos(ctx)
	})
	routerGroup.Get(GetProdutoRoute, func(ctx *fiber.Ctx) error {
		return controllers.BuscarProduto(ctx)
	})
	routerGroup.Get(GetProcessoEngenhariaRoute, func(ctx *fiber.Ctx) error {
		return controllers.BuscarProcessoEngenharia(ctx)
	})
	routerGroup.Get(GetAllLocaisRoute, func(ctx *fiber.Ctx) error {
		return controllers.BuscarLocais(ctx)
	})
	routerGroup.Get(GetLocalRoute, func(ctx *fiber.Ctx) error {
		return controllers.BuscarLocal(ctx)
	})
	routerGroup.Delete(RemoverProcessamentoInspecaoSaidaRoute, func(ctx *fiber.Ctx) error {
		return controllers.RemoverSaga(ctx)
	})
	routerGroup.Get(GetInspecaoSaidaRncRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetRncDetails(ctx)
	})
	routerGroup.Post(FileProviderProxyUploadFileWithSubDomainRoute, func(ctx *fiber.Ctx) error {
		return controllers.UploadFiles(ctx)
	})
	routerGroup.Post(FileProviderProxyGetFilesByDomainWithFiltersRoute, func(ctx *fiber.Ctx) error {
		return controllers.GetFilesByDomainWithFilters(ctx)
	})
	routerGroup.Delete(FileProviderProxyDeleteFileRoute, func(ctx *fiber.Ctx) error {
		return controllers.DeleteFile(ctx)
	})
	routerGroup.Get(FileProviderProxyDownloadFileRoute, func(ctx *fiber.Ctx) error {
		return controllers.DownloadFile(ctx)
	})
}
