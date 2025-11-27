package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
)

//go:generate mockgen -destination=../mocks/mock_EngenhariaService.go -package=mocks bitbucket.org/viasoftkorp/korp.logistica.estoque.ajusteestoque/interfaces IEngenhariaService
type IEngenhariaService interface {
	BuscarProcesso(codigoProduto string) (*dto.ProcessoEngenhariaOutput, error)
}
