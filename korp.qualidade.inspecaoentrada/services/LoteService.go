package services

import (
	"strconv"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
)

type LoteService struct {
	interfaces.ILoteService
	WmsProxyService   interfaces.IWmsProxyService
	ProdutoRepository interfaces.IProdutoRepository
}

func NewLoteService(wmsProxyService interfaces.IWmsProxyService, produtoRepository interfaces.IProdutoRepository) interfaces.ILoteService {
	return &LoteService{
		WmsProxyService:   wmsProxyService,
		ProdutoRepository: produtoRepository,
	}
}

func (service LoteService) GerarNumeroLote(input dto.GerarNumeroLoteInput) (*dto.GerarNumeroLoteOutput, error) {
	recnoFormulaLote, err := service.ProdutoRepository.GetRecnoFormulaLote(input.CodigoProduto)
	if err != nil {
		return nil, err
	}

	gerarNumeroLoteErpInput := dto.GerarNumeroLoteErpInput{
		IdFormulaLote: strconv.Itoa(*recnoFormulaLote),
		CodigoProduto: input.CodigoProduto,
	}

	gerarNumeroLoteErpOutput, err := service.WmsProxyService.GerarNumeroLote(gerarNumeroLoteErpInput)
	if err != nil {
		return nil, err
	}

	output := dto.GerarNumeroLoteOutput{
		NumeroLote: gerarNumeroLoteErpOutput.NumeroLote,
	}

	return &output, nil
}
