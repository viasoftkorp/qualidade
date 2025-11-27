import { getSolucaoRequestForId, updateSolucaoRequest, getSolucoesViewListRequest } from './../../../support/requests/solucoes/solucoes-requests';
import { SolucaoFormControls } from './../../../../apps/rnc/src/app/pages/settings/solucao/solucao-form-controls';
import { strings, codes } from 'cypress/support/test-utils';
import { Interception } from 'cypress/types/net-stubbing';

describe("Atualizar solucao com sucesso",() => {
  it("Visitar tela de solucao",() => {
    cy.batchRequestStub(getSolucoesViewListRequest()).then((alias: any) => {
      cy.visit('rnc/configuracoes/solucoes');
      cy.wait(alias);
    })
  })
  it("Clicar na primeira solucao da lista",() => {
    cy.batchRequestStub(getSolucaoRequestForId()).then(alias => {
      cy.get('rnc-solucao vs-grid tbody tr ').eq(0).click()
      cy.wait(alias)
    })
  })
  it("Preencher formulário",() => {
    cy.get("rnc-solucao-editor vs-button[type='save'] button").should('be.disabled')

    cy.getVsInput(SolucaoFormControls.descricao).clear().type(strings[3]);
    cy.get('vs-textarea[formcontrolname="detalhamento"]').clear().type(strings[3]);
    cy.get('vs-checkbox[formcontrolname="imediata"] input[type="checkbox"]').click({ force: true });
    cy.get("rnc-solucao-editor vs-button[type='save'] button").should('be.enabled');
  })

  it("Salvar nova solucao",() => {
    const putRequest = updateSolucaoRequest()

    cy.batchRequestStub(putRequest).then(aliases => {
      cy.get("rnc-solucao-editor vs-button[type='save'] button").click()
      cy.wait(aliases).then((interception:Interception) => {
        cy.validateRequestStatusCode(interception.response?.statusCode,200)

        putRequest.expectedBody.id = interception.request.body.id

        cy.validateRequestBody(interception.request.body,putRequest.expectedBody)
      })
    })
    cy.get("rnc-solucao-editor vs-button[type='save'] button").should('be.disabled')
  })
})
