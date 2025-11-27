package tests

// import (
// 	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
// 	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
// 	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/enums"
// 	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/mocks"
// 	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
// 	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/services"
// 	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
// 	"github.com/golang/mock/gomock"
// 	"github.com/shopspring/decimal"
// 	"github.com/stretchr/testify/assert"
// 	"testing"
// )

// func InstanciarMocksInspecaoEntradaSagaService(mockCtrl *gomock.Controller) (
// 	*mocks.MockIInspecaoEntradaRepository,
// 	*mocks.MockIInspecaoEntradaExecutadoWebRepository,
// 	*mocks.MockIExternalInspecaoEntradaSagaService,
// 	*mocks.MockINotaFiscalRepository,
// 	*mocks.MockILocaisRepository,
// 	*mocks.MockIParametrosRepository,
// 	*mocks.MockIEstoquePedidoVendaRepository,
// 	*mocks.MockIProdutoRepository,
// 	*mocks.MockIInspecaoEntradaPedidoVendaLoteRepository,
// ) {
// 	mockInspecaoEntradaRepository := mocks.NewMockIInspecaoEntradaRepository(mockCtrl)
// 	mockInspecaoEntradaExecutadoWebRepository := mocks.NewMockIInspecaoEntradaExecutadoWebRepository(mockCtrl)
// 	mockExternalInspecaoEntradaSagaService := mocks.NewMockIExternalInspecaoEntradaSagaService(mockCtrl)
// 	mockNotaFiscalRepository := mocks.NewMockINotaFiscalRepository(mockCtrl)
// 	mockLocaisRepository := mocks.NewMockILocaisRepository(mockCtrl)
// 	mockParametrosRepository := mocks.NewMockIParametrosRepository(mockCtrl)
// 	mockEstoquePedidoVendaRepository := mocks.NewMockIEstoquePedidoVendaRepository(mockCtrl)
// 	mockProdutoRepository := mocks.NewMockIProdutoRepository(mockCtrl)
// 	mockInspecaoEntradaPedidoVendaLoteRepository := mocks.NewMockIInspecaoEntradaPedidoVendaLoteRepository(mockCtrl)

// 	return mockInspecaoEntradaRepository, mockInspecaoEntradaExecutadoWebRepository, mockExternalInspecaoEntradaSagaService,
// 		mockNotaFiscalRepository, mockLocaisRepository, mockParametrosRepository, mockEstoquePedidoVendaRepository, mockProdutoRepository, mockInspecaoEntradaPedidoVendaLoteRepository
// }

// func TestPublicarSagaInspecaoEntrada_UtilizarReservaDePedido(t *testing.T) {
// 	mockContrl := gomock.NewController(t)
// 	defer mockContrl.Finish()

// 	mockInspecaoEntradaRepository, mockInspecaoEntradaExecutadoWebRepository, mockExternalInspecaoEntradaSagaService,
// 		mockNotaFiscalRepository, mockLocaisRepository, mockParametrosRepository, mockEstoquePedidoVendaRepository, mockProdutoRepository, mockInspecaoEntradaPedidoVendaLoteRepository := InstanciarMocksInspecaoEntradaSagaService(mockContrl)
// 	mockUow := mocks.NewMockUnitOfWork(mockContrl)

// 	inspecaoEntradaService := services.NewInspecaoEntradaSagaService(mockInspecaoEntradaRepository, mockInspecaoEntradaExecutadoWebRepository, mockExternalInspecaoEntradaSagaService,
// 		mockNotaFiscalRepository, mockLocaisRepository, mockParametrosRepository, mockEstoquePedidoVendaRepository, mockProdutoRepository, mockUow, mockInspecaoEntradaPedidoVendaLoteRepository)

// 	input := &dto.FinalizarInspecaoInput{
// 		Lote:                 "1234",
// 		CodigoInspecao:       100,
// 		Resultado:            "Aprovado",
// 		QuantidadeAprovada:   50,
// 		QuantidadeRejeitada:  50,
// 		CodigoLocalPrincipal: 1,
// 		CodigoLocalReprovado: 2,
// 	}

// 	inspecaoEntradaModelJoin := []models.InspecaoEntradaJoin{
// 		{
// 			NumeroPedido:         "PEDIDO 1",
// 			Odf:                  1010,
// 			Lote:                 "1234",
// 			QuantidadeLote:       decimal.NewFromInt(150),
// 			Recno:                10,
// 			CodigoInspecao:       100,
// 			NotaFiscal:           200,
// 			Resultado:            "Aprovado",
// 			CodigoLocalAprovado:  1,
// 			QuantidadeAprovada:   decimal.NewFromInt(50),
// 			CodigoLocalReprovado: 2,
// 			QuantidadeReprovada:  decimal.NewFromInt(50),
// 		},
// 		{
// 			NumeroPedido:         "PEDIDO 2",
// 			Odf:                  1212,
// 			Lote:                 "1234",
// 			QuantidadeLote:       decimal.NewFromInt(100),
// 			Recno:                10,
// 			CodigoInspecao:       100,
// 			NotaFiscal:           200,
// 			Resultado:            "Reprovado",
// 			CodigoLocalAprovado:  1,
// 			QuantidadeAprovada:   decimal.NewFromInt(0),
// 			CodigoLocalReprovado: 2,
// 			QuantidadeReprovada:  decimal.NewFromInt(10),
// 		},
// 	}

// 	notaFiscalModel := models.NotaFiscalModel{
// 		IdNotaFiscal:           123456789,
// 		NotaFiscal:             200,
// 		Plano:                  123,
// 		Lote:                   "1234",
// 		DescricaoForneced:      "FORNECEDOR",
// 		CodigoProduto:          "4321",
// 		DescricaoProduto:       "PRODUTO 1",
// 		Quantidade:             decimal.NewFromInt(200),
// 		QuantidadeInspecionar:  decimal.NewFromInt(100),
// 		QuantidadeInspecionada: decimal.NewFromInt(100),
// 		RecnoRateioLote:        10,
// 		CodigoLocal:            3,
// 	}

// 	estoqueLocal := &models.EstoqueLocalValores{
// 		Recno:      1,
// 		Quantidade: decimal.NewFromInt(100),
// 		ValorPago:  decimal.NewFromInt(500),
// 	}

// 	series := new([]models.Serie)
// 	serieInput := []dto.InspecaoEntradaSerieProducaoBackgroundInputDto{}
// 	transferencias := []dto.InspecaoEntradaTransferenciaBackgroundInputDto{
// 		{
// 			Quantidade:        50,
// 			Documento:         "Transferencia do lugar LOCAL ENTRADA para lugar PRINCIPAL via inspecao web",
// 			LocalDestino:      1,
// 			LocalOrigem:       3,
// 			Fator:             1,
// 			NumeroPedido:      "PEDIDO 1",
// 			UltimoValorPago:   500,
// 			OrdemFabricacao:   1010,
// 			TipoTransferencia: enums.Aprovado,
// 			SeriesProducao:    serieInput,
// 		},
// 		{
// 			Quantidade:        50,
// 			Fator:             1,
// 			Documento:         "Transferencia do lugar LOCAL ENTRADA para lugar NAO CONFORME via inspecao web",
// 			LocalDestino:      2,
// 			LocalOrigem:       3,
// 			NumeroPedido:      "PEDIDO 1",
// 			UltimoValorPago:   500,
// 			OrdemFabricacao:   1010,
// 			TipoTransferencia: enums.Reprovado,
// 			SeriesProducao:    serieInput,
// 		},
// 		{
// 			Quantidade:        10,
// 			Fator:             1,
// 			Documento:         "Transferencia do lugar LOCAL ENTRADA para lugar NAO CONFORME via inspecao web",
// 			LocalDestino:      2,
// 			LocalOrigem:       3,
// 			NumeroPedido:      "PEDIDO 2",
// 			UltimoValorPago:   500,
// 			OrdemFabricacao:   1212,
// 			TipoTransferencia: enums.Reprovado,
// 			SeriesProducao:    serieInput,
// 		},
// 	}

// 	movimentaEstoque := dto.MovimentarEstoqueInspecaoBackgroundInputDto{
// 		IdSaga:             utils.NewGuidAsString(),
// 		Lote:               "1234",
// 		Estorno:            false,
// 		RecnoInspecao:      10,
// 		CodigoProduto:      "4321",
// 		Resultado:          "Parcialmente Aprov.",
// 		OrigemMovimentacao: "",
// 		NotaFiscal:         200,
// 		Transferencias:     transferencias,
// 	}

// 	mockLocaisRepository.EXPECT().BuscarLocalDescricao(notaFiscalModel.CodigoLocal).
// 		Return("LOCAL ENTRADA", nil).Times(1)
// 	mockLocaisRepository.EXPECT().BuscarLocalDescricao(input.CodigoLocalPrincipal).
// 		Return("PRINCIPAL", nil).Times(1)
// 	mockLocaisRepository.EXPECT().BuscarLocalDescricao(input.CodigoLocalReprovado).
// 		Return("NAO CONFORME", nil).Times(2)
// 	mockParametrosRepository.EXPECT().BuscarValorParametro(consts.UtilizarReservaDePedidoNaLocalizacaoDeEstoque, consts.UtilizarReservaDePedidoNaLocalizacaoDeEstoqueSecao).
// 		Return(true, nil).Times(1)
// 	mockInspecaoEntradaRepository.EXPECT().BuscarInspecaoEntradaDetalhesPeloCodigoJoin(input.CodigoInspecao).
// 		Return(inspecaoEntradaModelJoin, nil).Times(1)
// 	mockNotaFiscalRepository.EXPECT().BuscarNotaFiscal(inspecaoEntradaModelJoin[0].NotaFiscal, input.Lote).
// 		Return(notaFiscalModel, nil).Times(1)
// 	mockEstoquePedidoVendaRepository.EXPECT().BuscarEstoqueLocalValoresPorProduto(notaFiscalModel.CodigoProduto, notaFiscalModel.Lote, notaFiscalModel.CodigoLocal).
// 		Return(estoqueLocal, nil).Times(1)
// 	mockEstoquePedidoVendaRepository.EXPECT().BuscarSeries(estoqueLocal.Recno).
// 		Return(*series, nil).Times(2)

// 	mockUow.EXPECT().Begin().Return(nil).Times(1)
// 	mockUow.EXPECT().UnitOfWorkGuard().Return().Times(1)

// 	mockInspecaoEntradaExecutadoWebRepository.EXPECT().
// 		BuscarInspecaoEntradaExecutadoWeb(10).
// 		Return(nil, nil).
// 		Times(1)

// 	inspecaoExecutadaWeb := &entities.InspecaoEntradaExecutadoWeb{
// 		RecnoInspecaoEntrada:  10,
// 		IdInspecaoEntradaSaga: movimentaEstoque.IdSaga,
// 		Estorno:               false,
// 		CodigoProduto:         "4321",
// 	}
// 	mockInspecaoEntradaExecutadoWebRepository.EXPECT().
// 		InserirInspecaoEntradaExecutadoWeb(inspecaoExecutadaWeb).
// 		Return(nil).
// 		Times(1)

// 	mockExternalInspecaoEntradaSagaService.EXPECT().
// 		PublicarSaga(movimentaEstoque).
// 		Return("Guid", nil).
// 		Times(1)

// 	mockUow.EXPECT().Complete().Return(nil).Times(1)

// 	result, validacaoDTO, err := inspecaoEntradaService.PublicarSagaInspecaoEntrada(input)
// 	assert.Nil(t, validacaoDTO)
// 	assert.Nil(t, err)
// 	assert.Equal(t, true, result)
// }

// func TestPublicarSagaInspecaoEntrada_NaoUtilizarReservaDePedido(t *testing.T) {
// 	mockContrl := gomock.NewController(t)
// 	defer mockContrl.Finish()

// 	mockInspecaoEntradaRepository, mockInspecaoEntradaExecutadoWebRepository, mockExternalInspecaoEntradaSagaService,
// 		mockNotaFiscalRepository, mockLocaisRepository, mockParametrosRepository, mockEstoquePedidoVendaRepository, mockProdutoRepository, mockInspecaoEntradaPedidoVendaLoteRepository := InstanciarMocksInspecaoEntradaSagaService(mockContrl)
// 	mockUow := mocks.NewMockUnitOfWork(mockContrl)

// 	inspecaoEntradaService := services.NewInspecaoEntradaSagaService(mockInspecaoEntradaRepository, mockInspecaoEntradaExecutadoWebRepository, mockExternalInspecaoEntradaSagaService,
// 		mockNotaFiscalRepository, mockLocaisRepository, mockParametrosRepository, mockEstoquePedidoVendaRepository, mockProdutoRepository, mockUow, mockInspecaoEntradaPedidoVendaLoteRepository)

// 	input := &dto.FinalizarInspecaoInput{
// 		Lote:                 "1234",
// 		CodigoInspecao:       100,
// 		Resultado:            "Aprovado",
// 		QuantidadeAprovada:   50,
// 		QuantidadeRejeitada:  50,
// 		CodigoLocalPrincipal: 1,
// 		CodigoLocalReprovado: 2,
// 	}

// 	inspecaoEntradaModel := &models.InspecaoEntrada{
// 		Lote:                "1234",
// 		QuantidadeLote:      decimal.NewFromInt(150),
// 		Recno:               10,
// 		CodigoInspecao:      100,
// 		NotaFiscal:          200,
// 		Resultado:           "Aprovado",
// 		QuantidadeAprovada:  decimal.NewFromInt(50),
// 		QuantidadeReprovada: decimal.NewFromInt(50),
// 	}

// 	notaFiscalModel := models.NotaFiscalModel{
// 		IdNotaFiscal:           123456789,
// 		NotaFiscal:             200,
// 		Plano:                  123,
// 		Lote:                   "1234",
// 		DescricaoForneced:      "FORNECEDOR",
// 		CodigoProduto:          "4321",
// 		DescricaoProduto:       "PRODUTO 1",
// 		Quantidade:             decimal.NewFromInt(200),
// 		QuantidadeInspecionar:  decimal.NewFromInt(100),
// 		QuantidadeInspecionada: decimal.NewFromInt(100),
// 		RecnoRateioLote:        10,
// 		CodigoLocal:            3,
// 	}

// 	estoqueLocal := &models.EstoqueLocalValores{
// 		Recno:      1,
// 		Quantidade: decimal.NewFromInt(100),
// 		ValorPago:  decimal.NewFromInt(500),
// 	}

// 	series := new([]models.Serie)
// 	serieInput := []dto.InspecaoEntradaSerieProducaoBackgroundInputDto{}
// 	transferencias := []dto.InspecaoEntradaTransferenciaBackgroundInputDto{
// 		{
// 			Quantidade:        50,
// 			Documento:         "Transferencia do lugar LOCAL ENTRADA para lugar PRINCIPAL via inspecao web",
// 			LocalDestino:      1,
// 			LocalOrigem:       3,
// 			Fator:             1,
// 			UltimoValorPago:   500,
// 			TipoTransferencia: enums.Aprovado,
// 			SeriesProducao:    serieInput,
// 		},
// 		{
// 			Quantidade:        50,
// 			Fator:             1,
// 			Documento:         "Transferencia do lugar LOCAL ENTRADA para lugar NAO CONFORME via inspecao web",
// 			LocalDestino:      2,
// 			LocalOrigem:       3,
// 			UltimoValorPago:   500,
// 			TipoTransferencia: enums.Reprovado,
// 			SeriesProducao:    serieInput,
// 		},
// 	}

// 	movimentaEstoque := dto.MovimentarEstoqueInspecaoBackgroundInputDto{
// 		IdSaga:             utils.NewGuidAsString(),
// 		Lote:               "1234",
// 		Estorno:            false,
// 		RecnoInspecao:      10,
// 		CodigoProduto:      "4321",
// 		Resultado:          "Parcialmente Aprov.",
// 		OrigemMovimentacao: "",
// 		NotaFiscal:         200,
// 		Transferencias:     transferencias,
// 	}

// 	mockLocaisRepository.EXPECT().BuscarLocalDescricao(notaFiscalModel.CodigoLocal).
// 		Return("LOCAL ENTRADA", nil).Times(1)
// 	mockParametrosRepository.EXPECT().BuscarValorParametro(consts.UtilizarReservaDePedidoNaLocalizacaoDeEstoque, consts.UtilizarReservaDePedidoNaLocalizacaoDeEstoqueSecao).
// 		Return(false, nil).Times(1)
// 	mockInspecaoEntradaRepository.EXPECT().BuscarInspecaoEntradaDetalhesPeloCodigo(input.CodigoInspecao).
// 		Return(inspecaoEntradaModel, nil).Times(1)
// 	mockNotaFiscalRepository.EXPECT().BuscarNotaFiscal(inspecaoEntradaModel.NotaFiscal, input.Lote).
// 		Return(notaFiscalModel, nil).Times(1)
// 	mockEstoquePedidoVendaRepository.EXPECT().BuscarEstoqueLocalValoresPorProduto(notaFiscalModel.CodigoProduto, notaFiscalModel.Lote, notaFiscalModel.CodigoLocal).
// 		Return(estoqueLocal, nil).Times(1)
// 	mockEstoquePedidoVendaRepository.EXPECT().BuscarSeries(estoqueLocal.Recno).
// 		Return(*series, nil).Times(1)
// 	mockLocaisRepository.EXPECT().BuscarLocalDescricao(input.CodigoLocalPrincipal).
// 		Return("PRINCIPAL", nil).Times(1)
// 	mockLocaisRepository.EXPECT().BuscarLocalDescricao(input.CodigoLocalReprovado).
// 		Return("NAO CONFORME", nil).Times(1)

// 	mockUow.EXPECT().Begin().Return(nil).Times(1)
// 	mockUow.EXPECT().UnitOfWorkGuard().Return().Times(1)

// 	mockInspecaoEntradaExecutadoWebRepository.EXPECT().
// 		BuscarInspecaoEntradaExecutadoWeb(10).
// 		Return(nil, nil).
// 		Times(1)

// 	inspecaoExecutadaWeb := &entities.InspecaoEntradaExecutadoWeb{
// 		RecnoInspecaoEntrada:  10,
// 		IdInspecaoEntradaSaga: movimentaEstoque.IdSaga,
// 		Estorno:               false,
// 		CodigoProduto:         "4321",
// 	}
// 	mockInspecaoEntradaExecutadoWebRepository.EXPECT().
// 		InserirInspecaoEntradaExecutadoWeb(inspecaoExecutadaWeb).
// 		Return(nil).
// 		Times(1)

// 	mockExternalInspecaoEntradaSagaService.EXPECT().
// 		PublicarSaga(movimentaEstoque).
// 		Return("Guid", nil).
// 		Times(1)

// 	mockUow.EXPECT().Complete().Return(nil).Times(1)

// 	result, validacaoDTO, err := inspecaoEntradaService.PublicarSagaInspecaoEntrada(input)
// 	assert.Nil(t, validacaoDTO)
// 	assert.Nil(t, err)
// 	assert.Equal(t, true, result)
// }

// func TestPublicarSagaInspecaoEntradaEstorno_(t *testing.T) {
// 	mockContrl := gomock.NewController(t)
// 	defer mockContrl.Finish()

// 	mockInspecaoEntradaRepository, mockInspecaoEntradaExecutadoWebRepository, mockExternalInspecaoEntradaSagaService,
// 		mockNotaFiscalRepository, mockLocaisRepository, mockParametrosRepository, mockEstoquePedidoVendaRepository, mockProdutoRepository, mockInspecaoEntradaPedidoVendaLoteRepository := InstanciarMocksInspecaoEntradaSagaService(mockContrl)
// 	mockUow := mocks.NewMockUnitOfWork(mockContrl)

// 	inspecaoEntradaService := services.NewInspecaoEntradaSagaService(mockInspecaoEntradaRepository, mockInspecaoEntradaExecutadoWebRepository, mockExternalInspecaoEntradaSagaService,
// 		mockNotaFiscalRepository, mockLocaisRepository, mockParametrosRepository, mockEstoquePedidoVendaRepository, mockProdutoRepository, mockUow, mockInspecaoEntradaPedidoVendaLoteRepository)

// 	RecnoInspecaoEntrada := 1

// 	InspecaoExecutada := &entities.InspecaoEntradaExecutadoWeb{
// 		RecnoInspecaoEntrada:  RecnoInspecaoEntrada,
// 		IdInspecaoEntradaSaga: "100",
// 		Estorno:               false,
// 	}

// 	transferenciasEstorno := []dto.InspecaoEntradaTransferenciaBackgroundInputDto{
// 		{
// 			Quantidade:        50,
// 			Documento:         "Transferencia do lugar PRINCIPAL para lugar LOCAL ENTRADA via inspecao web",
// 			LocalDestino:      3,
// 			LocalOrigem:       1,
// 			Fator:             1,
// 			NumeroPedido:      "PEDIDO 1",
// 			UltimoValorPago:   500,
// 			TipoTransferencia: enums.Aprovado,
// 			OrdemFabricacao:   0,
// 			SeriesProducao:    make([]dto.InspecaoEntradaSerieProducaoBackgroundInputDto, 0),
// 			Lote:              "1234",
// 			LoteOrigem:        "1234",
// 		},
// 		{
// 			Quantidade:        50,
// 			Fator:             1,
// 			Documento:         "Transferencia do lugar NAO CONFORME para lugar LOCAL ENTRADA via inspecao web",
// 			LocalDestino:      3,
// 			LocalOrigem:       2,
// 			NumeroPedido:      "PEDIDO 1",
// 			UltimoValorPago:   500,
// 			TipoTransferencia: enums.Reprovado,
// 			OrdemFabricacao:   0,
// 			SeriesProducao:    make([]dto.InspecaoEntradaSerieProducaoBackgroundInputDto, 0),
// 			Lote:              "1234",
// 			LoteOrigem:        "1234",
// 		},
// 		{
// 			Quantidade:        10,
// 			Fator:             1,
// 			Documento:         "Transferencia do lugar NAO CONFORME para lugar LOCAL ENTRADA via inspecao web",
// 			LocalDestino:      3,
// 			LocalOrigem:       2,
// 			NumeroPedido:      "PEDIDO 2",
// 			UltimoValorPago:   500,
// 			TipoTransferencia: enums.Reprovado,
// 			OrdemFabricacao:   0,
// 			SeriesProducao:    make([]dto.InspecaoEntradaSerieProducaoBackgroundInputDto, 0),
// 			Lote:              "1234",
// 			LoteOrigem:        "1234",
// 		},
// 	}

// 	transferenciasOriginais := []dto.SagaInspecaoEntradaTransferenciaOutput{
// 		{
// 			Quantidade:        50,
// 			LocalDestino:      1,
// 			LocalOrigem:       3,
// 			NumeroPedido:      "PEDIDO 1",
// 			TipoTransferencia: enums.Aprovado,
// 		},
// 		{
// 			Quantidade:        50,
// 			LocalDestino:      2,
// 			LocalOrigem:       3,
// 			NumeroPedido:      "PEDIDO 1",
// 			TipoTransferencia: enums.Reprovado,
// 		},
// 		{
// 			Quantidade:        10,
// 			LocalDestino:      2,
// 			LocalOrigem:       3,
// 			NumeroPedido:      "PEDIDO 2",
// 			TipoTransferencia: enums.Reprovado,
// 		},
// 	}

// 	sagaExecutada := &dto.SagaInspecaoEntradaOutput{
// 		Id:                  "200",
// 		Status:              3,
// 		Erro:                "",
// 		NumeroRetentativas:  1,
// 		NumeroExecucoes:     1,
// 		IdUsuarioExecucao:   "300",
// 		NomeUsuarioExecucao: "Usuario",
// 		CodigoProduto:       "4321",
// 		QuantidadeTotal:     100,
// 		Resultado:           "Parcialmente Aprov.",
// 		NotaFiscal:          200,
// 		Lote:                "1234",
// 		Estorno:             false,
// 		RecnoInspecao:       RecnoInspecaoEntrada,
// 		Transferencias:      transferenciasOriginais,
// 	}

// 	movimentarEstoqueInputEstorno := dto.MovimentarEstoqueInspecaoBackgroundInputDto{
// 		IdSaga:         utils.NewGuidAsString(),
// 		CodigoProduto:  "4321",
// 		NotaFiscal:     200,
// 		Lote:           "1234",
// 		Estorno:        true,
// 		RecnoInspecao:  RecnoInspecaoEntrada,
// 		Transferencias: transferenciasEstorno,
// 	}

// 	estoqueLocalPedido1 := &models.EstoqueLocalValores{
// 		Recno:      400,
// 		Quantidade: decimal.NewFromFloat(100),
// 		ValorPago:  decimal.NewFromFloat(500),
// 	}

// 	estoqueLocalPedido2 := &models.EstoqueLocalValores{
// 		Recno:      500,
// 		Quantidade: decimal.NewFromFloat(100),
// 		ValorPago:  decimal.NewFromFloat(500),
// 	}

// 	mockInspecaoEntradaExecutadoWebRepository.EXPECT().BuscarInspecaoEntradaExecutadoWeb(RecnoInspecaoEntrada).
// 		Return(InspecaoExecutada, nil).Times(1)
// 	mockExternalInspecaoEntradaSagaService.EXPECT().BuscarSaga(InspecaoExecutada.IdInspecaoEntradaSaga).
// 		Return(sagaExecutada, nil).Times(1)
// 	mockLocaisRepository.EXPECT().BuscarLocalDescricao(transferenciasEstorno[0].LocalOrigem).
// 		Return("PRINCIPAL", nil).Times(1)
// 	mockLocaisRepository.EXPECT().BuscarLocalDescricao(transferenciasEstorno[0].LocalDestino).
// 		Return("LOCAL ENTRADA", nil).Times(1)
// 	mockLocaisRepository.EXPECT().BuscarLocalDescricao(transferenciasEstorno[1].LocalOrigem).
// 		Return("NAO CONFORME", nil).Times(1)
// 	mockLocaisRepository.EXPECT().BuscarLocalDescricao(transferenciasEstorno[1].LocalDestino).
// 		Return("LOCAL ENTRADA", nil).Times(1)
// 	mockLocaisRepository.EXPECT().BuscarLocalDescricao(transferenciasEstorno[2].LocalOrigem).
// 		Return("NAO CONFORME", nil).Times(1)
// 	mockLocaisRepository.EXPECT().BuscarLocalDescricao(transferenciasEstorno[2].LocalDestino).
// 		Return("LOCAL ENTRADA", nil).Times(1)
// 	mockEstoquePedidoVendaRepository.EXPECT().BuscarEstoqueLocalValoresPorProduto(sagaExecutada.CodigoProduto, sagaExecutada.Lote, transferenciasEstorno[0].LocalOrigem).
// 		Return(estoqueLocalPedido1, nil).Times(1)
// 	mockEstoquePedidoVendaRepository.EXPECT().BuscarEstoqueLocalValoresPorProduto(sagaExecutada.CodigoProduto, sagaExecutada.Lote, transferenciasEstorno[1].LocalOrigem).
// 		Return(estoqueLocalPedido1, nil).Times(1)
// 	mockEstoquePedidoVendaRepository.EXPECT().BuscarEstoqueLocalValoresPorProduto(sagaExecutada.CodigoProduto, sagaExecutada.Lote, transferenciasEstorno[1].LocalOrigem).
// 		Return(estoqueLocalPedido2, nil).Times(1)

// 	mockUow.EXPECT().Begin().Return(nil).Times(1)
// 	mockUow.EXPECT().UnitOfWorkGuard().Return().Times(1)

// 	mockInspecaoEntradaExecutadoWebRepository.EXPECT().
// 		BuscarInspecaoEntradaExecutadoWeb(RecnoInspecaoEntrada).
// 		Return(nil, nil).
// 		Times(1)

// 	inspecaoExecutadaWeb := &entities.InspecaoEntradaExecutadoWeb{
// 		RecnoInspecaoEntrada:  RecnoInspecaoEntrada,
// 		IdInspecaoEntradaSaga: movimentarEstoqueInputEstorno.IdSaga,
// 		Estorno:               true,
// 		CodigoProduto:         "4321",
// 	}
// 	mockInspecaoEntradaExecutadoWebRepository.EXPECT().
// 		InserirInspecaoEntradaExecutadoWeb(inspecaoExecutadaWeb).
// 		Return(nil).
// 		Times(1)

// 	mockExternalInspecaoEntradaSagaService.EXPECT().
// 		PublicarSaga(movimentarEstoqueInputEstorno).
// 		Return("Guid", nil).
// 		Times(1)

// 	mockUow.EXPECT().Complete().Return(nil).Times(1)

// 	result, validacaoDTO, err := inspecaoEntradaService.PublicarSagaInspecaoEntradaEstorno(RecnoInspecaoEntrada)
// 	assert.Nil(t, validacaoDTO)
// 	assert.Nil(t, err)
// 	assert.Equal(t, true, result)
// }
