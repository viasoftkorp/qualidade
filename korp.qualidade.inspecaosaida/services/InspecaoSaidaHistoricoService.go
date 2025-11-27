package services

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
)

type InspecaoSaidaHistoricoService struct {
	interfaces.IInspecaoSaidaHistoricoService
	Uow                                 unit_of_work.UnitOfWork
	InspecaoSaidaHistoricoRepository    interfaces.IInspecaoSaidaHistoricoRepository
	BaseParams                          *models.BaseParams
	InspecaoSaidaSagaService            interfaces.IExternalInspecaoSaidaSagaService
	InspecaoSaidaExecutadoWebRepository interfaces.IInspecaoSaidaExecutadoWebRepository
	LocaisRepository                    interfaces.ILocaisRepository
}

func NewInspecaoSaidaHistoricoService(
	uow unit_of_work.UnitOfWork,
	inspecaoSaidaHistoricoRepository interfaces.IInspecaoSaidaHistoricoRepository,
	baseParams *models.BaseParams,
	inspecaoSaidaSagaService interfaces.IExternalInspecaoSaidaSagaService,
	inspecaoSaidaExecutadoWebRepository interfaces.IInspecaoSaidaExecutadoWebRepository,
	locaisRepository interfaces.ILocaisRepository) interfaces.IInspecaoSaidaHistoricoService {
	return &InspecaoSaidaHistoricoService{
		Uow:                                 uow,
		InspecaoSaidaHistoricoRepository:    inspecaoSaidaHistoricoRepository,
		BaseParams:                          baseParams,
		InspecaoSaidaSagaService:            inspecaoSaidaSagaService,
		InspecaoSaidaExecutadoWebRepository: inspecaoSaidaExecutadoWebRepository,
		LocaisRepository:                    locaisRepository,
	}
}

func (service *InspecaoSaidaHistoricoService) GetAllInspecaoSaidaHistoricoCabecalho(baseFilters *models.BaseFilter, filters *dto.InspecaoSaidaHistoricoCabecalhoFilters) (*dto.GetAllInspecaoSaidaHistoricoCabecalhoDTO, error) {
	items, err := service.InspecaoSaidaHistoricoRepository.GetAllInspecaoSaidaHistoricoCabecalho(baseFilters, filters)
	if err != nil {
		return nil, err
	}

	count, err := service.InspecaoSaidaHistoricoRepository.CountInspecaoSaidaHistoricoCabecalho(baseFilters, filters)
	if err != nil {
		return nil, err
	}

	return &dto.GetAllInspecaoSaidaHistoricoCabecalhoDTO{
		Items:      items,
		TotalCount: count,
	}, nil
}

func (service *InspecaoSaidaHistoricoService) GetAllInspecaoSaidaHistoricoItems(baseFilters *models.BaseFilter, filters *dto.InspecaoSaidaHistoricoCabecalhoFilters, odf int, codigoInspecao int) (*dto.GetAllInspecaoSaidaHistoricoItemsDTO, error) {
	items, err := service.InspecaoSaidaHistoricoRepository.GetAllInspecaoSaidaHistoricoItems(baseFilters, filters, odf, codigoInspecao)
	if err != nil {
		return nil, err
	}

	itemsHistorico, err := service.GetItemsDto(items)
	if err != nil {
		return nil, err
	}

	count, err := service.InspecaoSaidaHistoricoRepository.CountInspecaoSaidaHistoricoItems(baseFilters, filters, odf, codigoInspecao)
	if err != nil {
		return nil, err
	}

	return &dto.GetAllInspecaoSaidaHistoricoItemsDTO{
		Items:      itemsHistorico,
		TotalCount: count,
	}, nil
}

func (service *InspecaoSaidaHistoricoService) GetItemsDto(items []dto.InspecaoSaidaHistoricoItems) ([]dto.InspecaoSaidaHistoricoItemsDTO, error) {
	itemsHistorico := make([]dto.InspecaoSaidaHistoricoItemsDTO, 0)
	localDescricaoMapping := make(map[int]string)

	for i := 0; i < len(items); i++ {
		item := items[i]

		inspecaoSaidaExecutadoWeb, err := service.InspecaoSaidaExecutadoWebRepository.BuscarInspecaoSaidaExecutadoWeb(item.RecnoInspecao)
		if err != nil {
			return nil, err
		}

		saga, err := service.InspecaoSaidaSagaService.BuscarSaga(inspecaoSaidaExecutadoWeb.IdInspecaoSaidaSaga)
		if err != nil {
			return nil, err
		}

		transferencias := make([]dto.HistoricoInspecaoSaidaTransferenciaOutput, 0)
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

			transferencias = append(transferencias, dto.HistoricoInspecaoSaidaTransferenciaOutput{
				OrdemFabricacao:       item.OrdemFabricacao,
				OdfApontada:           item.OdfApontada,
				Quantidade:            transferencia.Quantidade,
				NumeroPedido:          transferencia.NumeroPedido,
				LocalOrigem:           transferencia.LocalOrigem,
				DescricaoLocalOrigem:  descricaoOrigem,
				LocalDestino:          transferencia.LocalDestino,
				DescricaoLocalDestino: descricaoDestino,
				TipoTransferencia:     transferencia.TipoTransferencia,
			})
		}

		itemHistorico := dto.InspecaoSaidaHistoricoItemsDTO{
			IdInspecao:             item.IdInspecao,
			CodigoInspecao:         item.CodigoInspecao,
			OdfApontada:            item.OdfApontada,
			RecnoInspecao:          item.RecnoInspecao,
			OrdemFabricacao:        item.OrdemFabricacao,
			CodigoProduto:          item.CodigoProduto,
			DescricaoProduto:       item.CodigoProduto + " - " + item.DescricaoProduto,
			QuantidadeInspecao:     item.QuantidadeInspecao,
			QuantidadeRetrabalhada: item.QuantidadeRetrabalhada,
			QuantidadeAprovada:     item.QuantidadeAprovada,
			QuantidadeReprovada:    item.QuantidadeReprovada,
			Inspetor:               item.Inspetor,
			TipoInspecao:           item.TipoInspecao,
			Resultado:              item.Resultado,
			DataInspecao:           utils.StringToTime(item.DataInspecao),
			NumeroPedido:           item.NumeroPedido,
			Cliente:                item.Cliente,
			Transferencias:         transferencias,
			CodigoRnc:              item.CodigoRnc,
			IdRnc:                  item.IdRnc,
		}

		if saga.OrdemRetrabalho != nil {
			itemHistorico.OdfRetrabalho = saga.OrdemRetrabalho.OrdemFabricacao
		}

		itemsHistorico = append(itemsHistorico, itemHistorico)
	}

	return itemsHistorico, nil
}
