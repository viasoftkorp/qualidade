package controllers

import (
	"errors"
	"strconv"

	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/services"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/utils"
	"github.com/gofiber/fiber/v2"
)

func GetInspecoesSaida(ctx *fiber.Ctx) error {
	baseFilters, err := utils.GetInputBaseFilterFromCtx(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	odf, err := strconv.Atoi(ctx.Query("odf"))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	var filters dto.InspecaoSaidaFilters
	err = ctx.QueryParser(&filters)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoSaidaService, err := services.GetInspecaoSaidaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	inspecoes, validacaoDTO := inspecaoSaidaService.BuscarInspecoesSaida(odf, baseFilters, &filters)

	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(inspecoes)
	}
}

func GetPlanosNovaInspecao(ctx *fiber.Ctx) error {
	recnoProcesso, err := strconv.Atoi(ctx.Query("recnoProcesso"))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	plano := ctx.Query("plano")
	if plano == "" {
		return errors.New("plano missing")
	}

	filter, err := utils.GetInputBaseFilterFromCtx(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoSaidaService, err := services.GetInspecaoSaidaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	planos, validacaoDTO := inspecaoSaidaService.BuscarPlanosNovaInspecao(recnoProcesso, plano, filter)

	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
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

	inspecaoSaidaService, err := services.GetInspecaoSaidaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	inspecao, validacaoDTO := inspecaoSaidaService.CriarInspecao(input)
	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(inspecao)
	}
}

func AtualizarInspecao(ctx *fiber.Ctx) error {
	var input *dto.AtualizarInspecaoInput

	err := ctx.BodyParser(&input)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoSaidaService, err := services.GetInspecaoSaidaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	validacaoDTO := inspecaoSaidaService.AtualizarInspecao(input)
	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(nil)
	}
}

func GetInspecaoSaida(ctx *fiber.Ctx) error {
	codInspecao, err := strconv.Atoi(ctx.Params("codInspecao", ""))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoSaidaService, err := services.GetInspecaoSaidaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	inspecao, validacaoDTO := inspecaoSaidaService.BuscarInspecaoSaidaPeloCodigo(codInspecao)

	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(inspecao)
	}
}

func GetInspecaoSaidaItens(ctx *fiber.Ctx) error {
	codInspecao, err := strconv.Atoi(ctx.Params("codInspecao", ""))

	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	filter, err := utils.GetInputBaseFilterFromCtx(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoSaidaService, err := services.GetInspecaoSaidaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	itens, validacaoDTO := inspecaoSaidaService.BuscarInspecaoSaidaItensPeloCodigo(codInspecao, filter)
	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(itens)
	}
}

func RemoverInspecaoSaida(ctx *fiber.Ctx) error {
	codInspecao, err := strconv.Atoi(ctx.Params("codInspecao", ""))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoSaidaService, err := services.GetInspecaoSaidaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	validacaoDTO := inspecaoSaidaService.RemoverInspecaoSaidaPeloCodigo(codInspecao)

	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(nil)
	}
}

func ImprimirInspecaoSaida(ctx *fiber.Ctx) error {
	codInspecao, err := strconv.Atoi(ctx.Params("codInspecao", ""))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoSaidaService, err := services.GetInspecaoSaidaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	x, validacaoDTO := inspecaoSaidaService.ImprimirInspecaoSaida(codInspecao)

	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(x)
	}
}

func GetResultadoInspecao(ctx *fiber.Ctx) error {
	codInspecao, err := strconv.Atoi(ctx.Params("codInspecao", ""))
	if err != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(fiber.Map{"error": err.Error()})
	}

	inspecaoSaidaService, err := services.GetInspecaoSaidaService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err.Error()})
	}

	resultado, validacaoDTO := inspecaoSaidaService.BuscarResultadoInspecao(codInspecao)

	if validacaoDTO != nil {
		return ctx.Status(fiber.StatusBadRequest).JSON(validacaoDTO)
	} else {
		return ctx.Status(fiber.StatusOK).JSON(resultado)
	}
}
