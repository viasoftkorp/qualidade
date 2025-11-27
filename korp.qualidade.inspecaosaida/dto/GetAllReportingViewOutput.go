package dto

import "time"

type GetAllReportingViewOutput struct {
	IsFavorite            bool       `json:"isFavorite"`
	TenantId              string     `json:"tenantId"`
	AppId                 string     `json:"appId"`
	ReportingEngine       int        `json:"reportingEngine"`
	ReportingType         int        `json:"reportingType"`
	IsDefault             bool       `json:"isDefault"`
	ReportId              string     `json:"reportId"`
	Extension             string     `json:"extension"`
	Description           string     `json:"description"`
	Domain                int        `json:"domain"`
	Area                  string     `json:"area"`
	IsCustom              bool       `json:"isCustom"`
	LastExecutionDateTime *time.Time `json:"lastExecutionDateTime,omitempty"`
	LastExecutionUserId   string     `json:"lastExecutionUserId"`
	LastExecutionUsername string     `json:"lastExecutionUsername"`
	CreationTime          *time.Time `json:"creationTime,omitempty"`
	CreatorId             string     `json:"creatorId"`
	LastModificationTime  *time.Time `json:"lastModificationTime,omitempty"`
	LastModifierId        string     `json:"lastModifierId"`
	Id                    string     `json:"id"`
	ActiveRevisionId      string     `json:"activeRevisionId"`
}

type GetAllReportingViewOutputPagedResultDto struct {
	Items      []*GetAllReportingViewOutput `json:"items"`
	TotalCount int                          `json:"totalCount"`
}
