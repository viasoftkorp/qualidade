package controllers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/services"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	"github.com/gofiber/fiber/v2"
)

func GetOrdensProducao(ctx *fiber.Ctx) error {
	baseFilters, err := utils.GetInputBaseFilterFromCtx(ctx)

	var filters dto.OrdemProducaoFilters
	err = ctx.QueryParser(&filters)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	ordemProducaoService, err := services.GetOrdemProducaoService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	ordens, validacaoDTO := ordemProducaoService.BuscarOrdensInspecao(baseFilters, &filters)
	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(ordens)
	}
}
