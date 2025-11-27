package queries

const GetAllInspecaoEntradaItensHistoricoInspecao = `
SELECT
	DISTINCT
	QA_INSPECAO_ENTRADA.R_E_C_N_O_ AS RecnoInspecao,
	QA_INSPECAO_ENTRADA.COD_INSP AS CodigoInspecao,
	QA_INSPECAO_ENTRADA.CODNOTA AS NotaFiscal,
	ESTOQUE.CODIGO AS CodigoProduto,
	ESTOQUE.DESCRI AS DescricaoProduto,
	QA_INSPECAO_ENTRADA.QTD_INSPECAO AS QuantidadeInspecao,
	CAST(QA_INSPECAO_ENTRADA.DATAINSP AS DATE) AS DataInspecao,
	QA_INSPECAO_ENTRADA.INSPETOR AS Inspetor,
	QA_INSPECAO_ENTRADA.RESULTADO AS Resultado,
	QA_INSPECAO_ENTRADA.QTD_APROVADO AS QuantidadeAprovada,
	QA_INSPECAO_ENTRADA.QTD_REJEITADO AS QuantidadeReprovada,
    InspecaoEntradaExecutadoWeb.ID_RNC AS IdRnc
	FROM QA_INSPECAO_ENTRADA
	INNER JOIN InspecaoEntradaExecutadoWeb ON InspecaoEntradaExecutadoWeb.RECNO_INSPECAO_ENTRADA = QA_INSPECAO_ENTRADA.R_E_C_N_O_
	INNER JOIN ESTOQUE ON InspecaoEntradaExecutadoWeb.CODIGO_PRODUTO = ESTOQUE.CODIGO
	@JoinFilters
	WHERE Estorno = 0 AND INSPECIONADO = 'S'
	@AdvancedFilter
	AND QA_INSPECAO_ENTRADA.RECNO_HISTLISE = @` + NamedRecnoItemNotaFiscal + `
	AND QA_INSPECAO_ENTRADA.LOTE = @` + NamedLote + "\n"

const GetInspecaoEntradaItensHistoricoInspecaoPagination = `
@Sorting
OFFSET @Skip ROWS
FETCH NEXT @PageSize ROWS ONLY
`

const GetInspecoesEntradaItensHistoricoCount = `
SELECT COUNT(DISTINCT QA_INSPECAO_ENTRADA.R_E_C_N_O_)
	FROM QA_INSPECAO_ENTRADA
	INNER JOIN InspecaoEntradaExecutadoWeb ON InspecaoEntradaExecutadoWeb.RECNO_INSPECAO_ENTRADA = QA_INSPECAO_ENTRADA.R_E_C_N_O_
	INNER JOIN ESTOQUE ON InspecaoEntradaExecutadoWeb.CODIGO_PRODUTO = ESTOQUE.CODIGO
	@JoinFilters
	WHERE Estorno = 0 AND INSPECIONADO = 'S'
	@AdvancedFilter
	AND QA_INSPECAO_ENTRADA.RECNO_HISTLISE = @` + NamedRecnoItemNotaFiscal + `
	AND QA_INSPECAO_ENTRADA.LOTE = @` + NamedLote + "\n"
