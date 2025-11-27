package tests

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/mocks"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/services"
	"errors"
	"github.com/golang/mock/gomock"
	"github.com/stretchr/testify/assert"
	"testing"
)

func TestBuscarParametroBool(t *testing.T) {
	mockCtrl := gomock.NewController(t)
	mockParametroRepo := mocks.NewMockIParametroRepository(mockCtrl)
	service := services.NewParametroService(mockParametroRepo)
	parametro := "Parametro teste"
	secao := "Secao teste"

	t.Run("ErroBuscarParametroBool", func(t1 *testing.T) {

		mockParametroRepo.EXPECT().
			BuscarParametroBool(parametro, secao).
			Return(true, errors.New("Test 1")).
			Times(1)
		actualOutput, err := service.BuscarParametroBool(parametro, secao)

		assert.Equal(t1, errors.New("Test 1"), err)
		assert.Equal(t1, true, actualOutput)
	})

	mockParametroRepo.EXPECT().
		BuscarParametroBool(parametro, secao).
		Return(true, nil).
		Times(1)
	actualOutput, err := service.BuscarParametroBool(parametro, secao)

	assert.Nil(t, err)
	assert.Equal(t, true, actualOutput)

}
