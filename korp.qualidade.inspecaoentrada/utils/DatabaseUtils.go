package utils

import (
	"sync"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/infrastructures"
	"bitbucket.org/viasoftkorp/korp.sdk/ambient_data"
	"gorm.io/gorm"
)

var mux sync.RWMutex
var databases = make(map[string]int)

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

	return err
}
