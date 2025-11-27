package entities

import (
	"github.com/google/uuid"
	"github.com/shopspring/decimal"
)

type InspecaoEntradaPedidoVenda struct {
	Id                      uuid.UUID        `gorm:"column:ID"`
	RecnoInspecaoEntrada    int              `gorm:"column:RECNO_INSPECAO_ENTRADA"`
	RecnoEstoquePedidoVenda int              `gorm:"column:RECNO_ESTOQUE_PEDIDO_VENDA"`
	NumeroPedido            string           `gorm:"column:NUMERO_PEDIDO"`
	QuantidadeAprovar       *decimal.Decimal `gorm:"column:QUANTIDADE_APROVAR;type:decimal(19,6)"`
	QuantidadeReprovar      *decimal.Decimal `gorm:"column:QUANTIDADE_REPROVAR;type:decimal(19,6)"`
	CodigoLocalAprovar      *int             `gorm:"column:CODIGO_LOCAL_APROVAR;"`
	CodigoLocalReprovar     *int             `gorm:"column:CODIGO_LOCAL_REPROVAR;"`
}

func (InspecaoEntradaPedidoVenda) TableName() string {
	return "QA_INSPECAO_ENTRADA_PEDIDO_VENDA_WEB"
}
