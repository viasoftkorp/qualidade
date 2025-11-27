package controllers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/services"
	"github.com/gofiber/fiber/v2"
)

func GerarNumeroLote(ctx *fiber.Ctx) error {
	var input dto.GerarNumeroLoteInput
	err := ctx.BodyParser(&input)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	loteService, err := services.GetLoteService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := loteService.GerarNumeroLote(input)

	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(err)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result.NumeroLote)
	}
}
