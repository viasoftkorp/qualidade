package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
)

//go:generate mockgen -destination=../mocks/mock_InspecaoEntradaRepository.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IInspecaoEntradaRepository
type IInspecaoEntradaRepository interface {
	BuscarInspecaoEntradaDetalhesPeloCodigo(codigoInspecao int) (*models.InspecaoEntrada, error)
	BuscarInspecaoEntradaDetalhesPeloCodigoJoin(codigoInspecao int) ([]models.InspecaoEntradaJoin, error)
	BuscarInspecoesEntrada(notaFiscal int, lote string, filter *models.BaseFilter) ([]entities.InspecaoEntrada, error)
	BuscarQuantidadeInspecoesEntrada(recnoRateioLote int, lote string) (int64, error)
	RemoverInspecaoEntrada(recno int) error
	BuscarInspecaoEntradaPeloCodigo(codigoInspecao int) (entities.InspecaoEntrada, error)
	BuscarNovoCodigoInspecao() int
	CriarInspecao(inspecaoModel *models.InspecaoEntrada) error
	AtualizarQuantidadeInspecaoPeloCodigo(codigoInspecao int, novaQuantidade float64) error
	BuscarInspecaoEntradaPeloRecno(recno int) (*entities.InspecaoEntrada, error)
	BuscarInformacoesPreenchimentoRNC(recnoInspecao, recnoEmpresa int, codigoProduto string) (*dto.RncDetailsOutputDTO, error)
}
