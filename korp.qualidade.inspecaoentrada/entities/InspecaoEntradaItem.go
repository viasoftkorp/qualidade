package entities

import (
	"github.com/google/uuid"
	"github.com/shopspring/decimal"
)

type InspecaoEntradaItem struct {
	Recno                  int             `gorm:"primaryKey;column:R_E_C_N_O_"`
	Id                     uuid.UUID       `gorm:"column:Id"`
	LegacyIdPlanoInspecao  int             `gorm:"column:RECNO_PLANO_INSPECAO"`
	Plano                  string          `gorm:"column:PLANO"`
	Descricao              string          `gorm:"column:DESCRICAO_PLANO"`
	Metodo                 string          `gorm:"column:METODO"`
	Sequencia              string          `gorm:"column:SEQUENCIA"`
	Resultado              string          `gorm:"column:RESULTADO"`
	MaiorValorInspecionado decimal.Decimal `gorm:"column:MAIORVALOR"`
	MenorValorInspecionado decimal.Decimal `gorm:"column:MENORVALOR"`
	CodigoInspecao         int             `gorm:"column:CODINSPECAO"`
	Observacao             string          `gorm:"column:OBSERVACAO"`
}

func (InspecaoEntradaItem) TableName() string {
	return "QA_ITEM_INSPECAO_ENTRADA"
}
