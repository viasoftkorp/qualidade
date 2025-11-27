package dto

import "bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/models"

type GetFilesByDomainWithFiltersInput struct {
	FileProviderBaseInput
	models.BaseFilter
	Subdomains []string
}
