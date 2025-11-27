package entities

type InspecaoEntradaExecutadoWeb struct {
	Id                    string  `gorm:"primaryKey"`
	RecnoInspecaoEntrada  int     `gorm:"column:RECNO_INSPECAO_Entrada;type:INTEGER"`
	CodigoProduto         string  `gorm:"column:CODIGO_PRODUTO;type:NVARCHAR(450)"`
	IdInspecaoEntradaSaga string  `gorm:"column:ID_INSPECAO_Entrada_SAGA;type:NVARCHAR(450);"`
	Estorno               bool    `gorm:"column:ESTORNO;type:BIT"`
	IdRnc                 *string `gorm:"column:ID_RNC;type:NVARCHAR(450);"`
}

func (InspecaoEntradaExecutadoWeb) TableName() string {
	return "InspecaoEntradaExecutadoWeb"
}
