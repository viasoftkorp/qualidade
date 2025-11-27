package utils

func Contains(array []string, itemToFind string) bool {
	for _, item := range array {
		if item == itemToFind {
			return true
		}
	}
	return false
}
