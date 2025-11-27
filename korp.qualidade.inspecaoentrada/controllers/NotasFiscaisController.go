package controllers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/services"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	"github.com/gofiber/fiber/v2"
)

func GetNotasFiscais(ctx *fiber.Ctx) error {
	filter, err := utils.GetInputBaseFilterFromCtx(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	var filters dto.NotaFiscalFilters
	err = ctx.QueryParser(&filters)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	notaFiscalService, err := services.GetNotaFiscalService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	notasFiscais, validacaoDTO, err := notaFiscalService.BuscarNotasFiscais(filter, &filters)

	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(err)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(notasFiscais)
	}
}
