package utils

import (
	"encoding/json"
	"fmt"
	"strings"
	"time"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/models"
)

func ApplyAdvancedFilter(advancedFilter string) string {
	var conditions []string

	if advancedFilter == "" {
		return ""
	}

	var deserializedAdvancedFilter models.AdvancedFilter
	json.Unmarshal([]byte(advancedFilter), &deserializedAdvancedFilter)

	for _, rule := range deserializedAdvancedFilter.Rules {
		if len(rule.Rules) == 0 {
			var value = NormalizeAdvancedFilterValue(rule.Value)
			switch rule.Operator {
			case "contains":
				conditions = append(conditions, fmt.Sprintf("%s LIKE '%s'", rule.Field, "%"+value+"%"))
			case "equal":
				conditions = append(conditions, fmt.Sprintf("%s = '%s'", strings.ToUpper(rule.Field), value))
			case "not_equal":
				conditions = append(conditions, fmt.Sprintf("%s != '%s'", strings.ToUpper(rule.Field), value))
			case "greater":
				conditions = append(conditions, fmt.Sprintf("%s > '%s'", strings.ToUpper(rule.Field), value))
			case "greater_or_equal":
				conditions = append(conditions, fmt.Sprintf("%s >= '%s'", strings.ToUpper(rule.Field), value))
			case "less":
				conditions = append(conditions, fmt.Sprintf("%s < '%s'", strings.ToUpper(rule.Field), value))
			case "less_or_equal":
				conditions = append(conditions, fmt.Sprintf("%s <= '%s'", strings.ToUpper(rule.Field), value))
			case "is_null":
				conditions = append(conditions, fmt.Sprintf("%s is null", strings.ToUpper(rule.Field)))
			case "is_not_null":
				conditions = append(conditions, fmt.Sprintf("%s is not null", strings.ToUpper(rule.Field)))
			case "is_empty":
				conditions = append(conditions, fmt.Sprintf("%s = ''", strings.ToUpper(rule.Field)))
			case "is_not_empty":
				conditions = append(conditions, fmt.Sprintf("%s != ''", strings.ToUpper(rule.Field)))
			case "begins_with":
				conditions = append(conditions, fmt.Sprintf("%s LIKE '%s'", strings.ToUpper(rule.Field), value+"%"))
			case "not_begins_with":
				conditions = append(conditions, fmt.Sprintf("%s NOT LIKE '%s'", strings.ToUpper(rule.Field), value+"%"))
			case "ends_with":
				conditions = append(conditions, fmt.Sprintf("%s LIKE '%s'", strings.ToUpper(rule.Field), "%"+value))
			case "not_ends_with":
				conditions = append(conditions, fmt.Sprintf("%s NOT LIKE '%s'", strings.ToUpper(rule.Field), "%"+value))
			}
		} else {
			var specificCondition []string
			for _, rule := range rule.Rules {
				var value = NormalizeAdvancedFilterValue(rule.Value)
				switch rule.Operator {
				case "contains":
					specificCondition = append(specificCondition, fmt.Sprintf("%s LIKE '%s'", strings.ToUpper(rule.Field), "%"+value+"%"))
				case "equal":
					specificCondition = append(specificCondition, fmt.Sprintf("%s = '%s'", strings.ToUpper(rule.Field), value))
				case "not_equal":
					specificCondition = append(specificCondition, fmt.Sprintf("%s != '%s'", strings.ToUpper(rule.Field), value))
				case "greater":
					specificCondition = append(specificCondition, fmt.Sprintf("%s > '%s'", strings.ToUpper(rule.Field), value))
				case "greater_or_equal":
					specificCondition = append(specificCondition, fmt.Sprintf("%s >= '%s'", strings.ToUpper(rule.Field), value))
				case "less":
					specificCondition = append(specificCondition, fmt.Sprintf("%s < '%s'", strings.ToUpper(rule.Field), value))
				case "less_or_equal":
					specificCondition = append(specificCondition, fmt.Sprintf("%s <= '%s'", strings.ToUpper(rule.Field), value))
				case "is_null":
					specificCondition = append(specificCondition, fmt.Sprintf("%s is null", strings.ToUpper(rule.Field)))
				case "is_not_null":
					specificCondition = append(specificCondition, fmt.Sprintf("%s is not null", strings.ToUpper(rule.Field)))
				case "is_empty":
					specificCondition = append(specificCondition, fmt.Sprintf("%s = ''", strings.ToUpper(rule.Field)))
				case "is_not_empty":
					specificCondition = append(specificCondition, fmt.Sprintf("%s != ''", strings.ToUpper(rule.Field)))
				case "begins_with":
					specificCondition = append(specificCondition, fmt.Sprintf("%s LIKE '%s'", strings.ToUpper(rule.Field), value+"%"))
				case "not_begins_with":
					specificCondition = append(specificCondition, fmt.Sprintf("%s NOT LIKE '%s'", strings.ToUpper(rule.Field), value+"%"))
				case "ends_with":
					specificCondition = append(specificCondition, fmt.Sprintf("%s LIKE '%s'", strings.ToUpper(rule.Field), "%"+value))
				case "not_ends_with":
					specificCondition = append(specificCondition, fmt.Sprintf("%s NOT LIKE '%s'", strings.ToUpper(rule.Field), "%"+value))
				}
			}

			subQuery := strings.Join(specificCondition, fmt.Sprintf(" %s ", rule.Condition))
			conditions = append(conditions, fmt.Sprintf(" (%s)", subQuery))
		}
	}

	query := strings.Join(conditions, fmt.Sprintf(" %s ", deserializedAdvancedFilter.Condition))

	return query
}

func NormalizeAdvancedFilterValue(value string) string {
	var normalizedValue string

	if IsDateValue(value) {
		normalizedValue = NormalizeDate(value)
	} else {
		normalizedValue = value
	}

	return normalizedValue
}

func IsDateValue(value string) bool {
	_, err := time.Parse("2006-01-02T15:04:05Z07:00", value)
	return err == nil
}

func NormalizeDate(date string) string {
	dateTime, _ := time.Parse("2006-01-02T15:04:05Z07:00", date)
	normalizedDate := dateTime.Format("20060102")

	return normalizedDate
}
