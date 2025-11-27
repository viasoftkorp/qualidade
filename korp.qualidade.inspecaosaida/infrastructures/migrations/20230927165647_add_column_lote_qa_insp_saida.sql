-- +goose Up
-- +goose StatementBegin
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'QA_INSPECAO_SAIDA' AND COLUMN_NAME='LOTE')
BEGIN
ALTER TABLE QA_INSPECAO_SAIDA ADD LOTE [nvarchar](50) NULL
END
-- +goose StatementEnd

-- +goose Down
-- +goose StatementBegin
-- +goose StatementEnd
