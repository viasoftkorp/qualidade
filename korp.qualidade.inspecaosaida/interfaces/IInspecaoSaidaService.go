package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
)

//go:generate mockgen -destination=../mocks/mock_InspecaoSaidaService.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IInspecaoSaidaService
//go:generate mockgen -destination=../mocks/mock_unit_of_work.go -package=mocks bitbucket.org/viasoftkorp/korp.sdk/unit-of-work UnitOfWork
type IInspecaoSaidaService interface {
	BuscarInspecoesSaida(odf int, baseFilters *models.BaseFilter, filters *dto.InspecaoSaidaFilters) (*dto.GetInspecaoSaidaDTO, *dto.ValidacaoDTO)
	BuscarPlanosNovaInspecao(recnoProcesso int, codProduto string, filter *models.BaseFilter) (*dto.GetPlanosInspecaoDTO, *dto.ValidacaoDTO)
	CriarInspecao(input *dto.NovaInspecaoInput) (*models.InspecaoSaida, *dto.ValidacaoDTO)
	BuscarInspecaoSaidaPeloCodigo(codigoInspecao int) (*dto.InspecaoSaidaDTO, *dto.ValidacaoDTO)
	BuscarInspecaoSaidaItensPeloCodigo(codigoInspecao int, filter *models.BaseFilter) (*dto.GetInspecaoSaidaItensDTO, *dto.ValidacaoDTO)
	RemoverInspecaoSaidaPeloCodigo(codigoInspecao int) *dto.ValidacaoDTO
	AtualizarInspecao(input *dto.AtualizarInspecaoInput) *dto.ValidacaoDTO
	BuscarResultadoInspecao(codigoInspecao int) (string, *dto.ValidacaoDTO)
	ImprimirInspecaoSaida(codigoInspecao int, notaFiscal models.NotaFiscal) ([]byte, *dto.ValidacaoDTO)
	BuscarNotasRelatorio(codigoInspecao int, filter *models.BaseFilter) (models.PagedResultDto[models.NotaFiscal], error)
}
