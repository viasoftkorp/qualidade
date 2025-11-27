package dto

import "github.com/google/uuid"

type PedidoVendaLoteDto struct {
	Id                           uuid.UUID `json:"id"`
	IdInspecaoEntradaPedidoVenda uuid.UUID `json:"idInspecaoEntradaPedidoVenda"`
	NumeroLote                   string    `json:"numeroLote"`
	Quantidade                   float64   `json:"quantidade"`
}

type GetAllPedidoVendaLotes struct {
	Items      []PedidoVendaLoteDto `json:"items"`
	TotalCount int64                `json:"totalCount"`
}
