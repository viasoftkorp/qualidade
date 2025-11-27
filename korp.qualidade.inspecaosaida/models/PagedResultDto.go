package models

type PagedResultDto[T any] struct {
	Items      []T `json:"items"`
	TotalCount int `json:"totalCount"`
}
