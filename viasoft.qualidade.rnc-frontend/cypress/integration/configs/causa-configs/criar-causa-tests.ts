import { codes,strings } from 'cypress/support/test-utils';
import { Interception } from 'cypress/types/net-stubbing';
import { CausaFormControls } from '@viasoft/rnc/app/pages/settings/causa/causa-editor-modal/causa-form-controls';
import { createNewCausaRequest, getAllCausasRequest, getCausasViewListRequest } from 'cypress/support/requests/causas/causas.request';

describe.skip("Criar causa com sucesso",() => {
  VisitarTelaPrincipal()
  ClicarBotaoNovo()
  PreencherFormulario()
  Salvar()
})

function VisitarTelaPrincipal(){
  it("Visitar tela de causa",() => {
    cy.batchRequestStub(getCausasViewListRequest()).then(alias => {
      cy.visit('rnc/configuracoes/causas');
      cy.wait(alias);
    })
  })
}

function ClicarBotaoNovo(){
  it("Clicar botão novo",() => {
    cy.get('vs-header vs-button[icon=plus] button').click()
  })
}

function PreencherFormulario(){
  it("Preencher formulário",() => {
    cy.get("rnc-causa-editor-modal vs-button[type='save'] button").should('be.disabled')
    cy.getVsInput(CausaFormControls.descricao).type(strings[2])
    cy.get('vs-textarea').type(strings[2])
    cy.get("rnc-causa-editor-modal vs-button[type='save'] button").should('be.enabled')
  })
}

function Salvar(){
  it("Salvar nova Causa",() => {
    const postRequest = createNewCausaRequest()

    cy.intercept(
      postRequest.method,
      postRequest.url,
      postRequest.response
    ).as('postCausasRequest')

    const refreshGridRequest = getCausasViewListRequest()

    refreshGridRequest.response.body.items.push({
      codigo:codes[2],
      descricao:strings[2],
      detalhamento:strings[2]
    })

    cy.intercept(
      refreshGridRequest.method,
      refreshGridRequest.url,
      refreshGridRequest.response)
      .as('getCausaRequest')

    cy.get("rnc-causa-editor-modal vs-button[type='save'] button").click()

    cy.wait('@postCausasRequest').then((interception: Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode,200)

      postRequest.expectedBody.id = interception.request.body.id

      cy.validateRequestBody(interception.request.body,postRequest.expectedBody)
    })
    cy.wait('@getCausaRequest')
  })
}
