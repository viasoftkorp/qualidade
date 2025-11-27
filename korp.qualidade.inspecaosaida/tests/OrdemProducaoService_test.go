package tests

import (
	"errors"
	"testing"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/mappers"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/mocks"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/services"
	"github.com/golang/mock/gomock"
	"github.com/shopspring/decimal"
	"github.com/stretchr/testify/assert"
)

func InstanciarOrdensProducao() []models.OrdemProducao {
	qtdProduzida := decimal.NewFromInt(14)
	return []models.OrdemProducao{
		{Plano: "Plano Teste", Lote: "87-6578-9", CodigoProduto: "11-5432321-3452432", QuantidadeProduzida: qtdProduzida},
	}
}

func InstanciarBaseFilter() *models.BaseFilter {
	return &models.BaseFilter{
		Filter:   "Test",
		Skip:     0,
		PageSize: 25,
	}
}

func InstanciarValidacaoDTO(code int, message string) *dto.ValidacaoDTO {
	return &dto.ValidacaoDTO{
		Code:    code,
		Message: message,
	}
}

func InstanciarOrdemProducaoFilters() *dto.OrdemProducaoFilters {
	codProduto := "7193-888"
	return &dto.OrdemProducaoFilters{
		CodigoProduto: &codProduto,
	}
}

func TestBuscarOrdensInspecao(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	mockOrdemProdRepo := mocks.NewMockIOrdemProducaoRepository(mockCtrl)
	service := services.NewOrdemProducaoService(mockOrdemProdRepo)
	baseFilter := InstanciarBaseFilter()
	ordens := InstanciarOrdensProducao()
	ordemProducaoFilter := InstanciarOrdemProducaoFilters()
	var expectedValidacaoDTO *dto.ValidacaoDTO
	expectedOutput := &dto.GetOrdemProducaoDTO{
		Items:      mappers.MapOrdemProducaoEntitiesToDTOs(ordens),
		TotalCount: int64(0),
	}
	t.Run("ErroBuscarOrdensInspecao", func(t1 *testing.T) {
		mockOrdemProdRepo.EXPECT().
			BuscarOrdensInspecao(baseFilter, ordemProducaoFilter).
			Return(ordens, errors.New("Test 1")).
			Times(1)
		actualOutput, err := service.BuscarOrdensInspecao(baseFilter, ordemProducaoFilter)
		expectedValidacaoDTO = InstanciarValidacaoDTO(1, "Test 1")
		assert.Nil(t1, actualOutput)
		assert.Equal(t1, expectedValidacaoDTO, err)
	})

	mockOrdemProdRepo.EXPECT().
		BuscarOrdensInspecao(baseFilter, ordemProducaoFilter).
		Return(ordens, nil).
		Times(1)
	actualOutput, err := service.BuscarOrdensInspecao(baseFilter, ordemProducaoFilter)
	assert.Nil(t, err)
	assert.Equal(t, expectedOutput, actualOutput)

}
