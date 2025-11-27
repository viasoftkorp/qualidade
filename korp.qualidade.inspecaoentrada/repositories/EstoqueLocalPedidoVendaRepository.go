package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/queries"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"context"
	"database/sql"
	"github.com/google/uuid"
	"github.com/shopspring/decimal"
	"time"
)

type EstoqueLocalPedidoVendaRepository struct {
	interfaces.IEstoquePedidoVendaRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewEstoqueLocalPedidoVendaRepository(uow unit_of_work.UnitOfWork, params *models.BaseParams) (interfaces.IEstoquePedidoVendaRepository, error) {
	return &EstoqueLocalPedidoVendaRepository{
		Uow:        uow,
		BaseParams: params,
	}, nil
}

func (repo *EstoqueLocalPedidoVendaRepository) BuscarAlocacaoEstoquePedidoVenda(id uuid.UUID) (entities.InspecaoEntradaPedidoVenda, error) {
	var result entities.InspecaoEntradaPedidoVenda

	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	updateQueryResult := repo.Uow.GetDb().WithContext(ctx).
		Table(entities.InspecaoEntradaPedidoVenda{}.TableName()).
		Where(&entities.InspecaoEntradaPedidoVenda{Id: id}).
		Scan(&result)

	return result, updateQueryResult.Error
}

func (repo *EstoqueLocalPedidoVendaRepository) BuscarQuantidadeTotalAlocadaPedidoVenda(recnoInspecaoEntrada int) (*dto.EstoqueLocalPedidoVendoTotalizacaoDTO, error) {
	var output dto.EstoqueLocalPedidoVendoTotalizacaoDTO

	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	queryResult := repo.Uow.GetDb().WithContext(ctx).Raw(queries.GetQuantidadeTotalAlocacaoPedidoVendaLocalInspecao,
		sql.Named(queries.NamedRecnoInspecaoEntrada, recnoInspecaoEntrada)).
		Scan(&output)

	if queryResult.Error != nil {
		return nil, queryResult.Error
	}

	return &output, nil
}

func (repo *EstoqueLocalPedidoVendaRepository) BuscarEstoqueLocalPedidosVendas(filter *models.BaseFilter, recnoInspecaoEntrada int, lote, codigoProduto string) ([]dto.EstoqueLocalPedidoVendaAlocacaoDTO, error) {
	var items []dto.EstoqueLocalPedidoVendaAlocacaoDTO

	queryItem := queries.GetAlocacaoPedidoVendaLocalInspecao
	queryItem += queries.GetAlocacaoPedidoVendaLocalInspecaoPagination

	var args = models.QueryFilter{
		Skip:     filter.Skip,
		PageSize: filter.PageSize,
	}

	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	itemsQueryResult := repo.Uow.GetDb().WithContext(ctx).Raw(queryItem,
		sql.Named(queries.NamedLote, lote),
		sql.Named(queries.NamedProdutoCodigo, codigoProduto),
		sql.Named(queries.NamedRecnoInspecaoEntrada, recnoInspecaoEntrada),
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno),
		args).
		Scan(&items)

	if itemsQueryResult.Error != nil {
		return nil, itemsQueryResult.Error
	}

	return items, nil
}

func (repo *EstoqueLocalPedidoVendaRepository) BuscarTotalCountEstoqueLocalPedidosVendas(recnoInspecaoEntrada int, lote, codigoProduto string) (int64, error) {
	var totalCount int64

	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	countQueryResult := repo.Uow.GetDb().WithContext(ctx).Raw(queries.GetTotalCountAlocacaoPedidoVendaLocalInspecao,
		sql.Named(queries.NamedLote, lote),
		sql.Named(queries.NamedProdutoCodigo, codigoProduto),
		sql.Named(queries.NamedRecnoInspecaoEntrada, recnoInspecaoEntrada),
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno)).
		Scan(&totalCount)

	if countQueryResult.Error != nil {
		return 0, countQueryResult.Error
	}

	return totalCount, nil
}

func (repo *EstoqueLocalPedidoVendaRepository) SeedEstoqueLocalPedidosVendasInspecaoValues(localAprovado, localReprovado, recnoInspecao int, lote, codigoProduto string) error {
	var RecnoEstoquePedidoVenda []int

	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	valuesToSeeQueryResult := repo.Uow.GetDb().WithContext(ctx).Raw(queries.GetSeedValuesAlocacaoPedidoVendaLocalInspecao,
		sql.Named(queries.NamedLote, lote),
		sql.Named(queries.NamedProdutoCodigo, codigoProduto),
		sql.Named(queries.NamedRecnoInspecaoEntrada, recnoInspecao),
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno)).
		Scan(&RecnoEstoquePedidoVenda)

	if valuesToSeeQueryResult.Error != nil {
		return valuesToSeeQueryResult.Error
	}

	if len(RecnoEstoquePedidoVenda) != 0 {
		entitiesToInsert := make([]entities.InspecaoEntradaPedidoVenda, 0)
		for _, recno := range RecnoEstoquePedidoVenda {
			defaultQuantidade := decimal.NewFromFloat(0)
			entitiesToInsert = append(entitiesToInsert, entities.InspecaoEntradaPedidoVenda{
				Id:                      uuid.New(),
				RecnoEstoquePedidoVenda: recno,
				RecnoInspecaoEntrada:    recnoInspecao,
				QuantidadeAprovar:       &defaultQuantidade,
				QuantidadeReprovar:      &defaultQuantidade,
				CodigoLocalAprovar:      &localAprovado,
				CodigoLocalReprovar:     &localReprovado,
			})
		}

		if len(entitiesToInsert) > 0 {
			err := repo.Uow.GetDb().Where("RECNO_INSPECAO_ENTRADA = ?", recnoInspecao).
				Delete(&entities.InspecaoEntradaPedidoVenda{}).Error
			if err != nil {
				return err
			}
			createQueryResult := repo.Uow.GetDb().Create(entitiesToInsert)
			if createQueryResult.Error != nil {
				return createQueryResult.Error
			}
		}
	}

	return nil
}

func (repo *EstoqueLocalPedidoVendaRepository) AtualizarDistribuicaoInspecaoEstoquePedidoVenda(id uuid.UUID, input dto.EstoqueLocalPedidoVendaAlocacaoDTO) error {
	quantidadeAprovadaDecimal := decimal.NewFromFloat(input.QuantidadeAprovada)
	quantidadeReprovadaDecimal := decimal.NewFromFloat(input.QuantidadeReprovada)

	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	updateQueryResult := repo.Uow.GetDb().WithContext(ctx).
		Where(&entities.InspecaoEntradaPedidoVenda{Id: id}).
		Updates(&entities.InspecaoEntradaPedidoVenda{
			CodigoLocalReprovar: &input.CodigoLocalReprovado,
			CodigoLocalAprovar:  &input.CodigoLocalAprovado,
			QuantidadeAprovar:   &quantidadeAprovadaDecimal,
			QuantidadeReprovar:  &quantidadeReprovadaDecimal,
		})

	return updateQueryResult.Error
}

func (repo *EstoqueLocalPedidoVendaRepository) RemoverInspecaoEntradaPedidoVenda(recnoInspecao int) error {
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()

	result := repo.Uow.GetDb().WithContext(ctx).
		Where(&entities.InspecaoEntradaPedidoVenda{
			RecnoInspecaoEntrada: recnoInspecao,
		}).
		Delete(&entities.InspecaoEntradaPedidoVenda{})

	return result.Error
}

func (repo *EstoqueLocalPedidoVendaRepository) BuscarEstoqueLocalValoresPorProduto(codigoProduto string, lote string, codigoLocal int) (*models.EstoqueLocalValores, error) {
	var estoqueLocalValores *models.EstoqueLocalValores

	itemsQueryResult := repo.Uow.GetDb().Raw(queries.BuscarEstoqueLocalValoresPorProduto,
		sql.Named(queries.NamedProdutoCodigo, codigoProduto),
		sql.Named(queries.NamedLote, lote),
		sql.Named(queries.NamedLocal, codigoLocal),
		sql.Named(queries.NamedEmpresaRecno, repo.BaseParams.CompanyRecno)).
		Scan(&estoqueLocalValores)

	if itemsQueryResult.Error != nil {
		return nil, itemsQueryResult.Error
	}

	return estoqueLocalValores, nil
}

func (repo *EstoqueLocalPedidoVendaRepository) BuscarPacotes(recnoEstoqueLocal int) ([]models.Pacote, error) {
	pacotes := make([]models.Pacote, 0)

	itemsQueryResult := repo.Uow.GetDb().Raw(queries.BuscarPacotes,
		sql.Named(queries.NamedLocal, recnoEstoqueLocal)).
		Scan(&pacotes)

	if itemsQueryResult.Error != nil {
		return nil, itemsQueryResult.Error
	}

	return pacotes, nil
}

func (repo *EstoqueLocalPedidoVendaRepository) BuscarSeries(recnoEstoqueLocal int) ([]models.Serie, error) {
	series := make([]models.Serie, 0)

	itemsQueryResult := repo.Uow.GetDb().Raw(queries.BuscarSeries,
		sql.Named(queries.NamedLocal, recnoEstoqueLocal)).
		Scan(&series)

	if itemsQueryResult.Error != nil {
		return nil, itemsQueryResult.Error
	}

	return series, nil
}
