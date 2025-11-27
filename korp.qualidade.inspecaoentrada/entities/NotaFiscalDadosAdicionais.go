package entities

type NotaFiscalDadosAdicionais struct {
	Id           string `gorm:"NEWID();column:Id;type:UNIQUEIDENTIFIER;NOT NULL"`
	IdNotaFiscal string `gorm:"column:IdNotaFiscal"`
	Observacao   string `gorm:"column:OBSERVACAO"`
}

func (NotaFiscalDadosAdicionais) TableName() string {
	return "QA_INSPECAO_ENTRADA_NOTA_FISCAL_DADOS_ADICIONAIS"
}
