package controllers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/services"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	"github.com/gofiber/fiber/v2"
	"strconv"
)

func PublicarFinalizarInspecaoSaida(ctx *fiber.Ctx) error {
	var input *dto.FinalizarInspecaoInput

	err := ctx.BodyParser(&input)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	service, err := services.GetInspecaoSaidaSagaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, validacaoDTO, err := service.PublicarSagaInspecaoSaida(input)
	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}

func PublicarEstornarInspecaoSaida(ctx *fiber.Ctx) error {
	recnoInspecao, err := strconv.Atoi(ctx.Params("recnoInspecao"))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	service, err := services.GetInspecaoSaidaSagaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, validacaoDTO, err := service.PublicarSagaInspecaoSaidaEstorno(recnoInspecao)
	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}

func BuscarSagas(ctx *fiber.Ctx) error {
	baseFilters, err := utils.GetInputBaseFilterFromCtx(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	var filters dto.ProcessamentoInspecaoSaidaFilters
	err = ctx.QueryParser(&filters)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	estorno, err := strconv.ParseBool(ctx.Query("estorno"))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	service, err := services.GetInspecaoSaidaSagaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.BuscarSagas(baseFilters, &filters, estorno)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}

func ReprocessarSaga(ctx *fiber.Ctx) error {
	id := ctx.Params("id")
	if id == "" {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": "Empty parameter: id"})
	}

	service, err := services.GetInspecaoSaidaSagaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	err = service.ReprocessarSaga(id)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	} else {
		return ctx.Status(fiber.StatusOK).JSON("")
	}
}

func RemoverSaga(ctx *fiber.Ctx) error {
	id := ctx.Params("id")
	if id == "" {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": "Empty parameter: id"})
	}

	service, err := services.GetInspecaoSaidaSagaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	err = service.RemoverSaga(id)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	} else {
		return ctx.Status(fiber.StatusOK).JSON("")
	}
}
