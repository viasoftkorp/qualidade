package tests

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/mocks"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/services"
	"errors"
	"github.com/golang/mock/gomock"
	"github.com/stretchr/testify/assert"
	"testing"
)

func InstanciarProdOutput(codigo string) *dto.ProdutoOutput {
	return &dto.ProdutoOutput{
		Codigo: codigo,
	}
}

func InstanciarProdutos() []dto.ProdutoOutput {
	return []dto.ProdutoOutput{
		{Id: "32187", Codigo: "1887347", Descricao: "Argamassa 14kgs areia fina", Unidade: "14.75"},
	}
}

func TestBuscarProduto(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	mockProdRepo := mocks.NewMockIProdutoRepository(mockCtrl)
	service := services.NewProdutoService(mockProdRepo)

	codigo := "321789"
	produto := InstanciarProdOutput(codigo)
	t.Run("ErroBuscarProdutoPeloCodigo", func(t *testing.T) {
		mockProdRepo.EXPECT().
			BuscarProdutoPeloCodigo(codigo).
			Return(produto, errors.New("Test 1")).
			Times(1)
		actualOutput, err := service.BuscarProduto(codigo)
		assert.Nil(t, actualOutput)
		assert.Equal(t, errors.New("Test 1"), err)
	})

	mockProdRepo.EXPECT().
		BuscarProdutoPeloCodigo(codigo).
		Return(produto, nil).
		Times(1)
	actualOutput, err := service.BuscarProduto(codigo)
	expectedOutput := produto
	assert.Nil(t, err)
	assert.Equal(t, expectedOutput, actualOutput)

}

func TestBuscarProdutos(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	mockProdRepo := mocks.NewMockIProdutoRepository(mockCtrl)
	service := services.NewProdutoService(mockProdRepo)
	baseFilters := InstanciarBaseFilter()
	produtos := InstanciarProdutos()
	totalCount := int64(5093)

	expectedOutput := &dto.GetProdutos{
		Items:      produtos,
		TotalCount: totalCount,
	}
	t.Run("ErroBuscarProdutos", func(t1 *testing.T) {
		mockProdRepo.EXPECT().
			BuscarProdutos(baseFilters).
			Return(produtos, errors.New("Test 1")).
			Times(1)
		actualOutput, err := service.BuscarProdutos(baseFilters)

		assert.Nil(t1, actualOutput)
		assert.Equal(t1, errors.New("Test 1"), err)
	})

	t.Run("ErroBuscarProdutosTotalCount", func(t2 *testing.T) {
		mockProdRepo.EXPECT().
			BuscarProdutos(baseFilters).
			Return(produtos, nil).
			Times(1)
		mockProdRepo.EXPECT().
			BuscarProdutosTotalCount(baseFilters).
			Return(int64(5093), errors.New("Test 2")).
			Times(1)
		actualOutput, err := service.BuscarProdutos(baseFilters)

		assert.Nil(t2, actualOutput)
		assert.Equal(t2, errors.New("Test 2"), err)
	})

	mockProdRepo.EXPECT().
		BuscarProdutos(baseFilters).
		Return(produtos, nil).
		Times(1)
	mockProdRepo.EXPECT().
		BuscarProdutosTotalCount(baseFilters).
		Return(int64(5093), nil).
		Times(1)
	actualOutput, err := service.BuscarProdutos(baseFilters)

	assert.Equal(t, expectedOutput, actualOutput)
	assert.Equal(t, nil, err)
}
