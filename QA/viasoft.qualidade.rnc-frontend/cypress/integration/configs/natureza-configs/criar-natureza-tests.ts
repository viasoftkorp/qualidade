import { codes,strings } from 'cypress/support/test-utils';
import { Interception } from 'cypress/types/net-stubbing';
import { NaturezaFormControls } from './../../../../apps/rnc/src/app/pages/settings/natureza/natureza-editor-modal/natureza-form-controls';
import { getAllNaturezasRequest, createNewNaturezaRequest, getNaturezasViewListRequest } from './../../../support/requests/naturezas/naturezas.request';

describe.skip("Criar natureza com sucesso",() => {
  VisitarTelaPrincipal()
  ClicarBotaoNovo()
  PreencherFormulario()
  Salvar()
})

function VisitarTelaPrincipal(){
  it("Visitar tela de natureza",() => {
    cy.batchRequestStub(getNaturezasViewListRequest()).then(alias => {
      cy.visit('rnc/configuracoes/naturezas');
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
    cy.get("rnc-natureza-editor-modal vs-button[type='save'] button").should('be.disabled')
    cy.getVsInput(NaturezaFormControls.descricao).type(strings[2])
    cy.get("rnc-natureza-editor-modal vs-button[type='save'] button").should('be.enabled')
  })
}

function Salvar(){
  it("Salvar nova natureza",() => {
    const postRequest = createNewNaturezaRequest()

    cy.intercept(
      postRequest.method,
      postRequest.url,
      postRequest.response)
      .as('postNaturezasRequest')

    const refreshGridRequest = getNaturezasViewListRequest()

    refreshGridRequest.response.body.items.push({
      codigo:codes[2],
      descricao:strings[2]
    })

    cy.intercept(
      refreshGridRequest.method,
      refreshGridRequest.url,
      refreshGridRequest.response)
      .as('getNaturezaRequest')

    cy.get("rnc-natureza-editor-modal vs-button[type='save'] button").click()

    cy.wait('@postNaturezasRequest').then((interception: Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode,200)

      postRequest.expectedBody.id = interception.request.body.id

      cy.validateRequestBody(interception.request.body,postRequest.expectedBody)
    })
    cy.wait('@getNaturezaRequest')
  })
}
