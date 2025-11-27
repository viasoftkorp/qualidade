package controllers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/repositories"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/services"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	"bitbucket.org/viasoftkorp/korp.sdk/ambient_data"
	unit_of_work "bitbucket.org/viasoftkorp/korp.sdk/unit-of-work"
	"github.com/gofiber/fiber/v2"
	"strconv"
)

func GetLocaisRoute(ctx *fiber.Ctx) error {
	ambientDataModel, err := ambient_data.GetAmbientDataFromContext(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	uow, err := unit_of_work.NewUnitOfWork(ambientDataModel, nil)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	baseParams, err := utils.GetBaseParamsFromCtx(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	repo, err := repositories.NewLocaisRepository(uow, baseParams)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	var result []map[string]interface{}
	if ctx.Params("tipoLocal") == "Retrabalho" {
		result, err = repo.BuscarLocaisRetrabalho()
	} else if ctx.Params("tipoLocal") == "Aprovado" {
		result, err = repo.BuscarLocaisAprovados()
	} else if ctx.Params("tipoLocal") == "Reprovado" {
		result, err = repo.BuscarLocaisReprovados()
	}

	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}

func BuscarLocal(ctx *fiber.Ctx) error {
	codigo := ctx.Params("codigo")
	if codigo == "" {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": "Empty parameter: codigo"})
	}

	service, err := services.GetLocalService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	codigoLocal, err := strconv.Atoi(codigo)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.BuscarLocal(codigoLocal)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}

func BuscarLocais(ctx *fiber.Ctx) error {
	filterInput, err := utils.GetInputBaseFilterFromCtx(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	service, err := services.GetLocalService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.BuscarLocais(filterInput)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}
