package utils

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"encoding/json"
	"fmt"
	"log"
	"time"
)

const showLogs = true

/*func ErrorMessage(err error) {
	log.Println(fmt.Sprintf("%s: ERROR!: %s", GetDateNow(), err.Error()))
}*/

func LogErrorValidacao(httpMethod string, route string, error *dto.ValidacaoDTO) {
	errorAsJson, _ := json.Marshal(error)
	logMessage := fmt.Sprintf("ERROR %s %s - %s", httpMethod, route, errorAsJson)
	log.Println(logMessage)
}

func LogError(httpMethod string, message string, err interface{}) {
	/*if len(args) > 0 {
		message = fmt.Sprintf(message, args...)
	}*/
	logMessage := fmt.Sprintf("ERROR %s %s - %s", httpMethod, message, err)
	log.Println(logMessage)
}

func LogWarning(httpMethod string, message string, err interface{}) {
	/*if len(args) > 0 {
		message = fmt.Sprintf(message, args...)
	}*/
	logMessage := fmt.Sprintf("WARNING %s %s - %s", httpMethod, message, err)
	log.Println(logMessage)
}

func LogMessage(message string) {
	if showLogs {
		fmt.Println(fmt.Sprintf("%s: INFO: %s", GetDateNow(), message))
	}
}

func GetDateNow() string {
	now := time.Now().Local()
	dateTime := now.Format("02/01/2006 15:04:05")

	return dateTime
}
