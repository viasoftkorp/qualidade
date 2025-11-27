package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
)

//go:generate mockgen -destination=../mocks/mock_OrdemProducaoRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IOrdemProducaoRepository
type IOrdemProducaoRepository interface {
	BuscarOrdensInspecao(baseFilters *models.BaseFilter, filters *dto.OrdemProducaoFilters) ([]models.OrdemProducao, error)
	BuscarQuantidadeOrdensInspecao(baseFilters *models.BaseFilter, filters *dto.OrdemProducaoFilters) (int64, error)
	BuscarOrdem(odf int) *models.OrdemProducao
	BuscarOrdemPaiHistoricoMovimentacao(lote, codigoProduto string, localDestino int) (*int, error)
	BuscarEstoqueLocalRelatorio(odf int) *models.OrdemProducao
}
