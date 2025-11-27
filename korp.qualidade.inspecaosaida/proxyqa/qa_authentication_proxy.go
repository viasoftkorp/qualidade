package proxyqa

import (
	"bytes"
	"fmt"
	"net/http"
	"strings"
	"time"

	korpCoreAuthorization "bitbucket.org/viasoftkorp/korp.sdk/authorization/services"
	"bitbucket.org/viasoftkorp/korp.sdk/environment"
	"bitbucket.org/viasoftkorp/korp.sdk/proxy"
	"bitbucket.org/viasoftkorp/korp.sdk/rest_client"
	"github.com/gofiber/fiber/v2"
)

func QaKorpAuthenticationProxy(apiPrefix string) fiber.Handler {
	return func(c *fiber.Ctx) error {
		gateway, err := environment.GetGateway()
		if err != nil {
			return err
		}

		requestPath := string(c.Request().URI().Path())
		requestURI := gateway + requestPath
		httpMethod := c.Method()

		var accessToken string

		if strings.HasPrefix(requestPath, apiPrefix+"authentication/") {
			accessToken, err = korpCoreAuthorization.GetAccessToken(c.Context(), "Viasoft.Authentication")
			if err != nil {
				return err
			}

			requestURI, httpMethod = GetUserEndpoint(requestURI, requestPath, apiPrefix, httpMethod)
		} else {
			return c.Next()
		}

		if requestURI == "" || httpMethod == "" {
			return c.Status(http.StatusBadRequest).JSON(nil)
		}

		body := bytes.NewBuffer(c.Body())
		requestInput := proxy.CreateRequestInput{
			HttpMethod:    httpMethod,
			Uri:           requestURI,
			ContentType:   c.Get("Content-Type"),
			TenantId:      c.Get("TenantId"),
			UserId:        c.Get("UserId"),
			Body:          body,
			AppId:         c.Get("AppId"),
			EnvironmentId: c.Get("EnvironmentId"),
		}

		r, err := createRequest(requestInput)
		if err != nil {
			return err
		}

		response, err := doRequest(r, accessToken)
		if err != nil {
			return err
		}

		defer response.Body.Close()

		contentType := response.Header.Get("Content-Type")
		c.Set("Content-Type", contentType)

		return c.Status(response.StatusCode).SendStream(response.Body)
	}
}

func GetUserEndpoint(requestURI string, requestPath string, apiPrefix string, httpMethod string) (string, string) {
	if httpMethod == "GET" {
		if strings.Contains(requestPath, "image") {
			posFirst := strings.Index(requestPath, "users/")
			posLast := strings.Index(requestPath, "/image")
			userId := requestPath[posFirst+6 : posLast]
			return strings.ReplaceAll(requestURI, apiPrefix+"authentication/users/"+userId+"/image",
				"/oauth/users/"+userId+"/image"), httpMethod
		}

		sptResult := make([]string, 0)
		if strings.Contains(requestPath, "users/") {
			sptResult = strings.Split(requestPath, "users/")
			if len(sptResult[1]) == 0 {
				sptResult = make([]string, 0)
			}
		}

		if len(sptResult) > 1 {
			return strings.ReplaceAll(requestURI, apiPrefix+"authentication/users", "/oauth/Users"), httpMethod
		} else {
			return strings.ReplaceAll(requestURI, apiPrefix+"authentication/users", "/oauth/Users/GetAll"), httpMethod
		}
	}

	return "", ""
}

func createRequest(input proxy.CreateRequestInput) (output *http.Request, err error) {
	r, err := http.NewRequest(input.HttpMethod, input.Uri, input.Body)
	if err != nil {
		return nil, err
	}
	r.Header.Set("Content-Type", input.ContentType)
	r.Header.Set("TenantId", input.TenantId)
	r.Header.Set("EnvironmentId", input.EnvironmentId)
	r.Header.Set("UserId", input.UserId)
	r.Header.Set("AppId", input.AppId)
	return r, nil
}

func doRequest(request *http.Request, accessToken string) (*http.Response, error) {
	client := rest_client.CreateHttpClient(time.Second * 60)
	request.Header.Add("Authorization", fmt.Sprintf("Bearer %s", accessToken))

	res, err := client.Do(request)
	if err != nil {
		return nil, err
	}

	return res, nil
}
