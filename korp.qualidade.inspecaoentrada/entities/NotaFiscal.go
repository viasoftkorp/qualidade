package entities

import "github.com/shopspring/decimal"

type NotaFiscal struct {
	IdNotaFiscal     int             `gorm:"primaryKey;column:R_E_C_N_O_"`
	NotaFiscal       int             `gorm:"column:NFISCAL"`
	CodigoProduto    string          `gorm:"column:ITEM"`
	DescricaoProduto string          `gorm:"column:DESCRI"`
	IdEmpresa        int             `gorm:"column:EMPRESA_RECNO"`
	Quantidade       decimal.Decimal `gorm:"column:QTENT;type:decimal(19,6)"`
	DataEntrada      string          `gorm:"column:DTENT"`
	DataEmissao      string          `gorm:"column:DTEMI"`
	Fornecedor       string          `gorm:"column:FORNECE"`
}

func (NotaFiscal) TableName() string {
	return "HISTLISE"
}
