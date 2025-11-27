package dto

type FileProviderBaseInput struct {
	AppId       string
	Domain      string
	SubDomain   string
	ForceUpload string
	File        FileUploadRequest
}

type FileUploadRequest struct {
	FileName    string `json:"fileName"`
	FileContent string `json:"fileContent"` // O conteúdo do arquivo como string base64
}
