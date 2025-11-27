package services

import (
	"net/http"
	"strconv"
	"time"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	"bitbucket.org/viasoftkorp/korp.sdk/ambient_data"
	"bitbucket.org/viasoftkorp/korp.sdk/api/tenant_management"
	"bitbucket.org/viasoftkorp/korp.sdk/environment"
	"github.com/gofiber/fiber/v2"
)

type WmsProxyService struct {
	BaseParams *models.BaseParams
	Ctx        *fiber.Ctx
}

func NewWmsProxyService(baseParams *models.BaseParams, ctx *fiber.Ctx) interfaces.IWmsProxyService {
	return &WmsProxyService{
		BaseParams: baseParams,
		Ctx:        ctx,
	}
}

func (service WmsProxyService) GerarNumeroLote(input dto.GerarNumeroLoteErpInput) (*dto.GerarNumeroLoteErpOutput, error) {
	basePath, err := service.getBasePath()
	if err != nil {
		return nil, err
	}

	var output *dto.GerarNumeroLoteErpOutput
	headers := service.getDefaultHeaders()
	endpoint := *basePath + "/MontarFormulaLote"

	var request = utils.
		NewRequest(http.MethodPost, endpoint).
		WithHttpHeader(&headers).
		WithTimeout(time.Second * 15).
		WithBody(input).
		Call(&output)

	if request.Error != nil {
		return nil, request.Error
	}

	return output, nil
}

func (service WmsProxyService) getDefaultHeaders() http.Header {
	header := make(http.Header)
	header.Add("Usuario", service.BaseParams.UserLogin)
	header.Add("IdEmpresa", strconv.Itoa(service.BaseParams.LegacyCompanyId))
	header.Add("Content-Type", fiber.MIMEApplicationJSON)

	return header
}

func (service WmsProxyService) getBasePath() (*string, error) {
	gateway, err := environment.GetGateway()
	if err != nil {
		return nil, err
	}

	ambientDatamodel := &ambient_data.AmbientDataModel{
		TenantId:      service.BaseParams.TenantId,
		EnvironmentId: service.BaseParams.EnvironmentId,
	}

	environment, err := tenant_management.NewTenantManagementAPI().GetEnvironment(ambientDatamodel)
	if err != nil {
		return nil, err
	}

	endpoint := gateway + "/korp/services/" + environment.DesktopDatabaseVersion + "/Logistica/" + environment.DatabaseName + "/WmsService"

	return &endpoint, nil
}
