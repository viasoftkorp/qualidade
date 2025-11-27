package controllers

import (
	"strconv"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/repositories"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	"bitbucket.org/viasoftkorp/korp.sdk/ambient_data"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"github.com/gofiber/fiber/v2"
)

func GetRncDetails(ctx *fiber.Ctx) error {
	recnoInspecao, err := strconv.Atoi(ctx.Params("recnoInspecao"))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	baseParams, err := utils.GetBaseParamsFromCtx(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	ambientDataModel, err := ambient_data.GetAmbientDataFromContext(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	uow, err := unit_of_work.NewUnitOfWork(ambientDataModel, utils.MigrateEntities)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoSaidaRepository, err := repositories.NewInspecaoEntradaRepository(uow)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := inspecaoSaidaRepository.BuscarInformacoesPreenchimentoRNC(recnoInspecao, baseParams.LegacyCompanyId, ctx.Query("codigoProduto"), ctx.Query("codigoFornecedor"))
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}
