package utils

import (
	"fmt"
	"strconv"
	"time"
)

func SetDateTimeZone(dataHora *time.Time) *time.Time {
	if dataHora == nil {
		return nil
	}

	loc := time.FixedZone("UTC-3", -3*60*60)

	// Declaring t for Zone method
	t := dataHora.In(loc)

	return &t
}

func StringToTime(data string) *time.Time {
	if data == "" {
		return nil
	}

	dataRune := []rune(data)
	anoRune, _ := strconv.Atoi(string(dataRune[0:4]))
	mesRuneAux, _ := strconv.Atoi(string(dataRune[4:6]))
	mesRune := time.Month(mesRuneAux)
	diaRune, _ := strconv.Atoi(string(dataRune[6:8]))

	location, err := time.LoadLocation("America/Sao_Paulo")
	if err != nil {
		fmt.Println(err)
	}

	dataTime := time.Date(anoRune, mesRune, diaRune, 0, 0, 0, 0, location)

	return &dataTime
}
