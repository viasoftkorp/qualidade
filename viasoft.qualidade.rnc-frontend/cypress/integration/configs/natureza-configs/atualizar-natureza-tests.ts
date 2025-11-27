import { codes,strings } from 'cypress/support/test-utils';
import { Interception } from 'cypress/types/net-stubbing';
import { NaturezaFormControls } from './../../../../apps/rnc/src/app/pages/settings/natureza/natureza-editor-modal/natureza-form-controls';
import { getAllNaturezasRequest, getNaturezaRequestForId, getNaturezasViewListRequest, updateNaturezaRequest } from './../../../support/requests/naturezas/naturezas.request';

describe("Atualizar natureza com sucesso",() => {
  VisitarTelaPrincipal()
  ClicarNatureza()
  PreencherFormulario()
  Salvar()
})

function VisitarTelaPrincipal(){
  it("Visitar tela de natureza",() => {
    cy.batchRequestStub(getNaturezasViewListRequest()).then((alias: any) => {
      cy.visit('rnc/configuracoes/naturezas');
      cy.wait(alias);
    })
  })
}

function ClicarNatureza(){
  it("Clicar na primeira natureza da lista",() => {
    const getRequest = getNaturezaRequestForId();
    cy.intercept(
      getRequest.method,
      getRequest.url,
      getRequest.response)
      .as('getNaturezaRequest')

    cy.get('rnc-natureza vs-grid tbody tr ').eq(0).click()

    cy.wait('@getNaturezaRequest')
  })
}

function PreencherFormulario(){
  it("Preencher formulário",() => {
    cy.get("rnc-natureza-editor-modal vs-button[type='save'] button").should('be.disabled')

    cy.getVsInput(NaturezaFormControls.descricao).clear().type(strings[3])

    cy.get("rnc-natureza-editor-modal vs-button[type='save'] button").should('be.enabled')
  })
}

function Salvar(){
  it("Salvar nova natureza",() => {

    const putRequest = updateNaturezaRequest()

    cy.intercept(
      putRequest.method,
      putRequest.url,
      putRequest.response)
      .as('putNaturezasRequest')

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

    cy.wait('@putNaturezasRequest').then((interception: Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode,200)

      putRequest.expectedBody.id = interception.request.body.id

      cy.validateRequestBody(interception.request.body,putRequest.expectedBody)
    })

    cy.wait('@getNaturezaRequest')

  })
}
