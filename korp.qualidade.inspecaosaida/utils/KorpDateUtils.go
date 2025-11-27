package utils

import (
	"strconv"
	"time"
)

func SetDateTimeZone(dataHora *time.Time) *time.Time {
	if dataHora == nil {
		return nil
	}

	loc := time.FixedZone("UTC-3", -3*60*60)

	// Declaring t for Zone method
	t := dataHora.In(loc).Add(time.Hour * 3)

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

	dataTime := time.Date(anoRune, mesRune, diaRune, 0, 0, 0, 0, time.Local)

	return &dataTime
}
