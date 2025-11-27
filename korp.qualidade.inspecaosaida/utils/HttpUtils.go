package utils

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
	"errors"
	"github.com/gofiber/fiber/v2"
	"strconv"
	"strings"
)

const ApiPrefix = "/qualidade/inspecao-saida"

func GetInputBaseFilterFromCtx(ctx *fiber.Ctx) (*models.BaseFilter, error) {
	var err error
	var skip, pageSize int

	var filter = ctx.Query("filter")
	skip, err = strconv.Atoi(ctx.Query("skip"))
	if err != nil {
		return nil, err
	}
	pageSize, err = strconv.Atoi(ctx.Query("pageSize"))
	if err != nil {
		return nil, err
	}

	var GetInputBaseFilter = models.BaseFilter{
		Filter:   filter,
		Skip:     skip,
		PageSize: pageSize,
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

	companyRecno := ctx.Get("CompanyRecno", "")
	var companyRecnoInt, err = strconv.Atoi(companyRecno)
	if err != nil {
		return nil, errors.New("CompanyRecno header missing")
	}

	userLogin := ctx.Get("UserLogin")

	var baseParams = models.BaseParams{
		TenantId:      tenantId,
		EnvironmentId: environmentId,
		CompanyId:     companyId,
		CompanyRecno:  companyRecnoInt,
		UserId:        userId,
		UserLogin:     userLogin,
	}

	return &baseParams, nil
}

func IsSuccessStatusCode(code int) bool {
	return code >= 200 && code <= 299
}
