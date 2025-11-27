package models

type AdvancedFilter struct {
	Condition string
	Rules     []Rules
}

type Rules struct {
	Field     string
	Operator  string
	Type      string
	Value     string
	Condition string
	Rules     []Rules
}
