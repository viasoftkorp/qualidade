package controllers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/services"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	"github.com/gofiber/fiber/v2"
)

func BuscarProduto(ctx *fiber.Ctx) error {
	codigo := ctx.Params("codigo")
	if codigo == "" {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": "Empty parameter: codigo"})
	}

	service, err := services.GetProdutoService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.BuscarProduto(codigo)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}

func BuscarProdutos(ctx *fiber.Ctx) error {
	filterInput, err := utils.GetInputBaseFilterFromCtx(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	service, err := services.GetProdutoService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.BuscarProdutos(filterInput)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}
