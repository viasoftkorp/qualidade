package dto

import "bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"

type GetFilesByDomainWithFiltersInput struct {
	FileProviderBaseInput
	models.DefaultFilter
	Subdomains []string
}
