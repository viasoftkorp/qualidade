package controllers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/services"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	"github.com/gofiber/fiber/v2"
	"strconv"
	"strings"
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

	recnoItemNotaFiscal, err := strconv.Atoi(ctx.Query("recnoItemNotaFiscal"))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	notaFiscal, err := strconv.Atoi(ctx.Params("notaFiscal"))
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	lote := ctx.Params("lote")
	containsScape := strings.Contains(lote, "%3A")
	if containsScape {
		lote = strings.ReplaceAll(lote, "%3A", "/")
	}

	var filters dto.InspecaoEntradaHistoricoCabecalhoFilters
	err = ctx.QueryParser(&filters)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	service, err := services.GetInspecaoEntradaHistoricoService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.GetAllInspecaoEntradaHistoricoItems(recnoItemNotaFiscal, notaFiscal, lote, baseFilters, &filters)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}
