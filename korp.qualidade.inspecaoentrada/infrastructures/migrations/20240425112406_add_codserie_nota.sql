-- +goose Up
-- +goose StatementBegin
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'QA_INSPECAO_ENTRADA' AND COLUMN_NAME='CODSERIE')
BEGIN
ALTER TABLE QA_INSPECAO_ENTRADA ADD CODSERIE [varchar](5) NULL
END
-- +goose StatementEnd

-- +goose Down
-- +goose StatementBegin
-- +goose StatementEnd
