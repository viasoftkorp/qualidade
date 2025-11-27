package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
)

//go:generate mockgen -destination=../mocks/mock_EngenhariaRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IEngenhariaRepository
type IEngenhariaRepository interface {
	BuscarProcesso(codigoProduto string) (*dto.ProcessoEngenhariaOutput, error)
}
