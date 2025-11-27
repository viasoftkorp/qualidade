package tests

import (
	"errors"
	"testing"
	"time"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/enums"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/mocks"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/services"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	"github.com/golang/mock/gomock"
	"github.com/shopspring/decimal"
	"github.com/stretchr/testify/assert"
)

func InstanciarGetAllProcessamentoInspecaoSaidaOutput() *dto.GetAllProcessamentoInspecaoSaidaOutput {
	return &dto.GetAllProcessamentoInspecaoSaidaOutput{
		TotalCount: 0,
		Items: []dto.ProcessamentoInspecaoSaidaOutput{
			{
				IdSaga:             "321",
				Status:             1,
				Erro:               "",
				NumeroRetentativas: 0,
				NumeroExecucoes:    1,
				QuantidadeTotal:    477,
				Resultado:          "Ok",
				CodigoProduto:      "321",
				DescricaoProduto:   "321 - Desc",
				Odf:                14, IdUsuarioExecucao: "123",
				NomeUsuarioExecucao: "Daniel",
				DataExecucao:        &time.Time{},
				Lote:                "14777-89",
				Estorno:             false,
				Transferencias: []dto.ProcessamentoInspecaoSaidaTransferenciaOutput{
					{
						OrdemFabricacao:       14,
						Quantidade:            32,
						NumeroPedido:          "2329",
						LocalOrigem:           22,
						DescricaoLocalOrigem:  "22 - OrigemDesc",
						LocalDestino:          23,
						DescricaoLocalDestino: "23 - Destinoesc",
						TipoTransferencia:     12,
					},
				},
			},
		},
	}
}

func InstanciarSagaInspecaoSaidaOutput() *dto.SagaInspecaoSaidaOutput {
	return &dto.SagaInspecaoSaidaOutput{
		RecnoInspecao: 1793,
		Transferencias: []dto.SagaInspecaoSaidaTransferenciaOutput{
			{
				LocalOrigem:       14,
				LocalDestino:      18,
				Quantidade:        22,
				NumeroPedido:      "94827",
				TipoTransferencia: 19,
				OrdemFabricacao:   12,
				Sequencial:        19,
				SeriesProducao: []dto.InspecaoSaidaSerieProducaoBackgroundOutputDto{
					{Serie: "3213217-7", RecnoSerie: 19},
				},
			},
		},
	}
}

func InstanciarMovimentarEstoqueInspecaoBackgroundInputDto() dto.MovimentarEstoqueInspecaoBackgroundInputDto {
	return dto.MovimentarEstoqueInspecaoBackgroundInputDto{
		Lote:            "",
		Estorno:         true,
		RecnoInspecao:   0,
		CodigoProduto:   "",
		OrdemFabricacao: 0,
		Transferencias: []dto.InspecaoSaidaTransferenciaBackgroundInputDto{
			{
				Fator:             1,
				LocalOrigem:       18,
				LocalDestino:      14,
				Quantidade:        22,
				NumeroPedido:      "94827",
				Documento:         "Transferencia do local Destino para local Origem via inspecao web",
				TipoTransferencia: 19,
				OrdemFabricacao:   12,
				Sequencial:        19,
				SeriesProducao: []dto.InspecaoSaidaSerieProducaoBackgroundInputDto{
					{Serie: "3213217-7", RecnoSerie: 19},
				},
			},
		},
	}
}

func InstanciarInspecaosSaidaExecutadoWeb(recno int, idSaga string, estorno bool, quantidadeTrasnferida decimal.Decimal) *entities.InspecaoSaidaExecutadoWeb {
	return &entities.InspecaoSaidaExecutadoWeb{
		RecnoInspecaoSaida:    recno,
		IdInspecaoSaidaSaga:   idSaga,
		Estorno:               estorno,
		QuantidadeTransferida: &quantidadeTrasnferida,
	}

}

func InstanciarMovimentarEstoqueInspecaoBackgroundInputDTO2(pesoBruto, pesoLiquido *float64) dto.MovimentarEstoqueInspecaoBackgroundInputDto {
	return dto.MovimentarEstoqueInspecaoBackgroundInputDto{
		IdSaga:          "GuidTest",
		Lote:            "",
		Estorno:         true,
		RecnoInspecao:   0,
		CodigoProduto:   "",
		OrdemFabricacao: 0,
		Transferencias: []dto.InspecaoSaidaTransferenciaBackgroundInputDto{
			{
				Fator:             1,
				LocalOrigem:       18,
				LocalDestino:      14,
				Quantidade:        22,
				NumeroPedido:      "94827",
				Documento:         "Transferencia do lugar Destino para lugar Origem via inspecao web",
				TipoTransferencia: 19,
				PesoBruto:         pesoBruto,
				PesoLiquido:       pesoLiquido,
				OrdemFabricacao:   12,
				Sequencial:        19,
				SeriesProducao: []dto.InspecaoSaidaSerieProducaoBackgroundInputDto{
					{Serie: "3213217-7", RecnoSerie: 19},
				},
			},
		},
	}
}

func TestRemoverSagaoK(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	sagaService := mocks.NewMockIExternalInspecaoSaidaSagaService(mockCtrl)
	mockWebRepo := mocks.NewMockIInspecaoSaidaExecutadoWebRepository(mockCtrl)
	service := services.NewInspecaoSaidaSagaService(nil, mockWebRepo, sagaService, nil, nil, nil, nil, nil, nil)
	id := "123"

	t.Run("ErroRemoverSaga", func(t2 *testing.T) {
		sagaService.EXPECT().
			RemoverSaga(id).
			Return(errors.New("Test 1")).
			Times(1)

		actualOutput := service.RemoverSaga(id)

		assert.Equal(t2, errors.New("Test 1"), actualOutput)
	})

	sagaService.EXPECT().
		RemoverSaga(id).
		Return(nil).
		Times(1)

	t.Run("ErroWebRepoRemoverSaga", func(t3 *testing.T) {
		mockWebRepo.EXPECT().
			RemoverSaga(id).
			Return(errors.New("Test 2")).
			Times(1)

		actualOutput := service.RemoverSaga(id)

		assert.Equal(t3, errors.New("Test 2"), actualOutput)
	})

	sagaService.EXPECT().
		RemoverSaga(id).
		Return(nil).
		Times(1)
	mockWebRepo.EXPECT().
		RemoverSaga(id).
		Return(nil).
		Times(1)

	actualOutput := service.RemoverSaga(id)

	assert.Equal(t, actualOutput, nil)
}

func TestReprocessarSaga(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	sagaService := mocks.NewMockIExternalInspecaoSaidaSagaService(mockCtrl)
	service := services.NewInspecaoSaidaSagaService(nil, nil, sagaService, nil, nil, nil, nil, nil, nil)
	id := "177"
	t.Run("ErroReprocessarSaga", func(t2 *testing.T) {
		sagaService.EXPECT().
			ReprocessarSaga(id).
			Return(errors.New("Test 1")).
			Times(1)

		actualOutput := service.ReprocessarSaga(id)
		assert.Equal(t2, errors.New("Test 1"), actualOutput)
	})

	sagaService.EXPECT().
		ReprocessarSaga(id).
		Return(nil).
		Times(1)
	actualOutput := service.ReprocessarSaga(id)

	assert.Nil(t, actualOutput)
}

func TestBuscarSagasOk(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	mockInspecaoSaidaSagaService := mocks.NewMockIExternalInspecaoSaidaSagaService(mockCtrl)
	mockProdutoRepo := mocks.NewMockIProdutoRepository(mockCtrl)
	mockLocaisRepo := mocks.NewMockILocaisRepository(mockCtrl)
	mockOrdemProducaoRepo := mocks.NewMockIOrdemProducaoRepository(mockCtrl)
	service := services.NewInspecaoSaidaSagaService(nil, nil, mockInspecaoSaidaSagaService, mockOrdemProducaoRepo, mockLocaisRepo, nil, nil, mockProdutoRepo, nil)

	baseFilter := &models.BaseFilter{
		PageSize: 1,
		Skip:     4,
	}
	userId := "4444"
	filter := &dto.ProcessamentoInspecaoSaidaFilters{
		IdUsuarioExecucao: &userId,
	}
	result := &dto.GetAllSagaInspecaoSaidaOutput{
		Items: []dto.SagaInspecaoSaidaOutput{
			{
				CodigoProduto:       "321",
				Id:                  "321",
				Status:              1,
				Erro:                "",
				NumeroRetentativas:  0,
				NumeroExecucoes:     1,
				QuantidadeTotal:     477,
				Resultado:           "Ok",
				IdUsuarioExecucao:   "123",
				NomeUsuarioExecucao: "Daniel",
				DataExecucao:        &time.Time{},
				Lote:                "14777-89",
				Estorno:             false,
				Transferencias: []dto.SagaInspecaoSaidaTransferenciaOutput{
					{
						OrdemFabricacao:   14,
						NumeroPedido:      "2329",
						Quantidade:        32,
						LocalDestino:      23,
						LocalOrigem:       22,
						TipoTransferencia: 12,
					},
				},
			},
		},
	}

	t.Run("ErroBuscarSagas", func(t2 *testing.T) {
		mockInspecaoSaidaSagaService.EXPECT().
			BuscarSagas(baseFilter, filter, false).
			Return(nil, errors.New("Test 1"))
		actualOutput, err := service.BuscarSagas(baseFilter, filter, false)

		assert.Nil(t2, actualOutput)
		assert.Equal(t2, errors.New("Test 1"), err)
	})

	mockInspecaoSaidaSagaService.EXPECT().
		BuscarSagas(baseFilter, filter, false).
		Return(result, nil).
		Times(4)

	t.Run("ErroBuscarProdutoDescricao", func(t3 *testing.T) {
		mockProdutoRepo.EXPECT().
			BuscarProdutoDescricao(result.Items[0].CodigoProduto).
			Return("", errors.New("Test 2")).
			Times(1)

		actualOutput, err := service.BuscarSagas(baseFilter, filter, false)
		assert.Nil(t3, actualOutput)
		assert.Equal(t3, errors.New("Test 2"), err)
	})

	mockProdutoRepo.EXPECT().
		BuscarProdutoDescricao(result.Items[0].CodigoProduto).
		Return("Desc", nil).
		Times(3)

	t.Run("ErroBuscarLocalDescricao", func(t3 *testing.T) {
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(result.Items[0].Transferencias[0].LocalOrigem).
			Return("", errors.New("Test 3")).
			Times(1)
		actualOutput, err := service.BuscarSagas(baseFilter, filter, false)

		assert.Equal(t3, errors.New("Test 3"), err)
		assert.Nil(t3, actualOutput)
	})

	t.Run("ErroBuscarLocalDescricao2", func(t4 *testing.T) {
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(result.Items[0].Transferencias[0].LocalDestino).
			Return("Destinoesc", nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(result.Items[0].Transferencias[0].LocalOrigem).
			Return("descricaoDestino", errors.New("Test 4")).
			Times(1)
		actualOutput, err := service.BuscarSagas(baseFilter, filter, false)

		assert.Nil(t4, actualOutput)
		assert.Equal(t4, errors.New("Test 4"), err)
	})

	mockLocaisRepo.EXPECT().
		BuscarLocalDescricao(result.Items[0].Transferencias[0].LocalOrigem).
		Return("OrigemDesc", nil).
		Times(1)

	actualOutput, err := service.BuscarSagas(baseFilter, filter, false)
	expectedOutput := InstanciarGetAllProcessamentoInspecaoSaidaOutput()

	assert.Nil(t, err)
	assert.Equal(t, expectedOutput, actualOutput)

}

func TestPublicarSagaInspecaoSaidaEstornoOk(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	mockWebRepo := mocks.NewMockIInspecaoSaidaExecutadoWebRepository(mockCtrl)
	mockLocaisRepo := mocks.NewMockILocaisRepository(mockCtrl)
	mockEstoqueRepo := mocks.NewMockIEstoquePedidoVendaRepository(mockCtrl)
	sagaService := mocks.NewMockIExternalInspecaoSaidaSagaService(mockCtrl)
	uow := mocks.NewMockUnitOfWork(mockCtrl)
	service := services.NewInspecaoSaidaSagaService(nil, mockWebRepo, sagaService, nil, mockLocaisRepo, nil, mockEstoqueRepo, nil, uow)
	recno := 1793
	transferredQuantity := decimal.NewFromInt(int64(22))
	inspecaoWebResult := &entities.InspecaoSaidaExecutadoWeb{
		Id:                    "GUID",
		IdInspecaoSaidaSaga:   "32189",
		Estorno:               true,
		QuantidadeTransferida: &transferredQuantity,
	}
	sagaServiceResult := InstanciarSagaInspecaoSaidaOutput()
	pesoLiquido := decimal.NewFromInt(15)
	pesoBruto := decimal.NewFromInt(45)
	floatPesoLiquido := float64(15)
	floatPesoBruto := float64(45)
	estoqueLocalValores := &models.EstoqueLocalValores{}
	estoqueLocalValores2 := &models.EstoqueLocalValores{
		PesoLiquido: &pesoLiquido,
		PesoBruto:   &pesoBruto,
	}
	//estoquePvdResult := []dto.EstoqueLocalPedidoVendaAlocacaoDTO{}
	movimentarEstoqueInput := InstanciarMovimentarEstoqueInspecaoBackgroundInputDto()

	movimentarEstoqueInput2 := InstanciarMovimentarEstoqueInspecaoBackgroundInputDTO2(&floatPesoBruto, &floatPesoLiquido)
	inspecaoSaidaWebNull := InstanciarInspecaosSaidaExecutadoWeb(recno, movimentarEstoqueInput2.IdSaga, true, decimal.NewFromInt(22))

	t.Run("ErroBuscarInspecaoSaidaExecutadoWeb", func(t1 *testing.T) {
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(recno).
			Return(nil, errors.New("Test 1")).
			Times(1)
		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaidaEstorno(recno)

		assert.Equal(t1, false, actualOutput)
		assert.Equal(t1, (*dto.ValidacaoDTO)(nil), validacaoDTO)
		assert.Equal(t1, errors.New("Test 1"), err)
	})

	t.Run("ErroBuscarSaga", func(t2 *testing.T) {
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(recno).
			Return(inspecaoWebResult, nil).
			Times(1)
		sagaService.EXPECT().
			BuscarSaga(inspecaoWebResult.IdInspecaoSaidaSaga).
			Return(nil, errors.New("Test 2")).
			Times(1)
		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaidaEstorno(recno)

		assert.Equal(t2, false, actualOutput)
		assert.Equal(t2, (*dto.ValidacaoDTO)(nil), validacaoDTO)
		assert.Equal(t2, errors.New("Test 2"), err)
	})

	t.Run("ErroPreencherTransferenciasEstorno", func(t3 *testing.T) {
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(recno).
			Return(inspecaoWebResult, nil).
			Times(1)
		sagaService.EXPECT().
			BuscarSaga(inspecaoWebResult.IdInspecaoSaidaSaga).
			Return(sagaServiceResult, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(sagaServiceResult.Transferencias[0].LocalDestino).
			Return("", errors.New("Test 3")).
			Times(1)

		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaidaEstorno(recno)

		assert.Equal(t3, false, actualOutput)
		assert.Equal(t3, (*dto.ValidacaoDTO)(nil), validacaoDTO)
		assert.Equal(t3, errors.New("Test 3"), err)
	})

	t.Run("ErroPreencherTransferenciasEstorno2", func(t3 *testing.T) {
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(recno).
			Return(inspecaoWebResult, nil).
			Times(1)
		sagaService.EXPECT().
			BuscarSaga(inspecaoWebResult.IdInspecaoSaidaSaga).
			Return(sagaServiceResult, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(sagaServiceResult.Transferencias[0].LocalDestino).
			Return("Destinoesc", nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(sagaServiceResult.Transferencias[0].LocalOrigem).
			Return("Destinoesc", errors.New("Test 3.1")).
			Times(1)

		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaidaEstorno(recno)

		assert.Equal(t3, false, actualOutput)
		assert.Equal(t3, (*dto.ValidacaoDTO)(nil), validacaoDTO)
		assert.Equal(t3, errors.New("Test 3.1"), err)
	})

	t.Run("ErroBuscarEstoqueLocalValoresPorProduto", func(t4 *testing.T) {
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(1793).
			Return(inspecaoWebResult, nil).
			Times(1)
		sagaService.EXPECT().
			BuscarSaga("32189").
			Return(sagaServiceResult, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(18).
			Return("Origem", nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(14).
			Return("Origem", nil).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarEstoqueLocalValoresPorProduto(sagaServiceResult.CodigoProduto, sagaServiceResult.Lote, sagaServiceResult.Transferencias[0].OrdemFabricacao, sagaServiceResult.Transferencias[0].LocalDestino).
			Return(estoqueLocalValores, errors.New("Test 5")).
			Times(1)
		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaidaEstorno(recno)
		assert.Equal(t4, false, actualOutput)
		assert.Equal(t4, (*dto.ValidacaoDTO)(nil), validacaoDTO)
		assert.Equal(t4, errors.New("Test 5"), err)
	})

	t.Run("ErroPublicarSaga1", func(t5 *testing.T) {
		sagaService.EXPECT().
			BuscarSaga(inspecaoWebResult.IdInspecaoSaidaSaga).
			Return(sagaServiceResult, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(sagaServiceResult.Transferencias[0].LocalOrigem).
			Return("Origem", nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(sagaServiceResult.Transferencias[0].LocalDestino).
			Return("Destino", nil).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarEstoqueLocalValoresPorProduto(sagaServiceResult.CodigoProduto, sagaServiceResult.Lote, sagaServiceResult.Transferencias[0].OrdemFabricacao, sagaServiceResult.Transferencias[0].LocalDestino).
			Return(estoqueLocalValores2, nil).
			Times(1)
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(recno).
			Return(inspecaoWebResult, nil).
			Times(1)
		uow.EXPECT().
			Begin().
			Times(1)
		uow.EXPECT().
			UnitOfWorkGuard().
			Times(1)
		uow.EXPECT().
			Rollback().
			Times(1)
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(sagaServiceResult.RecnoInspecao).
			Return(inspecaoWebResult, errors.New("Test 5")).
			Times(1)
		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaidaEstorno(recno)

		assert.Equal(t5, false, actualOutput)
		assert.Equal(t5, (*dto.ValidacaoDTO)(nil), validacaoDTO)
		assert.Equal(t5, errors.New("Test 5"), err)
	})

	t.Run("ErroPublicarSaga2", func(t6 *testing.T) {
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(sagaServiceResult.RecnoInspecao).
			Return(inspecaoWebResult, nil).
			Times(1)
		sagaService.EXPECT().
			BuscarSaga(inspecaoWebResult.IdInspecaoSaidaSaga).
			Return(sagaServiceResult, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(sagaServiceResult.Transferencias[0].LocalOrigem).
			Return("Origem", nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(sagaServiceResult.Transferencias[0].LocalDestino).
			Return("Destino", nil).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarEstoqueLocalValoresPorProduto(sagaServiceResult.CodigoProduto, sagaServiceResult.Lote, sagaServiceResult.Transferencias[0].OrdemFabricacao, sagaServiceResult.Transferencias[0].LocalDestino).
			Return(estoqueLocalValores2, nil).
			Times(1)
		uow.EXPECT().
			Begin().
			Times(1)
		uow.EXPECT().
			UnitOfWorkGuard().
			Times(1)
		uow.EXPECT().
			Rollback().
			Times(1)
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(sagaServiceResult.RecnoInspecao).
			Return(nil, nil).
			Times(1)
		mockWebRepo.EXPECT().
			InserirInspecaoSaidaExecutadoWeb(inspecaoSaidaWebNull).
			Return(errors.New("Test 6")).
			Times(1)
		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaidaEstorno(recno)

		assert.Equal(t6, false, actualOutput)
		assert.Equal(t6, (*dto.ValidacaoDTO)(nil), validacaoDTO)
		assert.Equal(t6, errors.New("Test 6"), err)
	})

	t.Run("ErroPublicarSaga3", func(t7 *testing.T) {
		uow.EXPECT().
			UnitOfWorkGuard().
			Times(1)
		sagaService.EXPECT().
			BuscarSaga(inspecaoWebResult.IdInspecaoSaidaSaga).
			Return(sagaServiceResult, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(sagaServiceResult.Transferencias[0].LocalOrigem).
			Return("Origem", nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(sagaServiceResult.Transferencias[0].LocalDestino).
			Return("Destino", nil).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarEstoqueLocalValoresPorProduto(sagaServiceResult.CodigoProduto, sagaServiceResult.Lote, sagaServiceResult.Transferencias[0].OrdemFabricacao, sagaServiceResult.Transferencias[0].LocalDestino).
			Return(estoqueLocalValores2, nil).
			Times(1)
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(recno).
			Return(inspecaoWebResult, nil).
			Times(1)
		uow.EXPECT().
			Begin().
			Times(1)
		uow.EXPECT().
			Rollback().
			Times(1)
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(sagaServiceResult.RecnoInspecao).
			Return(inspecaoWebResult, nil).
			Times(1)
		uow.EXPECT().
			Complete().
			Return(errors.New("Test 8")).
			Times(1)
		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaidaEstorno(recno)

		assert.Equal(t7, false, actualOutput)
		assert.Equal(t7, (*dto.ValidacaoDTO)(nil), validacaoDTO)
		assert.Equal(t7, errors.New("Test 8"), err)
	})

	t.Run("ErroPublicarSaga4", func(t7 *testing.T) {
		uow.EXPECT().
			UnitOfWorkGuard().
			Times(1)
		sagaService.EXPECT().
			BuscarSaga(inspecaoWebResult.IdInspecaoSaidaSaga).
			Return(sagaServiceResult, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(sagaServiceResult.Transferencias[0].LocalOrigem).
			Return("Origem", nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(sagaServiceResult.Transferencias[0].LocalDestino).
			Return("Destino", nil).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarEstoqueLocalValoresPorProduto(sagaServiceResult.CodigoProduto, sagaServiceResult.Lote, sagaServiceResult.Transferencias[0].OrdemFabricacao, sagaServiceResult.Transferencias[0].LocalDestino).
			Return(estoqueLocalValores2, nil).
			Times(1)
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(recno).
			Return(inspecaoWebResult, nil).
			Times(1)
		uow.EXPECT().
			Begin().
			Times(1)
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(sagaServiceResult.RecnoInspecao).
			Return(inspecaoWebResult, nil).
			Times(1)
		uow.EXPECT().
			Complete().
			Return(nil).
			Times(1)
		movimentarEstoqueInput.IdSaga = "32189"
		movimentarEstoqueInput.RecnoInspecao = 1793
		movimentarEstoqueInput.Transferencias[0].PesoLiquido = &floatPesoLiquido
		movimentarEstoqueInput.Transferencias[0].PesoBruto = &floatPesoBruto

		sagaService.EXPECT().
			PublicarSaga(movimentarEstoqueInput).
			Return("", errors.New("Test 8")).
			Times(1)
		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaidaEstorno(recno)

		assert.Equal(t7, false, actualOutput)
		assert.Equal(t7, (*dto.ValidacaoDTO)(nil), validacaoDTO)
		assert.Equal(t7, errors.New("Test 8"), err)
	})

	uow.EXPECT().
		UnitOfWorkGuard().
		Times(1)
	sagaService.EXPECT().
		BuscarSaga(inspecaoWebResult.IdInspecaoSaidaSaga).
		Return(sagaServiceResult, nil).
		Times(1)
	mockLocaisRepo.EXPECT().
		BuscarLocalDescricao(sagaServiceResult.Transferencias[0].LocalOrigem).
		Return("Origem", nil).
		Times(1)
	mockLocaisRepo.EXPECT().
		BuscarLocalDescricao(sagaServiceResult.Transferencias[0].LocalDestino).
		Return("Destino", nil).
		Times(1)
	mockEstoqueRepo.EXPECT().
		BuscarEstoqueLocalValoresPorProduto(sagaServiceResult.CodigoProduto, sagaServiceResult.Lote, sagaServiceResult.Transferencias[0].OrdemFabricacao, sagaServiceResult.Transferencias[0].LocalDestino).
		Return(estoqueLocalValores2, nil).
		Times(1)
	mockWebRepo.EXPECT().
		BuscarInspecaoSaidaExecutadoWeb(recno).
		Return(inspecaoWebResult, nil).
		Times(1)
	uow.EXPECT().
		Begin().
		Times(1)
	mockWebRepo.EXPECT().
		BuscarInspecaoSaidaExecutadoWeb(sagaServiceResult.RecnoInspecao).
		Return(inspecaoWebResult, nil).
		Times(1)
	uow.EXPECT().
		Complete().
		Return(nil).
		Times(1)
	movimentarEstoqueInput.IdSaga = "32189"
	movimentarEstoqueInput.RecnoInspecao = 1793
	movimentarEstoqueInput.Transferencias[0].PesoLiquido = &floatPesoLiquido
	movimentarEstoqueInput.Transferencias[0].PesoBruto = &floatPesoBruto

	sagaService.EXPECT().
		PublicarSaga(movimentarEstoqueInput).
		Return("", nil).
		Times(1)
	actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaidaEstorno(recno)

	assert.Equal(t, true, actualOutput)
	assert.Equal(t, (*dto.ValidacaoDTO)(nil), validacaoDTO)
	assert.Nil(t, err)

}

func TestPublicarSagaInspecaoSaida(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	mockWebRepo := mocks.NewMockIInspecaoSaidaExecutadoWebRepository(mockCtrl)
	mockLocaisRepo := mocks.NewMockILocaisRepository(mockCtrl)
	mockEstoqueRepo := mocks.NewMockIEstoquePedidoVendaRepository(mockCtrl)
	mockInspecaoSaidaRepo := mocks.NewMockIInspecaoSaidaRepository(mockCtrl)
	mockOrdemProducaoRepo := mocks.NewMockIOrdemProducaoRepository(mockCtrl)
	mockUow := mocks.NewMockUnitOfWork(mockCtrl)
	sagaService := mocks.NewMockIExternalInspecaoSaidaSagaService(mockCtrl)
	service := services.NewInspecaoSaidaSagaService(mockInspecaoSaidaRepo, mockWebRepo, sagaService, mockOrdemProducaoRepo, mockLocaisRepo, nil, mockEstoqueRepo, nil, mockUow)
	localOrigem := 327
	odf := 14
	lote := "149889-7"
	recno := 321321
	idSaga := "1000"
	quantidadeAprovada := decimal.NewFromInt(1)
	quantidadeReprovada := decimal.NewFromInt(1)
	quantidadeRetrabalhada := decimal.NewFromInt(1)
	quantidadeLote := decimal.NewFromInt(10)
	placeDescription := "Description test."
	pesoLiquido := decimal.NewFromInt(15)
	pesoBruto := decimal.NewFromInt(45)
	floatPesoLiquido := utils.DecimalToFloat64(pesoLiquido)
	floatPesoBruto := utils.DecimalToFloat64(pesoBruto)
	descricaoDestino := "Destino description"
	// floatPesoLiquido := utils.DecimalToFloat64(pesoLiquido)
	// floatPesoBruto := utils.DecimalToFloat64(pesoBruto)
	input := &dto.FinalizarInspecaoInput{
		CodInspecao:            14543,
		QuantidadeReprovada:    1,
		QuantidadeAprovada:     1,
		QuantidadeRetrabalhada: 1,
	}
	ordemProducao := &models.OrdemProducao{
		CodigoProduto: "43298324",
		Lote:          lote,
		ODF:           odf,
	}
	inspecaoSaida := &models.InspecaoSaida{
		Recno:                  321321,
		Odf:                    odf,
		Lote:                   lote,
		QuantidadeAprovada:     quantidadeAprovada,
		QuantidadeReprovada:    quantidadeReprovada,
		QuantidadeRetrabalhada: quantidadeRetrabalhada,
		QtdLote:                quantidadeLote,
	}
	estoqueLocalValores := &models.EstoqueLocalValores{
		Recno:       recno,
		PesoLiquido: &pesoLiquido,
		PesoBruto:   &pesoBruto,
	}
	qtdTrasnferida := decimal.NewFromFloat(input.QuantidadeAprovada * 3)
	series := []models.Serie{
		{Serie: "432", RecnoSerie: recno},
	}
	movimentarEstoqueInput := dto.MovimentarEstoqueInspecaoBackgroundInputDto{
		Lote:            ordemProducao.Lote,
		Estorno:         false,
		RecnoInspecao:   0,
		CodigoProduto:   ordemProducao.CodigoProduto,
		OrdemFabricacao: ordemProducao.ODF,
		Resultado:       "Parcialmente Aprov.",
		Transferencias: []dto.InspecaoSaidaTransferenciaBackgroundInputDto{
			{Quantidade: input.QuantidadeAprovada,
				LocalOrigem:       localOrigem,
				LocalDestino:      input.CodigoLocalAprovado,
				Documento:         "Transferencia do local Description test. para local Destino description via inspecao web",
				Fator:             1,
				NumeroPedido:      ordemProducao.NumeroPedido,
				TipoTransferencia: enums.Aprovado,
				PesoLiquido:       &floatPesoLiquido,
				PesoBruto:         &floatPesoBruto,
				DataValidade:      utils.StringToTime(estoqueLocalValores.DataValidade),
				DataFabricacao:    utils.StringToTime(estoqueLocalValores.DataFabricacao),
				UltimoValorPago:   utils.DecimalToFloat64(estoqueLocalValores.ValorPago),
				SeriesProducao: []dto.InspecaoSaidaSerieProducaoBackgroundInputDto{
					{Serie: "432", RecnoSerie: recno},
				},
				OrdemFabricacao: ordemProducao.ODF,
			},
			{Quantidade: input.QuantidadeAprovada,
				LocalOrigem:       localOrigem,
				LocalDestino:      input.CodigoLocalAprovado,
				Documento:         "Transferencia do local Description test. para local Destino description via inspecao web",
				Fator:             1,
				NumeroPedido:      ordemProducao.NumeroPedido,
				TipoTransferencia: enums.Reprovado,
				PesoLiquido:       &floatPesoLiquido,
				PesoBruto:         &floatPesoBruto,
				DataValidade:      utils.StringToTime(estoqueLocalValores.DataValidade),
				DataFabricacao:    utils.StringToTime(estoqueLocalValores.DataFabricacao),
				UltimoValorPago:   utils.DecimalToFloat64(estoqueLocalValores.ValorPago),
				SeriesProducao:    []dto.InspecaoSaidaSerieProducaoBackgroundInputDto{},
				OrdemFabricacao:   ordemProducao.ODF,
			},
			{
				Quantidade:        input.QuantidadeAprovada,
				LocalOrigem:       localOrigem,
				LocalDestino:      input.CodigoLocalAprovado,
				Documento:         "Transferencia do local Description test. para local Destino description via inspecao web",
				Fator:             1,
				NumeroPedido:      ordemProducao.NumeroPedido,
				TipoTransferencia: enums.Retrabalhado,
				PesoLiquido:       &floatPesoLiquido,
				PesoBruto:         &floatPesoBruto,
				DataValidade:      utils.StringToTime(estoqueLocalValores.DataValidade),
				DataFabricacao:    utils.StringToTime(estoqueLocalValores.DataFabricacao),
				UltimoValorPago:   utils.DecimalToFloat64(estoqueLocalValores.ValorPago),
				SeriesProducao:    []dto.InspecaoSaidaSerieProducaoBackgroundInputDto{},
				OrdemFabricacao:   ordemProducao.ODF,
			},
		},
	}
	inspecaoSaidaExecutadaWeb := &entities.InspecaoSaidaExecutadoWeb{
		Id:                    "GUID",
		RecnoInspecaoSaida:    0,
		IdInspecaoSaidaSaga:   idSaga,
		Estorno:               false,
		QuantidadeTransferida: &qtdTrasnferida,
	}
	t.Run("ErroBuscarLocalSaida", func(t1 *testing.T) {
		mockLocaisRepo.EXPECT().
			BuscarLocalSaida().
			Return(localOrigem, errors.New("Test 1")).
			Times(1)
		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaida(input)
		assert.Equal(t1, false, actualOutput)
		assert.Equal(t1, errors.New("Test 1"), err)
		assert.Nil(t1, validacaoDTO)
	})

	t.Run("ErroBuscarLocalDescricao", func(t2 *testing.T) {
		mockLocaisRepo.EXPECT().
			BuscarLocalSaida().
			Return(localOrigem, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(localOrigem).
			Return(placeDescription, errors.New("Test 2")).
			Times(1)
		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaida(input)
		assert.Equal(t2, false, actualOutput)
		assert.Equal(t2, errors.New("Test 2"), err)
		assert.Nil(t2, validacaoDTO)
	})

	t.Run("ErroBuscarInspecaoSaidaDetalhesPeloCodigo", func(t3 *testing.T) {
		mockLocaisRepo.EXPECT().
			BuscarLocalSaida().
			Return(localOrigem, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(localOrigem).
			Return(placeDescription, nil).
			Times(1)
		mockInspecaoSaidaRepo.EXPECT().
			BuscarInspecaoSaidaDetalhesPeloCodigo(input.CodInspecao).
			Return(inspecaoSaida, errors.New("Test 3")).
			Times(1)
		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaida(input)
		assert.Equal(t3, false, actualOutput)
		assert.Equal(t3, errors.New("Test 3"), err)
		assert.Nil(t3, validacaoDTO)
	})

	t.Run("ErroServiceGetOdfEstoqueLocal", func(t4 *testing.T) {
		mockLocaisRepo.EXPECT().
			BuscarLocalSaida().
			Return(localOrigem, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(localOrigem).
			Return(placeDescription, nil).
			Times(1)
		mockInspecaoSaidaRepo.EXPECT().
			BuscarInspecaoSaidaDetalhesPeloCodigo(input.CodInspecao).
			Return(inspecaoSaida, nil).
			Times(1)
		mockOrdemProducaoRepo.EXPECT().
			BuscarOrdem(odf, lote).
			Return(ordemProducao).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarEstoqueLocalValoresPorProduto(ordemProducao.CodigoProduto, ordemProducao.Lote, ordemProducao.ODF, localOrigem).
			Return(estoqueLocalValores, errors.New("Test 4")).
			Times(1)
		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaida(input)
		assert.Equal(t4, false, actualOutput)
		assert.Equal(t4, errors.New("Test 4"), err)
		assert.Nil(t4, validacaoDTO)
	})

	t.Run("ErroServicePreencherTransferenciasFromInput", func(t5 *testing.T) {
		mockLocaisRepo.EXPECT().
			BuscarLocalSaida().
			Return(localOrigem, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(localOrigem).
			Return(placeDescription, nil).
			Times(1)
		mockInspecaoSaidaRepo.EXPECT().
			BuscarInspecaoSaidaDetalhesPeloCodigo(input.CodInspecao).
			Return(inspecaoSaida, nil).
			Times(1)
		mockOrdemProducaoRepo.EXPECT().
			BuscarOrdem(odf, lote).
			Return(ordemProducao).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarEstoqueLocalValoresPorProduto(ordemProducao.CodigoProduto, ordemProducao.Lote, ordemProducao.ODF, localOrigem).
			Return(estoqueLocalValores, nil).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarSeries(estoqueLocalValores.Recno).
			Return(series, errors.New("Test 5")).
			Times(1)
		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaida(input)
		assert.Equal(t5, false, actualOutput)
		assert.Equal(t5, errors.New("Test 5"), err)
		assert.Nil(t5, validacaoDTO)
	})

	t.Run("ErroServicePreencherTransferenciasFromInput", func(t6 *testing.T) {
		mockLocaisRepo.EXPECT().
			BuscarLocalSaida().
			Return(localOrigem, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(localOrigem).
			Return(placeDescription, nil).
			Times(1)
		mockInspecaoSaidaRepo.EXPECT().
			BuscarInspecaoSaidaDetalhesPeloCodigo(input.CodInspecao).
			Return(inspecaoSaida, nil).
			Times(1)
		mockOrdemProducaoRepo.EXPECT().
			BuscarOrdem(odf, lote).
			Return(ordemProducao).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarEstoqueLocalValoresPorProduto(ordemProducao.CodigoProduto, ordemProducao.Lote, ordemProducao.ODF, localOrigem).
			Return(estoqueLocalValores, nil).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarSeries(estoqueLocalValores.Recno).
			Return(series, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, errors.New("Test 6")).
			Times(1)
		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaida(input)
		assert.Equal(t6, false, actualOutput)
		assert.Equal(t6, errors.New("Test 6"), err)
		assert.Nil(t6, validacaoDTO)
	})

	t.Run("ErroBuscarLocalDescricao1", func(t7 *testing.T) {
		mockLocaisRepo.EXPECT().
			BuscarLocalSaida().
			Return(localOrigem, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(localOrigem).
			Return(placeDescription, nil).
			Times(1)
		mockInspecaoSaidaRepo.EXPECT().
			BuscarInspecaoSaidaDetalhesPeloCodigo(input.CodInspecao).
			Return(inspecaoSaida, nil).
			Times(1)
		mockOrdemProducaoRepo.EXPECT().
			BuscarOrdem(odf, lote).
			Return(ordemProducao).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarEstoqueLocalValoresPorProduto(ordemProducao.CodigoProduto, ordemProducao.Lote, ordemProducao.ODF, localOrigem).
			Return(estoqueLocalValores, nil).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarSeries(estoqueLocalValores.Recno).
			Return(series, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, errors.New("Test 7.1")).
			Times(1)

		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaida(input)
		assert.Equal(t7, false, actualOutput)
		assert.Equal(t7, errors.New("Test 7.1"), err)
		assert.Nil(t7, validacaoDTO)
	})

	t.Run("ErroBuscarLocalDescricao2", func(t7 *testing.T) {
		mockLocaisRepo.EXPECT().
			BuscarLocalSaida().
			Return(localOrigem, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(localOrigem).
			Return(placeDescription, nil).
			Times(1)
		mockInspecaoSaidaRepo.EXPECT().
			BuscarInspecaoSaidaDetalhesPeloCodigo(input.CodInspecao).
			Return(inspecaoSaida, nil).
			Times(1)
		mockOrdemProducaoRepo.EXPECT().
			BuscarOrdem(odf, lote).
			Return(ordemProducao).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarEstoqueLocalValoresPorProduto(ordemProducao.CodigoProduto, ordemProducao.Lote, ordemProducao.ODF, localOrigem).
			Return(estoqueLocalValores, nil).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarSeries(estoqueLocalValores.Recno).
			Return(series, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, errors.New("Test 7.2")).
			Times(1)

		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaida(input)
		assert.Equal(t7, false, actualOutput)
		assert.Equal(t7, errors.New("Test 7.2"), err)
		assert.Nil(t7, validacaoDTO)
	})

	t.Run("ErroBuscarLocalDescricao3", func(t7 *testing.T) {
		mockLocaisRepo.EXPECT().
			BuscarLocalSaida().
			Return(localOrigem, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(localOrigem).
			Return(placeDescription, nil).
			Times(1)
		mockInspecaoSaidaRepo.EXPECT().
			BuscarInspecaoSaidaDetalhesPeloCodigo(input.CodInspecao).
			Return(inspecaoSaida, nil).
			Times(1)
		mockOrdemProducaoRepo.EXPECT().
			BuscarOrdem(odf, lote).
			Return(ordemProducao).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarEstoqueLocalValoresPorProduto(ordemProducao.CodigoProduto, ordemProducao.Lote, ordemProducao.ODF, localOrigem).
			Return(estoqueLocalValores, nil).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarSeries(estoqueLocalValores.Recno).
			Return(series, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, errors.New("Test 7.3")).
			Times(1)

		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaida(input)
		assert.Equal(t7, false, actualOutput)
		assert.Equal(t7, errors.New("Test 7.3"), err)
		assert.Nil(t7, validacaoDTO)
	})

	t.Run("ErroBuscarLocalDescricao4", func(t7 *testing.T) {
		mockLocaisRepo.EXPECT().
			BuscarLocalSaida().
			Return(localOrigem, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(localOrigem).
			Return(placeDescription, nil).
			Times(1)
		mockInspecaoSaidaRepo.EXPECT().
			BuscarInspecaoSaidaDetalhesPeloCodigo(input.CodInspecao).
			Return(inspecaoSaida, nil).
			Times(1)
		mockOrdemProducaoRepo.EXPECT().
			BuscarOrdem(odf, lote).
			Return(ordemProducao).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarEstoqueLocalValoresPorProduto(ordemProducao.CodigoProduto, ordemProducao.Lote, ordemProducao.ODF, localOrigem).
			Return(estoqueLocalValores, nil).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarSeries(estoqueLocalValores.Recno).
			Return(series, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, errors.New("Test 7.4")).
			Times(1)

		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaida(input)
		assert.Equal(t7, false, actualOutput)
		assert.Equal(t7, errors.New("Test 7.4"), err)
		assert.Nil(t7, validacaoDTO)
	})

	t.Run("ErroServicePublicarSaga", func(t8 *testing.T) {
		mockUow.EXPECT().
			Begin().
			Times(1)
		mockUow.EXPECT().
			UnitOfWorkGuard().
			Times(1)
		mockUow.EXPECT().
			Complete().
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalSaida().
			Return(localOrigem, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(localOrigem).
			Return(placeDescription, nil).
			Times(1)
		mockInspecaoSaidaRepo.EXPECT().
			BuscarInspecaoSaidaDetalhesPeloCodigo(input.CodInspecao).
			Return(inspecaoSaida, nil).
			Times(1)
		mockOrdemProducaoRepo.EXPECT().
			BuscarOrdem(odf, lote).
			Return(ordemProducao).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarEstoqueLocalValoresPorProduto(ordemProducao.CodigoProduto, ordemProducao.Lote, ordemProducao.ODF, localOrigem).
			Return(estoqueLocalValores, nil).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarSeries(estoqueLocalValores.Recno).
			Return(series, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, nil).
			Times(1)
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(recno).
			Return(inspecaoSaidaExecutadaWeb, nil).
			Times(1)
		movimentarEstoqueInput.IdSaga = "1000"
		movimentarEstoqueInput.RecnoInspecao = 321321
		sagaService.EXPECT().
			PublicarSaga(movimentarEstoqueInput).
			Return("", nil).
			Times(1)
		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaida(input)
		assert.Equal(t8, true, actualOutput)
		assert.Equal(t8, nil, err)
		assert.Nil(t8, validacaoDTO)
	})

	t.Run("ErroInserirInspecaoSaidaExecutadoWeb", func(t9 *testing.T) {
		mockUow.EXPECT().
			Begin().
			Times(2)
		mockUow.EXPECT().
			UnitOfWorkGuard().
			Times(2)
		mockUow.EXPECT().
			Complete().
			Times(2)
		mockLocaisRepo.EXPECT().
			BuscarLocalSaida().
			Return(localOrigem, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(localOrigem).
			Return(placeDescription, nil).
			Times(1)
		mockInspecaoSaidaRepo.EXPECT().
			BuscarInspecaoSaidaDetalhesPeloCodigo(input.CodInspecao).
			Return(inspecaoSaida, nil).
			Times(1)
		mockOrdemProducaoRepo.EXPECT().
			BuscarOrdem(odf, lote).
			Return(ordemProducao).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarEstoqueLocalValoresPorProduto(ordemProducao.CodigoProduto, ordemProducao.Lote, ordemProducao.ODF, localOrigem).
			Return(estoqueLocalValores, nil).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarSeries(estoqueLocalValores.Recno).
			Return(series, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, nil).
			Times(1)
		movimentarEstoqueInput.IdSaga = "1000"
		movimentarEstoqueInput.RecnoInspecao = 321321

		sagaService.EXPECT().
			PublicarSaga(movimentarEstoqueInput).
			Return(idSaga, nil).
			Times(1)
		mockWebRepo.EXPECT().
			BuscarInspecaoSaidaExecutadoWeb(recno).
			Return(inspecaoSaidaExecutadaWeb, nil).
			Times(2)
		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaida(input)
		assert.Equal(t9, true, actualOutput)
		assert.Equal(t9, nil, err)
		assert.Nil(t9, validacaoDTO)
	})

	t.Run("IfCompararQuantidades", func(t10 *testing.T) {
		mockLocaisRepo.EXPECT().
			BuscarLocalSaida().
			Return(localOrigem, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(localOrigem).
			Return(placeDescription, nil).
			Times(1)
		inspecaoSaida.QtdLote = decimal.NewFromInt(1)
		mockInspecaoSaidaRepo.EXPECT().
			BuscarInspecaoSaidaDetalhesPeloCodigo(input.CodInspecao).
			Return(inspecaoSaida, nil).
			Times(1)
		mockOrdemProducaoRepo.EXPECT().
			BuscarOrdem(odf, lote).
			Return(ordemProducao).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarEstoqueLocalValoresPorProduto(ordemProducao.CodigoProduto, ordemProducao.Lote, ordemProducao.ODF, localOrigem).
			Return(estoqueLocalValores, nil).
			Times(1)
		mockEstoqueRepo.EXPECT().
			BuscarSeries(estoqueLocalValores.Recno).
			Return(series, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, nil).
			Times(1)
		mockLocaisRepo.EXPECT().
			BuscarLocalDescricao(input.CodigoLocalAprovado).
			Return(descricaoDestino, nil).
			Times(1)
		inspecaoSaida.QtdLote = decimal.NewFromInt(10)
		sagaService.EXPECT().
			PublicarSaga(movimentarEstoqueInput).
			Return(idSaga, nil).
			Times(1)
		actualOutput, validacaoDTO, err := service.PublicarSagaInspecaoSaida(input)

		assert.Equal(t10, true, actualOutput)
		assert.Nil(t10, validacaoDTO)
		assert.Nil(t10, err)
	})

}
