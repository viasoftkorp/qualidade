package services

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/externalServices"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/repositories"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	"bitbucket.org/viasoftkorp/korp.sdk/ambient_data"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"github.com/gofiber/fiber/v2"
)

func GetOrdemProducaoService(ctx *fiber.Ctx) (interfaces.IOrdemProducaoService, error) {
	ambientDataModel, err := ambient_data.GetAmbientDataFromContext(ctx)
	if err != nil {
		return nil, err
	}

	baseParams, err := getBaseParams(ctx)
	if err != nil {
		return nil, err
	}

	uow, err := unit_of_work.NewUnitOfWork(ambientDataModel, utils.MigrateEntities)
	if err != nil {
		return nil, err
	}

	ordemProducaoRepository, err := repositories.NewOrdemProducaoRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	service := NewOrdemProducaoService(ordemProducaoRepository)

	return service, nil
}

func GetInspecaoSaidaService(ctx *fiber.Ctx) (interfaces.IInspecaoSaidaService, error) {
	ambientDataModel, err := ambient_data.GetAmbientDataFromContext(ctx)
	if err != nil {
		return nil, err
	}

	baseParams, err := getBaseParams(ctx)
	if err != nil {
		return nil, err
	}

	uow, err := unit_of_work.NewUnitOfWork(ambientDataModel, utils.MigrateEntities)
	if err != nil {
		return nil, err
	}

	inspecaoSaidaRepository, err := repositories.NewInspecaoSaidaRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	inspecaoSaidaItemRepository, err := repositories.NewInspecaoSaidaItemRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	ordemProducaoRepository, err := repositories.NewOrdemProducaoRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	planosInspecaoRepository, err := repositories.NewPlanosInspecaoRepository(uow, baseParams)
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

	service := NewInspecaoSaidaService(uow, inspecaoSaidaRepository, inspecaoSaidaItemRepository, ordemProducaoRepository,
		planosInspecaoRepository, baseParams, impressaoService, empresaRepository)

	return service, nil
}

func GetParametroService(ctx *fiber.Ctx) (interfaces.IParametroService, error) {
	ambientDataModel, err := ambient_data.GetAmbientDataFromContext(ctx)
	if err != nil {
		return nil, err
	}

	uow, err := unit_of_work.NewUnitOfWork(ambientDataModel, utils.MigrateEntities)
	if err != nil {
		return nil, err
	}

	repo := repositories.NewParametroRepository(uow)

	service := NewParametroService(repo)

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

func GetInspecaoSaidaSagaService(ctx *fiber.Ctx) (interfaces.IInspecaoSaidaSagaService, error) {
	uow, err := GetUnitOfWork(ctx)
	if err != nil {
		return nil, err
	}

	baseParams, err := getBaseParams(ctx)
	if err != nil {
		return nil, err
	}

	inspecaoSaidaRepository, err := repositories.NewInspecaoSaidaRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	inspecaoSaidaExecutadoWebRepository, err := repositories.NewInspecaoSaidaExecutadoWebRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	ordemProducaoRepository, err := repositories.NewOrdemProducaoRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	estoqueLocalPedidoVendaRepository, err := repositories.NewEstoqueLocalPedidoVendaRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	localRepository, err := repositories.NewLocaisRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	parametroRepository := repositories.NewParametroRepository(uow)

	externalSagaService := externalServices.NewExternalInspecaoSaidaSagaService(baseParams)

	produtoRepository, err := repositories.NewProdutoRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	service := NewInspecaoSaidaSagaService(
		inspecaoSaidaRepository,
		inspecaoSaidaExecutadoWebRepository,
		externalSagaService,
		ordemProducaoRepository,
		localRepository,
		parametroRepository,
		estoqueLocalPedidoVendaRepository,
		produtoRepository,
		uow)
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

func GetEngenhariaService(ctx *fiber.Ctx) (interfaces.IEngenhariaService, error) {
	uow, err := GetUnitOfWork(ctx)
	if err != nil {
		return nil, err
	}

	baseParams, err := getBaseParams(ctx)
	if err != nil {
		return nil, err
	}

	engenhariaRepository, err := repositories.NewEngenhariaRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	service := NewEngenhariaService(engenhariaRepository)
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

func GetInspecaoSaidaHistoricoService(ctx *fiber.Ctx) (interfaces.IInspecaoSaidaHistoricoService, error) {
	uow, err := GetUnitOfWork(ctx)
	if err != nil {
		return nil, err
	}

	baseParams, err := getBaseParams(ctx)
	if err != nil {
		return nil, err
	}

	inspecaoSaidaHistoricoRepository := repositories.NewInspecaoSaidaHistoricoRepository(uow, baseParams)

	externalSagaService := externalServices.NewExternalInspecaoSaidaSagaService(baseParams)

	inspecaoSaidaExecutadoWebRepository, err := repositories.NewInspecaoSaidaExecutadoWebRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	localRepository, err := repositories.NewLocaisRepository(uow, baseParams)
	if err != nil {
		return nil, err
	}

	service := NewInspecaoSaidaHistoricoService(uow, inspecaoSaidaHistoricoRepository, baseParams, externalSagaService,
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
