package dto

type CreateDefaultReportTemplateInput struct {
	ReportId    string
	Description string
	Template    []byte
}
