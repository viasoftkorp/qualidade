-- +goose Up
-- +goose StatementBegin
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InspecaoSaidaExecutadoWeb]'))
BEGIN
CREATE TABLE [dbo].[InspecaoSaidaExecutadoWeb](
	[id] [nvarchar](256) NOT NULL,
	[RECNO_INSPECAO_SAIDA] [int] NULL,
	[ID_INSPECAO_SAIDA_SAGA] [nvarchar](450) NULL,
	[ESTORNO] [bit] NULL,
	[QUANTIDADE_TRANSFERIDA] [decimal](19, 6) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QA_INSPECAO_SAIDA_PEDIDO_VENDA_WEB]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[QA_INSPECAO_SAIDA_PEDIDO_VENDA_WEB]
END
-- +goose StatementEnd

-- +goose Down
-- +goose StatementBegin
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InspecaoSaidaExecutadoWeb]') AND type in (N'U'))
BEGIN
DROP TABLE [dbo].[InspecaoSaidaExecutadoWeb]
END
-- +goose StatementEnd
