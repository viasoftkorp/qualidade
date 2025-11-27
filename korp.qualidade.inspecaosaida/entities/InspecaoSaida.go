package entities

import (
	"github.com/google/uuid"
	"github.com/shopspring/decimal"
)

type InspecaoSaida struct {
	Recno                  int             `gorm:"primaryKey;column:R_E_C_N_O_"`
	Id                     uuid.UUID       `gorm:"column:Id"`
	CodigoInspecao         int             `gorm:"column:CODINSP"`
	Odf                    int             `gorm:"column:NUMODF"`
	Cliente                string          `gorm:"column:CLIENTE"`
	Pedido                 string          `gorm:"column:NUMPED"`
	IsoTs                  string          `gorm:"column:ISO_TS"`
	Inspecionado           string          `gorm:"column:INSPECIONADO"`
	DataInspecao           string          `gorm:"column:DATAINSP"`
	TipoInspecao           string          `gorm:"column:TIPOINSP"`
	Inspetor               string          `gorm:"column:INSPETOR"`
	Resultado              string          `gorm:"column:RESULTADO"`
	Lote                   string          `gorm:"column:LOTE"`
	QuantidadeInspecao     decimal.Decimal `gorm:"column:QTD_INSPECAO;type:decimal(19,6)"`
	QuantidadeLote         decimal.Decimal `gorm:"column:QTD_LOTE;type:decimal(19,6)"`
	QuantidadeAceita       decimal.Decimal `gorm:"column:QTD_ACEITO;type:decimal(19,6)"`
	QuantidadeRetrabalhada decimal.Decimal `gorm:"column:QTD_RETRABALHO;type:decimal(19,6)"`
	QuantidadeAprovada     decimal.Decimal `gorm:"column:QTD_APROVADO;type:decimal(19,6)"`
	QuantidadeReprovada    decimal.Decimal `gorm:"column:QTD_REJEITADO;type:decimal(19,6)"`
	IdEmpresa              int             `gorm:"column:EMPRESA_RECNO"`
	CodigoProduto          *string         `gorm:"column:CODIGO_PRODUTO"`
}

func (InspecaoSaida) TableName() string {
	return "QA_INSPECAO_SAIDA"
}
