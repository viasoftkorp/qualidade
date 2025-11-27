package dto

type FinalizarInspecaoInput struct {
	CodInspecao            int          `json:"codInspecao"`
	QuantidadeAprovada     float64      `json:"quantidadeAprovada"`
	QuantidadeReprovada    float64      `json:"quantidadeReprovada"`
	QuantidadeRetrabalhada float64      `json:"quantidadeRetrabalhada"`
	CodigoLocalAprovado    int          `json:"codigoLocalAprovado"`
	CodigoLocalReprovado   int          `json:"codigoLocalReprovado"`
	CodigoLocalRetrabalho  int          `json:"codigoLocalRetrabalho"`
	Rnc                    *RncInputDTO `json:"rnc"`
}
