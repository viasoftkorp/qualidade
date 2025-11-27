package entities

import "github.com/shopspring/decimal"

type InspecaoSaidaExecutadoWeb struct {
	Id                    string           `gorm:"primaryKey"`
	RecnoInspecaoSaida    int              `gorm:"column:RECNO_INSPECAO_SAIDA;type:INTEGER"`
	IdInspecaoSaidaSaga   string           `gorm:"column:ID_INSPECAO_SAIDA_SAGA;type:NVARCHAR(450);"`
	IdRnc                 *string          `gorm:"column:ID_RNC;type:NVARCHAR(450);"`
	CodigoRnc             *int             `gorm:"column:CODIGO_RNC;type:INTEGER;"`
	Estorno               bool             `gorm:"column:ESTORNO;type:BIT"`
	QuantidadeTransferida *decimal.Decimal `gorm:"column:QUANTIDADE_TRANSFERIDA;type:decimal(19,6)"`
}

func (InspecaoSaidaExecutadoWeb) TableName() string {
	return "InspecaoSaidaExecutadoWeb"
}
