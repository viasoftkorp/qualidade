package entities

import (
	"github.com/google/uuid"
	"github.com/shopspring/decimal"
)

type InspecaoEntrada struct {
	Recno               int             `gorm:"primaryKey;column:R_E_C_N_O_"`
	CodigoInspecao      int             `gorm:"column:COD_INSP"`
	NotaFiscal          int             `gorm:"column:CODNOTA"`
	Inspecionado        string          `gorm:"column:INSPECIONADO"`
	DataInspecao        string          `gorm:"column:DATAINSP"`
	Inspetor            string          `gorm:"column:INSPETOR"`
	Resultado           string          `gorm:"column:RESULTADO"`
	Lote                string          `gorm:"column:LOTE"`
	QuantidadeInspecao  decimal.Decimal `gorm:"column:QTD_INSPECAO;type:decimal(19,6)"`
	QuantidadeLote      decimal.Decimal `gorm:"column:QTD_LOTE;type:decimal(19,6)"`
	QuantidadeAceita    decimal.Decimal `gorm:"column:QTD_ACEITO;type:decimal(19,6)"`
	QuantidadeAprovada  decimal.Decimal `gorm:"column:QTD_APROVADO;type:decimal(19,6)"`
	QuantidadeReprovada decimal.Decimal `gorm:"column:QTD_REJEITADO;type:decimal(19,6)"`
	Id                  uuid.UUID       `gorm:"column:Id"`
}

func (InspecaoEntrada) TableName() string {
	return "QA_INSPECAO_ENTRADA"
}
