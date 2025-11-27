package repositories

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/queries"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"database/sql"
	"errors"
	"github.com/google/uuid"
	"gorm.io/gorm"
	"strconv"
	"time"
)

type InspecaoSaidaRepository struct {
	interfaces.IInspecaoSaidaRepository
	Uow        unit_of_work.UnitOfWork
	BaseParams *models.BaseParams
}

func NewInspecaoSaidaRepository(uow unit_of_work.UnitOfWork, params *models.BaseParams) (
	interfaces.IInspecaoSaidaRepository, error) {
	return &InspecaoSaidaRepository{
		Uow:        uow,
		BaseParams: params,
	}, nil
}

func (repo *InspecaoSaidaRepository) BuscarInspecoesSaida(odf int, filter *models.BaseFilter) ([]*entities.InspecaoSaida, error) {
	var result []*entities.InspecaoSaida

	res := repo.Uow.GetDb().Table(entities.InspecaoSaida{}.TableName()).Where(entities.InspecaoSaida{
		IdEmpresa: repo.BaseParams.CompanyRecno,
		Odf:       odf,
	}).
		Where("R_E_C_N_O_ NOT IN (SELECT DISTINCT RECNO_INSPECAO_SAIDA FROM InspecaoSaidaExecutadoWeb)").
		Where("Inspecionado != 'S'").
		Limit(filter.PageSize).
		Offset(filter.Skip).
		Scan(&result)

	return result, res.Error
}

func (repo *InspecaoSaidaRepository) BuscarQuantidadeInspecoesSaida(odf int) (int64, error) {
	var result int64

	res := repo.Uow.GetDb().Table(entities.InspecaoSaida{}.TableName()).Where(entities.InspecaoSaida{
		IdEmpresa: repo.BaseParams.CompanyRecno,
		Odf:       odf,
	}).
		Count(&result)

	return result, res.Error
}

func (repo *InspecaoSaidaRepository) BuscarInspecaoSaidaDetalhesPeloCodigo(codigoInspecao int) (*models.InspecaoSaida, error) {
	var result *models.InspecaoSaida

	res := repo.Uow.GetDb().
		Raw(queries.GetInspecaoSaidaCodigo,
			sql.Named(queries.NamedCodigoInspecaoSaida, codigoInspecao)).
		Find(&result)

	return result, res.Error
}

func (repo *InspecaoSaidaRepository) BuscarInspecaoSaidaPeloCodigo(codigoInspecao int) (*entities.InspecaoSaida, error) {
	var result *entities.InspecaoSaida

	res := repo.Uow.GetDb().Where(entities.InspecaoSaida{
		CodigoInspecao: codigoInspecao,
	}).Find(&result)

	return result, res.Error
}

func (repo *InspecaoSaidaRepository) BuscarInspecaoSaidaPeloRecno(recno int) (*entities.InspecaoSaida, error) {
	var result *entities.InspecaoSaida

	res := repo.Uow.GetDb().Where(entities.InspecaoSaida{
		Recno: recno,
	}).Find(&result)

	return result, res.Error
}

func (repo *InspecaoSaidaRepository) RemoverInspecaoSaida(recno int) error {
	res := repo.Uow.GetDb().
		Where(&entities.InspecaoSaida{
			Recno: recno,
		}).
		Delete(&entities.InspecaoSaida{})

	return res.Error
}

func (repo *InspecaoSaidaRepository) BuscarNovoCodigoInspecao() int {
	codigoInspecao := 0

	repo.Uow.GetDb().
		Table(entities.InspecaoSaida{}.TableName()).
		Select("MAX(CODINSP)").
		Find(&codigoInspecao)

	return codigoInspecao + 1
}

func (repo *InspecaoSaidaRepository) CriarInspecao(inspecaoModel *models.InspecaoSaida) error {
	entity := &entities.InspecaoSaida{
		Id:                 uuid.New(),
		CodigoInspecao:     inspecaoModel.CodigoInspecao,
		Odf:                inspecaoModel.Odf,
		Pedido:             inspecaoModel.Pedido,
		IsoTs:              "F",
		DataInspecao:       inspecaoModel.DataInspecao,
		TipoInspecao:       "Inspeção Final",
		Inspetor:           inspecaoModel.Inspetor,
		QuantidadeInspecao: inspecaoModel.QtdInspecao,
		QuantidadeLote:     inspecaoModel.QtdLote,
		IdEmpresa:          inspecaoModel.IdEmpresa,
		Cliente:            inspecaoModel.Cliente,
		Lote:               inspecaoModel.Lote,
		CodigoProduto:      &inspecaoModel.CodigoProduto,
	}

	res := repo.Uow.GetDb().Create(&entity)

	return res.Error
}

func (repo *InspecaoSaidaRepository) AtualizarQuantidadeInspecaoPeloCodigo(codigoInspecao int, novaQuantidade float64) error {
	res := repo.Uow.GetDb().
		Table(entities.InspecaoSaida{}.TableName()).
		Where("CODINSP = ?", codigoInspecao).
		Update("QTD_INSPECAO", novaQuantidade)

	return res.Error
}

type RncDetailsEstoque struct {
	Revisao    string
	IdProduto  string
	IdPpedlise string
}

type RncDetailsEstoqueLocal struct {
	DataFabricacao string
	SaldoLote      float64
	CodigoLocal    int
}

func (repo *InspecaoSaidaRepository) BuscarInformacoesPreenchimentoRNC(recnoInspecao int) (*dto.RncDetailsOutputDTO, error) {
	output := dto.RncDetailsOutputDTO{}

	insp, err := repo.BuscarInspecaoSaidaPeloRecno(recnoInspecao)
	if err != nil {
		return nil, err
	}

	var idCliente string
	res := repo.Uow.GetDb().Raw(`SELECT cast(Id as varchar(36)) AS idCliente FROM CLIENTES WHERE CODIGO = ?`, insp.Cliente).First(&idCliente)
	if res.Error != nil {
		if errors.Is(res.Error, gorm.ErrRecordNotFound) {
			output.IdCliente = nil
		} else {
			return nil, res.Error
		}
	} else {
		output.IdCliente = &idCliente
	}

	var detailsEstoqueLocal RncDetailsEstoqueLocal
	res = repo.Uow.GetDb().Raw(`
		SELECT 
			ESTOQUE_LOCAL.DTFABRICACAO AS DataFabricacao, 
			ESTOQUE_LOCAL.QTDE AS SaldoLote,
			ESTOQUE_LOCAL.LOCAL AS CodigoLocal
		FROM ESTOQUE_LOCAL
		INNER JOIN LOCAIS ON LOCAIS.CODIGO = ESTOQUE_LOCAL.LOCAL AND LOCAIS.EMPRESA_RECNO = ESTOQUE_LOCAL.EMPRESA_RECNO
		WHERE ESTOQUE_LOCAL.CODIGO = ? AND ESTOQUE_LOCAL.LOTE = ? AND ESTOQUE_LOCAL.ODF = ? AND ESTOQUE_LOCAL.EMPRESA_RECNO = ? AND LOCAIS.LOCAL_CQ_SAIDA = 'S'
	`, insp.CodigoProduto, insp.Lote, insp.Odf, repo.BaseParams.CompanyRecno).Scan(&detailsEstoqueLocal)
	if res.Error != nil {
		return nil, res.Error
	}

	var OdfPai int
	res = repo.Uow.GetDb().Raw(`SELECT TOP 1 ODF as OdfPai FROM HISREAL WHERE HISREAL.LOTE = ? AND HISREAL.CODIGO = ? AND HISREAL.LOCAL_DESTINO = ? AND
		HISREAL.FORMA = 'E' AND HISREAL.ESTORNADO_APT_PRODUCAO = 'N' AND HISREAL.EMPRESA_RECNO = ?`, insp.Lote, insp.CodigoProduto, detailsEstoqueLocal.CodigoLocal, repo.BaseParams.CompanyRecno).
		First(&OdfPai)
	if res.Error != nil {
		if !errors.Is(res.Error, gorm.ErrRecordNotFound) {
			return nil, res.Error
		}
	}

	if OdfPai != 0 {
		output.NumeroOdf = strconv.Itoa(OdfPai)
	} else {
		output.NumeroOdf = strconv.Itoa(insp.Odf)
	}

	var detailsEstoque RncDetailsEstoque
	res = repo.Uow.GetDb().Raw(`
		SELECT 
			cast(ESTOQUE.ID as varchar(36)) AS IdProduto, 
			COALESCE(PPEDLISE.REVISAO, FECHA.REVISAO) AS Revisao
		FROM ESTOQUE 
		LEFT JOIN PPEDLISE ON PPEDLISE.CODPCA = ESTOQUE.CODIGO
        LEFT JOIN FECHA ON FECHA.PROCESSO = ESTOQUE.CODIGO
		WHERE COALESCE(PPEDLISE.NUMODF, FECHA.NODF) = ? AND COALESCE(PPEDLISE.NUMPED, FECHA.NUMPED) = ? AND COALESCE(PPEDLISE.EMPRESA_RECNO, FECHA.EMPRESA_RECNO) = ?
	`, output.NumeroOdf, insp.Pedido, repo.BaseParams.CompanyRecno).Scan(&detailsEstoque)
	if res.Error != nil {
		return nil, res.Error
	}

	output.NumeroLote = insp.Lote
	output.IdProduto = detailsEstoque.IdProduto
	output.Revisao, _ = strconv.Atoi(detailsEstoque.Revisao)
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

type RncDetailsMaterial struct {
	CodigoProduto   string
	CodigoCategoria string
}

func (repo *InspecaoSaidaRepository) PreencherInformacoesMaterialRetrabalho(rnc dto.RncInputDTO, ordemRetrabalho *dto.InspecaoSaidaOrdemRetrabalhoBackgroundDto) error {
	for i, material := range rnc.Materiais {
		var detailsMaterial RncDetailsMaterial
		res := repo.Uow.GetDb().Raw(`SELECT CODIGO AS CodigoProduto, CATEGORIA AS CodigoCategoria FROM ESTOQUE WHERE ID = ?`, material.IdProduto).Scan(&detailsMaterial)
		if res.Error != nil {
			return res.Error
		}

		operacao := "999"
		if material.OperacaoEngenharia != "" {
			operacao = material.OperacaoEngenharia
		}

		ordemRetrabalho.Materias = append(ordemRetrabalho.Materias, dto.InspecaoSaidaOrdemRetrabalhoMaterialackgroundDto{
			Quantidade:      material.Quantidade,
			CodigoProduto:   detailsMaterial.CodigoProduto,
			Operacao:        operacao,
			CodigoCategoria: detailsMaterial.CodigoCategoria,
			Sequencia:       strconv.Itoa(i + 1),
		})
	}
	return nil
}

func (repo *InspecaoSaidaRepository) PreencherInformacoesMaquinaRetrabalho(rnc dto.RncInputDTO, ordemRetrabalho *dto.InspecaoSaidaOrdemRetrabalhoBackgroundDto) error {
	for _, recurso := range rnc.Recursos {
		var recnoMaquina int
		res := repo.Uow.GetDb().Raw(`SELECT R_E_C_N_O_ AS recnoMaquina FROM MAQUINA WHERE ID = ?`, recurso.IdRecurso).First(&recnoMaquina)
		if res.Error != nil {
			return res.Error
		}

		sequencia := "1"
		materialNaOperacao := make([]string, 0)
		for _, material := range ordemRetrabalho.Materias {
			if material.Operacao == recurso.OperacaoEngenharia {
				materialNaOperacao = append(materialNaOperacao, material.CodigoProduto)
			}
		}

		if len(materialNaOperacao) > 0 {
			sequencia = strconv.Itoa(len(materialNaOperacao) + 1)
		}

		ordemRetrabalho.Maquinas = append(ordemRetrabalho.Maquinas, dto.InspecaoSaidaOrdemRetrabalhoMaquinaBackgroundDto{
			RecnoMaquina:      recnoMaquina,
			QuantidadeHoras:   recurso.Horas,
			Operacao:          recurso.OperacaoEngenharia,
			Sequencia:         sequencia,
			DescricaoOperacao: recurso.Detalhamento,
		})
	}
	return nil
}
