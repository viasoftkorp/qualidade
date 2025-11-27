package services

import (
	"bytes"
	"encoding/json"
	"fmt"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	consulSdk "bitbucket.org/viasoftkorp/korp.sdk/consul"
	"bitbucket.org/viasoftkorp/korp.sdk/environment"
)

type ImpressaoService struct {
	BaseParams *models.BaseParams
}

func NewImpressaoService(
	baseParams *models.BaseParams) (interfaces.IImpressaoService, error) {
	return &ImpressaoService{
		BaseParams: baseParams,
	}, nil
}

func (service *ImpressaoService) ExportReportStimulsoft(report interface{}, reportId string) ([]byte, error) {
	gateway, err := consulSdk.GetPropertyFromConsulNoError("ReportGateway", "")
	if err != nil {
		return nil, err
	}

	if gateway == nil {
		gateway, err = environment.GetGateway()
		if err != nil {
			return nil, err
		}
	}

	reportJson, err := json.Marshal(report)
	if err != nil {
		return nil, err
	}

	r, err := utils.CreateRequest(models.CreateRequestInput{
		HttpMethod:    "POST",
		Uri:           fmt.Sprintf("%s/reporting/reports/%s/export", gateway, reportId),
		Body:          bytes.NewBuffer(reportJson),
		ContentType:   "application/json",
		TenantId:      service.BaseParams.TenantId,
		EnvironmentId: service.BaseParams.EnvironmentId,
		UserId:        service.BaseParams.UserId,
	})
	if err != nil {
		return nil, err
	}

	return utils.DoRequest(r)
}
