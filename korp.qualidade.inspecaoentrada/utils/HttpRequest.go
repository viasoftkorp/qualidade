package utils

import (
	"bytes"
	"context"
	"encoding/json"
	"errors"
	"fmt"
	"io"
	"net/http"
	"net/url"
	"time"

	korpCoreAuthorization "bitbucket.org/viasoftkorp/korp.sdk/authorization/services"
	"bitbucket.org/viasoftkorp/korp.sdk/rest_client"
)

var (
	ErrUrlNotFound              = errors.New("url not found")
	ErrUnsuccessfullyStatusCode = errors.New("unsuccessfully status code")
	ErrUnexpectedResponse       = errors.New("unexpected response")
)

type (
	Manager struct {
		Resp        *http.Response
		Req         *http.Request
		RequestTime time.Duration
		Error       error
		StatusCode  int
		BodyBytes   []byte
	}

	HttpRequest struct {
		exception   any
		body        any
		client      *http.Client
		header      *http.Header
		timeout     time.Duration
		query       *url.Values
		context     context.Context
		method      string
		endpoint    string
		serviceName string
		acessToken  string
	}

	IHttpRequest interface {
		WithTimeout(timeOut time.Duration) IHttpRequest
		WithHttpHeader(header *http.Header) IHttpRequest
		WithAcessToken(acessToken string) IHttpRequest
		WithBody(body any) IHttpRequest
		WithQuery(query *url.Values) IHttpRequest
		WithContext(context context.Context) IHttpRequest
		WithException(exception any) IHttpRequest
		WithClient(client *http.Client) IHttpRequest
		WithServiceName(serviceName string) IHttpRequest
		Call(response any) *Manager
	}
)

func NewRequest(method string, endpoint string) IHttpRequest {
	return &HttpRequest{method: method, endpoint: endpoint}
}

func (h *HttpRequest) WithTimeout(timeOut time.Duration) IHttpRequest {
	h.timeout = timeOut
	return h
}

func (h *HttpRequest) WithHttpHeader(header *http.Header) IHttpRequest {
	h.header = header
	return h
}

func (h *HttpRequest) WithAcessToken(acessToken string) IHttpRequest {
	h.acessToken = acessToken
	return h
}

func (h *HttpRequest) WithBody(body any) IHttpRequest {
	h.body = body
	return h
}

func (h *HttpRequest) WithQuery(query *url.Values) IHttpRequest {
	h.query = query
	return h
}

func (h *HttpRequest) WithContext(context context.Context) IHttpRequest {
	h.context = context
	return h
}

func (h *HttpRequest) WithException(exception any) IHttpRequest {
	h.exception = exception
	return h
}

func (h *HttpRequest) WithClient(client *http.Client) IHttpRequest {
	h.client = client
	return h
}

func (h *HttpRequest) WithServiceName(serviceName string) IHttpRequest {
	h.serviceName = serviceName
	return h
}

func (h *HttpRequest) Call(response any) *Manager {
	var httpManager Manager
	httpManager.Error = h.setDefaultValues()
	if httpManager.Error != nil {
		return &httpManager
	}

	var body []byte
	if h.body != nil {
		var isByte bool
		body, isByte = h.body.([]byte)
		if !isByte {
			body, httpManager.Error = json.Marshal(h.body)
			if httpManager.Error != nil {
				return &httpManager
			}
		}
	}

	httpManager.Req, httpManager.Error = http.NewRequest(h.method, h.endpoint, bytes.NewBuffer(body))
	if httpManager.Error != nil {
		return &httpManager
	}

	if h.header != nil {
		httpManager.Req.Header = *h.header
	}

	httpManager.Req.Header.Add("Authorization", fmt.Sprintf("Bearer %s", h.acessToken))

	if h.query != nil {
		httpManager.Req.URL.RawQuery = h.query.Encode()
	}

	startRequest := time.Now()
	httpManager.Resp, httpManager.Error = h.client.Do(httpManager.Req)
	if httpManager.Error != nil {
		return &httpManager
	}
	httpManager.RequestTime = time.Since(startRequest)
	httpManager.StatusCode = httpManager.Resp.StatusCode

	defer httpManager.Resp.Body.Close()
	body, httpManager.Error = io.ReadAll(httpManager.Resp.Body)
	if httpManager.Error != nil {
		return &httpManager
	}
	httpManager.BodyBytes = body

	if !h.isSuccessStatusCode(httpManager.StatusCode) {
		if httpManager.StatusCode == http.StatusNotFound {
			httpManager.Error = ErrUrlNotFound
			return &httpManager
		}

		if h.exception != nil {
			httpManager.Error = h.unmarshal(body, h.exception)
			if httpManager.Error != nil {
				return &httpManager
			}
		}

		httpManager.Error = ErrUnsuccessfullyStatusCode
		return &httpManager
	}

	if response != nil {
		httpManager.Error = h.unmarshal(body, response)
		if httpManager.Error != nil {
			return &httpManager
		}
	}
	return &httpManager
}

func (h *HttpRequest) setDefaultValues() error {
	var err error
	if h.context == nil {
		h.context = context.Background()
	}

	if h.client == nil {
		h.client = rest_client.CreateHttpClient(time.Second * 60)
	}

	if h.timeout.Seconds() == 0 {
		h.timeout = time.Second * 600
	}

	h.client.Timeout = h.timeout

	if h.serviceName != "" && h.acessToken == "" {
		h.acessToken, err = korpCoreAuthorization.GetAccessToken(h.context, h.serviceName)
		if err != nil {
			return err
		}
	}
	return nil
}

func (h *HttpRequest) isSuccessStatusCode(code int) bool {
	return code >= 200 && code <= 299
}

func (h *HttpRequest) unmarshal(body []byte, decode any) error {
	err := json.Unmarshal(body, decode)
	if err != nil {
		switch err.(type) {
		case *json.SyntaxError:
			return ErrUnexpectedResponse
		default:
			return err
		}
	}
	return nil
}
