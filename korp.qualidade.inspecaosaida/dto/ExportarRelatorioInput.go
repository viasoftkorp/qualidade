package dto

import (
	"time"

	"github.com/google/uuid"
	"github.com/shopspring/decimal"
)

type ExportarRelatorioInput struct {
	ReportingOutputType string
	Data                *ExportarRelatorioData
}

type ExportarRelatorioData struct {
	Inspecao     *InspecaoDataSource
	ItemInspecao *ItemInspecaoDataSource
}

type InspecaoDataSource struct {
	Inspecao []InspecaoRelatorio
}

type ItemInspecaoDataSource struct {
	ItemInspecao []ItemInspecaoRelatorio
}

type InspecaoRelatorio struct {
	LogoEmpresa              string
	DataEmissao              string
	CodigoProduto            string
	DescricaoProduto         string
	Lote                     string
	DataFabricacao           *time.Time
	DataValidade             *time.Time
	Recno                    int
	Id                       uuid.UUID
	CodigoInspecao           int
	Odf                      int
	Cliente                  *string
	Pedido                   string
	IsoTs                    string
	Inspecionado             string
	DataInspecao             *time.Time
	TipoInspecao             string
	Inspetor                 string
	Resultado                string
	QuantidadeInspecao       decimal.Decimal
	QuantidadeLote           decimal.Decimal
	QuantidadeAceita         decimal.Decimal
	QuantidadeRetrabalhada   decimal.Decimal
	QuantidadeAprovada       decimal.Decimal
	QuantidadeReprovada      decimal.Decimal
	IdEmpresa                int
	Usuario                  string
	NotaFiscal               *int
	QuantidadeLoteNotaFiscal float64
}

type ItemInspecaoRelatorio struct {
	Id                    uuid.UUID
	LegacyIdPlanoInspecao int
	Plano                 string
	Odf                   int
	Descricao             string
	Metodo                string
	Sequencia             string
	Resultado             string
	MaiorValor            decimal.Decimal
	MenorValor            decimal.Decimal
	MaiorValorBase        decimal.Decimal
	MenorValorBase        decimal.Decimal
	CodigoInspecao        int
	IdEmpresa             int
	Observacao            string
}
