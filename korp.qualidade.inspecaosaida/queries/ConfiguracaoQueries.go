package queries

const GetConfiguracao = `
SELECT 
CAST(Id AS VARCHAR(36)) AS Id,
USAR_NOTA_IMPRESSAO_RELATORIO AS UsarNotaImpressaoRelatorio
FROM QA_INSPECAO_SAIDA_CONFIGURACAO
`

const UpdateConfiguracao = `
UPDATE QA_INSPECAO_SAIDA_CONFIGURACAO
SET USAR_NOTA_IMPRESSAO_RELATORIO = @UsarNotaImpressaoRelatorio
WHERE Id = '@Id'
`
