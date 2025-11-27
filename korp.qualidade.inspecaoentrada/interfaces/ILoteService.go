package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
)

type ILoteService interface {
	GerarNumeroLote(input dto.GerarNumeroLoteInput) (*dto.GerarNumeroLoteOutput, error)
}
