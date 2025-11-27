package services

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	korpCoreAuthorization "bitbucket.org/viasoftkorp/korp.sdk/authorization/services"
	"bitbucket.org/viasoftkorp/korp.sdk/environment"
	"bitbucket.org/viasoftkorp/korp.sdk/rest_client"
	"context"
	"github.com/gofiber/fiber/v2"
	"io"
	"mime/multipart"
	"net/http"
	"strconv"
	"time"
)

const SERVICE_NAME = "Viasoft.FileProvider"

type FileProviderProxyService struct {
	BaseParams *models.BaseParams
	Ctx        *fiber.Ctx
}

func NewFileProviderProxyService(baseParams *models.BaseParams, ctx *fiber.Ctx) interfaces.IFileProviderProxyService {
	return &FileProviderProxyService{
		BaseParams: baseParams,
		Ctx:        ctx,
	}
}

func (service *FileProviderProxyService) UploadFiles(input *dto.FileProviderBaseInput) (int, error) {
	var basePath, err = service.getBasePath()
	if err != nil {
		return http.StatusInternalServerError, err
	}
	var endpoint = basePath + "/app/" + input.AppId

	if !utils.IsEmptyOrWhitespace(input.Domain) {
		endpoint += "/domain/" + input.Domain
	}

	if !utils.IsEmptyOrWhitespace(input.SubDomain) {
		endpoint += "/subdomain/" + input.SubDomain
	}

	endpoint += "/file"

	if !utils.IsEmptyOrWhitespace(input.ForceUpload) {
		endpoint += "?forceupload=" + input.ForceUpload
	}

	body, w := io.Pipe()
	writer := multipart.NewWriter(w)

	var writerUtils = utils.NewWriterUtils(writer, w)

	multipartForm, err := service.Ctx.MultipartForm()

	if err != nil {
		return http.StatusInternalServerError, err
	}

	err = writerUtils.CopyFilesByMultipartForm(*multipartForm)

	if err != nil {
		return http.StatusInternalServerError, err
	}

	var headers = service.getDefaultHeaders()

	var contentType = writer.FormDataContentType()
	headers.Set("Content-Type", contentType)

	req, err := http.NewRequest("POST", endpoint, body)

	if err != nil {
		return http.StatusInternalServerError, err
	}

	req.Header = headers
	token, err := service.getToken()
	if err != nil {
		return http.StatusInternalServerError, err
	}
	req.Header.Set("Authorization", "Bearer "+token)

	client := rest_client.CreateHttpClient(time.Second * 60)

	resp, err := client.Do(req)

	if err != nil {
		return http.StatusInternalServerError, err
	}

	defer func(Body io.ReadCloser) {
		Body.Close()
	}(resp.Body)

	return resp.StatusCode, nil
}

func (service *FileProviderProxyService) GetFilesByDomainWithFilters(input *dto.GetFilesByDomainWithFiltersInput) (int, *dto.PagedProvidedFileGetMultipleWithFiltersOutput, error) {
	var basePath, err = service.getBasePath()
	if err != nil {
		return http.StatusInternalServerError, nil, err
	}

	var endpoint = basePath + "/app/" + input.AppId + "/domains"

	var headers = service.getDefaultHeaders()

	token, err := service.getToken()

	if err != nil {
		return http.StatusInternalServerError, nil, err
	}
	var output *dto.PagedProvidedFileGetMultipleWithFiltersOutput

	var request = utils.NewRequest(http.MethodPost, endpoint).
		WithHttpHeader(&headers).
		WithTimeout(time.Second * 15).
		WithAcessToken(token).
		WithBody(input).
		Call(&output)

	err = request.Error

	if err != nil {
		return http.StatusInternalServerError, nil, err
	}
	return request.StatusCode, output, nil
}

func (service *FileProviderProxyService) DeleteFile(id string) (int, error) {
	var basePath, err = service.getBasePath()
	if err != nil {
		return http.StatusInternalServerError, err
	}
	var endpoint = basePath + "/file/" + id

	var headers = service.getDefaultHeaders()

	token, err := service.getToken()

	if err != nil {
		return http.StatusInternalServerError, err
	}

	var request = utils.NewRequest(http.MethodDelete, endpoint).
		WithHttpHeader(&headers).
		WithTimeout(time.Second * 15).
		WithAcessToken(token).
		Call(nil)

	err = request.Error

	if err != nil {
		return http.StatusInternalServerError, err
	}

	return request.StatusCode, nil
}

func (service *FileProviderProxyService) DownloadFile(id string) (int, error) {
	var basePath, err = service.getBasePath()
	if err != nil {
		return http.StatusInternalServerError, err
	}
	var endpoint = basePath + "/file/" + id + "/download"

	var headers = service.getDefaultHeaders()

	token, err := service.getToken()
	if err != nil {
		return http.StatusInternalServerError, err
	}

	headers.Add("Authorization", "Bearer "+token)

	req, err := http.NewRequest(http.MethodGet, endpoint, nil)
	if err != nil {
		return http.StatusInternalServerError, err
	}
	req.Header = headers

	client := rest_client.CreateHttpClient(time.Second * 60)

	resp, err := client.Do(req)
	if err != nil {
		return http.StatusInternalServerError, err
	}
	defer resp.Body.Close()
	contentDisposition := resp.Header.Get("Content-Disposition")
	contentType := resp.Header.Get("Content-Type")

	service.Ctx.Set("Content-Type", contentType)
	service.Ctx.Set("Content-Disposition", contentDisposition)
	service.Ctx.Set("Access-Control-Expose-Headers", "Content-Type, Content-Disposition")

	err = service.Ctx.SendStream(resp.Body)
	return resp.StatusCode, err
}

func (service *FileProviderProxyService) getToken() (string, error) {
	token, err := korpCoreAuthorization.GetAccessToken(context.Background(), SERVICE_NAME)

	return token, err
}

func (service *FileProviderProxyService) getDefaultHeaders() http.Header {
	header := make(http.Header)
	header.Add("TenantId", service.BaseParams.TenantId)
	header.Add("EnvironmentId", service.BaseParams.EnvironmentId)
	header.Add("LegacyCompanyId", strconv.Itoa(service.BaseParams.LegacyCompanyId))
	header.Add("CompanyId", service.BaseParams.CompanyId)
	header.Add("UserLogin", service.BaseParams.UserLogin)
	header.Add("UserId", service.BaseParams.UserId)
	header.Add("Content-Type", fiber.MIMEApplicationJSON)

	return header
}
func (service *FileProviderProxyService) getBasePath() (string, error) {
	const BASE_PATH = "FileProvider"
	gateway, err := environment.GetGateway()
	if err != nil {
		return "", err
	}
	return gateway + "/" + BASE_PATH, nil
}
