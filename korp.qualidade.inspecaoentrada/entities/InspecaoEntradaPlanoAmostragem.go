package entities

type InspecaoEntradaPlanoAmostragem struct {
	Id                    string  `gorm:"column:ID"`
	QuantidadeMinima      float64 `gorm:"column:QTD_MIN"`
	QuantidadeMaxima      float64 `gorm:"column:QTD_MAX"`
	QuantidadeInspecionar float64 `gorm:"column:QTD_INSPEC"`
}

func (InspecaoEntradaPlanoAmostragem) TableName() string {
	return "QA_PLANO_AMOSTRAGEM_SAIDA"
}
