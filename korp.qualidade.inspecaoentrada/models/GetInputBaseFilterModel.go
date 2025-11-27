package models

type BaseFilter struct {
	Filter         string
	AdvancedFilter string
	Sorting        string
	Skip           int
	PageSize       int
}
