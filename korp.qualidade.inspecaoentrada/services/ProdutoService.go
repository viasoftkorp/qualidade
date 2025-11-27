package services

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
)

type ProdutoService struct {
	interfaces.IProdutoService
	ProdutoRepository interfaces.IProdutoRepository
}

func NewProdutoService(produtoRepository interfaces.IProdutoRepository) interfaces.IProdutoService {
	return &ProdutoService{
		ProdutoRepository: produtoRepository,
	}
}
func (service *ProdutoService) BuscarProduto(codigo string) (*dto.ProdutoOutput, error) {
	produto, err := service.ProdutoRepository.BuscarProdutoPeloCodigo(codigo)
	if err != nil {
		return nil, err
	}

	return produto, nil
}

func (service *ProdutoService) BuscarProdutos(filter *models.BaseFilter) (*dto.GetProdutos, error) {
	produtos, err := service.ProdutoRepository.BuscarProdutos(filter)
	if err != nil {
		return nil, err
	}

	count, err := service.ProdutoRepository.BuscarProdutosTotalCount(filter)
	if err != nil {
		return nil, err
	}

	getProdutos := dto.GetProdutos{
		Items:      produtos,
		TotalCount: count,
	}

	return &getProdutos, nil
}
