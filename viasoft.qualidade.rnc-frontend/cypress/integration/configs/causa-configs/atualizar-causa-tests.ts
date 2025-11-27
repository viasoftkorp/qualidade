import { Interception } from 'cypress/types/net-stubbing';
import { codes, strings } from 'cypress/support/test-utils';
import { CausaFormControls } from './../../../../apps/rnc/src/app/pages/settings/causa/causa-editor-modal/causa-form-controls';
import { getAllCausasRequest, getCausaRequestForId, getCausasViewListRequest, updateCausaRequest } from './../../../support/requests/causas/causas.request';

describe("Atualizar causa com sucesso",() => {
  VisitarTelaPrincipal()
  ClicarCausa()
  PreencherFormulario()
  Salvar()
})

function VisitarTelaPrincipal(){
  it("Visitar tela principal",() => {
    cy.batchRequestStub(getCausasViewListRequest()).then(alias => {
      cy.visit('rnc/configuracoes/causas')
      cy.wait(alias)
    })
  })
}

function ClicarCausa(){
  it("Clicar causa",() => {
    const getRequest = getCausaRequestForId();
    cy.intercept(
      getRequest.method,
      getRequest.url,
      getRequest.response)
      .as('getCausaRequest')

    cy.get('rnc-causa vs-grid tbody tr').eq(0).click()

    cy.wait('@getCausaRequest')
  })
}

function PreencherFormulario() {
  it("Preencher Formulário",() => {
    cy.get('rnc-causa-editor-modal vs-button[type="save"] button').should('be.disabled')

    cy.getVsInput(CausaFormControls.descricao).clear().type(strings[3])
    cy.get('vs-textarea').clear().type(strings[3])

    cy.get('rnc-causa-editor-modal vs-button[type="save"] button').should('be.enabled')
  })
}

function Salvar(){
  it("Salvar nova causa",() => {
    const putRequest = updateCausaRequest()

    cy.intercept(
      putRequest.method,
      putRequest.url,
      putRequest.response
    )
    .as('putCausaRequest')

    const refreshGridRequest = getCausasViewListRequest()

    refreshGridRequest.response.body.items.push({
      codigo:codes[2],
      descricao:strings[2]
    })

    cy.intercept(
      refreshGridRequest.method,
      refreshGridRequest.url,
      refreshGridRequest.response)
      .as('getCausaRequest')

    cy.get('rnc-causa-editor-modal vs-button[type="save"] button').click()

    cy.wait('@putCausaRequest').then((interception:Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode,200)

      putRequest.expectedBody.id = interception.request.body.id

      cy.validateRequestBody(interception.request.body,putRequest.expectedBody)
    })

    cy.wait('@getCausaRequest')
  })
}
