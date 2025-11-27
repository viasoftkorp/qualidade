package queries

const GetLocalPeloCodigo = `SELECT CAST(Id AS VARCHAR(36)) AS Id, CODIGO AS Codigo, DESCRICAO AS Descricao, 
UNIDADE AS Unidade FROM LOCAIS
WHERE EMPRESA_RECNO = @` + NamedEmpresaRecno + ` AND CODIGO = @` + NamedLocal

const GetLocais = `SELECT CAST(LOCAIS.Id AS VARCHAR(36)) AS Id, LOCAIS.CODIGO AS Codigo,
LOCAIS.DESCRICAO AS Descricao, LOCAIS.UNIDADE AS Unidade FROM LOCAIS
`

const GetLocaisTotalCount = `SELECT COUNT(*) FROM LOCAIS`

const GetLocaisFilter = `WHERE EMPRESA_RECNO = @` + NamedEmpresaRecno + ` AND (LOWER(CODIGO) LIKE LOWER(@` + NamedFilter + `) 
OR LOWER(DESCRICAO) LIKE LOWER(@` + NamedFilter + `) OR LOWER(UNIDADE) LIKE LOWER(@` + NamedFilter + `))`

const GetLocaisPaginacao = ` ORDER BY Sorting
	OFFSET @` + NamedSkip + ` ROWS
	FETCH NEXT @` + NamedPageSize + ` ROWS ONLY
`
