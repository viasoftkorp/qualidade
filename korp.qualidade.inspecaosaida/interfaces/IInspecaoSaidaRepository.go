package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
)

//go:generate mockgen -destination=../mocks/mock_InspecaoSaidaRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces IInspecaoSaidaRepository
type IInspecaoSaidaRepository interface {
	BuscarInspecaoSaidaDetalhesPeloCodigo(codigoInspecao int) (*models.InspecaoSaida, error)
	BuscarInspecoesSaida(odf int, filter *models.BaseFilter) ([]*entities.InspecaoSaida, error)
	BuscarQuantidadeInspecoesSaida(odf int) (int64, error)
	BuscarInspecaoSaidaPeloCodigo(codigoInspecao int) (*entities.InspecaoSaida, error)
	RemoverInspecaoSaida(recno int) error
	BuscarNovoCodigoInspecao() int
	CriarInspecao(inspecaoModel *models.InspecaoSaida) error
	AtualizarQuantidadeInspecaoPeloCodigo(codigoInspecao int, novaQuantidade float64) error
	BuscarInspecaoSaidaPeloRecno(recno int) (*entities.InspecaoSaida, error)
	BuscarInformacoesPreenchimentoRNC(recnoInspecao int) (*dto.RncDetailsOutputDTO, error)
	PreencherInformacoesMaterialRetrabalho(rnc dto.RncInputDTO, ordemRetrabalho *dto.InspecaoSaidaOrdemRetrabalhoBackgroundDto) error
	PreencherInformacoesMaquinaRetrabalho(rnc dto.RncInputDTO, ordemRetrabalho *dto.InspecaoSaidaOrdemRetrabalhoBackgroundDto) error
}
