package queries

const GetProdutoPeloCodigo = `SELECT CAST(Id AS VARCHAR(36)) AS Id, CODIGO AS Codigo, DESCRI AS Descricao, 
UNIDADE AS Unidade FROM ESTOQUE
WHERE CODIGO = @` + NamedProdutoCodigo

const GetProdutos = `SELECT CAST(ESTOQUE.Id AS VARCHAR(36)) AS Id, ESTOQUE.CODIGO AS Codigo,
ESTOQUE.DESCRI AS Descricao, ESTOQUE.UNIDADE AS Unidade FROM ESTOQUE
`

const GetProdutosTotalCount = `SELECT COUNT(*) FROM ESTOQUE
`

const GetProdutosFilter = `WHERE (LOWER(CODIGO) LIKE LOWER(@` + NamedFilter + `) 
OR LOWER(DESCRI) LIKE LOWER(@` + NamedFilter + `) OR LOWER(UNIDADE) LIKE LOWER(@` + NamedFilter + `))`

const GetProdutosPaginacao = ` ORDER BY Sorting
	OFFSET @` + NamedSkip + ` ROWS
	FETCH NEXT @` + NamedPageSize + ` ROWS ONLY
`
