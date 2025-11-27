package controllers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/services"
	"github.com/gofiber/fiber/v2"
)

func GetConfiguracao(ctx *fiber.Ctx) error {
	service, err := services.GetConfiguracaoService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.GetConfiguracao()
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}

func UpdateConfiguracao(ctx *fiber.Ctx) error {
	var input dto.ConfiguracaoDto

	err := ctx.BodyParser(&input)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	service, err := services.GetConfiguracaoService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	err = service.UpdateConfiguracao(input)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	} else {
		return ctx.Status(fiber.StatusOK).JSON("")
	}
}
