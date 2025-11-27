package entities

type Local struct {
	IdLocal                 int    `gorm:"primaryKey;column:R_E_C_N_O_"`
	IdEmpresa               int    `gorm:"column:EMPRESA_RECNO"`
	Codigo                  int    `gorm:"column:CODIGO"`
	Sigla                   string `gorm:"column:SIGLA"`
	Descricao               string `gorm:"column:DESCRICAO"`
	LocalExpedicao          string `gorm:"column:LOCAL_EXPEDICAO"`
	LocalConferenciaSaida   string `gorm:"column:LOCAL_CQ_SAIDA"`
	LocalConferenciaEntrada string `gorm:"column:LOCAL_CQ_ENTRADA"`
	LocalPrincipal          string `gorm:"column:LOCAL_PRINCIPAL"`
	LocalReprovado          string `gorm:"column:LOCAL_REPROVADO"`
	Planejar                string `gorm:"column:PLANEJAR"`
	CodigoArmazem           string `gorm:"column:CODIGO_ARMAZEM"`
}

func (Local) TableName() string {
	return "LOCAIS"
}
