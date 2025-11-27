package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
)

//go:generate mockgen -destination=../mocks/mock_InspecaoEntradaService.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IInspecaoEntradaService
type IInspecaoEntradaService interface {
	BuscarInspecoesEntrada(notaFiscal int, lote string, filter *models.BaseFilter) (*dto.GetInspecaoEntradaDTO, *dto.ValidacaoDTO, error)
	BuscarPlanosNovaInspecao(plano int, codigoProduto string, filter *models.BaseFilter) (*dto.GetPlanosInspecaoDTO, *dto.ValidacaoDTO, error)
	CriarInspecao(input *dto.NovaInspecaoInput) (int, *dto.ValidacaoDTO, error)
	BuscarInspecaoEntradaPeloCodigo(codigoInspecao int) (dto.InspecaoEntradaDTO, *dto.ValidacaoDTO, error)
	BuscarInspecaoEntradaItensPeloCodigo(codigoProduto string, codigoInspecao int, filter *models.BaseFilter) (*dto.GetInspecaoEntradaItensDTO, *dto.ValidacaoDTO, error)
	RemoverInspecaoEntradaPeloCodigo(codigoInspecao int) (*dto.ValidacaoDTO, error)
	AtualizarInspecao(input *dto.AtualizarInspecaoInput) (*dto.ValidacaoDTO, error)
	BuscarResultadoInspecao(codigoInspecao int) (string, *dto.ValidacaoDTO, error)
	BuscarValorParametro(chaveParametro, secao string) (bool, error)
}
