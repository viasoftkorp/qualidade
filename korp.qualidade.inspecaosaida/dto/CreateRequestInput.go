package dto

import "bytes"

type CreateRequestInput struct {
	HttpMethod    string
	Uri           string
	Body          *bytes.Buffer
	ContentType   string
	TenantId      string
	EnvironmentId string
	UserId        string
	AppId         string
}
