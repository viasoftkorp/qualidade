package utils

import "github.com/shopspring/decimal"

func DecimalToFloat64(decimalValue decimal.Decimal) (newVal float64) {
	result, _ := decimalValue.Float64()
	return result
}
