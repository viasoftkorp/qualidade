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
	"time"
)

type InspecaoEntradaRepository struct {
	interfaces.IInspecaoEntradaRepository
	Uow unit_of_work.UnitOfWork
}

func NewInspecaoEntradaRepository(uow unit_of_work.UnitOfWork) (
	interfaces.IInspecaoEntradaRepository, error) {
	return &InspecaoEntradaRepository{
		Uow: uow,
	}, nil
}

func (repo *InspecaoEntradaRepository) BuscarInspecoesEntrada(notaFiscal int, lote string, filter *models.BaseFilter) ([]entities.InspecaoEntrada, error) {
	var result []entities.InspecaoEntrada

	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()
	res := repo.Uow.GetDb().WithContext(ctx).Table(entities.InspecaoEntrada{}.TableName()).Where(entities.InspecaoEntrada{
		NotaFiscal: notaFiscal,
		Lote:       lote,
	}).
		Where("R_E_C_N_O_ NOT IN (SELECT DISTINCT RECNO_INSPECAO_ENTRADA FROM InspecaoEntradaExecutadoWeb)").
		Limit(filter.PageSize).
		Offset(filter.Skip).
		Scan(&result)

	return result, res.Error
}

func (repo *InspecaoEntradaRepository) BuscarQuantidadeInspecoesEntrada(notaFiscal int, lote string) (int64, error) {
	var result int64
	ctx, cancel := context.WithTimeout(context.Background(), 30*time.Second)
	defer cancel()
	res := repo.Uow.GetDb().WithContext(ctx).Table(entities.InspecaoEntrada{}.TableName()).Where(entities.InspecaoEntrada{
		NotaFiscal: notaFiscal,
		Lote:       lote,
	}).
		Count(&result)

	return result, res.Error
}

func (repo *InspecaoEntradaRepository) BuscarInspecaoEntradaDetalhesPeloCodigo(codigoInspecao int) (*models.InspecaoEntrada, error) {
	var result *models.InspecaoEntrada

	res := repo.Uow.GetDb().
		Raw(queries.GetInspecaoEntradaCodigo,
			sql.Named(queries.NamedCodigoInspecaoEntrada, codigoInspecao)).
		Find(&result)

	return result, res.Error
}

func (repo *InspecaoEntradaRepository) BuscarInspecaoEntradaDetalhesPeloCodigoJoin(codigoInspecao int) ([]models.InspecaoEntradaJoin, error) {
	var result []models.InspecaoEntradaJoin

	res := repo.Uow.GetDb().
		Raw(queries.GetInspecaoEntradaCodigoJoin,
			sql.Named(queries.NamedCodigoInspecaoEntrada, codigoInspecao)).
		Find(&result)

	return result, res.Error
}

func (repo *InspecaoEntradaRepository) BuscarInspecaoEntradaPeloRecno(recno int) (*entities.InspecaoEntrada, error) {
	var result *entities.InspecaoEntrada

	res := repo.Uow.GetDb().Where(entities.InspecaoEntrada{
		Recno: recno,
	}).Find(&result)

	return result, res.Error
}

func (repo *InspecaoEntradaRepository) BuscarInspecaoEntradaPeloCodigo(codigoInspecao int) (entities.InspecaoEntrada, error) {
	var result entities.InspecaoEntrada

	res := repo.Uow.GetDb().Where(entities.InspecaoEntrada{
		CodigoInspecao: codigoInspecao,
	}).Find(&result)

	return result, res.Error
}

func (repo *InspecaoEntradaRepository) RemoverInspecaoEntrada(recno int) error {
	res := repo.Uow.GetDb().
		Where(&entities.InspecaoEntrada{
			Recno: recno,
		}).
		Delete(&entities.InspecaoEntrada{})

	repo.Uow.GetDb().
		Where(&entities.InspecaoEntradaPedidoVenda{
			RecnoInspecaoEntrada: recno,
		}).
		Delete(&entities.InspecaoEntradaPedidoVenda{})

	return res.Error
}

func (repo *InspecaoEntradaRepository) BuscarNovoCodigoInspecao() int {
	codigoInspecao := 0

	repo.Uow.GetDb().
		Table(entities.InspecaoEntrada{}.TableName()).
		Select("MAX(COD_INSP)").
		Find(&codigoInspecao)

	return codigoInspecao + 1
}

func (repo *InspecaoEntradaRepository) CriarInspecao(inspecaoModel *models.InspecaoEntrada) error {
	entity := &entities.InspecaoEntrada{
		Id:                 uuid.New(),
		Lote:               inspecaoModel.Lote,
		NotaFiscal:         inspecaoModel.NotaFiscal,
		CodigoInspecao:     inspecaoModel.CodigoInspecao,
		DataInspecao:       inspecaoModel.DataInspecao,
		Inspetor:           inspecaoModel.Inspetor,
		QuantidadeInspecao: inspecaoModel.QuantidadeInspecao,
		QuantidadeLote:     inspecaoModel.QuantidadeLote,
	}

	res := repo.Uow.GetDb().Create(&entity)

	return res.Error
}

func (repo *InspecaoEntradaRepository) AtualizarQuantidadeInspecaoPeloCodigo(codigoInspecao int, novaQuantidade float64) error {
	res := repo.Uow.GetDb().
		Table(entities.InspecaoEntrada{}.TableName()).
		Where("COD_INSP = ?", codigoInspecao).
		Update("QTD_INSPECAO", novaQuantidade)

	return res.Error
}

type RncDetailsNota struct {
	IdNotaFiscal     string
	CodigoFornecedor string
	NumeroNota       string
}

type RncDetailsEstoqueLocal struct {
	DataFabricacao string
	SaldoLote      float64
}

func (repo *InspecaoEntradaRepository) BuscarInformacoesPreenchimentoRNC(recnoInspecao, recnoEmpresa int, codigoProduto string) (*dto.RncDetailsOutputDTO, error) {
	output := dto.RncDetailsOutputDTO{}

	insp, err := repo.BuscarInspecaoEntradaPeloRecno(recnoInspecao)
	if err != nil {
		return nil, err
	}

	var detailsNota RncDetailsNota
	res := repo.Uow.GetDb().Raw(`SELECT cast(Id as varchar(36)) AS IdNotaFiscal, NFISCAL AS NumeroNota, CODFOR AS CodigoFornecedor FROM HISTLISE_FOR WHERE NFISCAL = ? AND EMPRESA_RECNO = ?`, insp.NotaFiscal, recnoEmpresa).First(&detailsNota)
	if res.Error != nil {
		return nil, res.Error
	}

	var idFornecedor string
	res = repo.Uow.GetDb().Raw(`SELECT cast(Id as varchar(36)) AS idFornecedor FROM FORNECED WHERE CODIGO = ?`, detailsNota.CodigoFornecedor).First(&idFornecedor)
	if res.Error != nil {
		return nil, res.Error
	}

	var idProduto string
	res = repo.Uow.GetDb().Raw(`SELECT cast(ESTOQUE.ID as varchar(36)) AS IdProduto FROM ESTOQUE WHERE ESTOQUE.CODIGO = ?`, codigoProduto).Scan(&idProduto)
	if res.Error != nil {
		return nil, res.Error
	}

	var detailsEstoqueLocal RncDetailsEstoqueLocal
	res = repo.Uow.GetDb().Raw(`
		SELECT 
			ESTOQUE_LOCAL.DTFABRICACAO AS DataFabricacao, 
			ESTOQUE_LOCAL.QTDE AS SaldoLote 
		FROM ESTOQUE_LOCAL
		INNER JOIN LOCAIS ON LOCAIS.CODIGO = ESTOQUE_LOCAL.LOCAL AND LOCAIS.EMPRESA_RECNO = ESTOQUE_LOCAL.EMPRESA_RECNO
		WHERE ESTOQUE_LOCAL.CODIGO = ? AND ESTOQUE_LOCAL.LOTE = ? AND ESTOQUE_LOCAL.EMPRESA_RECNO = ? AND LOCAIS.LOCAL_CQ_ENTRADA = 'S'
	`, codigoProduto, insp.Lote, recnoEmpresa).Scan(&detailsEstoqueLocal)
	if res.Error != nil {
		return nil, res.Error
	}

	output.IdNotaFiscal = detailsNota.IdNotaFiscal
	output.NumeroNota = detailsNota.NumeroNota
	output.NumeroLote = insp.Lote
	output.IdFornecedor = idFornecedor
	output.IdProduto = idProduto
	if len(detailsEstoqueLocal.DataFabricacao) > 0 {
		dataFabricacaoDate, err := time.Parse("20060102", detailsEstoqueLocal.DataFabricacao)
		if err != nil {
			return nil, err
		}
		output.DataFabricacaoLote = &dataFabricacaoDate
	}
	output.QuantidadeTotalLote = detailsEstoqueLocal.SaldoLote

	return &output, nil
}
