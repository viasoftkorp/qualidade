package controllers

import (
	"strings"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/services"
	"github.com/gofiber/fiber/v2"
)

func BuscarProcessoEngenharia(ctx *fiber.Ctx) error {
	codigoProduto := ctx.Params("codigoProduto")
	containsScape := strings.Contains(codigoProduto, "%3A")

	if containsScape {
		codigoProduto = strings.ReplaceAll(codigoProduto, "%3A", "/")
	}

	if codigoProduto == "" {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": "Empty parameter: codigoProduto"})
	}

	service, err := services.GetEngenhariaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.BuscarProcesso(codigoProduto)

	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}
