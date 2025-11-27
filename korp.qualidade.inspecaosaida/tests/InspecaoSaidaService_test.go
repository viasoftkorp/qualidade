package tests

import (
	"errors"
	"log"
	"strconv"
	"testing"
	"time"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/mappers"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/mocks"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/services"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	"github.com/golang/mock/gomock"
	"github.com/google/uuid"
	"github.com/shopspring/decimal"
	"github.com/stretchr/testify/assert"
)

func InstanciarBaseparams() *models.BaseParams {
	return &models.BaseParams{
		UserLogin:       "",
		LegacyCompanyId: 14793,
	}
}

func GetUUIDWithoutError() uuid.UUID {
	id, err := uuid.NewUUID()
	if err != nil {
		log.Fatal(err)
	}
	return id
}

func InstanciarInput(odf int, qtd float64, id1 uuid.UUID, id2 uuid.UUID) *dto.NovaInspecaoInput {
	return &dto.NovaInspecaoInput{
		Odf:        odf,
		Quantidade: qtd,
		PlanosInspecao: []*dto.PlanoInspecaoDTO{
			{Id: id1.String(), Resultado: "Parcialmente Aprov."},
			{Id: id2.String(), Resultado: "Parcialmente Aprov."},
		},
	}
}

func InstanciarInspecaoSaidaItem(input dto.NovaInspecaoInput, baseParams models.BaseParams, plano *models.PlanoInspecao, id1 uuid.UUID, id2 uuid.UUID) []*models.InspecaoSaidaItem {
	return []*models.InspecaoSaidaItem{
		{
			LegacyIdPlanoInspecao: plano.LegacyId,
			Plano:                 input.Plano,
			Odf:                   input.Odf,
			Descricao:             plano.Descricao,
			Metodo:                plano.Metodo,
			Sequencia:             strconv.Itoa(1),
			Resultado:             plano.Resultado,
			MaiorValor:            decimal.NewFromFloat(plano.MaiorValor),
			MenorValor:            decimal.NewFromFloat(plano.MenorValor),
			CodigoInspecao:        14,
			IdEmpresa:             baseParams.LegacyCompanyId,
		},
		{
			LegacyIdPlanoInspecao: plano.LegacyId,
			Plano:                 input.Plano,
			Odf:                   input.Odf,
			Descricao:             plano.Descricao,
			Metodo:                plano.Metodo,
			Sequencia:             strconv.Itoa(2),
			Resultado:             "Parcialmente Aprov.",
			MaiorValor:            decimal.NewFromFloat(plano.MaiorValor),
			MenorValor:            decimal.NewFromFloat(plano.MenorValor),
			CodigoInspecao:        14,
			IdEmpresa:             baseParams.LegacyCompanyId,
		},
	}
}

func InstanciarOrdemProducao(qtdProduzida decimal.Decimal) *models.OrdemProducao {
	return &models.OrdemProducao{
		QuantidadeProduzida: qtdProduzida,
	}
}

func InstanciarFilters() *models.BaseFilter {
	return &models.BaseFilter{
		Skip:     14,
		PageSize: 1,
	}
}

func InstanciarInspecaoSaida(ordemProducao *models.OrdemProducao, qtd float64) *models.InspecaoSaida {
	return &models.InspecaoSaida{
		CodigoInspecao: 14,
		Cliente:        ordemProducao.Cliente,
		Pedido:         ordemProducao.NumeroPedido,
		Odf:            ordemProducao.ODF,
		DataInspecao:   time.Now().Format("20060102"),
		Inspetor:       "",
		QtdInspecao:    decimal.NewFromFloat(qtd),
		IdEmpresa:      14793,
		QtdLote:        ordemProducao.QuantidadeLote,
	}
}

func InstanciarPlanosInspecaoNaoAlterados(id1 uuid.UUID, id2 uuid.UUID) []*models.PlanoInspecao {

	return []*models.PlanoInspecao{
		{},
		{Id: id1},
	}
}

func InstanciarMapperSaidaDTO(inspecaoSaida *models.InspecaoSaida) *dto.InspecaoSaidaDTO {
	return &dto.InspecaoSaidaDTO{
		CodigoInspecao:         inspecaoSaida.CodigoInspecao,
		ODF:                    inspecaoSaida.Odf,
		DataInspecao:           utils.StringToTime(inspecaoSaida.DataInspecao),
		Inspetor:               inspecaoSaida.Inspetor,
		Resultado:              inspecaoSaida.Resultado,
		QuantidadeInspecao:     utils.DecimalToFloat64(inspecaoSaida.QtdInspecao),
		QuantidadeLote:         utils.DecimalToFloat64(inspecaoSaida.QtdLote),
		QuantidadeAceita:       utils.DecimalToFloat64(inspecaoSaida.QuantidadeAceita),
		QuantidadeRetrabalhada: utils.DecimalToFloat64(inspecaoSaida.QuantidadeRetrabalhada),
		QuantidadeAprovada:     utils.DecimalToFloat64(inspecaoSaida.QuantidadeAprovada),
		QuantidadeReprovada:    utils.DecimalToFloat64(inspecaoSaida.QuantidadeReprovada),
		Lote:                   inspecaoSaida.Lote,
		RecnoInspecaoSaida:     inspecaoSaida.Recno,
		CodigoProduto:          inspecaoSaida.CodigoProduto,
		QuantidadeOrdem:        utils.DecimalToFloat64(inspecaoSaida.QuantidadeOrdem),
		NumeroPedido:           inspecaoSaida.NumeroPedido,
	}
}

func InstanciarSimpleInspecaoSaidaItens() []*models.InspecaoSaidaItem {
	return []*models.InspecaoSaidaItem{
		{Plano: "Mudar o mundo"},
	}
}

func InstanciarSimpleEntitiesInspecaoSaidaItens(id uuid.UUID) []*entities.InspecaoSaidaItem {
	return []*entities.InspecaoSaidaItem{
		{Id: id, Plano: "Mudar o mundo"},
	}
}

func InstanciarSimpleInspecaoSaida() *entities.InspecaoSaida {
	return &entities.InspecaoSaida{
		CodigoInspecao: 575788,
		Recno:          575788,
	}
}

func InstanciarUUID() {

}

func InstanciarAtualizarInspecaoInput(id uuid.UUID) *dto.AtualizarInspecaoInput {
	return &dto.AtualizarInspecaoInput{
		CodInspecao: 58763,
		Itens: []*dto.InspecaoSaidaItemDTO{
			{Id: id.String(), MaiorValor: 87.65, MenorValor: 74.6, Resultado: "Faturado"},
		},
	}
}
func TestBuscarInspecoesSaidas(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	mockInspecaoSaidaRepo := mocks.NewMockIInspecaoSaidaRepository(mockCtrl)
	service := services.NewInspecaoSaidaService(nil, mockInspecaoSaidaRepo, nil, nil, nil, nil, nil, nil)

	odf := 14
	baseFilters := &models.BaseFilter{
		PageSize: 1,
		Skip:     0,
	}
	filters := &dto.InspecaoSaidaFilters{}
	inspecoesResult := []*models.InspecaoSaida{}
	qtdInspecoes := int64(0)
	t.Run("ErroBuscarInspecoesSaida", func(t1 *testing.T) {

		mockInspecaoSaidaRepo.EXPECT().
			BuscarInspecoesSaida(odf, baseFilters, filters).
			Return(inspecoesResult, errors.New("Test 2")).
			Times(1)

		actualOutput, err := service.BuscarInspecoesSaida(odf, baseFilters, filters)
		validacaoDTO := &dto.ValidacaoDTO{
			Code:    3,
			Message: "Test 2",
		}
		assert.Nil(t, actualOutput)
		assert.Equal(t, validacaoDTO, err)

	})

	t.Run("BuscarQuantidadeInspecoesSaida", func(t2 *testing.T) {

		mockInspecaoSaidaRepo.EXPECT().
			BuscarInspecoesSaida(odf, baseFilters, filters).
			Return(inspecoesResult, nil).
			Times(1)

		mockInspecaoSaidaRepo.EXPECT().
			BuscarQuantidadeInspecoesSaida(odf, baseFilters, filters).
			Return(qtdInspecoes, errors.New("Test 2")).
			Times(1)
		actualOutput, err := service.BuscarInspecoesSaida(odf, baseFilters, filters)
		validacaoDTO := &dto.ValidacaoDTO{
			Code:    4,
			Message: "Test 2",
		}
		assert.Nil(t, actualOutput)
		assert.Equal(t, validacaoDTO, err)

	})

	mockInspecaoSaidaRepo.EXPECT().
		BuscarInspecoesSaida(odf, baseFilters, filters).
		Return(inspecoesResult, nil).
		Times(1)

	mockInspecaoSaidaRepo.EXPECT().
		BuscarQuantidadeInspecoesSaida(odf, baseFilters, filters).
		Return(qtdInspecoes, nil).
		Times(1)

	expectedOutput := &dto.GetInspecaoSaidaDTO{
		Items:      mappers.MapInspecaoSaidaDetalhesToDTOs(inspecoesResult),
		TotalCount: qtdInspecoes,
	}
	actualOutput, err := service.BuscarInspecoesSaida(odf, baseFilters, filters)

	assert.Nil(t, err)
	assert.Equal(t, expectedOutput, actualOutput)

}

func TestBuscarPlanosNovaInspecao(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	mockPlanoInspecaoRepo := mocks.NewMockIPlanosInspecaoRepository(mockCtrl)
	service := services.NewInspecaoSaidaService(nil, nil, nil, nil, mockPlanoInspecaoRepo, nil, nil, nil)
	odf := 14
	plano := "Teste"
	filter := &models.BaseFilter{
		Skip:     14,
		PageSize: 12,
	}
	planoInspecao := []*models.PlanoInspecao{}

	t.Run("ErroBuscarPlanosNovaInspecao", func(t1 *testing.T) {
		mockPlanoInspecaoRepo.EXPECT().
			BuscarPlanosNovaInspecao(odf, plano, filter).
			Return(planoInspecao, errors.New("Test 1")).
			Times(1)
		actualOuput, err := service.BuscarPlanosNovaInspecao(odf, plano, filter)
		expectedOutput := &dto.ValidacaoDTO{
			Code:    5,
			Message: "Test 1",
		}

		assert.Nil(t, actualOuput)
		assert.Equal(t, expectedOutput, err)
	})

	t.Run("ErroBuscarQuantidadePlanosNovaInspecao", func(t1 *testing.T) {
		mockPlanoInspecaoRepo.EXPECT().
			BuscarPlanosNovaInspecao(odf, plano, filter).
			Return(planoInspecao, nil).
			Times(1)
		mockPlanoInspecaoRepo.EXPECT().
			BuscarQuantidadePlanosNovaInspecao(odf, plano).
			Return(int64(14), errors.New("Test 2")).
			Times(1)
		actualOuput, err := service.BuscarPlanosNovaInspecao(odf, plano, filter)
		expectedOutput := &dto.ValidacaoDTO{
			Code:    6,
			Message: "Test 2",
		}
		assert.Nil(t, actualOuput)
		assert.Equal(t, expectedOutput, err)
	})

	mockPlanoInspecaoRepo.EXPECT().
		BuscarPlanosNovaInspecao(odf, plano, filter).
		Return(planoInspecao, nil).
		Times(1)
	mockPlanoInspecaoRepo.EXPECT().
		BuscarQuantidadePlanosNovaInspecao(odf, plano).
		Return(int64(14), nil).
		Times(1)
	actualOuput, err := service.BuscarPlanosNovaInspecao(odf, plano, filter)
	expectedOutput := &dto.GetPlanosInspecaoDTO{Items: nil, TotalCount: 14}

	assert.Nil(t, err)
	assert.Equal(t, expectedOutput, actualOuput)
}

// func TestCriarInspecao(t *testing.T) {
// 	mockCtrl := gomock.NewController(t)
// 	mockPlanoInspecaoRepo := mocks.NewMockIPlanosInspecaoRepository(mockCtrl)
// 	mockInspecaoSaidaItemRepo := mocks.NewMockIInspecaoSaidaItemRepository(mockCtrl)
// 	mockOrdemProducaoRepo := mocks.NewMockIOrdemProducaoRepository(mockCtrl)
// 	mockInspecaoRepo := mocks.NewMockIInspecaoSaidaRepository(mockCtrl)
// 	baseParams := InstanciarBaseparams()
// 	uow := mocks.NewMockUnitOfWork(mockCtrl)
// 	id1 := GetUUIDWithoutError()
// 	id2 := GetUUIDWithoutError()
// 	service := services.NewInspecaoSaidaService(uow, mockInspecaoRepo, mockInspecaoSaidaItemRepo, mockOrdemProducaoRepo, mockPlanoInspecaoRepo, baseParams)
// 	odf := 1231
// 	qtdProduzida := decimal.NewFromInt(14)
// 	input := InstanciarInput(odf, 10, id1, id2)
// 	inputErro := InstanciarInput(odf, 20, id1, id2)
// 	ordemProducao := InstanciarOrdemProducao(qtdProduzida)
// 	ordemProducaoErro := InstanciarOrdemProducao(qtdProduzida)
// 	inspecaoModel := InstanciarInspecaoSaida(ordemProducao, 10)
// 	planosInspecaoNaoAlterados := InstanciarPlanosInspecaoNaoAlterados(id1, id2)
// 	itensInspecao := InstanciarInspecaoSaidaItem(*input, *baseParams, planosInspecaoNaoAlterados[0], id1, id2)
// 	t.Run("ErroBuscarOrdem", func(t1 *testing.T) {
// 		mockOrdemProducaoRepo.EXPECT().
// 			BuscarOrdem(input.Odf).
// 			Return(nil).
// 			Times(1)
// 		actualOutput, err := service.CriarInspecao(input)
// 		validacaoDTO := &dto.ValidacaoDTO{
// 			Code:    7,
// 			Message: "Ordem de produção não encontrada.",
// 		}

// 		assert.Equal(t1, 0, actualOutput)
// 		assert.Equal(t1, validacaoDTO, err)
// 	})

// 	t.Run("ErroBuscarOrdemCompareInputToOrdem", func(t2 *testing.T) {
// 		mockOrdemProducaoRepo.EXPECT().
// 			BuscarOrdem(inputErro.Odf).
// 			Return(ordemProducaoErro).
// 			Times(1)

// 		actualOutput, err := service.CriarInspecao(inputErro)
// 		validacaoDTO := &dto.ValidacaoDTO{
// 			Code:    28,
// 			Message: "Atenção a quantidade da inspeção não pode ser maior que a quantidade restante para inspecionar!",
// 		}

// 		assert.Equal(t2, 0, actualOutput)
// 		assert.Equal(t2, validacaoDTO, err)
// 	})

// 	t.Run("ErroCriarInspecao", func(t3 *testing.T) {
// 		mockOrdemProducaoRepo.EXPECT().
// 			BuscarOrdem(inputErro.Odf).
// 			Return(ordemProducaoErro).
// 			Times(1)
// 		mockInspecaoRepo.EXPECT().
// 			BuscarNovoCodigoInspecao().
// 			Return(14).
// 			Times(1)
// 		uow.EXPECT().
// 			Begin().
// 			Times(1)
// 		mockInspecaoRepo.EXPECT().
// 			CriarInspecao(inspecaoModel).
// 			Return(errors.New("Test 3")).
// 			Times(1)
// 		uow.EXPECT().
// 			Rollback().
// 			Times(1)
// 		uow.EXPECT().
// 			UnitOfWorkGuard().
// 			Times(1)
// 		actualOutput, err := service.CriarInspecao(input)
// 		validacaoDTO := &dto.ValidacaoDTO{
// 			Code:    8,
// 			Message: "Test 3",
// 		}
// 		assert.Equal(t3, 0, actualOutput)
// 		assert.Equal(t3, validacaoDTO, err)

// 	})

// 	t.Run("ErroCriarInspecao", func(t4 *testing.T) {
// 		mockOrdemProducaoRepo.EXPECT().
// 			BuscarOrdem(inputErro.Odf).
// 			Return(ordemProducaoErro).
// 			Times(1)
// 		mockInspecaoRepo.EXPECT().
// 			BuscarNovoCodigoInspecao().
// 			Return(14).
// 			Times(1)
// 		uow.EXPECT().
// 			Begin().
// 			Times(1)
// 		mockInspecaoRepo.EXPECT().
// 			CriarInspecao(inspecaoModel).
// 			Return(nil).
// 			Times(1)
// 		uow.EXPECT().
// 			Rollback().
// 			Times(1)
// 		uow.EXPECT().
// 			UnitOfWorkGuard().
// 			Times(1)
// 		mockPlanoInspecaoRepo.EXPECT().
// 			BuscarTodosPlanosOdfProduto(ordemProducaoErro.RecnoProcesso, input.Plano).
// 			Return(planosInspecaoNaoAlterados, errors.New("Test 4")).
// 			Times(1)
// 		actualOutput, err := service.CriarInspecao(input)
// 		validacaoDTO := &dto.ValidacaoDTO{
// 			Code:    9,
// 			Message: "Test 4",
// 		}
// 		assert.Equal(t4, 0, actualOutput)
// 		assert.Equal(t4, validacaoDTO, err)

// 	})

// 	t.Run("ErroCriarInspecao", func(t5 *testing.T) {
// 		mockOrdemProducaoRepo.EXPECT().
// 			BuscarOrdem(inputErro.Odf).
// 			Return(ordemProducaoErro).
// 			Times(1)
// 		mockInspecaoRepo.EXPECT().
// 			BuscarNovoCodigoInspecao().
// 			Return(14).
// 			Times(1)
// 		uow.EXPECT().
// 			Begin().
// 			Times(1)
// 		mockInspecaoRepo.EXPECT().
// 			CriarInspecao(inspecaoModel).
// 			Return(nil).
// 			Times(1)
// 		uow.EXPECT().
// 			UnitOfWorkGuard().
// 			Times(1)
// 		uow.EXPECT().
// 			Rollback().
// 			Times(1)
// 		mockPlanoInspecaoRepo.EXPECT().
// 			BuscarTodosPlanosOdfProduto(ordemProducaoErro.RecnoProcesso, input.Plano).
// 			Return(planosInspecaoNaoAlterados, nil).
// 			Times(1)
// 		mockInspecaoSaidaItemRepo.EXPECT().
// 			RemoverInspecaoSaidaItensPeloCodigo(inspecaoModel.CodigoInspecao).
// 			Return(nil).
// 			Times(1)
// 		mockInspecaoSaidaItemRepo.EXPECT().
// 			CriarItensInspecao(itensInspecao).
// 			Return(errors.New("Test 5")).
// 			Times(1)
// 		actualOutput, err := service.CriarInspecao(input)
// 		validacaoDTO := &dto.ValidacaoDTO{
// 			Code:    10,
// 			Message: "Test 5",
// 		}
// 		assert.Equal(t5, 0, actualOutput)
// 		assert.Equal(t5, validacaoDTO, err)

// 	})
// 	mockOrdemProducaoRepo.EXPECT().
// 		BuscarOrdem(inputErro.Odf).
// 		Return(ordemProducaoErro).
// 		Times(1)
// 	mockInspecaoRepo.EXPECT().
// 		BuscarNovoCodigoInspecao().
// 		Return(14).
// 		Times(1)
// 	uow.EXPECT().
// 		Begin().
// 		Times(1)
// 	mockInspecaoRepo.EXPECT().
// 		CriarInspecao(inspecaoModel).
// 		Return(nil).
// 		Times(1)
// 	uow.EXPECT().
// 		UnitOfWorkGuard().
// 		Times(1)
// 	uow.EXPECT().
// 		Complete().
// 		Times(1)
// 	mockPlanoInspecaoRepo.EXPECT().
// 		BuscarTodosPlanosOdfProduto(ordemProducaoErro.RecnoProcesso, input.Plano).
// 		Return(planosInspecaoNaoAlterados, nil).
// 		Times(1)
// 	mockInspecaoSaidaItemRepo.EXPECT().
// 		RemoverInspecaoSaidaItensPeloCodigo(inspecaoModel.CodigoInspecao).
// 		Return(nil).
// 		Times(1)
// 	mockInspecaoSaidaItemRepo.EXPECT().
// 		CriarItensInspecao(itensInspecao).
// 		Return(nil).
// 		Times(1)
// 	actualOutput, err := service.CriarInspecao(input)
// 	expectedErr := (*dto.ValidacaoDTO)(nil)
// 	expectedOutput := 14
// 	assert.Equal(t, expectedOutput, actualOutput)
// 	assert.Equal(t, expectedErr, err)
// }

func TestBuscarInspecaoSaidaPeloCodigo(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	mockPlanoInspecaoRepo := mocks.NewMockIPlanosInspecaoRepository(mockCtrl)
	mockInspecaoSaidaItemRepo := mocks.NewMockIInspecaoSaidaItemRepository(mockCtrl)
	mockOrdemProducaoRepo := mocks.NewMockIOrdemProducaoRepository(mockCtrl)
	mockInspecaoRepo := mocks.NewMockIInspecaoSaidaRepository(mockCtrl)
	mockImpressaoService := mocks.NewMockIImpressaoService(mockCtrl)
	mockEmpresaRepo := mocks.NewMockIEmpresaRepository(mockCtrl)
	baseParams := InstanciarBaseparams()
	uow := mocks.NewMockUnitOfWork(mockCtrl)
	service := services.NewInspecaoSaidaService(uow, mockInspecaoRepo, mockInspecaoSaidaItemRepo, mockOrdemProducaoRepo, mockPlanoInspecaoRepo, baseParams, mockImpressaoService, mockEmpresaRepo)
	codInspecao := 14
	ordemProducao := InstanciarOrdemProducao(decimal.NewFromInt(14))
	inspecaoSaida := InstanciarInspecaoSaida(ordemProducao, 14)
	result := InstanciarMapperSaidaDTO(inspecaoSaida)

	t.Run("ErroBuscarInspecaoSaidaDetalhesPeloCodigo", func(t1 *testing.T) {
		mockInspecaoRepo.EXPECT().
			BuscarInspecaoSaidaDetalhesPeloCodigo(codInspecao).
			Return(inspecaoSaida, errors.New("Test 1")).
			Times(1)

		actualOutput, err := service.BuscarInspecaoSaidaPeloCodigo(codInspecao)
		validacaoDTO := &dto.ValidacaoDTO{
			Code:    11,
			Message: "Test 1",
		}
		assert.Nil(t1, actualOutput)
		assert.Equal(t1, validacaoDTO, err)

	})

	t.Run("IfResultNull", func(t2 *testing.T) {
		inspecaoSaida.Resultado = "1234"
		mockInspecaoRepo.EXPECT().
			BuscarInspecaoSaidaDetalhesPeloCodigo(codInspecao).
			Return(inspecaoSaida, nil).
			Times(1)

		actualOutput, err := service.BuscarInspecaoSaidaPeloCodigo(codInspecao)
		validacaoDTO := &dto.ValidacaoDTO{
			Code:    26,
			Message: "A inspeção informada já foi finalizada, portanto não é possível alterá-la.",
		}
		assert.Nil(t2, actualOutput)
		assert.Equal(t2, validacaoDTO, err)
	})
	inspecaoSaida.Resultado = ""

	mockInspecaoRepo.EXPECT().
		BuscarInspecaoSaidaDetalhesPeloCodigo(codInspecao).
		Return(inspecaoSaida, nil).
		Times(1)

	actualOutput, err := service.BuscarInspecaoSaidaPeloCodigo(codInspecao)

	assert.Nil(t, err)
	assert.Equal(t, result, actualOutput)
}

func TestBuscarInspecaoSaidaItensPeloCodigo(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	mockPlanoInspecaoRepo := mocks.NewMockIPlanosInspecaoRepository(mockCtrl)
	mockInspecaoSaidaItemRepo := mocks.NewMockIInspecaoSaidaItemRepository(mockCtrl)
	mockOrdemProducaoRepo := mocks.NewMockIOrdemProducaoRepository(mockCtrl)
	mockInspecaoRepo := mocks.NewMockIInspecaoSaidaRepository(mockCtrl)
	baseParams := InstanciarBaseparams()
	mockImpressaoService := mocks.NewMockIImpressaoService(mockCtrl)
	mockEmpresaRepo := mocks.NewMockIEmpresaRepository(mockCtrl)
	uow := mocks.NewMockUnitOfWork(mockCtrl)
	service := services.NewInspecaoSaidaService(uow, mockInspecaoRepo, mockInspecaoSaidaItemRepo, mockOrdemProducaoRepo, mockPlanoInspecaoRepo, baseParams, mockImpressaoService, mockEmpresaRepo)
	itens := InstanciarSimpleInspecaoSaidaItens()
	codInspecao := 22
	filter := InstanciarFilters()
	t.Run("ErroBuscarInspecaoSaidaItensPeloCodigo", func(t1 *testing.T) {
		mockInspecaoSaidaItemRepo.EXPECT().
			BuscarInspecaoSaidaItensPeloCodigo(codInspecao, filter).
			Return(itens, errors.New("Test 1")).
			Times(1)
		actualOutput, err := service.BuscarInspecaoSaidaItensPeloCodigo(codInspecao, filter)
		validacaoDTO := &dto.ValidacaoDTO{
			Code:    12,
			Message: "Test 1",
		}
		assert.Nil(t1, actualOutput)
		assert.Equal(t1, validacaoDTO, err)
	})

	t.Run("ErroBuscarQuantidadeInspecaoSaidaItensPeloCodigo", func(t2 *testing.T) {
		mockInspecaoSaidaItemRepo.EXPECT().
			BuscarInspecaoSaidaItensPeloCodigo(codInspecao, filter).
			Return(itens, nil).
			Times(1)
		mockInspecaoSaidaItemRepo.EXPECT().
			BuscarQuantidadeInspecaoSaidaItensPeloCodigo(codInspecao).
			Return(int64(14), errors.New("Test 2")).
			Times(1)
		actualOutput, err := service.BuscarInspecaoSaidaItensPeloCodigo(codInspecao, filter)
		validacaoDTO := &dto.ValidacaoDTO{
			Code:    13,
			Message: "Test 2",
		}
		assert.Nil(t2, actualOutput)
		assert.Equal(t2, validacaoDTO, err)
	})

	mockInspecaoSaidaItemRepo.EXPECT().
		BuscarInspecaoSaidaItensPeloCodigo(codInspecao, filter).
		Return(itens, nil).
		Times(1)
	mockInspecaoSaidaItemRepo.EXPECT().
		BuscarQuantidadeInspecaoSaidaItensPeloCodigo(codInspecao).
		Return(int64(14), nil).
		Times(1)
	actualOutput, err := service.BuscarInspecaoSaidaItensPeloCodigo(codInspecao, filter)
	expectedOutput := &dto.GetInspecaoSaidaItensDTO{
		Items:      mappers.MapInspecaoSaidaItemModelsToDTOs(itens),
		TotalCount: int64(14),
	}
	assert.Nil(t, err)
	assert.Equal(t, expectedOutput, actualOutput)

}

func TestRemoverInspecaoSaidaPeloCodigo(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	mockPlanoInspecaoRepo := mocks.NewMockIPlanosInspecaoRepository(mockCtrl)
	mockInspecaoSaidaItemRepo := mocks.NewMockIInspecaoSaidaItemRepository(mockCtrl)
	mockOrdemProducaoRepo := mocks.NewMockIOrdemProducaoRepository(mockCtrl)
	mockInspecaoRepo := mocks.NewMockIInspecaoSaidaRepository(mockCtrl)
	baseParams := InstanciarBaseparams()
	mockImpressaoService := mocks.NewMockIImpressaoService(mockCtrl)
	mockEmpresaRepo := mocks.NewMockIEmpresaRepository(mockCtrl)
	uow := mocks.NewMockUnitOfWork(mockCtrl)
	service := services.NewInspecaoSaidaService(uow, mockInspecaoRepo, mockInspecaoSaidaItemRepo, mockOrdemProducaoRepo, mockPlanoInspecaoRepo, baseParams, mockImpressaoService, mockEmpresaRepo)
	codInspecao := 575788
	inspecao := InstanciarSimpleInspecaoSaida()
	t.Run("ErroBuscarInspecaoSaidaPeloCodigo", func(t1 *testing.T) {
		mockInspecaoRepo.EXPECT().
			BuscarInspecaoSaidaPeloCodigo(codInspecao).
			Return(inspecao, errors.New("Test 1")).
			Times(1)
		err := service.RemoverInspecaoSaidaPeloCodigo(codInspecao)
		validacaoDTO := &dto.ValidacaoDTO{
			Code:    14,
			Message: "Test 1",
		}
		assert.Equal(t1, validacaoDTO, err)
	})
	t.Run("IfResultNull", func(t2 *testing.T) {
		mockInspecaoRepo.EXPECT().
			BuscarInspecaoSaidaPeloCodigo(codInspecao).
			Return(inspecao, nil).
			Times(1)
		inspecao.Resultado = "Finalizada"

		err := service.RemoverInspecaoSaidaPeloCodigo(codInspecao)
		validacaoDTO := &dto.ValidacaoDTO{
			Code:    15,
			Message: "A inspeção informada já foi finalizada, portanto não é possível removê-la.",
		}
		assert.Equal(t2, validacaoDTO, err)
	})

	t.Run("ErroRemoverInspecaoSaida", func(t3 *testing.T) {
		mockInspecaoRepo.EXPECT().
			BuscarInspecaoSaidaPeloCodigo(codInspecao).
			Return(inspecao, nil).
			Times(1)
		mockInspecaoRepo.EXPECT().
			RemoverInspecaoSaida(codInspecao).
			Return(errors.New("Test 3")).
			Times(1)
		inspecao.Resultado = ""

		err := service.RemoverInspecaoSaidaPeloCodigo(codInspecao)
		validacaoDTO := &dto.ValidacaoDTO{
			Code:    16,
			Message: "Test 3",
		}
		assert.Equal(t3, validacaoDTO, err)
	})

	t.Run("ErroRemoverInspecaoSaidaItensPeloCodigo", func(t4 *testing.T) {
		mockInspecaoRepo.EXPECT().
			BuscarInspecaoSaidaPeloCodigo(codInspecao).
			Return(inspecao, nil).
			Times(1)
		mockInspecaoRepo.EXPECT().
			RemoverInspecaoSaida(codInspecao).
			Return(nil).
			Times(1)
		inspecao.Resultado = ""
		mockInspecaoSaidaItemRepo.EXPECT().
			RemoverInspecaoSaidaItensPeloCodigo(codInspecao).
			Return(errors.New("Test 4")).
			Times(1)
		err := service.RemoverInspecaoSaidaPeloCodigo(codInspecao)
		validacaoDTO := &dto.ValidacaoDTO{
			Code:    22,
			Message: "Test 4",
		}
		assert.Equal(t4, validacaoDTO, err)
	})

	mockInspecaoRepo.EXPECT().
		BuscarInspecaoSaidaPeloCodigo(codInspecao).
		Return(inspecao, nil).
		Times(1)
	mockInspecaoRepo.EXPECT().
		RemoverInspecaoSaida(codInspecao).
		Return(nil).
		Times(1)
	inspecao.Resultado = ""
	mockInspecaoSaidaItemRepo.EXPECT().
		RemoverInspecaoSaidaItensPeloCodigo(codInspecao).
		Return(nil).
		Times(1)
	err := service.RemoverInspecaoSaidaPeloCodigo(codInspecao)
	assert.Nil(t, err)

}

// func TestAtualizarInspecao(t *testing.T) {
// 	mockCtrl := gomock.NewController(t)
// 	mockPlanoInspecaoRepo := mocks.NewMockIPlanosInspecaoRepository(mockCtrl)
// 	mockInspecaoSaidaItemRepo := mocks.NewMockIInspecaoSaidaItemRepository(mockCtrl)
// 	mockOrdemProducaoRepo := mocks.NewMockIOrdemProducaoRepository(mockCtrl)
// 	mockInspecaoRepo := mocks.NewMockIInspecaoSaidaRepository(mockCtrl)
// 	baseParams := InstanciarBaseparams()
// 	uow := mocks.NewMockUnitOfWork(mockCtrl)
// 	service := services.NewInspecaoSaidaService(uow, mockInspecaoRepo, mockInspecaoSaidaItemRepo, mockOrdemProducaoRepo, mockPlanoInspecaoRepo, baseParams)
// 	codInspecao := 58763
// 	inspecao := InstanciarSimpleInspecaoSaida()
// 	id := GetUUIDWithoutError()
// 	input := InstanciarAtualizarInspecaoInput(id)
// 	itens := InstanciarSimpleEntitiesInspecaoSaidaItens(id)
// 	t.Run("ErroBuscarInspecaoSaidaPeloCodigo", func(t1 *testing.T) {
// 		mockInspecaoRepo.EXPECT().
// 			BuscarInspecaoSaidaPeloCodigo(codInspecao).
// 			Return(inspecao, errors.New("Test 1")).
// 			Times(1)
// 		err := service.AtualizarInspecao(input)

// 		validacaoDTO := &dto.ValidacaoDTO{
// 			Code:    17,
// 			Message: "Test 1",
// 		}
// 		assert.Equal(t1, validacaoDTO, err)
// 	})
// 	t.Run("IfResultNull", func(t2 *testing.T) {
// 		mockInspecaoRepo.EXPECT().
// 			BuscarInspecaoSaidaPeloCodigo(codInspecao).
// 			Return(inspecao, nil).
// 			Times(1)
// 		inspecao.Resultado = "Finalizada"

// 		err := service.AtualizarInspecao(input)
// 		validacaoDTO := &dto.ValidacaoDTO{
// 			Code:    18,
// 			Message: "A inspeção informada já foi finalizada, portanto não é possível alterá-la.",
// 		}
// 		assert.Equal(t2, validacaoDTO, err)
// 	})

// 	t.Run("ErroAtualizarQuantidadeInspecaoPeloCodigo", func(t3 *testing.T) {
// 		mockInspecaoRepo.EXPECT().
// 			BuscarInspecaoSaidaPeloCodigo(codInspecao).
// 			Return(inspecao, nil).
// 			Times(1)
// 		mockInspecaoSaidaItemRepo.EXPECT().
// 			BuscarInspecaoSaidaItensEntitiesPeloCodigo(codInspecao).
// 			Return(itens, nil).
// 			Times(1)
// 		uow.EXPECT().
// 			Begin().
// 			Times(1)
// 		uow.EXPECT().
// 			UnitOfWorkGuard().
// 			Times(1)
// 		uow.EXPECT().
// 			Rollback().
// 			Times(1)
// 		inspecao.Resultado = ""
// 		mockInspecaoRepo.EXPECT().
// 			AtualizarQuantidadeInspecaoPeloCodigo(input.CodInspecao, input.QuantidadeInspecao).
// 			Return(errors.New("Test 3")).
// 			Times(1)

// 		err := service.AtualizarInspecao(input)
// 		validacaoDTO := &dto.ValidacaoDTO{
// 			Code:    19,
// 			Message: "Test 3",
// 		}
// 		assert.Equal(t3, validacaoDTO, err)
// 	})

// 	t.Run("ErroBuscarInspecaoSaidaItensEntitiesPeloCodigo", func(t4 *testing.T) {
// 		mockInspecaoRepo.EXPECT().
// 			BuscarInspecaoSaidaPeloCodigo(codInspecao).
// 			Return(inspecao, nil).
// 			Times(1)
// 		uow.EXPECT().
// 			Begin().
// 			Times(1)
// 		uow.EXPECT().
// 			UnitOfWorkGuard().
// 			Times(1)
// 		uow.EXPECT().
// 			Rollback().
// 			Times(1)
// 		inspecao.Resultado = ""
// 		mockInspecaoRepo.EXPECT().
// 			AtualizarQuantidadeInspecaoPeloCodigo(input.CodInspecao, input.QuantidadeInspecao).
// 			Return(nil).
// 			Times(1)
// 		mockInspecaoSaidaItemRepo.EXPECT().
// 			BuscarInspecaoSaidaItensEntitiesPeloCodigo(input.CodInspecao).
// 			Return(itens, errors.New("Test 4")).
// 			Times(1)

// 		err := service.AtualizarInspecao(input)
// 		validacaoDTO := &dto.ValidacaoDTO{
// 			Code:    20,
// 			Message: "Test 4",
// 		}
// 		assert.Equal(t4, validacaoDTO, err)
// 	})

// 	t.Run("ErroAtualizarInspecaoSaidaItens", func(t5 *testing.T) {
// 		mockInspecaoRepo.EXPECT().
// 			BuscarInspecaoSaidaPeloCodigo(codInspecao).
// 			Return(inspecao, nil).
// 			Times(1)
// 		uow.EXPECT().
// 			Begin().
// 			Times(1)
// 		uow.EXPECT().
// 			UnitOfWorkGuard().
// 			Times(1)
// 		uow.EXPECT().
// 			Rollback().
// 			Times(1)
// 		inspecao.Resultado = ""
// 		mockInspecaoRepo.EXPECT().
// 			AtualizarQuantidadeInspecaoPeloCodigo(input.CodInspecao, input.QuantidadeInspecao).
// 			Return(nil).
// 			Times(1)
// 		mockInspecaoSaidaItemRepo.EXPECT().
// 			BuscarInspecaoSaidaItensEntitiesPeloCodigo(input.CodInspecao).
// 			Return(itens, nil).
// 			Times(1)
// 		mockInspecaoSaidaItemRepo.EXPECT().
// 			AtualizarInspecaoSaidaItens(itens).
// 			Return(errors.New("Test 5")).
// 			Times(1)

// 		err := service.AtualizarInspecao(input)
// 		validacaoDTO := &dto.ValidacaoDTO{
// 			Code:    21,
// 			Message: "Test 5",
// 		}
// 		assert.Equal(t5, validacaoDTO, err)
// 	})

// 	mockInspecaoRepo.EXPECT().
// 		BuscarInspecaoSaidaPeloCodigo(codInspecao).
// 		Return(inspecao, nil).
// 		Times(1)
// 	uow.EXPECT().
// 		Begin().
// 		Times(1)
// 	uow.EXPECT().
// 		UnitOfWorkGuard().
// 		Times(1)
// 	uow.EXPECT().
// 		Complete().
// 		Times(1)
// 	inspecao.Resultado = ""
// 	mockInspecaoRepo.EXPECT().
// 		AtualizarQuantidadeInspecaoPeloCodigo(input.CodInspecao, input.QuantidadeInspecao).
// 		Return(nil).
// 		Times(1)
// 	mockInspecaoSaidaItemRepo.EXPECT().
// 		BuscarInspecaoSaidaItensEntitiesPeloCodigo(input.CodInspecao).
// 		Return(itens, nil).
// 		Times(1)
// 	mockInspecaoSaidaItemRepo.EXPECT().
// 		AtualizarInspecaoSaidaItens(itens).
// 		Return(nil).
// 		Times(1)

// 	err := service.AtualizarInspecao(input)
// 	assert.Nil(t, err)
// }

func TestBuscarResultadoInspecao(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	mockPlanoInspecaoRepo := mocks.NewMockIPlanosInspecaoRepository(mockCtrl)
	mockInspecaoSaidaItemRepo := mocks.NewMockIInspecaoSaidaItemRepository(mockCtrl)
	mockOrdemProducaoRepo := mocks.NewMockIOrdemProducaoRepository(mockCtrl)
	mockInspecaoRepo := mocks.NewMockIInspecaoSaidaRepository(mockCtrl)
	baseParams := InstanciarBaseparams()
	mockImpressaoService := mocks.NewMockIImpressaoService(mockCtrl)
	mockEmpresaRepo := mocks.NewMockIEmpresaRepository(mockCtrl)
	uow := mocks.NewMockUnitOfWork(mockCtrl)
	service := services.NewInspecaoSaidaService(uow, mockInspecaoRepo, mockInspecaoSaidaItemRepo, mockOrdemProducaoRepo, mockPlanoInspecaoRepo, baseParams, mockImpressaoService, mockEmpresaRepo)
	codInspecao := 98019
	id := GetUUIDWithoutError()
	itens := InstanciarSimpleEntitiesInspecaoSaidaItens(id)
	t.Run("ErroBuscarInspecaoSaidaItensEntitiesPeloCodigo", func(t1 *testing.T) {
		mockInspecaoSaidaItemRepo.EXPECT().
			BuscarInspecaoSaidaItensEntitiesPeloCodigo(codInspecao).
			Return(itens, errors.New("Test 1")).
			Times(1)
		actualOutput, err := service.BuscarResultadoInspecao(codInspecao)
		expectedValidacaoDTO := &dto.ValidacaoDTO{
			Code:    25,
			Message: "Test 1",
		}
		assert.Equal(t1, "", actualOutput)
		assert.Equal(t1, expectedValidacaoDTO, err)
	})

	t.Run("ErroNaoConforme", func(t2 *testing.T) {
		itens[0] = &entities.InspecaoSaidaItem{
			Resultado: "Não conforme",
		}
		mockInspecaoSaidaItemRepo.EXPECT().
			BuscarInspecaoSaidaItensEntitiesPeloCodigo(codInspecao).
			Return(itens, nil).
			Times(1)
		actualOutput, err := service.BuscarResultadoInspecao(codInspecao)
		expectedOutput := "Não Conforme"
		assert.Nil(t2, err)
		assert.Equal(t2, expectedOutput, actualOutput)
	})
	id = GetUUIDWithoutError()
	itens = InstanciarSimpleEntitiesInspecaoSaidaItens(id)
	mockInspecaoSaidaItemRepo.EXPECT().
		BuscarInspecaoSaidaItensEntitiesPeloCodigo(codInspecao).
		Return(itens, nil).
		Times(1)
	actualOutput, err := service.BuscarResultadoInspecao(codInspecao)
	expectedOutput := "Não Conforme"
	assert.Nil(t, err)
	assert.Equal(t, expectedOutput, actualOutput)
}
