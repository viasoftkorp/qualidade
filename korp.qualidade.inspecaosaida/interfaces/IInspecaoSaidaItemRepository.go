package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
)

//go:generate mockgen -destination=../mocks/mock_InspecaoSaidaItemRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IInspecaoSaidaItemRepository
type IInspecaoSaidaItemRepository interface {
	BuscarInspecaoSaidaItensPeloCodigo(codigoInspecao int, filter *models.BaseFilter) ([]*models.InspecaoSaidaItem, error)
	BuscarQuantidadeInspecaoSaidaItensPeloCodigo(codigoInspecao int) (int64, error)
	BuscarInspecaoSaidaItensEntitiesPeloCodigo(codigoInspecao int) ([]*entities.InspecaoSaidaItem, error)
	RemoverInspecaoSaidaItensPeloCodigo(codigoInspecao int) error
	CriarItensInspecao(itens []*models.InspecaoSaidaItem) error
	AtualizarInspecaoSaidaItens(itens []*entities.InspecaoSaidaItem) error
}
