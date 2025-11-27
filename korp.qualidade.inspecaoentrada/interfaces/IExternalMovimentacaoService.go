package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
)

//go:generate mockgen -destination=../mocks/mock_ExternalMovimentacaoService.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IExternalMovimentacaoService
type IExternalMovimentacaoService interface {
	RealizarMovimentacao(inspecao *entities.InspecaoEntrada) *dto.ValidacaoDTO
}
