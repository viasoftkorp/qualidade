package tests

import (
	"errors"
	"testing"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/mocks"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/services"
	"github.com/golang/mock/gomock"
	"github.com/shopspring/decimal"
	"github.com/stretchr/testify/assert"
)

func TestBuscarNotasFiscais_Ok(t *testing.T) {
	mockContrl := gomock.NewController(t)
	defer mockContrl.Finish()

	mockRepoNotaFiscal := mocks.NewMockINotaFiscalRepository(mockContrl)
	notaFiscalService := services.NewNotaFiscalService(mockRepoNotaFiscal)

	baseFilter := &models.BaseFilter{
		Filter:   "",
		Skip:     0,
		PageSize: 25,
	}

	filters := new(dto.NotaFiscalFilters)

	notasFiscais := []models.NotaFiscalModel{
		{
			NotaFiscal:        100,
			CodigoProduto:     "28145",
			DescricaoProduto:  "PRODUTO TESTE",
			DescricaoForneced: "Fornecedor 1",
			Quantidade:        decimal.NewFromInt(10),
		},
		{
			NotaFiscal:        101,
			CodigoProduto:     "98172",
			DescricaoProduto:  "PRODUTO TESTE 2",
			DescricaoForneced: "Fornecedor 1",
			Quantidade:        decimal.NewFromInt(10),
		},
	}

	notasFiscaisExpected := []dto.NotaFiscalDTO{
		{
			NotaFiscal:            100,
			CodigoProduto:         "28145",
			DescricaoProduto:      "PRODUTO TESTE",
			DescricaoForneced:     "Fornecedor 1",
			Quantidade:            10,
			QuantidadeInspecionar: 10,
		},
		{
			NotaFiscal:            101,
			CodigoProduto:         "98172",
			DescricaoProduto:      "PRODUTO TESTE 2",
			DescricaoForneced:     "Fornecedor 1",
			Quantidade:            10,
			QuantidadeInspecionar: 10,
		},
	}

	mockRepoNotaFiscal.EXPECT().BuscarNotasFiscais(baseFilter, filters).
		Times(1).
		Return(notasFiscais, nil)
	mockRepoNotaFiscal.EXPECT().BuscarNotasFiscaisTotalCount(baseFilter, filters).
		Times(1).
		Return(int64(2), nil)

	notasFiscaisOutput, validacaoDto, err := notaFiscalService.BuscarNotasFiscais(baseFilter, filters)
	assert.Nil(t, validacaoDto)
	assert.Nil(t, err)
	assert.Equal(t, notasFiscaisExpected, notasFiscaisOutput.Items)
	assert.Equal(t, int64(2), notasFiscaisOutput.TotalCount)
}

func TestBuscarOrdensInspecao_Erro_BuscarNotasFiscais(t *testing.T) {
	mockContrl := gomock.NewController(t)
	defer mockContrl.Finish()

	mockRepoNotaFiscal := mocks.NewMockINotaFiscalRepository(mockContrl)
	notaFiscalService := services.NewNotaFiscalService(mockRepoNotaFiscal)

	baseFilter := &models.BaseFilter{
		Filter:   "",
		Skip:     0,
		PageSize: 25,
	}

	filters := new(dto.NotaFiscalFilters)

	notasFiscais := []models.NotaFiscalModel{
		{
			NotaFiscal:        100,
			CodigoProduto:     "28145",
			DescricaoProduto:  "PRODUTO TESTE",
			DescricaoForneced: "Fornecedor 1",
			Quantidade:        decimal.NewFromInt(10),
		},
		{
			NotaFiscal:        101,
			CodigoProduto:     "98172",
			DescricaoProduto:  "PRODUTO TESTE 2",
			DescricaoForneced: "Fornecedor 1",
			Quantidade:        decimal.NewFromInt(10),
		},
	}

	mockRepoNotaFiscal.EXPECT().BuscarNotasFiscais(baseFilter, filters).
		Times(1).
		Return(notasFiscais, errors.New("error"))

	notasFiscaisOutput, validacaoDto, err := notaFiscalService.BuscarNotasFiscais(baseFilter, filters)
	assert.Nil(t, validacaoDto)
	assert.Nil(t, notasFiscaisOutput)
	assert.Equal(t, err, errors.New("error"))
}

func TestBuscarOrdensInspecao_Erro_BuscarNotasFiscaisTotalCount(t *testing.T) {
	mockContrl := gomock.NewController(t)
	defer mockContrl.Finish()

	mockRepoNotaFiscal := mocks.NewMockINotaFiscalRepository(mockContrl)
	notaFiscalService := services.NewNotaFiscalService(mockRepoNotaFiscal)

	baseFilter := &models.BaseFilter{
		Filter:   "",
		Skip:     0,
		PageSize: 25,
	}

	filters := new(dto.NotaFiscalFilters)

	notasFiscais := []models.NotaFiscalModel{
		{
			NotaFiscal:        100,
			CodigoProduto:     "28145",
			DescricaoProduto:  "PRODUTO TESTE",
			DescricaoForneced: "Fornecedor 1",
			Quantidade:        decimal.NewFromInt(10),
		},
		{
			NotaFiscal:        101,
			CodigoProduto:     "98172",
			DescricaoProduto:  "PRODUTO TESTE 2",
			DescricaoForneced: "Fornecedor 1",
			Quantidade:        decimal.NewFromInt(10),
		},
	}

	mockRepoNotaFiscal.EXPECT().BuscarNotasFiscais(baseFilter, filters).
		Times(1).
		Return(notasFiscais, nil)
	mockRepoNotaFiscal.EXPECT().BuscarNotasFiscaisTotalCount(baseFilter, filters).
		Times(1).
		Return(int64(2), errors.New("error"))

	notasFiscaisOutput, validacaoDto, err := notaFiscalService.BuscarNotasFiscais(baseFilter, filters)
	assert.Nil(t, validacaoDto)
	assert.Nil(t, notasFiscaisOutput)
	assert.Equal(t, err, errors.New("error"))
}
