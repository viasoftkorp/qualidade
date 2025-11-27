package dto

type GetProdutos struct {
	Items      []ProdutoOutput `json:"items"`
	TotalCount int64           `json:"totalCount"`
}

type ProdutoOutput struct {
	Id        string `json:"id"`
	Codigo    string `json:"codigo"`
	Descricao string `json:"descricao"`
	Unidade   string `json:"unidade"`
}
