package models

import (
	"github.com/google/uuid"
	"github.com/shopspring/decimal"
)

type InspecaoSaidaItem struct {
	Id                     uuid.UUID
	LegacyIdPlanoInspecao  int
	Plano                  string
	Odf                    int
	Descricao              string
	Metodo                 string
	Sequencia              string
	Resultado              string
	MaiorValor             decimal.Decimal
	MenorValor             decimal.Decimal
	MaiorValorBase         decimal.Decimal
	MenorValorBase         decimal.Decimal
	CodigoInspecao         int
	IdEmpresa              int
	Observacao             string
}
