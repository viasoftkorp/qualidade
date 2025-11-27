package externalServices

import (
	"bytes"
	"context"
	"encoding/json"
	"errors"
	"fmt"
	"io/ioutil"
	"net/http"
	"net/url"
	"strconv"
	"strings"
	"time"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	korpCoreAuthorization "bitbucket.org/viasoftkorp/korp.sdk/authorization/services"
	"bitbucket.org/viasoftkorp/korp.sdk/environment"
	"bitbucket.org/viasoftkorp/korp.sdk/rest_client"
	"github.com/gofiber/fiber/v2"
)

const InspecaoSaidaSagaServiceName = "Viasoft.Qualidade.Inspecao.Background"

type ExternalInspecaoSaidaSagaService struct {
	interfaces.IExternalInspecaoSaidaSagaService
	Params *models.BaseParams
}

func NewExternalInspecaoSaidaSagaService(params *models.BaseParams) interfaces.IExternalInspecaoSaidaSagaService {
	return &ExternalInspecaoSaidaSagaService{
		Params: params,
	}
}

func (service *ExternalInspecaoSaidaSagaService) getExternalServicePath() (string, error) {
	gateway, err := environment.GetGateway()
	if err != nil {
		return "", err
	}
	endPoint := fmt.Sprintf("%s/qualidade/inspecoes/saida/sagas", gateway)
	return endPoint, nil
}

func (service *ExternalInspecaoSaidaSagaService) setDefaultHeaders(request *http.Request) error {
	request.Header.Set("TenantId", service.Params.TenantId)
	request.Header.Set("EnvironmentId", service.Params.EnvironmentId)
	request.Header.Set("LegacyCompanyId", strconv.Itoa(service.Params.CompanyRecno))
	request.Header.Set("CompanyId", service.Params.CompanyId)
	request.Header.Set("UserId", service.Params.UserId)
	request.Header.Set("Content-Type", fiber.MIMEApplicationJSON)
	token, err := korpCoreAuthorization.GetAccessToken(context.Background(), InspecaoSaidaSagaServiceName)
	if err != nil {
		return err
	}
	request.Header.Set("Authorization", fmt.Sprintf("Bearer %s", token))
	return nil
}

func (service *ExternalInspecaoSaidaSagaService) ReprocessarSaga(idSaga string) error {
	path, err := service.getExternalServicePath()
	if err != nil {
		return err
	}

	path += "/" + idSaga
	request, err := http.NewRequest(http.MethodPut, path, nil)
	if err != nil {
		return err
	}

	err = service.setDefaultHeaders(request)
	if err != nil {
		return err
	}

	client := rest_client.CreateHttpClient(time.Second * 10)

	resp, err := client.Do(request)
	if err != nil {
		return err
	}
	defer resp.Body.Close()

	if !utils.IsSuccessStatusCode(resp.StatusCode) {
		bodyBytes, err := ioutil.ReadAll(resp.Body)
		if err != nil {
			return err
		}
		return errors.New(string(bodyBytes))
	}

	return nil
}

func (service *ExternalInspecaoSaidaSagaService) RemoverSaga(idSaga string) error {
	path, err := service.getExternalServicePath()
	if err != nil {
		return err
	}

	path += "/" + idSaga
	request, err := http.NewRequest(http.MethodDelete, path, nil)
	if err != nil {
		return err
	}

	err = service.setDefaultHeaders(request)
	if err != nil {
		return err
	}

	client := rest_client.CreateHttpClient(time.Second * 10)

	resp, err := client.Do(request)
	if err != nil {
		return err
	}
	defer resp.Body.Close()

	if !utils.IsSuccessStatusCode(resp.StatusCode) {
		bodyBytes, err := ioutil.ReadAll(resp.Body)
		if err != nil {
			return err
		}
		return errors.New(string(bodyBytes))
	}

	return nil
}

func (service *ExternalInspecaoSaidaSagaService) PublicarSaga(body interface{}) (string, error) {
	path, err := service.getExternalServicePath()
	if err != nil {
		return "", err
	}

	requestBody, err := json.Marshal(body)
	if err != nil {
		return "", err
	}

	fmt.Println(string(requestBody))

	request, err := http.NewRequest(http.MethodPost, path, bytes.NewBuffer(requestBody))
	if err != nil {
		return "", err
	}

	err = service.setDefaultHeaders(request)
	if err != nil {
		return "", err
	}

	client := rest_client.CreateHttpClient(time.Second * 10)

	resp, err := client.Do(request)
	if err != nil {
		return "", err
	}
	defer resp.Body.Close()

	if !utils.IsSuccessStatusCode(resp.StatusCode) {
		bodyBytes, err := ioutil.ReadAll(resp.Body)
		if err != nil {
			return "", err
		}
		return "", errors.New(string(bodyBytes))
	}

	bytesResult, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		return "", err
	}

	return strings.ReplaceAll(string(bytesResult), "\"", ""), nil
}

func (service *ExternalInspecaoSaidaSagaService) BuscarSaga(id string) (*dto.SagaInspecaoSaidaOutput, error) {
	path, err := service.getExternalServicePath()
	if err != nil {
		return nil, err
	}

	path += "/" + id

	request, err := http.NewRequest(http.MethodGet, path, nil)
	if err != nil {
		return nil, err
	}

	err = service.setDefaultHeaders(request)
	if err != nil {
		return nil, err
	}

	client := rest_client.CreateHttpClient(time.Second * 10)

	resp, err := client.Do(request)
	if err != nil {
		return nil, err
	}
	defer resp.Body.Close()

	if !utils.IsSuccessStatusCode(resp.StatusCode) {
		bodyBytes, err := ioutil.ReadAll(resp.Body)
		if err != nil {
			return nil, err
		}
		return nil, errors.New(string(bodyBytes))
	}

	bytesResult, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		return nil, err
	}

	var output dto.SagaInspecaoSaidaOutput
	err = json.Unmarshal(bytesResult, &output)
	if err != nil {
		return nil, err
	}

	return &output, nil
}

func (service *ExternalInspecaoSaidaSagaService) BuscarSagas(skip, pageSize int, filters *dto.ProcessamentoInspecaoSaidaFilters, estorno bool) (*dto.GetAllSagaInspecaoSaidaOutput, error) {
	path, err := service.getExternalServicePath()
	if err != nil {
		return nil, err
	}

	request, err := http.NewRequest(http.MethodGet, path, nil)
	if err != nil {
		return nil, err
	}

	err = service.setDefaultHeaders(request)
	if err != nil {
		return nil, err
	}

	query := make(url.Values)
	query.Add("MaxResultCount", strconv.Itoa(pageSize))
	query.Add("SkipCount", strconv.Itoa(skip))

	if filters != nil {
		query.Add("AdvancedFilter", service.GetAdvancedFilter(filters, estorno))
	}

	request.URL.RawQuery = query.Encode()

	client := rest_client.CreateHttpClient(time.Second * 10)

	resp, err := client.Do(request)
	if err != nil {
		return nil, err
	}
	defer resp.Body.Close()

	if !utils.IsSuccessStatusCode(resp.StatusCode) {
		bodyBytes, err := ioutil.ReadAll(resp.Body)
		if err != nil {
			return nil, err
		}
		return nil, errors.New(string(bodyBytes))
	}

	bytesResult, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		return nil, err
	}

	var output dto.GetAllSagaInspecaoSaidaOutput
	err = json.Unmarshal(bytesResult, &output)
	if err != nil {
		return nil, err
	}

	return &output, nil
}

func (service *ExternalInspecaoSaidaSagaService) GetAdvancedFilter(filters *dto.ProcessamentoInspecaoSaidaFilters, estorno bool) string {
	advancedFilter := `{"condition": "and", "rules": %s}`
	rules := make([]string, 0)
	advancedFilterRuleTemplate := `{"field": "%s", "operator": "%s", "type": "%s", "value": "%s"}`

	if filters.Status != nil {
		rules = append(rules, fmt.Sprintf(advancedFilterRuleTemplate, "status", "equal", "integer", strconv.Itoa(*filters.Status)))
	}
	if filters.NumeroExecucoes != nil {
		rules = append(rules, fmt.Sprintf(advancedFilterRuleTemplate, "numeroExecucoes", "equal", "integer", strconv.Itoa(*filters.NumeroExecucoes)))
	}
	if filters.Erro != nil {
		rules = append(rules, fmt.Sprintf(advancedFilterRuleTemplate, "erro", "contains", "string", *filters.Erro))
	}
	if filters.Resultado != nil {
		rules = append(rules, fmt.Sprintf(advancedFilterRuleTemplate, "resultado", "contains", "string", *filters.Resultado))
	}
	if filters.QuantidadeTotal != nil {
		rules = append(rules, fmt.Sprintf(advancedFilterRuleTemplate, "quantidadeTotal", "equal", "double", fmt.Sprintf("%f", *filters.QuantidadeTotal)))
	}
	if filters.CodigoProduto != nil {
		rules = append(rules, fmt.Sprintf(advancedFilterRuleTemplate, "codigoProduto", "contains", "string", *filters.CodigoProduto))
	}
	if filters.Odf != nil {
		rules = append(rules, fmt.Sprintf(advancedFilterRuleTemplate, "ordemFabricacao", "equal", "integer", strconv.Itoa(*filters.Odf)))
	}
	if filters.IdUsuarioExecucao != nil {
		rules = append(rules, fmt.Sprintf(advancedFilterRuleTemplate, "idUsuarioExecucao", "equal", "string", *filters.IdUsuarioExecucao))
	}
	if filters.DataExecucao != nil {
		data := *filters.DataExecucao
		rules = append(rules, fmt.Sprintf(advancedFilterRuleTemplate, "dataExecucao", "equal", "date", data.Format("2006-01-02T15:04:05-0700")))
	}
	rules = append(rules, fmt.Sprintf(advancedFilterRuleTemplate, "estorno", "equal", "boolean", strconv.FormatBool(estorno)))

	advancedFilterRule := "["
	for index, rule := range rules {
		if index < (len(rules) - 1) {
			advancedFilterRule += rule + ","
		} else {
			advancedFilterRule += rule
		}
	}
	advancedFilterRule += "]"

	fmt.Println(fmt.Sprintf(advancedFilter, advancedFilterRule))
	return fmt.Sprintf(advancedFilter, advancedFilterRule)
}
