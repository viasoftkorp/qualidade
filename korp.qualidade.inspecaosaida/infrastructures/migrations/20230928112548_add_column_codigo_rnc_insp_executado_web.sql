-- +goose Up
-- +goose StatementBegin
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'InspecaoSaidaExecutadoWeb' AND COLUMN_NAME='CODIGO_RNC')
BEGIN
ALTER TABLE InspecaoSaidaExecutadoWeb ADD CODIGO_RNC [int] NULL
END
-- +goose StatementEnd

-- +goose Down
-- +goose StatementBegin
-- +goose StatementEnd
