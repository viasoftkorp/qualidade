package controllers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/services"
	"github.com/gofiber/fiber/v2"
)

func GetParametroRoute(ctx *fiber.Ctx) error {
	service, err := services.GetParametroService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.BuscarParametroBool(ctx.Params("parametro"), ctx.Params("secao"))
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}
