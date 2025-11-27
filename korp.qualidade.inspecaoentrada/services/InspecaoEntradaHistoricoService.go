package services

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/mappers"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
)

type InspecaoEntradaHistoricoService struct {
	interfaces.IInspecaoEntradaHistoricoService
	InspecaoEntradaHistoricoRepository    interfaces.IInspecaoEntradaHistoricoRepository
	InspecaoEntradaSagaService            interfaces.IExternalInspecaoEntradaSagaService
	InspecaoEntradaExecutadoWebRepository interfaces.IInspecaoEntradaExecutadoWebRepository
	LocaisRepository                      interfaces.ILocaisRepository
}

func NewInspecaoEntradaHistoricoService(
	inspecaoEntradaHistoricoRepository interfaces.IInspecaoEntradaHistoricoRepository,
	inspecaoEntradaSagaService interfaces.IExternalInspecaoEntradaSagaService,
	inspecaoEntradaExecutadoWebRepository interfaces.IInspecaoEntradaExecutadoWebRepository,
	locaisRepository interfaces.ILocaisRepository) interfaces.IInspecaoEntradaHistoricoService {
	return &InspecaoEntradaHistoricoService{
		InspecaoEntradaHistoricoRepository:    inspecaoEntradaHistoricoRepository,
		InspecaoEntradaSagaService:            inspecaoEntradaSagaService,
		InspecaoEntradaExecutadoWebRepository: inspecaoEntradaExecutadoWebRepository,
		LocaisRepository:                      locaisRepository,
	}
}

func (service *InspecaoEntradaHistoricoService) GetAllInspecaoEntradaHistoricoCabecalho(baseFilters *models.BaseFilter, filters *dto.InspecaoEntradaHistoricoCabecalhoFilters) (*dto.GetNotasFiscaisOutput, error) {
	items, err := service.InspecaoEntradaHistoricoRepository.BuscarNotasFiscaisHistorico(baseFilters, filters)
	if err != nil {
		return nil, err
	}

	count, err := service.InspecaoEntradaHistoricoRepository.BuscarNotasFiscaisHistoricoTotalCount(baseFilters, filters)
	if err != nil {
		return nil, err
	}

	notasFiscaisDto := mappers.MapNotasModelToDTO(items)

	return &dto.GetNotasFiscaisOutput{
		Items:      notasFiscaisDto,
		TotalCount: count,
	}, nil
}

func (service *InspecaoEntradaHistoricoService) GetAllInspecaoEntradaHistoricoItems(recnoItemNotaFiscal int, notaFiscal int, lote string, baseFilters *models.BaseFilter, filters *dto.InspecaoEntradaHistoricoCabecalhoFilters) (*dto.GetAllInspecaoEntradaHistoricoItemsDTO, error) {
	items, err := service.InspecaoEntradaHistoricoRepository.BuscarInspecoesEntradaHistorico(recnoItemNotaFiscal, notaFiscal, lote, baseFilters, filters)
	if err != nil {
		return nil, err
	}

	itemsHistorico, err := service.GetItemsDto(items)
	if err != nil {
		return nil, err
	}

	count, err := service.InspecaoEntradaHistoricoRepository.BuscarQuantidadeInspecoesEntradaHistorico(recnoItemNotaFiscal, notaFiscal, lote, baseFilters, filters)
	if err != nil {
		return nil, err
	}

	return &dto.GetAllInspecaoEntradaHistoricoItemsDTO{
		Items:      itemsHistorico,
		TotalCount: count,
	}, nil
}

func (service *InspecaoEntradaHistoricoService) GetItemsDto(items []dto.InspecaoEntradaHistoricoItems) ([]dto.InspecaoEntradaHistoricoItemsDTO, error) {
	itemsHistorico := make([]dto.InspecaoEntradaHistoricoItemsDTO, 0)
	localDescricaoMapping := make(map[int]string)
	for i := 0; i < len(items); i++ {
		item := items[i]

		inspecaoEntradaExecutadoWeb, err := service.InspecaoEntradaExecutadoWebRepository.BuscarInspecaoEntradaExecutadoWeb(item.RecnoInspecao)
		if err != nil {
			return nil, err
		}

		saga, err := service.InspecaoEntradaSagaService.BuscarSaga(inspecaoEntradaExecutadoWeb.IdInspecaoEntradaSaga)
		if err != nil {
			return nil, err
		}

		transferencias := make([]dto.HistoricoInspecaoEntradaTransferenciaOutput, 0)
		for _, transferencia := range saga.Transferencias {
			descricaoOrigem, containsLocal := localDescricaoMapping[transferencia.LocalOrigem]
			if !containsLocal {
				descricaoOrigem, err = service.LocaisRepository.BuscarLocalDescricao(transferencia.LocalOrigem)
				if err != nil {
					return nil, err
				}

				localDescricaoMapping[transferencia.LocalOrigem] = descricaoOrigem
			}

			descricaoDestino, containsLocal := localDescricaoMapping[transferencia.LocalDestino]
			if !containsLocal {
				descricaoDestino, err = service.LocaisRepository.BuscarLocalDescricao(transferencia.LocalDestino)
				if err != nil {
					return nil, err
				}

				localDescricaoMapping[transferencia.LocalDestino] = descricaoDestino
			}

			transferencias = append(transferencias, dto.HistoricoInspecaoEntradaTransferenciaOutput{
				NotaFiscal:            item.NotaFiscal,
				Quantidade:            transferencia.Quantidade,
				NumeroPedido:          transferencia.NumeroPedido,
				LocalOrigem:           transferencia.LocalOrigem,
				DescricaoLocalOrigem:  descricaoOrigem,
				LocalDestino:          transferencia.LocalDestino,
				DescricaoLocalDestino: descricaoDestino,
				TipoTransferencia:     transferencia.TipoTransferencia,
				OrdemFabricacao:       transferencia.OrdemFabricacao,
				Lote:                  transferencia.Lote,
			})
		}

		itemsHistorico = append(itemsHistorico, dto.InspecaoEntradaHistoricoItemsDTO{
			RecnoInspecao:       item.RecnoInspecao,
			CodigoInspecao:      item.CodigoInspecao,
			NotaFiscal:          item.NotaFiscal,
			CodigoProduto:       item.CodigoProduto,
			DescricaoProduto:    item.CodigoProduto + " - " + item.DescricaoProduto,
			QuantidadeInspecao:  item.QuantidadeInspecao,
			QuantidadeAprovada:  item.QuantidadeAprovada,
			QuantidadeReprovada: item.QuantidadeReprovada,
			Inspetor:            item.Inspetor,
			Resultado:           item.Resultado,
			DataInspecao:        utils.SetDateTimeZone(item.DataInspecao),
			Transferencias:      transferencias,
			IdRnc:               item.IdRnc,
		})
	}

	return itemsHistorico, nil
}
