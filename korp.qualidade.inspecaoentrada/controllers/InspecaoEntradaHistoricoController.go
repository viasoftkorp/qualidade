package controllers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/services"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	"github.com/gofiber/fiber/v2"
	"strconv"
)

func GetInspecoesEntradaHistoricoCabecalho(ctx *fiber.Ctx) error {
	baseFilters, err := utils.GetInputBaseFilterFromCtx(ctx)

	var filters dto.InspecaoEntradaHistoricoCabecalhoFilters
	err = ctx.QueryParser(&filters)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	service, err := services.GetInspecaoEntradaHistoricoService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.GetAllInspecaoEntradaHistoricoCabecalho(baseFilters, &filters)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}

func GetInspecoesEntradaHistoricoItems(ctx *fiber.Ctx) error {
	baseFilters, err := utils.GetInputBaseFilterFromCtx(ctx)
	notaFiscal, err := strconv.Atoi(ctx.Params("notaFiscal"))
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	lote := ctx.Params("lote")

	service, err := services.GetInspecaoEntradaHistoricoService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.GetAllInspecaoEntradaHistoricoItems(notaFiscal, lote, baseFilters)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}
