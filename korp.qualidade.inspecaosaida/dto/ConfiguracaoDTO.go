package dto

import "github.com/google/uuid"

type ConfiguracaoDto struct {
	Id                         uuid.UUID `json:"id"`
	UsarNotaImpressaoRelatorio bool      `json:"usarNotaImpressaoRelatorio"`
}
