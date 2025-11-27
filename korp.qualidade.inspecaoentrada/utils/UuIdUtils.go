package utils

import (
	"flag"
	"github.com/google/uuid"
	"strings"
)

func NewGuidAsString() string {
	if flag.Lookup("test.v") == nil {
		guid := strings.ToUpper(uuid.New().String())
		return guid
	} else {
		return "GuidTest"
	}
}
