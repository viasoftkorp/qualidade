package controllers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/services"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	"github.com/gofiber/fiber/v2"
	"strconv"
)

func GetInspecoesSaidaHistoricoCabecalho(ctx *fiber.Ctx) error {
	baseFilters, err := utils.GetInputBaseFilterFromCtx(ctx)

	var filters dto.InspecaoSaidaHistoricoCabecalhoFilters
	err = ctx.QueryParser(&filters)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	service, err := services.GetInspecaoSaidaHistoricoService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.GetAllInspecaoSaidaHistoricoCabecalho(baseFilters, &filters)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}

func GetInspecoesSaidaHistoricoItems(ctx *fiber.Ctx) error {
	baseFilters, err := utils.GetInputBaseFilterFromCtx(ctx)
	odf, _ := strconv.Atoi(ctx.Params("odf"))

	service, err := services.GetInspecaoSaidaHistoricoService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.GetAllInspecaoSaidaHistoricoItems(baseFilters, odf)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}