package interfaces

//go:generate mockgen -destination=../mocks/mock_ImpressaoService.go -package=mocks bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/interfaces IImpressaoService
//go:generate mockgen -destination=../mocks/mock_unit_of_work.go -package=mocks bitbucket.org/viasoftkorp/korp.sdk/unit-of-work UnitOfWork
type IImpressaoService interface {
	ExportReportStimulsoft(report interface{}, reportId string) ([]byte, error)
}
