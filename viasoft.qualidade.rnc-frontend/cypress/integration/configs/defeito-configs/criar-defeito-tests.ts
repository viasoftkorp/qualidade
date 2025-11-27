import { createNewDefeitoRequest, getCausasList, getSolucoesList, getDefeitosViewListRequest } from './../../../support/requests/defeitos/defeitos-requests';
import { DefeitoFormControls } from './../../../../apps/rnc/src/app/pages/settings/defeito/defeito-editor-modal/defeito-form-controls';
import { codes,strings } from 'cypress/support/test-utils';
import { Interception } from 'cypress/types/net-stubbing';

describe("Criar defeito com sucesso",() => {
  it("Visitar tela de defeito",() => {
    cy.batchRequestStub(getDefeitosViewListRequest()).then(alias => {
      cy.visit('rnc/configuracoes/defeitos');
      cy.wait(alias);
    })
  })
  it("Clicar botão novo",() => {
    cy.get('vs-header vs-button[icon=plus] button').click()
  })
  it("Preencher formulário",() => {
    cy.get("rnc-defeito-editor-modal vs-button[type='save'] button").should('be.disabled')
    cy.getVsInput(DefeitoFormControls.descricao).type(strings[2])
    cy.getAutoCompleteItem({
      formControlName: DefeitoFormControls.idCausa,
      request: getCausasList(),
      selector: 'qa-causa-autocomplete-select'
    })
    cy.getAutoCompleteItem({
      formControlName: DefeitoFormControls.idSolucao,
      request: getSolucoesList(),
      selector: 'qa-solucao-autocomplete-select'
    })
    cy.get(`vs-textarea[formcontrolname=${DefeitoFormControls.detalhamento}]`).clear().type(strings[2])

    cy.get("rnc-defeito-editor-modal vs-button[type='save'] button").should('be.enabled')
  })
  it("Salvar nova Defeito",() => {
    const postRequest = createNewDefeitoRequest()

    cy.intercept(
      postRequest.method,
      postRequest.url,
      postRequest.response
    ).as('postDefeitosRequest')

    const refreshGridRequest = getDefeitosViewListRequest()

    refreshGridRequest.response.body.items.push({
      codigo:codes[2],
      descricao:strings[2],
      detalhamento:strings[2]
    })

    cy.intercept(
      refreshGridRequest.method,
      refreshGridRequest.url,
      refreshGridRequest.response)
      .as('getDefeitoRequest')

    cy.get("rnc-defeito-editor-modal vs-button[type='save'] button").click()

    cy.wait('@postDefeitosRequest').then((interception: Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode,200)

      postRequest.expectedBody.id = interception.request.body.id

      cy.validateRequestBody(interception.request.body,postRequest.expectedBody)
    })
    cy.wait('@getDefeitoRequest')
  })
})
