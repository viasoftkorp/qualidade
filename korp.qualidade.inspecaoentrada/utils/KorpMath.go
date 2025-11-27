package utils

import (
	"github.com/shopspring/decimal"
	"math"
)

func DecimalToFloat64(decimalValue decimal.Decimal) (newVal float64) {
	result, _ := decimalValue.Float64()
	return result
}

func Round(val float64, roundOn float64, places int) (newVal float64) {
	var round float64
	pow := math.Pow(10, float64(places))
	digit := pow * val
	_, div := math.Modf(digit)
	if div >= roundOn {
		round = math.Ceil(digit)
	} else {
		round = math.Floor(digit)
	}
	newVal = round / pow
	return
}
