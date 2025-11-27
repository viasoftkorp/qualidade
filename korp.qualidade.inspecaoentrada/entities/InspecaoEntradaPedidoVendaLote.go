package entities

import "github.com/google/uuid"

type InspecaoEntradaPedidoVendaLote struct {
	Id                           string    `gorm:"NEWID();column:Id;type:UNIQUEIDENTIFIER;NOT NULL"`
	IdInspecaoEntradaPedidoVenda uuid.UUID `gorm:"column:ID_INSPECAO_ENTRADA_PEDIDO_VENDA"`
	NumeroLote                   string    `gorm:"column:NUMERO_LOTE"`
	Quantidade                   float64   `gorm:"column:QUANTIDADE"`
}

func (InspecaoEntradaPedidoVendaLote) TableName() string {
	return "QA_INSPECAO_ENTRADA_PEDIDO_VENDA_WEB_LOTE"
}
