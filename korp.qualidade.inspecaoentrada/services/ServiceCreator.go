package services

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/repositories"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	"bitbucket.org/viasoftkorp/korp.sdk/ambient_data"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"github.com/gofiber/fiber/v2"
)

func GetPlanoAmostragemRepository(ctx *fiber.Ctx) (interfaces.IInspecaoEntradaPlanoAmostragemRepository, error) {
	baseParams, err := getBaseParams(ctx)
	if err != nil {
		return nil, err
	}

	uow, err := GetUnitOfWork(ctx)
	if err != nil {
		return nil, err
	}

	repository, err := repositories.NewInspecaoEntradaPlanoAmostragemRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	return repository, nil
}

func GetNotaFiscalService(ctx *fiber.Ctx) (interfaces.INotaFiscalService, error) {
	baseParams, err := getBaseParams(ctx)
	if err != nil {
		return nil, err
	}

	uow, err := GetUnitOfWork(ctx)
	if err != nil {
		return nil, err
	}

	notaFiscalRepository, err := repositories.NewNotaFiscalRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	service := NewNotaFiscalService(notaFiscalRepository)

	return service, nil
}

func GetInspecaoEntradaService(ctx *fiber.Ctx) (interfaces.IInspecaoEntradaService, error) {
	baseParams, err := getBaseParams(ctx)
	if err != nil {
		return nil, err
	}

	uow, err := GetUnitOfWork(ctx)
	if err != nil {
		return nil, err
	}

	inspecaoEntradaRepository, err := repositories.NewInspecaoEntradaRepository(uow)
	if err != nil {
		return nil, err
	}

	inspecaoEntradaItemRepository, err := repositories.NewInspecaoEntradaItemRepository(uow)
	if err != nil {
		return nil, err
	}
	notaFiscalRepository, err := repositories.NewNotaFiscalRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	planosInspecaoRepository, err := repositories.NewPlanosInspecaoRepository(uow)
	if err != nil {
		return nil, err
	}

	parametrosInspecaoRepository, err := repositories.NewParametrosRepository(uow)
	if err != nil {
		return nil, err
	}

	locaisRepository, err := repositories.NewLocaisRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	localPedidoVendaRepository, err := repositories.NewEstoqueLocalPedidoVendaRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	externalMovimentacaoService, err := GetExternalMovimentacaoService(ctx)
	if err != nil {
		return nil, err
	}

	impressaoService, err := NewImpressaoService(baseParams)
	if err != nil {
		return nil, err
	}

	empresaRepository, err := repositories.NewEmpresaRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	service := NewInspecaoEntradaService(uow, inspecaoEntradaRepository, inspecaoEntradaItemRepository, notaFiscalRepository, planosInspecaoRepository, parametrosInspecaoRepository, locaisRepository, localPedidoVendaRepository, externalMovimentacaoService, baseParams, impressaoService, empresaRepository)

	return service, nil
}

func GetEstoquePedidoVendaService(ctx *fiber.Ctx) (interfaces.IEstoquePedidoVendaService, error) {
	baseParams, err := getBaseParams(ctx)
	if err != nil {
		return nil, err
	}

	uow, err := GetUnitOfWork(ctx)
	if err != nil {
		return nil, err
	}

	estoqueLocalPedidoVendaRepository, err := repositories.NewEstoqueLocalPedidoVendaRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	locaisRepository, err := repositories.NewLocaisRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	inspecaoEntradaRepository, err := repositories.NewInspecaoEntradaRepository(uow)
	if err != nil {
		return nil, err
	}

	inspecaoEntradaPedidoVendaLoteRepository, err := repositories.NewInspecaoEntradaPedidoVendaLoteRepository(uow)
	if err != nil {
		return nil, err
	}

	service := NewEstoqueLocalPedidoVendaService(estoqueLocalPedidoVendaRepository, locaisRepository,
		inspecaoEntradaRepository, inspecaoEntradaPedidoVendaLoteRepository, uow)
	return service, nil
}

func GetExternalMovimentacaoService(ctx *fiber.Ctx) (interfaces.IExternalMovimentacaoService, error) {
	baseParams, err := getBaseParams(ctx)
	if err != nil {
		return nil, err
	}

	service := NewExternalMovimentacaoService(baseParams)

	return service, nil
}

func getBaseParams(ctx *fiber.Ctx) (*models.BaseParams, error) {
	baseParams, err := utils.GetBaseParamsFromCtx(ctx)
	if err != nil {
		return nil, err
	}

	return baseParams, nil
}

func GetUnitOfWork(ctx *fiber.Ctx) (unit_of_work.UnitOfWork, error) {
	ambientDataModel, err := ambient_data.GetAmbientDataFromContext(ctx)
	if err != nil {
		return nil, err
	}

	uow, err := unit_of_work.NewUnitOfWork(ambientDataModel, utils.MigrateEntities)
	if err != nil {
		return nil, err
	}

	return uow, nil
}

func GetInspecaoEntradaSagaService(ctx *fiber.Ctx) (interfaces.IInspecaoEntradaSagaService, error) {
	baseParams, err := getBaseParams(ctx)
	if err != nil {
		return nil, err
	}

	uow, err := GetUnitOfWork(ctx)

	inspecaoEntradaRepository, err := repositories.NewInspecaoEntradaRepository(uow)
	if err != nil {
		return nil, err
	}

	inspecaoEntradaExecutadoWebRepository, err := repositories.NewInspecaoEntradaExecutadoWebRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	notaFiscalRepository, err := repositories.NewNotaFiscalRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	estoqueLocalPedidoVendaRepository, err := repositories.NewEstoqueLocalPedidoVendaRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	locaisRepository, err := repositories.NewLocaisRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	parametroRepository, err := repositories.NewParametrosRepository(uow)
	if err != nil {
		return nil, err
	}

	externalSagaService := NewExternalInspecaoEntradaSagaService(baseParams)

	produtoRepository, err := repositories.NewProdutoRepository(uow, baseParams)

	inspecaoEntradaPedidoVendaLoteRepository, err := repositories.NewInspecaoEntradaPedidoVendaLoteRepository(uow)
	if err != nil {
		return nil, err
	}

	service := NewInspecaoEntradaSagaService(
		inspecaoEntradaRepository,
		inspecaoEntradaExecutadoWebRepository,
		externalSagaService,
		notaFiscalRepository,
		locaisRepository,
		parametroRepository,
		estoqueLocalPedidoVendaRepository,
		produtoRepository,
		uow,
		inspecaoEntradaPedidoVendaLoteRepository)
	return service, nil
}

func GetProdutoService(ctx *fiber.Ctx) (interfaces.IProdutoService, error) {
	uow, err := GetUnitOfWork(ctx)
	if err != nil {
		return nil, err
	}

	baseParams, err := getBaseParams(ctx)
	if err != nil {
		return nil, err
	}

	produtoRepository, err := repositories.NewProdutoRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	service := NewProdutoService(produtoRepository)
	return service, nil
}

func GetLocalService(ctx *fiber.Ctx) (interfaces.ILocalService, error) {
	uow, err := GetUnitOfWork(ctx)
	if err != nil {
		return nil, err
	}

	baseParams, err := getBaseParams(ctx)
	if err != nil {
		return nil, err
	}

	localRepository, err := repositories.NewLocaisRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	service := NewLocalService(localRepository)
	return service, nil
}

func GetInspecaoEntradaHistoricoService(ctx *fiber.Ctx) (interfaces.IInspecaoEntradaHistoricoService, error) {
	uow, err := GetUnitOfWork(ctx)
	if err != nil {
		return nil, err
	}

	baseParams, err := getBaseParams(ctx)
	if err != nil {
		return nil, err
	}

	inspecaoSaidaHistoricoRepository := repositories.NewInspecaoEntradaHistoricoRepository(uow, baseParams)

	externalSagaService := NewExternalInspecaoEntradaSagaService(baseParams)

	inspecaoSaidaExecutadoWebRepository, err := repositories.NewInspecaoEntradaExecutadoWebRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	localRepository, err := repositories.NewLocaisRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	service := NewInspecaoEntradaHistoricoService(inspecaoSaidaHistoricoRepository, externalSagaService,
		inspecaoSaidaExecutadoWebRepository, localRepository)
	return service, nil
}

func GetFileProviderProxyService(ctx *fiber.Ctx) (interfaces.IFileProviderProxyService, error) {
	baseParams, err := getBaseParams(ctx)
	if err != nil {
		return nil, err
	}

	service := NewFileProviderProxyService(baseParams, ctx)
	return service, nil
}

func GetLoteService(ctx *fiber.Ctx) (interfaces.ILoteService, error) {
	uow, err := GetUnitOfWork(ctx)
	if err != nil {
		return nil, err
	}

	baseParams, err := getBaseParams(ctx)
	if err != nil {
		return nil, err
	}

	wmsProxyService := NewWmsProxyService(baseParams, ctx)

	produtoRepository, err := repositories.NewProdutoRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	service := NewLoteService(wmsProxyService, produtoRepository)

	return service, nil
}
