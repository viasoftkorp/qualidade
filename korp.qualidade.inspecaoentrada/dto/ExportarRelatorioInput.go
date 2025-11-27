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
	LogoEmpresa      string
	DataEmissao      string
	CodigoProduto    string
	DescricaoProduto string
	DataFabricacao   *time.Time
	DataValidade     *time.Time
	IdEmpresa        int
	Usuario          string

	Recno               int
	CodigoInspecao      int
	NotaFiscal          int
	SerieNotaFiscal     string
	Inspecionado        string
	DataInspecao        *time.Time
	Inspetor            string
	Resultado           string
	Lote                string
	QuantidadeInspecao  decimal.Decimal
	QuantidadeLote      decimal.Decimal
	QuantidadeAceita    decimal.Decimal
	QuantidadeAprovada  decimal.Decimal
	QuantidadeReprovada decimal.Decimal
	Id                  uuid.UUID
}

type ItemInspecaoRelatorio struct {
	Id             uuid.UUID
	CodigoInspecao int
	Descricao      string
	Metodo         string
	Sequencia      string
	Resultado      string
	MaiorValor     decimal.Decimal
	MenorValor     decimal.Decimal
	MaiorValorBase decimal.Decimal
	MenorValorBase decimal.Decimal
	Observacao     string
}
