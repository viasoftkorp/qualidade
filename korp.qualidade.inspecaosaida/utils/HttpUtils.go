package utils

import (
	"context"
	"encoding/json"
	"errors"
	"fmt"
	"io"
	"net/http"
	"strconv"
	"strings"
	"time"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/exceptions"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	authorizationServicesSdk "bitbucket.org/viasoftkorp/korp.sdk/authorization/services"
	"bitbucket.org/viasoftkorp/korp.sdk/rest_client"
	"github.com/gofiber/fiber/v2"
)

const ApiPrefix = "/qualidade/inspecao-saida"

func GetInputBaseFilterFromCtx(ctx *fiber.Ctx) (*models.BaseFilter, error) {
	var err error
	var skip, pageSize int

	var filter = ctx.Query("filter")
	var advancedFilter = ctx.Query("advancedFilter")
	var sorting = ctx.Query("sorting")

	skip, err = strconv.Atoi(ctx.Query("skip"))
	if err != nil {
		return nil, err
	}
	pageSize, err = strconv.Atoi(ctx.Query("pageSize"))
	if err != nil {
		return nil, err
	}

	var GetInputBaseFilter = models.BaseFilter{
		Filter:         filter,
		AdvancedFilter: advancedFilter,
		Sorting:        sorting,
		Skip:           skip,
		PageSize:       pageSize,
	}

	return &GetInputBaseFilter, nil
}

func GetBaseParamsFromCtx(ctx *fiber.Ctx) (*models.BaseParams, error) {
	tenantId := ctx.Get("TenantId", "")
	if strings.TrimSpace(tenantId) == "" {
		return nil, errors.New("TenantId header missing")
	}

	environmentId := ctx.Get("EnvironmentId", "")
	if strings.TrimSpace(tenantId) == "" {
		return nil, errors.New("EnvironmentId header missing")
	}

	companyId := ctx.Get("CompanyId", "")
	if strings.TrimSpace(tenantId) == "" {
		return nil, errors.New("CompanyId header missing")
	}

	userId := ctx.Get("UserId", "")
	if strings.TrimSpace(tenantId) == "" {
		return nil, errors.New("UserId header missing")
	}

	legacyCompanyId := ctx.Get("LegacyCompanyId", "")
	var legacyCompanyIdAsInt, err = strconv.Atoi(legacyCompanyId)
	if err != nil {
		return nil, errors.New("LegacyCompanyId header missing")
	}

	userLogin := ctx.Get("UserLogin")

	var baseParams = models.BaseParams{
		TenantId:        tenantId,
		EnvironmentId:   environmentId,
		CompanyId:       companyId,
		LegacyCompanyId: legacyCompanyIdAsInt,
		UserId:          userId,
		UserLogin:       userLogin,
	}

	return &baseParams, nil
}

func IsSuccessStatusCode(code int) bool {
	return code >= 200 && code <= 299
}

func BaseFilterToHttpQueryParams(filter models.BaseFilter) string {
	var result = "?skipcount=" + strconv.Itoa(filter.Skip) + "&MaxResultCount=" + strconv.Itoa(filter.PageSize)

	if !IsEmptyOrWhitespace(filter.Filter) {
		result += "&filter=" + filter.Filter
	}
	if !IsEmptyOrWhitespace(filter.AdvancedFilter) {
		result += "&advancedfilter=" + filter.AdvancedFilter
	}
	if !IsEmptyOrWhitespace(filter.Sorting) {
		result += "&sorting=\"" + filter.Sorting + "\""
	}
	return result
}

func GetErrorFromBody(body []byte) (expected error, unexpectedError error) {
	var errorMessage exceptions.ErrorMessage
	unexpectedError = json.Unmarshal(body, &errorMessage)
	if unexpectedError != nil {
		return nil, unexpectedError
	}
	expected = errors.New(errorMessage.Error)
	return expected, unexpectedError
}

func CreateRequest(input models.CreateRequestInput) (output *http.Request, err error) {
	r, err := http.NewRequest(input.HttpMethod, input.Uri, input.Body)
	if err != nil {
		return nil, err
	}
	r.Header.Set("Content-Type", input.ContentType)
	r.Header.Set("TenantId", input.TenantId)
	r.Header.Set("EnvironmentId", input.EnvironmentId)
	r.Header.Set("UserId", input.UserId)
	return r, nil
}

func DoRequest(request *http.Request) ([]byte, error) {
	var body []byte
	var err error

	client := rest_client.CreateHttpClient(time.Second * 60)

	accessToken, err := authorizationServicesSdk.GetAccessToken(context.Background(), "Viasoft.Reporting")
	if err != nil {
		return nil, err
	}
	request.Header.Add("Authorization", fmt.Sprintf("Bearer %s", accessToken))

	res, err := client.Do(request)
	if err != nil {
		return nil, err
	}

	defer func(Body io.ReadCloser) {
		err := Body.Close()
		if err != nil {

		}
	}(res.Body)

	body, err = io.ReadAll(res.Body)
	if err != nil {
		return nil, err
	}

	if res.StatusCode != 200 && res.StatusCode != 201 {
		errorFromBody, err := GetErrorFromBody(body)
		if err != nil {
			return nil, err
		}
		return nil, errorFromBody
	}

	return body, nil
}
