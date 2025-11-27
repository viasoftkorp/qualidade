package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
)

//go:generate mockgen -destination=../mocks/mock_InspecaoEntradaItemRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IInspecaoEntradaItemRepository
type IInspecaoEntradaItemRepository interface {
	BuscarInspecaoEntradaItensPeloCodigo(codigoProduto string, codigoInspecao int, filter *models.BaseFilter) ([]models.InspecaoEntradaItem, error)
	BuscarQuantidadeInspecaoEntradaItensPeloCodigo(codigoInspecao int) (int64, error)
	BuscarInspecaoEntradaItensEntitiesPeloCodigo(codigoInspecao int) ([]*entities.InspecaoEntradaItem, error)
	RemoverInspecaoEntradaItensPeloCodigo(codigoInspecao int) error
	CriarItensInspecao(itens []models.InspecaoEntradaItem) error
	AtualizarInspecaoEntradaItens(itens []*entities.InspecaoEntradaItem) error
}
