package main

//go:generate goversioninfo

import (
	"bitbucket.org/viasoftkorp/korp.sdk/authorization/handler"
	authorizationServicesSdk "bitbucket.org/viasoftkorp/korp.sdk/authorization/services"
	authorizationStructSdk "bitbucket.org/viasoftkorp/korp.sdk/authorization/structs"
	"bitbucket.org/viasoftkorp/korp.sdk/auto_update"
	"bitbucket.org/viasoftkorp/korp.sdk/caching"
	consulSdk "bitbucket.org/viasoftkorp/korp.sdk/consul"
	"bitbucket.org/viasoftkorp/korp.sdk/fiber_helper"
	"bitbucket.org/viasoftkorp/korp.sdk/proxy"
	"bitbucket.org/viasoftkorp/korp.sdk/secret_manager"
	sentrySdk "bitbucket.org/viasoftkorp/korp.sdk/sentry"
	"bitbucket.org/viasoftkorp/korp.sdk/service_discovery"
	"bitbucket.org/viasoftkorp/korp.sdk/service_info"
	utilsSdk "bitbucket.org/viasoftkorp/korp.sdk/utils"
	"fmt"
	"github.com/gofiber/fiber/v2/middleware/logger"
	"log"
)

func main() {
	fmt.Println("Current Version: " + service_info.Version)
	service_info.ServiceName = "Korp.Qualidade.InspecaoEntrada"

	_, err := consulSdk.AddKorpConsul(consulSdk.KorpConsulConfig{})

	if err != nil {
		log.Fatalf(fmt.Sprintf("Couldn't add consul to the service due to: %s", err))
	}

	auto_update.MustDoUpdate()

	secret_manager.MustInitSecretManager(nil)

	go auto_update.SelfUpdateScheduler()

	err = authorizationServicesSdk.AddKorpAuthorization(authorizationStructSdk.KorpAuthorizationConfig{
		AllowFrontendRequests: true,
	})

	if err != nil {
		log.Fatalf(fmt.Sprintf("Couldn't add authorizations to the service due to: %s", err))
	}

	sentrySdk.InitSentry()

	defer sentrySdk.FlushSentry()

	caching.MustInitCaching()
	apiPrefixVersioned := fmt.Sprintf("/%s/qualidade/inspecao-entrada/", utilsSdk.GetVersionWithoutBuild(service_info.Version))

	app := fiber_helper.NewFiberApp()
	app.Use(logger.New())
	service_discovery.AddKorpFiberHealthCheck(app)
	app.Use(handler.KorpFiberAuthorizationHandler())
	app.Use(proxy.KorpAuthenticationProxy(apiPrefixVersioned))
	app.Use(proxy.KorpAuthorizationProxy(apiPrefixVersioned))

	initRoutes(app)

	service_discovery.MustAddServiceDiscovery(service_discovery.NewServiceDiscoveryConfig(fiber_helper.NewFiberShutdownServer(app), fmt.Sprintf("/%s/%s", utilsSdk.GetVersionWithoutBuild(service_info.Version), "qualidade/inspecao-entrada/")))

	fiber_helper.Listen(app)
}
