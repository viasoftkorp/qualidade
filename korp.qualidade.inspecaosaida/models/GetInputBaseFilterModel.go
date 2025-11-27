package models

type BaseFilter struct {
	AdvancedFilter string `json:"advancedFilter"`
	Filter         string `json:"filter"`
	Sorting        string `json:"sorting"`
	Skip           int    `json:"skip"`
	PageSize       int    `json:"pageSize"`
}

type DefaultFilter struct {
	AdvancedFilter string `json:"advancedFilter"`
	Filter         string `json:"filter"`
	Sorting        string `json:"sorting"`
	SkipCount      int    `json:"skipCount"`
	MaxResultCount int    `json:"maxResultCount"`
}
