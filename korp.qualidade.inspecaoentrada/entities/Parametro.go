package entities

import (
	"github.com/google/uuid"
)

type Parametro struct {
	Recno int       `gorm:"primaryKey;column:R_E_C_N_O_"`
	Id    uuid.UUID `gorm:"column:Id"`
	Valor string    `gorm:"column:VALOR"`
	Secao string    `gorm:"column:SECAO"`
	Chave string    `gorm:"column:CHAVE"`
	Md5   string    `gorm:"column:MD5"`
}

func (Parametro) TableName() string {
	return "PARAMETROS"
}
