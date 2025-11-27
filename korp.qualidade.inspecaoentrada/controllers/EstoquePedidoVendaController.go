package controllers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/services"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	"github.com/gofiber/fiber/v2"
	"github.com/google/uuid"
	"strconv"
)

func GetInspecaoEntradaAlocacaoPedido(ctx *fiber.Ctx) error {
	recnoInspecao, err := strconv.Atoi(ctx.Params("recnoInspecao"))

	codigoProduto := ctx.Query("codigoProduto")
	lote := ctx.Query("lote")

	filter, err := utils.GetInputBaseFilterFromCtx(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	service, err := services.GetEstoquePedidoVendaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.BuscarEstoqueLocalPedidosVendas(filter, recnoInspecao, lote, codigoProduto)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}

func AtualizarDistribuicaoInspecaoEstoquePedidoVenda(ctx *fiber.Ctx) error {
	id, err := uuid.Parse(ctx.Params("id"))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	recnoInspecao, err := strconv.Atoi(ctx.Params("recnoInspecao"))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	var input dto.EstoqueLocalPedidoVendaAlocacaoInput

	err = ctx.BodyParser(&input)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	service, err := services.GetEstoquePedidoVendaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	validacao, err := service.AtualizarDistribuicaoInspecaoEstoquePedidoVenda(id, recnoInspecao, input)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	} else if validacao != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacao)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(nil)
	}

}

func BuscarQuantidadeTotalAlocadaPedidoVenda(ctx *fiber.Ctx) error {
	recnoInspecao, err := strconv.Atoi(ctx.Params("recnoInspecao"))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	service, err := services.GetEstoquePedidoVendaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.BuscarQuantidadeTotalAlocadaPedidoVenda(recnoInspecao)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}

func BuscarPedidoVendaLotes(ctx *fiber.Ctx) error {
	idPedidoVendaLote, err := uuid.Parse(ctx.Params("id"))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	service, err := services.GetEstoquePedidoVendaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.BuscarPedidoVendaLotes(idPedidoVendaLote)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}
