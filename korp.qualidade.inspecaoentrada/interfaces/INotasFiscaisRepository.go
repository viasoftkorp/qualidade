package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
)

//go:generate mockgen -destination=../mocks/mock_NotaFiscalRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces INotaFiscalRepository
type INotaFiscalRepository interface {
	BuscarNotasFiscais(filter *models.BaseFilter, filters *dto.NotaFiscalFilters) ([]models.NotaFiscalModel, error)
	BuscarNotasFiscaisTotalCount(filters *dto.NotaFiscalFilters) (int64, error)
	BuscarNotaFiscal(notaFiscal int, lote string) (models.NotaFiscalModel, error)
	BuscarNumODFePedido(codInspecao int) (*dto.InformacoesReservaDTO, error)
}
