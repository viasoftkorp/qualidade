package tests

// import (
// 	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
// 	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
// 	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/enums"
// 	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/mappers"
// 	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/mocks"
// 	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
// 	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/services"
// 	"github.com/golang/mock/gomock"
// 	"github.com/shopspring/decimal"
// 	"github.com/stretchr/testify/assert"
// 	"testing"
// )

// func InstanciarMocksInspecaoEntradaHistoricoService(mockCtrl *gomock.Controller) (
// 	*mocks.MockIInspecaoEntradaHistoricoRepository,
// 	*mocks.MockIExternalInspecaoEntradaSagaService,
// 	*mocks.MockIInspecaoEntradaExecutadoWebRepository,
// 	*mocks.MockILocaisRepository,
// ) {
// 	mockInspecaoEntradaHistoricoRepository := mocks.NewMockIInspecaoEntradaHistoricoRepository(mockCtrl)
// 	mockExternalInspecaoEntradaSagaService := mocks.NewMockIExternalInspecaoEntradaSagaService(mockCtrl)
// 	mockInspecaoEntradaExecutadoWebRepository := mocks.NewMockIInspecaoEntradaExecutadoWebRepository(mockCtrl)
// 	mockLocaisRepository := mocks.NewMockILocaisRepository(mockCtrl)

// 	return mockInspecaoEntradaHistoricoRepository, mockExternalInspecaoEntradaSagaService, mockInspecaoEntradaExecutadoWebRepository, mockLocaisRepository
// }

// func TestGetAllInspecaoEntradaHistoricoCabecalho(t *testing.T) {
// 	mockContrl := gomock.NewController(t)
// 	defer mockContrl.Finish()

// 	mockInspecaoEntradaHistoricoRepository, mockExternalInspecaoEntradaSagaService, mockInspecaoEntradaExecutadoWebRepository, mockLocaisRepository := InstanciarMocksInspecaoEntradaHistoricoService(mockContrl)

// 	inspecaoEntradaHistoricoService := services.NewInspecaoEntradaHistoricoService(mockInspecaoEntradaHistoricoRepository, mockExternalInspecaoEntradaSagaService, mockInspecaoEntradaExecutadoWebRepository, mockLocaisRepository)

// 	baseFilters := &models.BaseFilter{
// 		Filter:   "",
// 		Skip:     0,
// 		PageSize: 25,
// 	}
// 	filters := &dto.InspecaoEntradaHistoricoCabecalhoFilters{}

// 	notasRepository := []models.NotaFiscalModel{
// 		{
// 			NotaFiscal:             1234,
// 			Lote:                   "A",
// 			Plano:                  "9999",
// 			DescricaoForneced:      " FORNECED 1",
// 			CodigoProduto:          "1111",
// 			DescricaoProduto:       "PRODUTO 1",
// 			Quantidade:             decimal.NewFromInt(10),
// 			QuantidadeInspecionada: decimal.NewFromInt(5),
// 			QuantidadeInspecionar:  decimal.NewFromInt(5),
// 		},
// 		{
// 			NotaFiscal:             1235,
// 			Lote:                   "B",
// 			Plano:                  "9999",
// 			DescricaoForneced:      " FORNECED 1",
// 			CodigoProduto:          "1111",
// 			DescricaoProduto:       "PRODUTO 1",
// 			Quantidade:             decimal.NewFromInt(20),
// 			QuantidadeInspecionada: decimal.NewFromInt(10),
// 			QuantidadeInspecionar:  decimal.NewFromInt(10),
// 		},
// 	}

// 	notasItems := mappers.MapNotasModelToDTO(notasRepository)
// 	notaTotalCount := int64(len(notasRepository))

// 	expectedOutput := &dto.GetNotasFiscaisOutput{
// 		Items:      notasItems,
// 		TotalCount: notaTotalCount,
// 	}

// 	mockInspecaoEntradaHistoricoRepository.EXPECT().BuscarNotasFiscaisHistorico(baseFilters, filters).
// 		Return(notasRepository, nil).Times(1)
// 	mockInspecaoEntradaHistoricoRepository.EXPECT().BuscarNotasFiscaisHistoricoTotalCount(baseFilters, filters).
// 		Return(notaTotalCount, nil).Times(1)

// 	output, err := inspecaoEntradaHistoricoService.GetAllInspecaoEntradaHistoricoCabecalho(baseFilters, filters)

// 	assert.Nil(t, err)
// 	assert.Equal(t, expectedOutput, output)
// }

// func TestGetAllInspecaoEntradaHistoricoItems(t *testing.T) {
// 	mockContrl := gomock.NewController(t)
// 	defer mockContrl.Finish()

// 	mockInspecaoEntradaHistoricoRepository, mockExternalInspecaoEntradaSagaService, mockInspecaoEntradaExecutadoWebRepository, mockLocaisRepository := InstanciarMocksInspecaoEntradaHistoricoService(mockContrl)

// 	inspecaoEntradaHistoricoService := services.NewInspecaoEntradaHistoricoService(mockInspecaoEntradaHistoricoRepository, mockExternalInspecaoEntradaSagaService, mockInspecaoEntradaExecutadoWebRepository, mockLocaisRepository)

// 	baseFilters := &models.BaseFilter{
// 		Filter:   "",
// 		Skip:     0,
// 		PageSize: 25,
// 	}

// 	filters := &dto.InspecaoEntradaHistoricoCabecalhoFilters{}

// 	transferenciasItem1 := []dto.HistoricoInspecaoEntradaTransferenciaOutput{
// 		{
// 			NotaFiscal:            1234,
// 			Quantidade:            5,
// 			LocalOrigem:           1,
// 			DescricaoLocalOrigem:  "LOCAL ENTRADA",
// 			LocalDestino:          2,
// 			DescricaoLocalDestino: "PRINCIPAL",
// 			TipoTransferencia:     enums.Aprovado,
// 		},
// 		{
// 			NotaFiscal:            1234,
// 			Quantidade:            5,
// 			LocalOrigem:           1,
// 			DescricaoLocalOrigem:  "LOCAL ENTRADA",
// 			LocalDestino:          3,
// 			DescricaoLocalDestino: "NAO CONFORME",
// 			TipoTransferencia:     enums.Reprovado,
// 		},
// 	}

// 	notaFiscal := 1234
// 	lote := "A"

// 	transferenciasItem2 := []dto.HistoricoInspecaoEntradaTransferenciaOutput{
// 		{
// 			NotaFiscal:            1234,
// 			Quantidade:            10,
// 			LocalOrigem:           1,
// 			DescricaoLocalOrigem:  "LOCAL ENTRADA",
// 			LocalDestino:          2,
// 			DescricaoLocalDestino: "PRINCIPAL",
// 			TipoTransferencia:     enums.Aprovado,
// 		},
// 		{
// 			NotaFiscal:            1234,
// 			Quantidade:            10,
// 			LocalOrigem:           1,
// 			DescricaoLocalOrigem:  "LOCAL ENTRADA",
// 			LocalDestino:          3,
// 			DescricaoLocalDestino: "NAO CONFORME",
// 			TipoTransferencia:     enums.Reprovado,
// 		},
// 	}

// 	transferenciasOriginais1 := []dto.SagaInspecaoEntradaTransferenciaOutput{
// 		{
// 			Quantidade:        5,
// 			LocalDestino:      2,
// 			LocalOrigem:       1,
// 			TipoTransferencia: enums.Aprovado,
// 		},
// 		{
// 			Quantidade:        5,
// 			LocalDestino:      3,
// 			LocalOrigem:       1,
// 			TipoTransferencia: enums.Reprovado,
// 		},
// 	}

// 	transferenciasOriginais2 := []dto.SagaInspecaoEntradaTransferenciaOutput{
// 		{
// 			Quantidade:        10,
// 			LocalDestino:      2,
// 			LocalOrigem:       1,
// 			TipoTransferencia: enums.Aprovado,
// 		},
// 		{
// 			Quantidade:        10,
// 			LocalDestino:      3,
// 			LocalOrigem:       1,
// 			TipoTransferencia: enums.Reprovado,
// 		},
// 	}

// 	itemsRepository := []dto.InspecaoEntradaHistoricoItems{
// 		{
// 			NotaFiscal:          1234,
// 			CodigoProduto:       "1111",
// 			DescricaoProduto:    "PRODUTO 1",
// 			RecnoInspecao:       200,
// 			QuantidadeInspecao:  10,
// 			QuantidadeAprovada:  5,
// 			QuantidadeReprovada: 5,
// 			Inspetor:            "inspetor",
// 			Resultado:           "Parcialmente Aprov.",
// 		},
// 		{
// 			NotaFiscal:          1234,
// 			CodigoProduto:       "1111",
// 			DescricaoProduto:    "PRODUTO 1",
// 			RecnoInspecao:       201,
// 			QuantidadeInspecao:  20,
// 			QuantidadeAprovada:  10,
// 			QuantidadeReprovada: 10,
// 			Inspetor:            "inspetor",
// 			Resultado:           "Parcialmente Aprov.",
// 		},
// 	}

// 	items := []dto.InspecaoEntradaHistoricoItemsDTO{
// 		{
// 			NotaFiscal:          1234,
// 			CodigoProduto:       "1111",
// 			DescricaoProduto:    "1111 - PRODUTO 1",
// 			RecnoInspecao:       200,
// 			QuantidadeInspecao:  10,
// 			QuantidadeAprovada:  5,
// 			QuantidadeReprovada: 5,
// 			Inspetor:            "inspetor",
// 			Resultado:           "Parcialmente Aprov.",
// 			Transferencias:      transferenciasItem1,
// 		},
// 		{
// 			NotaFiscal:          1234,
// 			CodigoProduto:       "1111",
// 			DescricaoProduto:    "1111 - PRODUTO 1",
// 			RecnoInspecao:       201,
// 			QuantidadeInspecao:  20,
// 			QuantidadeAprovada:  10,
// 			QuantidadeReprovada: 10,
// 			Inspetor:            "inspetor",
// 			Resultado:           "Parcialmente Aprov.",
// 			Transferencias:      transferenciasItem2,
// 		},
// 	}

// 	totalCount := int64(len(items))

// 	InspecaoExecutada1 := &entities.InspecaoEntradaExecutadoWeb{
// 		RecnoInspecaoEntrada:  200,
// 		IdInspecaoEntradaSaga: "100",
// 		Estorno:               false,
// 	}
// 	InspecaoExecutada2 := &entities.InspecaoEntradaExecutadoWeb{
// 		RecnoInspecaoEntrada:  201,
// 		IdInspecaoEntradaSaga: "101",
// 		Estorno:               false,
// 	}

// 	sagaExecutada1 := &dto.SagaInspecaoEntradaOutput{
// 		Id:                  "200",
// 		Status:              3,
// 		Erro:                "",
// 		NumeroRetentativas:  1,
// 		NumeroExecucoes:     1,
// 		IdUsuarioExecucao:   "300",
// 		NomeUsuarioExecucao: "Usuario",
// 		CodigoProduto:       "1111",
// 		QuantidadeTotal:     10,
// 		Resultado:           "Parcialmente Aprov.",
// 		NotaFiscal:          1234,
// 		Lote:                "A",
// 		Estorno:             false,
// 		RecnoInspecao:       200,
// 		Transferencias:      transferenciasOriginais1,
// 	}

// 	sagaExecutada2 := &dto.SagaInspecaoEntradaOutput{
// 		Id:                  "201",
// 		Status:              3,
// 		Erro:                "",
// 		NumeroRetentativas:  1,
// 		NumeroExecucoes:     1,
// 		IdUsuarioExecucao:   "300",
// 		NomeUsuarioExecucao: "Usuario",
// 		CodigoProduto:       "1111",
// 		QuantidadeTotal:     20,
// 		Resultado:           "Parcialmente Aprov.",
// 		NotaFiscal:          1234,
// 		Lote:                "A",
// 		Estorno:             false,
// 		RecnoInspecao:       201,
// 		Transferencias:      transferenciasOriginais2,
// 	}

// 	expectedOutput := &dto.GetAllInspecaoEntradaHistoricoItemsDTO{
// 		Items:      items,
// 		TotalCount: totalCount,
// 	}

// 	mockInspecaoEntradaHistoricoRepository.EXPECT().BuscarInspecoesEntradaHistorico(notaFiscal, lote, baseFilters, filters).
// 		Return(itemsRepository, nil).Times(1)
// 	mockInspecaoEntradaHistoricoRepository.EXPECT().BuscarQuantidadeInspecoesEntradaHistorico(notaFiscal, lote, baseFilters, filters).
// 		Return(totalCount, nil).Times(1)
// 	mockInspecaoEntradaExecutadoWebRepository.EXPECT().BuscarInspecaoEntradaExecutadoWeb(InspecaoExecutada1.RecnoInspecaoEntrada).
// 		Return(InspecaoExecutada1, nil).Times(1)
// 	mockInspecaoEntradaExecutadoWebRepository.EXPECT().BuscarInspecaoEntradaExecutadoWeb(InspecaoExecutada2.RecnoInspecaoEntrada).
// 		Return(InspecaoExecutada2, nil).Times(1)
// 	mockExternalInspecaoEntradaSagaService.EXPECT().BuscarSaga(InspecaoExecutada1.IdInspecaoEntradaSaga).
// 		Return(sagaExecutada1, nil).Times(1)
// 	mockExternalInspecaoEntradaSagaService.EXPECT().BuscarSaga(InspecaoExecutada2.IdInspecaoEntradaSaga).
// 		Return(sagaExecutada2, nil).Times(1)
// 	mockLocaisRepository.EXPECT().BuscarLocalDescricao(transferenciasOriginais1[0].LocalOrigem).
// 		Return("LOCAL ENTRADA", nil).Times(1)
// 	mockLocaisRepository.EXPECT().BuscarLocalDescricao(transferenciasOriginais1[0].LocalDestino).
// 		Return("PRINCIPAL", nil).Times(1)
// 	mockLocaisRepository.EXPECT().BuscarLocalDescricao(transferenciasOriginais2[1].LocalDestino).
// 		Return("NAO CONFORME", nil).Times(1)

// 	output, err := inspecaoEntradaHistoricoService.GetAllInspecaoEntradaHistoricoItems(notaFiscal, lote, baseFilters, filters)

// 	assert.Nil(t, err)
// 	assert.Equal(t, expectedOutput, output)
// }
