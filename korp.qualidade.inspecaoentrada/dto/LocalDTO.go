package dto

type GetLocais struct {
	Items      []LocalOutput `json:"items"`
	TotalCount int64         `json:"totalCount"`
}

type LocalOutput struct {
	Id        string `json:"id"`
	Codigo    string `json:"codigo"`
	Descricao string `json:"descricao"`
}
