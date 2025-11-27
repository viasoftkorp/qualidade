package interfaces

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
)

type IFileProviderProxyService interface {
	UploadFiles(input *dto.FileProviderBaseInput) (int, error)
	GetFilesByDomainWithFilters(input *dto.GetFilesByDomainWithFiltersInput) (int, *dto.PagedProvidedFileGetMultipleWithFiltersOutput, error)
	DeleteFile(id string) (int, error)
	DownloadFile(id string) (int, error)
}
