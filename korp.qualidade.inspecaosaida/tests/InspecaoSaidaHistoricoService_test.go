package tests

import (
	"errors"
	"testing"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/mocks"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/services"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	"github.com/golang/mock/gomock"
	"github.com/stretchr/testify/assert"
)

func InstanciarItens() []dto.InspecaoSaidaHistoricoItems {
	return []dto.InspecaoSaidaHistoricoItems{
		{RecnoInspecao: 325987, QuantidadeInspecao: 756819, CodigoProduto: "3948-901"},
	}
}

func InstanciarItensHistorico(itens []dto.InspecaoSaidaHistoricoItems, transferencias []dto.HistoricoInspecaoSaidaTransferenciaOutput) []dto.InspecaoSaidaHistoricoItemsDTO {
	historicoItens := []dto.InspecaoSaidaHistoricoItemsDTO{}

	for _, item := range itens {
		historicoItens = append(historicoItens, dto.InspecaoSaidaHistoricoItemsDTO{
			RecnoInspecao:          item.RecnoInspecao,
			OrdemFabricacao:        item.OrdemFabricacao,
			CodigoProduto:          item.CodigoProduto,
			DescricaoProduto:       item.CodigoProduto + " - " + item.DescricaoProduto,
			QuantidadeInspecao:     item.QuantidadeInspecao,
			QuantidadeRetrabalhada: item.QuantidadeRetrabalhada,
			QuantidadeAprovada:     item.QuantidadeAprovada,
			QuantidadeReprovada:    item.QuantidadeReprovada,
			Inspetor:               item.Inspetor,
			TipoInspecao:           item.TipoInspecao,
			Resultado:              item.Resultado,
			DataInspecao:           utils.StringToTime(item.DataInspecao),
			Transferencias:         transferencias,
		})
	}

	return historicoItens
}

func InstanciarTransferencias(saidaTransferencias []dto.InspecaoSaidaTransferenciaBackgroundInputDto, item dto.InspecaoSaidaHistoricoItems) []dto.HistoricoInspecaoSaidaTransferenciaOutput {

	transferencias := []dto.HistoricoInspecaoSaidaTransferenciaOutput{}

	for _, transferencia := range saidaTransferencias {
		transferencias = append(transferencias, dto.HistoricoInspecaoSaidaTransferenciaOutput{
			OrdemFabricacao:       item.OrdemFabricacao,
			Quantidade:            transferencia.Quantidade,
			NumeroPedido:          transferencia.NumeroPedido,
			LocalOrigem:           transferencia.LocalOrigem,
			DescricaoLocalOrigem:  "Origem desc",
			LocalDestino:          transferencia.LocalDestino,
			DescricaoLocalDestino: "Destino desc",
			TipoTransferencia:     transferencia.TipoTransferencia,
		})
	}
	return transferencias
}

func InstanciarInspecaoSaidaExecutadoWeb() *entities.InspecaoSaidaExecutadoWeb {
	return &entities.InspecaoSaidaExecutadoWeb{
		Estorno:             true,
		IdInspecaoSaidaSaga: "432432",
	}
}

func InstanciarSaga() *dto.SagaInspecaoSaidaOutput {

	return &dto.SagaInspecaoSaidaOutput{
		Id:      "432432",
		Estorno: true,
		Transferencias: []dto.SagaInspecaoSaidaTransferenciaOutput{
			{LocalOrigem: 22, LocalDestino: 18},
		},
	}
}

func TestGetAllInspecaoSaidaHistoricoCabecalhoOk(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	inspecaoMockRepository := mocks.NewMockIInspecaoSaidaHistoricoRepository(mockCtrl)
	service := services.NewInspecaoSaidaHistoricoService(nil, inspecaoMockRepository, nil, nil, nil, nil)
	baseFilter := &models.BaseFilter{
		PageSize: 13,
	}
	filter := &dto.InspecaoSaidaHistoricoCabecalhoFilters{}
	t.Run("ErroGetAllInspecaoSaidaHistoricoCabecalho", func(t2 *testing.T) {
		inspecaoMockRepository.EXPECT().
			GetAllInspecaoSaidaHistoricoCabecalho(baseFilter, filter).
			Return([]dto.InspecaoSaidaHistoricoCabecalhoDTO{}, errors.New("Teste 1")).
			Times(1)
		actualOutput, err := service.GetAllInspecaoSaidaHistoricoCabecalho(baseFilter, filter)
		assert.Nil(t2, actualOutput)
		assert.Equal(t2, errors.New("Teste 1"), err)
	})
	inspecaoMockRepository.EXPECT().
		GetAllInspecaoSaidaHistoricoCabecalho(baseFilter, filter).
		Return([]dto.InspecaoSaidaHistoricoCabecalhoDTO{}, nil).
		Times(2)
	t.Run("ErroCountInspecaoSaidaHistoricoCabecalho", func(t3 *testing.T) {
		inspecaoMockRepository.EXPECT().
			CountInspecaoSaidaHistoricoCabecalho(baseFilter, filter).
			Return(int64(14), errors.New("Teste 2")).
			Times(1)
		actualOutput, err := service.GetAllInspecaoSaidaHistoricoCabecalho(baseFilter, filter)

		assert.Nil(t3, actualOutput)
		assert.Equal(t3, errors.New("Teste 2"), err)

	})
	inspecaoMockRepository.EXPECT().
		CountInspecaoSaidaHistoricoCabecalho(baseFilter, filter).
		Return(int64(0), nil).
		Times(1)
	actualOutput, err := service.GetAllInspecaoSaidaHistoricoCabecalho(baseFilter, filter)

	assert.Nil(t, err)
	assert.Equal(t, actualOutput, &dto.GetAllInspecaoSaidaHistoricoCabecalhoDTO{
		Items:      []dto.InspecaoSaidaHistoricoCabecalhoDTO{},
		TotalCount: 0,
	})
}

func TestGetAllInspecaoSaidaHistoricoItemsOk(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	mockInspecaoRepo := mocks.NewMockIInspecaoSaidaHistoricoRepository(mockCtrl)
	mockWebRepo := mocks.NewMockIInspecaoSaidaExecutadoWebRepository(mockCtrl)
	mockSagaService := mocks.NewMockIExternalInspecaoSaidaSagaService(mockCtrl)
	mockLocaisRepo := mocks.NewMockILocaisRepository(mockCtrl)
	service := services.NewInspecaoSaidaHistoricoService(nil, mockInspecaoRepo, nil, mockSagaService, mockWebRepo, mockLocaisRepo)
	baseFilter := &models.BaseFilter{
		Skip:     10,
		PageSize: 1,
	}
	filters := &dto.InspecaoSaidaHistoricoCabecalhoFilters{}
	odf := 7
	codigoInspecao := 8
	descOrigem := "Descricao origem 1"
	itens := InstanciarItens()
	inspecaoSaidaExecutadoWeb := InstanciarInspecaoSaidaExecutadoWeb()
	saga := InstanciarSaga()
	t.Run("ErroGetAllInspecaoSaidaHistoricoItems", func(t1 *testing.T) {
		mockInspecaoRepo.EXPECT().
			GetAllInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao).
			Return(itens, errors.New("Test 1")).
			Times(1)
		actualOutput, err := service.GetAllInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao)

		assert.Nil(t1, actualOutput)
		assert.Equal(t1, errors.New("Test 1"), err)
	})

	t.Run("ErroBuscarInspecaoSaidaExecutadoWeb", func(t2 *testing.T) {
		mockInspecaoRepo.EXPECT().
			GetAllInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao).
			Return(itens, nil).
			Times(1)
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(itens[0].RecnoInspecao).
			Return(nil, errors.New("Test 2")).
			Times(1)

		actualOutput, err := service.GetAllInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao)

		assert.Nil(t2, actualOutput)
		assert.Equal(t2, errors.New("Test 2"), err)
	})

	t.Run("ErroBuscarSaga", func(t3 *testing.T) {
		mockInspecaoRepo.EXPECT().
			GetAllInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao).
			Return(itens, nil).
			Times(1)
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(itens[0].RecnoInspecao).
			Return(inspecaoSaidaExecutadoWeb, nil).
			Times(1)
		mockSagaService.EXPECT().
			BuscarSaga(inspecaoSaidaExecutadoWeb.IdInspecaoSaidaSaga).
			Return(saga, errors.New("Test 3")).
			Times(1)
		actualOutput, err := service.GetAllInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao)

		assert.Nil(t3, actualOutput)
		assert.Equal(t3, errors.New("Test 3"), err)
	})

	t.Run("ErroBuscarLocalDescricao", func(t4 *testing.T) {
		mockInspecaoRepo.EXPECT().
			GetAllInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao).
			Return(itens, nil).
			Times(1)
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(itens[0].RecnoInspecao).
			Return(inspecaoSaidaExecutadoWeb, nil).
			Times(1)
		mockSagaService.EXPECT().
			BuscarSaga(inspecaoSaidaExecutadoWeb.IdInspecaoSaidaSaga).
			Return(saga, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(saga.Transferencias[0].LocalOrigem).
			Return(descOrigem, errors.New("Test 4")).
			Times(1)
		actualOutput, err := service.GetAllInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao)

		assert.Nil(t4, actualOutput)
		assert.Equal(t4, errors.New("Test 4"), err)
	})

	t.Run("ErroBuscarLocalDescricao2", func(t5 *testing.T) {
		mockInspecaoRepo.EXPECT().
			GetAllInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao).
			Return(itens, nil).
			Times(1)
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(itens[0].RecnoInspecao).
			Return(inspecaoSaidaExecutadoWeb, nil).
			Times(1)
		mockSagaService.EXPECT().
			BuscarSaga(inspecaoSaidaExecutadoWeb.IdInspecaoSaidaSaga).
			Return(saga, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(saga.Transferencias[0].LocalOrigem).
			Return(descOrigem, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(saga.Transferencias[0].LocalDestino).
			Return(descOrigem, errors.New("Test 5")).
			Times(1)
		actualOutput, err := service.GetAllInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao)

		assert.Nil(t5, actualOutput)
		assert.Equal(t5, errors.New("Test 5"), err)
	})

	t.Run("ErroCountInspecaoSaidaHistoricoItems", func(t6 *testing.T) {
		mockInspecaoRepo.EXPECT().
			GetAllInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao).
			Return(itens, nil).
			Times(1)
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(itens[0].RecnoInspecao).
			Return(inspecaoSaidaExecutadoWeb, nil).
			Times(1)
		mockSagaService.EXPECT().
			BuscarSaga(inspecaoSaidaExecutadoWeb.IdInspecaoSaidaSaga).
			Return(saga, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(saga.Transferencias[0].LocalOrigem).
			Return(descOrigem, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(saga.Transferencias[0].LocalDestino).
			Return(descOrigem, nil).
			Times(1)
		mockInspecaoRepo.EXPECT().
			CountInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao).
			Return(int64(300), errors.New("Test 6")).
			Times(1)
		actualOutput, err := service.GetAllInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao)

		assert.Nil(t6, actualOutput)
		assert.Equal(t6, errors.New("Test 6"), err)
	})

	t.Run("ErroCountInspecaoSaidaHistoricoItems", func(t6 *testing.T) {
		mockInspecaoRepo.EXPECT().
			GetAllInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao).
			Return(itens, nil).
			Times(1)
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(itens[0].RecnoInspecao).
			Return(inspecaoSaidaExecutadoWeb, nil).
			Times(1)
		mockSagaService.EXPECT().
			BuscarSaga(inspecaoSaidaExecutadoWeb.IdInspecaoSaidaSaga).
			Return(saga, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(saga.Transferencias[0].LocalOrigem).
			Return(descOrigem, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(saga.Transferencias[0].LocalDestino).
			Return(descOrigem, nil).
			Times(1)
		mockInspecaoRepo.EXPECT().
			CountInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao).
			Return(int64(300), errors.New("Test 6")).
			Times(1)
		actualOutput, err := service.GetAllInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao)

		assert.Nil(t6, actualOutput)
		assert.Equal(t6, errors.New("Test 6"), err)
	})

	mockInspecaoRepo.EXPECT().
		GetAllInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao).
		Return(itens, nil).
		Times(1)
	mockWebRepo.EXPECT().
		BuscarInspecaoSaidaExecutadoWeb(itens[0].RecnoInspecao).
		Return(inspecaoSaidaExecutadoWeb, nil).
		Times(1)
	mockSagaService.EXPECT().
		BuscarSaga(inspecaoSaidaExecutadoWeb.IdInspecaoSaidaSaga).
		Return(saga, nil).
		Times(1)
	mockLocaisRepo.EXPECT().
		BuscarLocalDescricao(saga.Transferencias[0].LocalOrigem).
		Return(descOrigem, nil).
		Times(1)
	mockLocaisRepo.EXPECT().
		BuscarLocalDescricao(saga.Transferencias[0].LocalDestino).
		Return(descOrigem, nil).
		Times(1)
	mockInspecaoRepo.EXPECT().
		CountInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao).
		Return(int64(333), nil).
		Times(1)
	actualOutput, err := service.GetAllInspecaoSaidaHistoricoItems(baseFilter, filters, odf, codigoInspecao)
	expectedOutput := &dto.GetAllInspecaoSaidaHistoricoItemsDTO{
		Items: []dto.InspecaoSaidaHistoricoItemsDTO{
			{RecnoInspecao: 14},
		},
		TotalCount: 333,
	}

	assert.Nil(t, err)
	assert.NotEqual(t, expectedOutput, actualOutput)
}

/*
func TestGetItemsDtoOk(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	inspecaoSaidaExecutadoWebMockRepository := mocks.NewMockIInspecaoSaidaExecutadoWebRepository(mockCtrl)
	locaisMockRepository := mocks.NewMockILocaisRepository(mockCtrl)
	//produtoMockRepository := mocks.NewMockIProdutoRepository(mockCtrl)
	sagaService := mocks.NewMockIExternalInspecaoSaidaSagaService(mockCtrl)
	service := services.NewInspecaoSaidaHistoricoService(nil, nil, nil, sagaService, inspecaoSaidaExecutadoWebMockRepository, locaisMockRepository)
	items := []dto.InspecaoSaidaHistoricoItemsDTO{
		{RecnoInspecao: 19, OrdemFabricacao: 24, CodigoProduto: "5008", DescricaoProduto: "5008-Argamassa", QuantidadeInspecao: 20, QuantidadeRetrabalhada: 0, QuantidadeReprovada: 0, QuantidadeAprovada: 20},
		{RecnoInspecao: 21, OrdemFabricacao: 1, CodigoProduto: "47474", DescricaoProduto: "47474-Lixa", QuantidadeInspecao: 85, QuantidadeRetrabalhada: 0, QuantidadeReprovada: 0, QuantidadeAprovada: 20},
	}
	inspecaoSaidaExecutado1 := &entities.InspecaoSaidaExecutadoWeb{
		IdInspecaoSaidaSaga: "4389072",
		RecnoInspecaoSaida:  items[0].RecnoInspecao,
	}
	inspecaoSaidaExecutado2 := &entities.InspecaoSaidaExecutadoWeb{
		IdInspecaoSaidaSaga: "4389073",
		RecnoInspecaoSaida:  items[1].RecnoInspecao,
	}
	call1 := inspecaoSaidaExecutadoWebMockRepository.EXPECT().
		BuscarInspecaoSaidaExecutadoWeb(items[0].RecnoInspecao).
		Return(inspecaoSaidaExecutado1, nil).
		Times(2)

	inspecaoSaidaExecutadoWebMockRepository.EXPECT().
		BuscarInspecaoSaidaExecutadoWeb(items[1].RecnoInspecao).
		After(call1).
		Return(inspecaoSaidaExecutado2, nil).
		Times(1)

	saga1 := &dto.SagaInspecaoSaidaOutput{
		Transferencias: []dto.SagaInspecaoSaidaTransferenciaOutput{
			{LocalOrigem: 137},
		},
		RecnoInspecao: items[0].RecnoInspecao,
	}
	saga2 := &dto.SagaInspecaoSaidaOutput{
		Transferencias: []dto.SagaInspecaoSaidaTransferenciaOutput{
			{LocalOrigem: 138},
		},
		RecnoInspecao: items[0].RecnoInspecao,
	}
	call2 := sagaService.EXPECT().
		BuscarSaga(inspecaoSaidaExecutado1.IdInspecaoSaidaSaga).
		Return(saga1, nil).
		Times(1)

	sagaService.EXPECT().
		BuscarSaga(inspecaoSaidaExecutado2.IdInspecaoSaidaSaga).
		Return(saga2, nil).
		After(call2).
		Times(1)

	call3 := locaisMockRepository.EXPECT().
		BuscarLocalDescricao(saga1.Transferencias[0].LocalOrigem).
		Return("Descricao 1", nil).
		Times(1)

	locaisMockRepository.EXPECT().
		BuscarLocalDescricao(saga1.Transferencias[0].LocalOrigem).
		Return("Descricao 2", nil).
		After(call3).
		Times(1)

}
*/
