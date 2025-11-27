package dto

import (
	"github.com/google/uuid"
	"time"
)

type PagedProvidedFileGetMultipleWithFiltersOutput struct {
	TotalCount int                                        `json:"totalCount"`
	Items      []ProvidedFileGetMultipleWithFiltersOutput `json:"items"`
}

type ProvidedFileGetMultipleWithFiltersOutput struct {
	Id                    uuid.UUID  `json:"id"`
	Filename              string     `json:"filename"`
	Name                  string     `json:"name"`
	ContentType           string     `json:"contentType"`
	FileSize              int        `json:"fileSize"`
	Extension             string     `json:"extension"`
	CreationTime          time.Time  `json:"creationTime"`
	CreatorId             *uuid.UUID `json:"creatorId"`
	LastModification      *time.Time `json:"lastModification"`
	LastVisualizationTime *time.Time `json:"lastVisualizationTime"`
	UserName              string     `json:"userName"`
	Subdomain             string     `json:"subdomain"`
	Tags                  []TagDto   `json:"tags"`
}

type TagDto struct {
	Id    uuid.UUID `json:"id"`
	Name  string    `json:"name"`
	Color string    `json:"color"`
}
