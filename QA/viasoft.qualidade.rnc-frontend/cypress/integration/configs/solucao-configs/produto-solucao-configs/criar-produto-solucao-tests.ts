import {
  getProdutosList,
  createProdutoSolucaoRequest,
  getAllProdutosSolucaoView,
  getSolucaoRequestForId
 } from './../../../../support/requests/solucoes/solucoes-requests';
import { codes, ids, strings } from 'cypress/support/test-utils';
import { Interception } from 'cypress/types/net-stubbing';
import { SolucaoProdutoFormControls } from '@viasoft/rnc/app/pages/settings/solucao/solucao-produto/solucao-produto-form-controls';
import { getAllPagelessProductsRequest } from 'cypress/support/requests/logistics-products/products-requests';

describe("Criar solucao com sucesso", () => {
  it("Visitar tela de produto solucao", () => {
    cy.batchRequestStub([getSolucaoRequestForId(),getAllProdutosSolucaoView()]).then(alias => {
      cy.visit(`rnc/configuracoes/solucoes/${ids[0]}/produtos`);
      cy.wait(alias);
    })
  })
  it("Clicar botão novo", () => {
    cy.batchRequestStub(getAllPagelessProductsRequest()).then(alias => {
      cy.get('rnc-solucao-produto vs-button[icon=plus] button').click()
      cy.wait(alias);
    })
  })
  it("Preencher formulário", () => {
    cy.get("rnc-solucao-produto-editor-modal vs-button[type='save'] button").should('be.disabled')

    cy.get('div.vs-autocomplete-panel vs-button button').eq(0).click()
    cy.getVsTextArea(SolucaoProdutoFormControls.operacaoEngenharia).type(strings[0])
    cy.getVsInput(SolucaoProdutoFormControls.quantidade).clear().type('3');
    cy.get("rnc-solucao-produto-editor-modal vs-button[type='save'] button").should('be.enabled')
  })
  it("Salvar novo produto na solucao", () => {
    const postRequest = createProdutoSolucaoRequest()

    cy.intercept(
      postRequest.method,
      postRequest.url,
      postRequest.response)
      .as('postSolucoesRequest')

      const refreshGridRequest = getAllProdutosSolucaoView()

      refreshGridRequest.response.body.items.push({
        codigo:codes[2],
        descricao:strings[3],
        unidadeMedida: 'UN',
        quantidade: codes[2]
      })

      cy.intercept(
        refreshGridRequest.method,
        refreshGridRequest.url,
        refreshGridRequest.response)
        .as('getSolucaoRequest')

    cy.get("rnc-solucao-produto-editor-modal vs-button[type='save'] button").click()

    cy.wait('@postSolucoesRequest').then((interception: Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode, 200)

      postRequest.expectedBody.id = interception.request.body.id
      postRequest.expectedBody.idSolucao = interception.request.body.idSolucao
      interception.request.body.quantidade = Number.parseInt(interception.request.body.quantidade)

      cy.validateRequestBody(interception.request.body, postRequest.expectedBody)
    })

  })
})
