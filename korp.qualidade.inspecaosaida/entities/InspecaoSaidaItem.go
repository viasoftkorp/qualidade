package entities

import (
	"github.com/google/uuid"
	"github.com/shopspring/decimal"
)

type InspecaoSaidaItem struct {
	Recno          int             `gorm:"primaryKey;column:R_E_C_N_O_"`
	Id             uuid.UUID       `gorm:"column:Id"`
	Plano          string          `gorm:"column:PLANO"`
	Odf            int             `gorm:"column:NUMODF"`
	Descricao      string          `gorm:"column:DESCRICAO"`
	Metodo         string          `gorm:"column:METODO"`
	Sequencia      string          `gorm:"column:SEQUENCIA"`
	Resultado      string          `gorm:"column:RESULTADO"`
	MaiorValor     decimal.Decimal `gorm:"column:MAIORVALOR"`
	MenorValor     decimal.Decimal `gorm:"column:MENORVALOR"`
	CodigoInspecao int             `gorm:"column:CODINSP"`
	IdEmpresa      int             `gorm:"column:EMPRESA_RECNO"`
	Observacao     string          `gorm:"column:OBSERVACAO"`
}

func (InspecaoSaidaItem) TableName() string {
	return "QA_ITEM_INSPECAO_SAIDA"
}
