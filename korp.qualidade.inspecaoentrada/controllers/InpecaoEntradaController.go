package controllers

import (
	"errors"
	"strconv"
	"strings"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/services"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoEntrada/utils"
	"github.com/gofiber/fiber/v2"
)

func GetInspecoesEntrada(ctx *fiber.Ctx) error {
	baseFilters, err := utils.GetInputBaseFilterFromCtx(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	notaFiscal, err := strconv.Atoi(ctx.Params("notaFiscal"))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	lote := ctx.Params("lote")
	containsScape := strings.Contains(lote, "%3A")
	if containsScape {
		lote = strings.ReplaceAll(lote, "%3A", "/")
	}

	var filters dto.InspecaoEntradaFilters
	err = ctx.QueryParser(&filters)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	InspecaoEntradaService, err := services.GetInspecaoEntradaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	inspecoes, validacaoDTO, err := InspecaoEntradaService.BuscarInspecoesEntrada(notaFiscal, lote, baseFilters, &filters)

	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(err)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(inspecoes)
	}
}

func GetItensPlanoNovaInspecao(ctx *fiber.Ctx) error {
	codigoPlano := ctx.Params("codigoPlano")
	if codigoPlano == "" {
		return errors.New("codigoPlano missing")
	}

	codigoProduto := ctx.Query("codigoProduto")
	if codigoProduto == "" {
		return errors.New("codigoProduto missing")
	}

	filter, err := utils.GetInputBaseFilterFromCtx(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoEntradaService, err := services.GetInspecaoEntradaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	planos, validacaoDTO, err := inspecaoEntradaService.BuscarPlanosNovaInspecao(codigoPlano, codigoProduto, filter)

	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(err)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(planos)
	}
}

func CriarInspecao(ctx *fiber.Ctx) error {
	var input *dto.NovaInspecaoInput

	err := ctx.BodyParser(&input)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoEntradaService, err := services.GetInspecaoEntradaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	codigoInspecao, validacaoDTO, err := inspecaoEntradaService.CriarInspecao(input)

	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(err)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(codigoInspecao)
	}
}

func AtualizarInspecao(ctx *fiber.Ctx) error {
	var input *dto.AtualizarInspecaoInput

	err := ctx.BodyParser(&input)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoEntradaService, err := services.GetInspecaoEntradaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	validacaoDTO, err := inspecaoEntradaService.AtualizarInspecao(input)

	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(err)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(nil)
	}
}

func GetInspecaoEntrada(ctx *fiber.Ctx) error {
	codigoInspecao, err := strconv.Atoi(ctx.Params("codigoInspecao", ""))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoEntradaService, err := services.GetInspecaoEntradaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	inspecao, validacaoDTO, err := inspecaoEntradaService.BuscarInspecaoEntradaPeloCodigo(codigoInspecao)
	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(err)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(inspecao)
	}
}

func GetInspecaoEntradaItens(ctx *fiber.Ctx) error {
	codigoInspecao, err := strconv.Atoi(ctx.Params("codigoInspecao", ""))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	codigoProduto := ctx.Query("codigoProduto")
	if codigoProduto == "" {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": errors.New("codigoProduto missing").Error()})
	}

	filter, err := utils.GetInputBaseFilterFromCtx(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoEntradaService, err := services.GetInspecaoEntradaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	itens, validacaoDTO, err := inspecaoEntradaService.BuscarInspecaoEntradaItensPeloCodigo(codigoProduto, codigoInspecao, filter)
	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(err)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(itens)
	}
}

func RemoverInspecaoEntrada(ctx *fiber.Ctx) error {
	codigoInspecao, err := strconv.Atoi(ctx.Params("codigoInspecao", ""))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoEntradaService, err := services.GetInspecaoEntradaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	validacaoDTO, err := inspecaoEntradaService.RemoverInspecaoEntradaPeloCodigo(codigoInspecao)

	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(err)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(nil)
	}
}

func GetResultadoInspecao(ctx *fiber.Ctx) error {
	codigoInspecao, err := strconv.Atoi(ctx.Params("codigoInspecao", ""))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoEntradaService, err := services.GetInspecaoEntradaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	resultado, validacaoDTO, err := inspecaoEntradaService.BuscarResultadoInspecao(codigoInspecao)

	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(err)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(resultado)
	}
}

func GetValorParametro(ctx *fiber.Ctx) error {
	service, err := services.GetInspecaoEntradaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	result, err := service.BuscarValorParametro(ctx.Params("parametro"), ctx.Params("secao"))
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	} else {
		return ctx.Status(fiber.StatusOK).JSON(result)
	}
}

func ImprimirInspecaoEntrada(ctx *fiber.Ctx) error {
	codigoInspecao, err := strconv.Atoi(ctx.Params("codigoInspecao", ""))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoEntradaService, err := services.GetInspecaoEntradaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	relatorio, validacaoDTO := inspecaoEntradaService.ImprimirInspecaoEntrada(codigoInspecao)

	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(relatorio)
	}
}
