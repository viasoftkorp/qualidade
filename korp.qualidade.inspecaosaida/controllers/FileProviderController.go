package controllers

import (
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/dto"
	"bitbucket.org/viasoftkorp/Korp.Qualidade.InspecaoSaida/services"
	"github.com/gofiber/fiber/v2"
)

func UploadFiles(ctx *fiber.Ctx) error {
	fileProviderProxyService, err := services.GetFileProviderProxyService(ctx)
	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	}

	baseInput, err := getFileProviderBaseInput(ctx)
	statusCode, err := fileProviderProxyService.UploadFiles(baseInput)

	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	}

	return ctx.Status(statusCode).JSON("")
}
func GetFilesByDomainWithFilters(ctx *fiber.Ctx) error {
	fileProviderProxyService, err := services.GetFileProviderProxyService(ctx)
	var input *dto.GetFilesByDomainWithFiltersInput

	err = ctx.BodyParser(&input)

	baseInput, err := getFileProviderBaseInput(ctx)

	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	}

	input.AppId = baseInput.AppId

	statusCode, files, err := fileProviderProxyService.GetFilesByDomainWithFilters(input)

	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	}

	return ctx.Status(statusCode).JSON(files)
}

func DeleteFile(ctx *fiber.Ctx) error {
	fileProviderProxyService, err := services.GetFileProviderProxyService(ctx)

	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	}
	var fileId = ctx.Params("id")

	statusCode, err := fileProviderProxyService.DeleteFile(fileId)

	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	}

	return ctx.Status(statusCode).JSON(nil)
}

func DownloadFile(ctx *fiber.Ctx) error {
	fileProviderProxyService, err := services.GetFileProviderProxyService(ctx)

	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	}
	var fileId = ctx.Params("id")

	statusCode, err := fileProviderProxyService.DownloadFile(fileId)

	if err != nil {
		return ctx.Status(fiber.StatusInternalServerError).JSON(fiber.Map{"error": err})
	}
	if statusCode != 200 {
		return ctx.Status(statusCode).JSON(nil)
	}
	return nil
}

func getFileProviderBaseInput(ctx *fiber.Ctx) (*dto.FileProviderBaseInput, error) {
	var appId = ctx.Params("appid")

	var domain = ctx.Params("domain")

	var subdomain = ctx.Params("subdomain")
	var forceUpload = ctx.Query("forceUpload")

	var uploadFilesInput = dto.FileProviderBaseInput{
		AppId:       appId,
		Domain:      domain,
		SubDomain:   subdomain,
		ForceUpload: forceUpload,
	}

	return &uploadFilesInput, nil
}
