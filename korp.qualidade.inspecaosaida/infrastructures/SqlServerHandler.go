package infrastructures

import (
	"database/sql"
	"embed"

	"github.com/pressly/goose/v3"
)

//go:embed migrations/*.sql
var embedMigrations embed.FS

const TabelaVersionamentoMigrations = "INSP_SAIDA_WEB_MIGRATIONS_HISTORY"

func RunMigrateFiles(db *sql.DB) error {

	err := goose.SetDialect("mssql")
	if err != nil {
		return err
	}
	goose.SetBaseFS(embedMigrations)

	goose.SetTableName(TabelaVersionamentoMigrations)
	err = goose.Up(db, "migrations", goose.WithAllowMissing())
	if err != nil {
		return err
	}
	return nil
}
