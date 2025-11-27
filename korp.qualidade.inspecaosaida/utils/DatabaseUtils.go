package utils

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/infrastructures"
	"bitbucket.org/viasoftkorp/korp.sdk/ambient_data"
	"gorm.io/gorm"
)

func MigrateEntities(db *gorm.DB, ambientDataModel *ambient_data.AmbientDataModel) error {
	return migrate(db)
}

func migrate(db *gorm.DB) error {
	con, err := db.DB()
	if err != nil {
		return err
	}

	err = infrastructures.RunMigrateFiles(con)
	if err != nil {
		return err
	}

	return nil
}
