package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
)

//go:generate mockgen -destination=../mocks/mock_NotaFiscalService.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces INotaFiscalService
type INotaFiscalService interface {
	BuscarNotasFiscais(filter *models.BaseFilter, filters *dto.NotaFiscalFilters) (*dto.GetNotasFiscaisOutput, *dto.ValidacaoDTO, error)
	UpdateNotaFiscalDadosAdicionais(idNotaFiscal string, input *dto.NotaFiscalDadosAdicionaisDTO) error
}
