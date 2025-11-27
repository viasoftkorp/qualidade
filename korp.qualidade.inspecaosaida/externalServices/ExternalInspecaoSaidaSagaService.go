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
	request.Header.Set("LegacyCompanyId", strconv.Itoa(service.Params.LegacyCompanyId))
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

func (service *ExternalInspecaoSaidaSagaService) BuscarSagas(baseFilters *models.BaseFilter, filters *dto.ProcessamentoInspecaoSaidaFilters, estorno bool) (*dto.GetAllSagaInspecaoSaidaOutput, error) {
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
	query.Add("MaxResultCount", strconv.Itoa(baseFilters.PageSize))
	query.Add("SkipCount", strconv.Itoa(baseFilters.Skip))
	query.Add("Sorting", baseFilters.Sorting)
	query.Add("AdvancedFilter", service.GetAdvancedFilter(baseFilters, filters, estorno))

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

func (service *ExternalInspecaoSaidaSagaService) GetAdvancedFilter(baseFilters *models.BaseFilter, filters *dto.ProcessamentoInspecaoSaidaFilters, estorno bool) string {
	if (baseFilters == nil || baseFilters.AdvancedFilter == "") && (filters == nil) {
		return ""
	}

	var deserializedAdvancedFilter models.AdvancedFilter
	var serializedAdvancedFilter []byte

	if baseFilters != nil && baseFilters.AdvancedFilter != "" {
		serializedAdvancedFilter = []byte(baseFilters.AdvancedFilter)
	} else {
		serializedAdvancedFilter = []byte(`{"condition": "and", "rules": "[]"}`)
	}

	json.Unmarshal(serializedAdvancedFilter, &deserializedAdvancedFilter)

	if filters != nil {
		if filters.Status != nil {
			deserializedAdvancedFilter.Rules = append(deserializedAdvancedFilter.Rules, models.Rules{ Field: "status", Operator: "equal", Type: "integer", Value: strconv.Itoa(*filters.Status) })
		}
		if filters.NumeroExecucoes != nil {
			deserializedAdvancedFilter.Rules = append(deserializedAdvancedFilter.Rules, models.Rules{ Field: "numeroExecucoes", Operator: "equal", Type: "integer", Value: strconv.Itoa(*filters.NumeroExecucoes) })
		}
		if filters.Erro != nil {
			deserializedAdvancedFilter.Rules = append(deserializedAdvancedFilter.Rules, models.Rules{ Field: "erro", Operator: "contains", Type: "string", Value: *filters.Erro })
		}
		if filters.Resultado != nil {
			deserializedAdvancedFilter.Rules = append(deserializedAdvancedFilter.Rules, models.Rules{ Field: "resultado", Operator: "contains", Type: "string", Value: *filters.Resultado })
		}
		if filters.QuantidadeTotal != nil {
			deserializedAdvancedFilter.Rules = append(deserializedAdvancedFilter.Rules, models.Rules{ Field: "quantidadeTotal", Operator: "equal", Type: "double", Value: fmt.Sprintf("%f", *filters.QuantidadeTotal) })
		}
		if filters.CodigoProduto != nil {
			deserializedAdvancedFilter.Rules = append(deserializedAdvancedFilter.Rules, models.Rules{ Field: "codigoProduto", Operator: "contains", Type: "string", Value: *filters.CodigoProduto })
		}
		if filters.Odf != nil {
			deserializedAdvancedFilter.Rules = append(deserializedAdvancedFilter.Rules, models.Rules{ Field: "ordemFabricacao", Operator: "equal", Type: "integer", Value: strconv.Itoa(*filters.Odf) })
		}
		if filters.IdUsuarioExecucao != nil {
			deserializedAdvancedFilter.Rules = append(deserializedAdvancedFilter.Rules, models.Rules{ Field: "idUsuarioExecucao", Operator: "equal", Type: "string", Value: *filters.IdUsuarioExecucao })
		}
		if filters.DataExecucao != nil {
			data := *filters.DataExecucao
			deserializedAdvancedFilter.Rules = append(deserializedAdvancedFilter.Rules, models.Rules{ Field: "dataExecucao", Operator: "equal", Type: "date", Value: data.Format("2006-01-02T15:04:05-0700") })
		}
	}

	deserializedAdvancedFilter.Rules = append(deserializedAdvancedFilter.Rules, models.Rules{ Field: "estorno", Operator: "equal", Type: "boolean", Value: strconv.FormatBool(estorno) })

	serializedAdvancedFilter, _ = json.Marshal(deserializedAdvancedFilter)

	return string(serializedAdvancedFilter)
}
