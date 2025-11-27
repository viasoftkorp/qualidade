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

func InstanciarLocalOutput() *dto.LocalOutput {
	return &dto.LocalOutput{
		Codigo: "14763",
	}
}

func TestBuscarLocal(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	mockLocalRepo := mocks.NewMockILocaisRepository(mockCtrl)
	service := services.NewLocalService(mockLocalRepo)
	codigo := 14763
	expectedOutput := InstanciarLocalOutput()
	t.Run("ErroBuscarLocalPeloCodigo", func(t1 *testing.T) {
		mockLocalRepo.EXPECT().
			BuscarLocalPeloCodigo(codigo).
			Return(nil, errors.New("Test 1")).
			Times(1)
		actualOutput, err := service.BuscarLocal(codigo)
		assert.Nil(t1, actualOutput)
		assert.Equal(t1, errors.New("Test 1"), err)
	})

	mockLocalRepo.EXPECT().
		BuscarLocalPeloCodigo(codigo).
		Return(expectedOutput, nil).
		Times(1)
	actualOutput, err := service.BuscarLocal(codigo)

	assert.Nil(t, err)
	assert.Equal(t, expectedOutput, actualOutput)
}

func TestBuscarLocais(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	mockLocalRepo := mocks.NewMockILocaisRepository(mockCtrl)
	service := services.NewLocalService(mockLocalRepo)
	totalCount := int64(101)
	filter := InstanciarFilters()
	locais := []dto.LocalOutput{
		*InstanciarLocalOutput(),
	}
	t.Run("ErroBuscarLocais", func(t1 *testing.T) {
		mockLocalRepo.EXPECT().
			BuscarLocais(filter).
			Return(locais, errors.New("Test 1")).
			Times(1)
		actualOutput, err := service.BuscarLocais(filter)

		assert.Nil(t1, actualOutput)
		assert.Equal(t1, errors.New("Test 1"), err)
	})

	t.Run("ErroBuscarLocaisTotalCount", func(t2 *testing.T) {
		mockLocalRepo.EXPECT().
			BuscarLocais(filter).
			Return(locais, nil).
			Times(1)
		mockLocalRepo.EXPECT().
			BuscarLocaisTotalCount(filter).
			Return(totalCount, errors.New("Test 2")).
			Times(1)
		actualOutput, err := service.BuscarLocais(filter)

		assert.Nil(t2, actualOutput)
		assert.Equal(t2, errors.New("Test 2"), err)
	})

	expectedOutput := &dto.GetLocais{
		Items:      locais,
		TotalCount: totalCount,
	}
	mockLocalRepo.EXPECT().
		BuscarLocais(filter).
		Return(locais, nil).
		Times(1)
	mockLocalRepo.EXPECT().
		BuscarLocaisTotalCount(filter).
		Return(totalCount, nil).
		Times(1)
	actualOutput, err := service.BuscarLocais(filter)

	assert.Nil(t, err)
	assert.Equal(t, expectedOutput, actualOutput)
}
