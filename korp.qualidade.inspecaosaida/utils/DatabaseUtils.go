package utils

import (
	"bytes"
	"encoding/base64"
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"net/url"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/consts"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/enums"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/infrastructures"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/korp.sdk/ambient_data"
	consulSdk "bitbucket.org/viasoftkorp/korp.sdk/consul"
	"bitbucket.org/viasoftkorp/korp.sdk/environment"
	"gorm.io/gorm"
)

func MigrateEntities(db *gorm.DB, ambientDataModel *ambient_data.AmbientDataModel) error {
	return migrate(db, ambientDataModel)
}

func migrate(db *gorm.DB, ambientDataModel *ambient_data.AmbientDataModel) error {
	con, err := db.DB()
	if err != nil {
		return err
	}

	err = infrastructures.RunMigrateFiles(con)
	if err != nil {
		return err
	}

	err = CreateReportTemplates(ambientDataModel)
	if err != nil {
		return err
	}

	return nil
}

func CreateReportTemplates(ambientDataModel *ambient_data.AmbientDataModel) error {
	byteArray, err := base64.StdEncoding.DecodeString(consts.InspecaoReportTemplate)
	if err != nil {
		return err
	}

	inspecaoinput := &dto.CreateDefaultReportTemplateInput{
		ReportId:    consts.InspecaoReportId,
		Description: consts.InspecaoReportDescription,
		Template:    byteArray,
	}

	inspecaoReport, err := getReportByReportId(inspecaoinput, ambientDataModel)
	if inspecaoReport == nil {
		err = createUpdateDefaultReportTemplate(inspecaoinput, ambientDataModel, http.MethodPost)
	} else {
		err = createUpdateDefaultReportTemplate(inspecaoinput, ambientDataModel, http.MethodPut)
	}
	if err != nil {
		return err
	}

	return nil
}

func getReportByReportId(input *dto.CreateDefaultReportTemplateInput, ambientData *ambient_data.AmbientDataModel) (*dto.GetAllReportingViewOutput, error) {
	var output *dto.GetAllReportingViewOutput
	//não quebrar clientes que já rodam o serviço
	gateway, err := consulSdk.GetPropertyFromConsulNoError("ReportGateway", "")
	if err != nil {
		log.Fatalf("Couldn't get reportgateway due to error: %s", err)
	}

	if gateway == nil {
		gateway, err = environment.GetGateway()
		if err != nil {
			log.Fatalf("Couldn't get reportgateway due to error: %s", err)
		}
	}

	uri, err := url.Parse(fmt.Sprintf("%s/reporting/reports/%s", gateway, input.ReportId))
	if err != nil {
		return nil, err
	}

	requestInput := models.CreateRequestInput{
		HttpMethod:    "GET",
		Uri:           uri.String(),
		ContentType:   "",
		TenantId:      ambientData.TenantId,
		UserId:        ambientData.TenantId,
		EnvironmentId: ambientData.EnvironmentId,
		Body:          &bytes.Buffer{},
	}

	r, err := CreateRequest(requestInput)
	if err != nil {
		return nil, err
	}

	requestOutput, err := DoRequest(r)
	if err != nil {
		return nil, err
	}

	err = json.Unmarshal(requestOutput, &output)
	if err != nil {
		return nil, err
	}

	return output, nil
}

func createUpdateDefaultReportTemplate(input *dto.CreateDefaultReportTemplateInput, ambientData *ambient_data.AmbientDataModel, httpMethod string) error {
	//não quebrar clientes que já rodam o serviço
	gateway, err := consulSdk.GetPropertyFromConsulNoError("ReportGateway", "")
	if err != nil {
		log.Fatalf("Couldn't get reportgateway due to error: %s", err)
	}

	if gateway == nil {
		gateway, err = environment.GetGateway()
		if err != nil {
			log.Fatalf("Couldn't get reportgateway due to error: %s", err)
		}
	}

	inputJSON, err := json.Marshal(&dto.ReportDefaultCreateOrUpdateInput{
		ReportingEngine: enums.ReportingEngineStimulsoft,
		ReportingType:   enums.ReportingTypeReport,
		ReportId:        input.ReportId,
		Extension:       "pdf",
		Description:     input.Description,
		Domain:          enums.QualityAssurance,
		Area:            "",
		AppId:           consts.AppId,
		Template:        input.Template,
	})

	uri, err := url.Parse(fmt.Sprintf("%s/reporting/reports/default/%s", gateway, input.ReportId))
	if err != nil {
		return err
	}

	requestInput := models.CreateRequestInput{
		HttpMethod:    httpMethod,
		Uri:           uri.String(),
		ContentType:   "application/json",
		TenantId:      ambientData.TenantId,
		UserId:        ambientData.TenantId,
		EnvironmentId: ambientData.EnvironmentId,
		Body:          bytes.NewBuffer(inputJSON),
	}

	r, err := CreateRequest(requestInput)
	if err != nil {
		return err
	}

	_, err = DoRequest(r)
	return err
}
