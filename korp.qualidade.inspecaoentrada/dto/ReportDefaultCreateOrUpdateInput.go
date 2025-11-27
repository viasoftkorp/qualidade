package dto

type ReportDefaultCreateOrUpdateInput struct {
	ReportingEngine int
	ReportingType   int
	ReportId        string
	Extension       string
	Description     string
	Domain          int
	Area            string
	AppId           string
	Template        []byte
}
