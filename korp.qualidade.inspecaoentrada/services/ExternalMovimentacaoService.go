package services

import (
	"bytes"
	"encoding/json"
	"fmt"
	"io"
	"log"
	"net/http"
	"strconv"
	"strings"
	"time"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/entities"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/korp.sdk/ambient_data"
	"bitbucket.org/viasoftkorp/korp.sdk/api/tenant_management"
	consulSdk "bitbucket.org/viasoftkorp/korp.sdk/consul"
	"bitbucket.org/viasoftkorp/korp.sdk/rest_client"
	"bitbucket.org/viasoftkorp/korp.sdk/service_info"
	utilsSdk "bitbucket.org/viasoftkorp/korp.sdk/utils"
)

type movimentacaoException struct {
	Error struct {
		Code    string `json:"code"`
		Message string `json:"message"`
	} `json:"error"`
}

type ExternalMovimentacaoService struct {
	BaseParams *models.BaseParams
}

func NewExternalMovimentacaoService(baseParams *models.BaseParams) interfaces.IExternalMovimentacaoService {
	return &ExternalMovimentacaoService{
		BaseParams: baseParams,
	}
}

func (service *ExternalMovimentacaoService) getExternalServicePath() (string, error) {
	//http://localhost:1503/Korp/Services/Logistica/TESTCOMPLETE_V16_0_0/WmsService/Movimentar
	gateway, err := consulSdk.GetPropertyFromConsul("LegacyGateway", service_info.ServiceName)
	if err != nil {
		log.Fatal("Couldn't get legacygateway due to error: %s", err)
		return "", err
	}
	ambientDatamodel := &ambient_data.AmbientDataModel{
		TenantId:      service.BaseParams.TenantId,
		EnvironmentId: service.BaseParams.EnvironmentId,
	}
	ambient, err := tenant_management.NewTenantManagementAPI().GetEnvironment(ambientDatamodel)
	if err != nil {
		return "", err
	}
	endPoint := fmt.Sprintf("%s/%s/Korp/Services/Logistica/%s/WmsService", gateway, utilsSdk.GetVersionWithoutBuild(service_info.Version), ambient.DatabaseName)
	return endPoint, nil
}

func (service *ExternalMovimentacaoService) RealizarMovimentacao(inspecao *entities.InspecaoEntrada) *dto.ValidacaoDTO {

	endpoint, err := service.getExternalServicePath()
	if err != nil {
		return &dto.ValidacaoDTO{
			Code:    115,
			Message: err.Error(),
		}
	}

	endpoint += "/Movimentar"

	requestBody := bytes.NewBuffer(nil)

	req, err := http.NewRequest("POST", endpoint, requestBody)

	if err != nil {
		return &dto.ValidacaoDTO{
			Code:    115,
			Message: err.Error(),
		}
	}

	req.Header.Set("Usuario", service.BaseParams.UserLogin)
	req.Header.Set("IdEmpresa", strconv.Itoa(service.BaseParams.CompanyRecno))
	req.Header.Set("Content-Type", "application/json")

	/*utils.LogMessage(fmt.Sprintf("entrou na externalMovimentacaoService - RealizarMovimentacao passando o dto : " + string(postBody[:])))*/

	client := rest_client.CreateHttpClient(time.Second * 60)

	resp, err := client.Do(req)

	if err != nil {
		return &dto.ValidacaoDTO{
			Code:    116,
			Message: err.Error(),
		}
	}

	defer func(Body io.ReadCloser) {
		err := Body.Close()
		if err != nil {

		}
	}(resp.Body)

	if !(resp.StatusCode >= 200 && resp.StatusCode <= 299) {
		korpMessage, validacaoDTO := service.ocorreuErroInesperado(resp)

		if validacaoDTO != nil {
			return validacaoDTO
		}
		return &dto.ValidacaoDTO{
			Code:    118,
			Message: korpMessage,
		}
	}
	return nil
}

func (service *ExternalMovimentacaoService) ocorreuErroInesperado(resp *http.Response) (string, *dto.ValidacaoDTO) {
	movimentacaoException := &movimentacaoException{}
	if resp.StatusCode == http.StatusNotFound {
		return "", &dto.ValidacaoDTO{
			Code:    27,
			Message: "URL não encontrada.",
		}
	}

	err := json.NewDecoder(resp.Body).Decode(movimentacaoException)
	if err != nil {
		return "", &dto.ValidacaoDTO{
			Code:    28,
			Message: err.Error(),
		}
	}

	if movimentacaoException.Error.Code == "KorpMensagem" {
		return movimentacaoException.Error.Message, nil
	} else {
		return "", &dto.ValidacaoDTO{
			Code:    29,
			Message: strings.ReplaceAll(movimentacaoException.Error.Message, "|", "/"),
		}
	}
}
