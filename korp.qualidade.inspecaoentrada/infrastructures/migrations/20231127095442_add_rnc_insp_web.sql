-- +goose Up
-- +goose StatementBegin
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'InspecaoEntradaExecutadoWeb' AND COLUMN_NAME='ID_RNC')
BEGIN
ALTER TABLE InspecaoEntradaExecutadoWeb ADD ID_RNC [nvarchar](450) NULL
END
-- +goose StatementEnd

-- +goose Down
-- +goose StatementBegin
-- +goose StatementEnd
