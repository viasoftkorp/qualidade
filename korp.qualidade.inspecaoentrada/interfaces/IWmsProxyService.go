package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
)

type IWmsProxyService interface {
	GerarNumeroLote(input dto.GerarNumeroLoteErpInput) (*dto.GerarNumeroLoteErpOutput, error)
}
