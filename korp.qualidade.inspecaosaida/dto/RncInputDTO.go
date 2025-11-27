package dto

type RncInputDTO struct {
	IdRnc     *string               `json:"idRnc"`
	CodigoRnc *int                  `json:"codigoRnc"`
	Materiais []RncMaterialInputDTO `json:"materiais"`
	Recursos  []RncRecursoInputDTO  `json:"recursos"`
}

type RncMaterialInputDTO struct {
	Quantidade         float64 `json:"quantidade"`
	IdProduto          string  `json:"idProduto"`
	OperacaoEngenharia string  `json:"operacaoEngenharia"`
}

type RncRecursoInputDTO struct {
	IdRecurso          string  `json:"idRecurso"`
	OperacaoEngenharia string  `json:"operacaoEngenharia"`
	Horas              float64 `json:"horas"`
	Detalhamento       string  `json:"detalhamento"`
}
