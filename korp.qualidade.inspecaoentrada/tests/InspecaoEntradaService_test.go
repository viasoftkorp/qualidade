package tests

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/consts"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/mappers"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/mocks"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/services"
	"errors"
	"github.com/golang/mock/gomock"
	"github.com/google/uuid"
	"github.com/shopspring/decimal"
	"github.com/stretchr/testify/assert"
	"testing"
	"time"
)

func InstanciarMocksInspecaoEntrada(mockCtrl *gomock.Controller) (*mocks.MockUnitOfWork,
	*mocks.MockIInspecaoEntradaRepository,
	*mocks.MockIInspecaoEntradaItemRepository,
	*mocks.MockINotaFiscalRepository,
	*mocks.MockIPlanosInspecaoRepository,
	*mocks.MockIParametrosRepository,
	*mocks.MockILocaisRepository,
	*mocks.MockIEstoquePedidoVendaRepository,
	*mocks.MockIExternalMovimentacaoService,
	*models.BaseParams,
) {
	mockUow := mocks.NewMockUnitOfWork(mockCtrl)
	mockInspecaoEntradaRepository := mocks.NewMockIInspecaoEntradaRepository(mockCtrl)
	mockInspecaoEntradaItemRepository := mocks.NewMockIInspecaoEntradaItemRepository(mockCtrl)
	mockNotaFiscalRepository := mocks.NewMockINotaFiscalRepository(mockCtrl)
	mockPlanosInspecaoRepository := mocks.NewMockIPlanosInspecaoRepository(mockCtrl)
	mockParametrosRepository := mocks.NewMockIParametrosRepository(mockCtrl)
	mockLocaisRepository := mocks.NewMockILocaisRepository(mockCtrl)
	mockEstoquePedidoVendaRepository := mocks.NewMockIEstoquePedidoVendaRepository(mockCtrl)
	mockExternalMovimentacaoService := mocks.NewMockIExternalMovimentacaoService(mockCtrl)
	baseParams := &models.BaseParams{
		TenantId:      "tenant",
		EnvironmentId: "environment",
		CompanyId:     "company",
		CompanyRecno:  1,
		UserLogin:     "admin",
		UserId:        "user",
		AppId:         "1",
	}

	return mockUow, mockInspecaoEntradaRepository, mockInspecaoEntradaItemRepository, mockNotaFiscalRepository,
		mockPlanosInspecaoRepository, mockParametrosRepository, mockLocaisRepository, mockEstoquePedidoVendaRepository, mockExternalMovimentacaoService, baseParams
}

func TestBuscarInspecoesEntrada(t *testing.T) {
	mockContrl := gomock.NewController(t)
	defer mockContrl.Finish()

	mockUow, mockInspecaoEntradaRepository, mockInspecaoEntradaItemRepository, mockNotaFiscalRepository,
		mockPlanosInspecaoRepository, mockParametrosRepository, mockLocaisRepository, mockEstoquePedidoVendaRepository, mockExternalMovimentacaoService, baseParams := InstanciarMocksInspecaoEntrada(mockContrl)

	inspecaoEntradaService := services.NewInspecaoEntradaService(mockUow, mockInspecaoEntradaRepository, mockInspecaoEntradaItemRepository, mockNotaFiscalRepository,
		mockPlanosInspecaoRepository, mockParametrosRepository, mockLocaisRepository, mockEstoquePedidoVendaRepository, mockExternalMovimentacaoService, baseParams)

	notaFisc := 100
	lote := "1234"

	filterInput := &models.BaseFilter{
		Filter:   "",
		Skip:     0,
		PageSize: 25,
	}

	inspecoesEntities := []entities.InspecaoEntrada{
		{
			Recno:               1,
			Lote:                lote,
			Id:                  uuid.New(),
			CodigoInspecao:      1,
			NotaFiscal:          notaFisc,
			Inspecionado:        "S",
			DataInspecao:        "20220101",
			Inspetor:            "Inspetor Bugiganga",
			Resultado:           consts.InspecaoAprovada,
			QuantidadeInspecao:  decimal.NewFromInt(15),
			QuantidadeAceita:    decimal.NewFromInt(10),
			QuantidadeAprovada:  decimal.NewFromInt(10),
			QuantidadeReprovada: decimal.NewFromInt(0),
		},
		{
			Recno:               2,
			Lote:                lote,
			Id:                  uuid.New(),
			CodigoInspecao:      2,
			NotaFiscal:          notaFisc,
			Inspecionado:        "S",
			DataInspecao:        "20220101",
			Inspetor:            "Inspetor Bugiganga",
			Resultado:           consts.InspecaoNaoConforme,
			QuantidadeInspecao:  decimal.NewFromInt(15),
			QuantidadeAceita:    decimal.NewFromInt(0),
			QuantidadeAprovada:  decimal.NewFromInt(0),
			QuantidadeReprovada: decimal.NewFromInt(15),
		},
	}

	t.Run("ErroBuscarInspecoesEntrada", func(t2 *testing.T) {
		mockInspecaoEntradaRepository.EXPECT().BuscarInspecoesEntrada(notaFisc, lote, filterInput).Return(nil, errors.New("error"))

		actualOutput, validacaoDTO, err := inspecaoEntradaService.BuscarInspecoesEntrada(notaFisc, lote, filterInput)

		assert.Equal(t, err, errors.New("error"))
		assert.Nil(t, actualOutput)
		assert.Nil(t, validacaoDTO)
	})

	mockInspecaoEntradaRepository.EXPECT().BuscarInspecoesEntrada(notaFisc, lote, filterInput).Return(inspecoesEntities, nil).Times(2)

	t.Run("ErroBuscarQuantidadeInspecoesEntrada", func(t2 *testing.T) {
		mockInspecaoEntradaRepository.EXPECT().BuscarQuantidadeInspecoesEntrada(notaFisc, lote).Return(int64(0), errors.New("error"))

		actualOutput, validacaoDTO, err := inspecaoEntradaService.BuscarInspecoesEntrada(notaFisc, lote, filterInput)

		assert.Equal(t, err, errors.New("error"))
		assert.Nil(t, validacaoDTO)
		assert.Nil(t, actualOutput)
	})

	expectedOutput := &dto.GetInspecaoEntradaDTO{
		Items:      mappers.MapInspecaoEntradaEntitiesToDTOs(inspecoesEntities),
		TotalCount: int64(len(inspecoesEntities)),
	}

	mockInspecaoEntradaRepository.EXPECT().BuscarQuantidadeInspecoesEntrada(notaFisc, lote).Return(int64(len(inspecoesEntities)), nil)

	actualOutput, validacaoDTO, err := inspecaoEntradaService.BuscarInspecoesEntrada(notaFisc, lote, filterInput)

	assert.Nil(t, validacaoDTO)
	assert.Nil(t, err)
	assert.Equal(t, expectedOutput, actualOutput)
}

func TestBuscarPlanosNovaInspecao(t *testing.T) {
	mockContrl := gomock.NewController(t)
	defer mockContrl.Finish()

	mockUow, mockInspecaoEntradaRepository, mockInspecaoEntradaItemRepository, mockNotaFiscalRepository,
		mockPlanosInspecaoRepository, mockParametrosRepository, mockLocaisRepository, mockEstoquePedidoVendaRepository, mockExternalMovimentacaoService, baseParams := InstanciarMocksInspecaoEntrada(mockContrl)

	inspecaoEntradaService := services.NewInspecaoEntradaService(mockUow, mockInspecaoEntradaRepository, mockInspecaoEntradaItemRepository, mockNotaFiscalRepository,
		mockPlanosInspecaoRepository, mockParametrosRepository, mockLocaisRepository, mockEstoquePedidoVendaRepository, mockExternalMovimentacaoService, baseParams)

	plano := 100
	codigoProduto := "21435"

	filterInput := &models.BaseFilter{
		Filter:   "",
		Skip:     0,
		PageSize: 25,
	}

	planos := []models.PlanoInspecao{
		{
			Id:                     uuid.New(),
			CodigoProduto:          codigoProduto,
			Sequencia:              "1",
			Descricao:              "Peso",
			Resultado:              consts.InspecaoAprovada,
			MaiorValorInspecionado: 14,
			MenorValorInspecionado: 11,
			MaiorValorBase:         15,
			MenorValorBase:         10,
		},
		{
			Id:                     uuid.New(),
			CodigoProduto:          codigoProduto,
			Sequencia:              "2",
			Descricao:              "Altura",
			Resultado:              consts.InspecaoNaoConforme,
			MaiorValorInspecionado: 26,
			MenorValorInspecionado: 19,
			MaiorValorBase:         25,
			MenorValorBase:         20,
		},
	}

	t.Run("ErroBuscarPlanosNovaInspecao", func(t2 *testing.T) {
		mockPlanosInspecaoRepository.EXPECT().BuscarPlanosNovaInspecao(plano, codigoProduto, filterInput).
			Return(nil, errors.New("error"))

		actualOutput, validacaoDTO, err := inspecaoEntradaService.BuscarPlanosNovaInspecao(plano, codigoProduto, filterInput)

		assert.Equal(t, errors.New("error"), err)
		assert.Nil(t, validacaoDTO)
		assert.Nil(t, actualOutput)
	})

	mockPlanosInspecaoRepository.EXPECT().BuscarPlanosNovaInspecao(plano, codigoProduto, filterInput).Return(planos, nil).Times(2)

	t.Run("ErroBuscarQuantidadePlanosNovaInspecao", func(t2 *testing.T) {
		mockPlanosInspecaoRepository.EXPECT().BuscarQuantidadePlanosNovaInspecao(plano, codigoProduto).
			Return(int64(0), errors.New("error"))

		actualOutput, validacaoDTO, err := inspecaoEntradaService.BuscarPlanosNovaInspecao(plano, codigoProduto, filterInput)

		assert.Equal(t, errors.New("error"), err)
		assert.Nil(t, validacaoDTO)
		assert.Nil(t, actualOutput)
	})

	expectedOutput := &dto.GetPlanosInspecaoDTO{
		Items:      mappers.MapPlanosInspecaoModelsToDTOs(planos),
		TotalCount: int64(len(planos)),
	}

	mockPlanosInspecaoRepository.EXPECT().BuscarQuantidadePlanosNovaInspecao(plano, codigoProduto).
		Return(int64(len(planos)), nil)

	actualOutput, validacaoDTO, err := inspecaoEntradaService.BuscarPlanosNovaInspecao(plano, codigoProduto, filterInput)

	assert.Nil(t, validacaoDTO)
	assert.Nil(t, err)
	assert.Equal(t, expectedOutput, actualOutput)
}

func TestCriarInspecao(t *testing.T) {
	mockContrl := gomock.NewController(t)
	defer mockContrl.Finish()

	mockUow, mockInspecaoEntradaRepository, mockInspecaoEntradaItemRepository, mockNotaFiscalRepository,
		mockPlanosInspecaoRepository, mockParametrosRepository, mockLocaisRepository, mockEstoquePedidoVendaRepository, mockExternalMovimentacaoService, baseParams := InstanciarMocksInspecaoEntrada(mockContrl)

	inspecaoEntradaService := services.NewInspecaoEntradaService(mockUow, mockInspecaoEntradaRepository, mockInspecaoEntradaItemRepository, mockNotaFiscalRepository,
		mockPlanosInspecaoRepository, mockParametrosRepository, mockLocaisRepository, mockEstoquePedidoVendaRepository, mockExternalMovimentacaoService, baseParams)

	input := &dto.NovaInspecaoInput{
		Lote:       "A",
		NotaFiscal: int(1234),
		Plano:      28918,
		Quantidade: 10,
		PlanosInspecao: []*dto.PlanoInspecaoDTO{
			{
				Id:                     "6b0275ab-7c73-4a87-8aab-16522bdf759b",
				Descricao:              "Peso",
				Resultado:              consts.InspecaoAprovada,
				MaiorValorInspecionado: 5,
				MenorValorInspecionado: 2,
				MaiorValorBase:         6,
				MenorValorBase:         1,
				Metodo:                 "50 mm",
			},
		},
	}

	notaModel := models.NotaFiscalModel{
		Lote:             "A",
		IdNotaFiscal:     1,
		NotaFiscal:       1234,
		CodigoProduto:    "29831",
		DescricaoProduto: "PRODUTO TESTE",
		Quantidade:       decimal.NewFromFloat(100),
	}

	inspecaoModel := &models.InspecaoEntrada{
		CodigoInspecao:     540,
		NotaFiscal:         int(1234),
		Lote:               "A",
		DataInspecao:       time.Now().Format("20060102"),
		Inspetor:           baseParams.UserLogin,
		QuantidadeInspecao: decimal.NewFromFloat(input.Quantidade),
		QuantidadeLote:     notaModel.Quantidade,
		IdEmpresa:          1,
	}

	planosNaoAlterados := []models.PlanoInspecao{
		{
			Id:             uuid.MustParse("6b0275ab-7c73-4a87-8aab-16522bdf759b"),
			CodigoProduto:  "28918",
			Sequencia:      "1",
			Descricao:      "Peso",
			Resultado:      "",
			MaiorValorBase: 6,
			MenorValorBase: 1,
			Metodo:         "50 mm",
		},
		{
			Id:             uuid.MustParse("fcb6f330-b6f0-445e-9095-923a0cd7bcd1"),
			CodigoProduto:  "28918",
			Sequencia:      "2",
			Descricao:      "Altura",
			Resultado:      "",
			MaiorValorBase: 8,
			MenorValorBase: 2,
			Metodo:         "23 gr",
		},
	}

	itensInspecao := []models.InspecaoEntradaItem{
		{
			Plano:                  input.Plano,
			Descricao:              input.PlanosInspecao[0].Descricao,
			Metodo:                 input.PlanosInspecao[0].Metodo,
			Sequencia:              "1",
			Resultado:              input.PlanosInspecao[0].Resultado,
			MaiorValorInspecionado: decimal.NewFromFloat(input.PlanosInspecao[0].MaiorValorInspecionado),
			MenorValorInspecionado: decimal.NewFromFloat(input.PlanosInspecao[0].MenorValorInspecionado),
			CodigoInspecao:         inspecaoModel.CodigoInspecao,
		},
		{
			Plano:                  input.Plano,
			Descricao:              planosNaoAlterados[1].Descricao,
			Metodo:                 planosNaoAlterados[1].Metodo,
			Sequencia:              "2",
			Resultado:              planosNaoAlterados[1].Resultado,
			MaiorValorInspecionado: decimal.NewFromFloat(planosNaoAlterados[1].MaiorValorInspecionado),
			MenorValorInspecionado: decimal.NewFromFloat(planosNaoAlterados[1].MenorValorInspecionado),
			CodigoInspecao:         inspecaoModel.CodigoInspecao,
		},
	}

	t.Run("ErroBuscaNota", func(t2 *testing.T) {
		mockNotaFiscalRepository.EXPECT().BuscarNotaFiscal(input.NotaFiscal, input.Lote).Return(models.NotaFiscalModel{}, nil)

		expectedOutput := &dto.ValidacaoDTO{
			Code:    1,
			Message: "Nota Fiscal não encontrada.",
		}

		actualOutput, validacaoDTO, err := inspecaoEntradaService.CriarInspecao(input)

		assert.Nil(t, err)
		assert.Equal(t, expectedOutput, validacaoDTO)
		assert.Equal(t, 0, actualOutput)
	})

	mockInspecaoEntradaRepository.EXPECT().BuscarNovoCodigoInspecao().Return(inspecaoModel.CodigoInspecao).Times(4)
	mockNotaFiscalRepository.EXPECT().BuscarNotaFiscal(input.NotaFiscal, input.Lote).Return(notaModel, nil).Times(4)
	mockUow.EXPECT().Begin().Return(nil).Times(4)
	mockUow.EXPECT().UnitOfWorkGuard().Return().Times(4)
	mockUow.EXPECT().Rollback().Return(nil).Times(3)

	t.Run("ErroCriarInspecao", func(t2 *testing.T) {
		mockInspecaoEntradaRepository.EXPECT().CriarInspecao(inspecaoModel).Return(errors.New("error"))

		actualOutput, validacaoDTO, err := inspecaoEntradaService.CriarInspecao(input)

		assert.Nil(t, validacaoDTO)
		assert.Equal(t, errors.New("error"), err)
		assert.Equal(t, 0, actualOutput)
	})

	mockInspecaoEntradaRepository.EXPECT().CriarInspecao(inspecaoModel).Return(nil).Times(3)

	t.Run("ErroBuscarTodosPlanosNotaFiscalProduto", func(t2 *testing.T) {
		mockPlanosInspecaoRepository.EXPECT().BuscarTodosPlanosNotaFiscalProduto(input.Plano, input.CodigoProduto).Return(nil, errors.New("error"))

		actualOutput, validacaoDTO, err := inspecaoEntradaService.CriarInspecao(input)

		assert.Nil(t, validacaoDTO)
		assert.Equal(t, errors.New("error"), err)
		assert.Equal(t, 0, actualOutput)
	})

	mockPlanosInspecaoRepository.EXPECT().BuscarTodosPlanosNotaFiscalProduto(input.Plano, input.CodigoProduto).
		Return(planosNaoAlterados, nil).Times(2)

	mockInspecaoEntradaItemRepository.EXPECT().RemoverInspecaoEntradaItensPeloCodigo(inspecaoModel.CodigoInspecao).
		Return(nil).Times(2)

	t.Run("ErroCriarItensInspecao", func(t2 *testing.T) {
		mockInspecaoEntradaItemRepository.EXPECT().CriarItensInspecao(itensInspecao).Return(errors.New("error"))

		actualOutput, validacaoDTO, err := inspecaoEntradaService.CriarInspecao(input)

		assert.Nil(t, validacaoDTO)
		assert.Equal(t, errors.New("error"), err)
		assert.Equal(t, 0, actualOutput)
	})

	mockUow.EXPECT().Complete().Return(nil).Times(1)
	mockInspecaoEntradaItemRepository.EXPECT().CriarItensInspecao(itensInspecao).Return(nil)

	actualOutput, validacaoDTO, err := inspecaoEntradaService.CriarInspecao(input)

	assert.Nil(t, validacaoDTO)
	assert.Nil(t, err)
	assert.Equal(t, inspecaoModel.CodigoInspecao, actualOutput)
}

func TestAtualizarInspecao(t *testing.T) {
	mockContrl := gomock.NewController(t)
	defer mockContrl.Finish()

	mockUow, mockInspecaoEntradaRepository, mockInspecaoEntradaItemRepository, mockNotaFiscalRepository,
		mockPlanosInspecaoRepository, mockParametrosRepository, mockLocaisRepository, mockEstoquePedidoVendaRepository, mockExternalMovimentacaoService, baseParams := InstanciarMocksInspecaoEntrada(mockContrl)

	inspecaoEntradaService := services.NewInspecaoEntradaService(mockUow, mockInspecaoEntradaRepository, mockInspecaoEntradaItemRepository, mockNotaFiscalRepository,
		mockPlanosInspecaoRepository, mockParametrosRepository, mockLocaisRepository, mockEstoquePedidoVendaRepository, mockExternalMovimentacaoService, baseParams)

	itensInspecao := []dto.InspecaoEntradaItemDTO{
		{
			Descricao:              "Diametro",
			Metodo:                 "50mm",
			Sequencia:              "1",
			Resultado:              consts.InspecaoAprovada,
			MaiorValorInspecionado: 0.5,
			MenorValorInspecionado: 0.6,
		},
		{
			Descricao:              "Comprimento",
			Metodo:                 "50mm",
			Sequencia:              "2",
			Resultado:              consts.InspecaoAprovada,
			MaiorValorInspecionado: 0.8,
			MenorValorInspecionado: 0.9,
		},
	}

	itensInspecaoEntities := []*entities.InspecaoEntradaItem{
		{
			Descricao:              "Diametro",
			Metodo:                 "50mm",
			Sequencia:              "1",
			Resultado:              consts.InspecaoAprovada,
			MaiorValorInspecionado: decimal.NewFromFloat(0.5),
			MenorValorInspecionado: decimal.NewFromFloat(0.6),
		},
		{
			Descricao:              "Comprimento",
			Metodo:                 "50mm",
			Sequencia:              "2",
			Resultado:              "",
			MaiorValorInspecionado: decimal.NewFromFloat(0.8),
			MenorValorInspecionado: decimal.NewFromFloat(0.9),
		},
	}

	inspecaoEntity := &entities.InspecaoEntrada{
		CodigoInspecao:     28918,
		NotaFiscal:         int(1234),
		Inspecionado:       "S",
		Resultado:          "",
		DataInspecao:       time.Now().Format("20060102"),
		Inspetor:           baseParams.UserLogin,
		QuantidadeInspecao: decimal.NewFromFloat(20),
		QuantidadeLote:     decimal.NewFromFloat(100),
	}

	input := &dto.AtualizarInspecaoInput{
		CodigoInspecao:     28918,
		Itens:              itensInspecao,
		QuantidadeInspecao: 30,
	}

	mockInspecaoEntradaRepository.EXPECT().BuscarInspecaoEntradaPeloCodigo(inspecaoEntity.CodigoInspecao).Return(*inspecaoEntity, nil).Times(1)
	mockUow.EXPECT().Begin().Return(nil).Times(1)
	mockUow.EXPECT().UnitOfWorkGuard().Return().Times(1)
	mockInspecaoEntradaRepository.EXPECT().AtualizarQuantidadeInspecaoPeloCodigo(input.CodigoInspecao, input.QuantidadeInspecao).Return(nil).Times(1)
	mockInspecaoEntradaItemRepository.EXPECT().BuscarInspecaoEntradaItensEntitiesPeloCodigo(input.CodigoInspecao).Return(itensInspecaoEntities, nil)
	mockInspecaoEntradaItemRepository.EXPECT().AtualizarInspecaoEntradaItens(itensInspecaoEntities).Return(nil)
	mockUow.EXPECT().Complete().Return(nil).Times(1)

	validacaoDTO, err := inspecaoEntradaService.AtualizarInspecao(input)

	assert.Nil(t, validacaoDTO)
	assert.Nil(t, err)
}
