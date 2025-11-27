import { strings, codes } from "cypress/support/test-utils";
import { navigateToNaoConformidadeEditor } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { Interception } from "cypress/types/net-stubbing";
import {ProdutosNaoConformidadesFormControl} from '../../../../apps/rnc/src/app/pages/nao-conformidades/nao-conformidades-editor/produtos-nao-conformidades/produtos-nao-conformidades-editor-modal/produtos-nao-conformidades-form-control'
import { createNewProdutoNaoConformidadeRequest, getAllProdutosNaoConformidadesRequest } from "cypress/support/requests/nao-conformidade/produtos-nao-conformidades/produtos-nao-conformidade.requests";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { getAllPagelessProductsRequest } from "cypress/support/requests/logistics-products/products-requests";

describe("Criar produto não conformidade com sucesso",() => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();

    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Clicar em Produtos', () => {
    cy.batchRequestStub(getAllProdutosNaoConformidadesRequest()).then(aliases => {
      cy.getVsTabGroupItem('Produtos').click();
      cy.wait(aliases)
    })
  })
  it('Clicar botão adicionar produto', () => {
    cy.batchRequestStub(getAllPagelessProductsRequest())
      .then(alias => {
        cy.get('rnc-produtos-nao-conformidades vs-button[icon=plus] button').click()
        cy.wait(alias);
      })
  })

  it('Preencher campos', () => {
    cy.get('rnc-produtos-nao-conformidades-editor-modal vs-button[type=save] button').should('be.disabled');

    cy.get('div.vs-autocomplete-panel vs-button button').eq(0).click()

    cy.getVsInput(`${ProdutosNaoConformidadesFormControl.quantidade}`).type('3')
    cy.get(`rnc-produtos-nao-conformidades-editor-modal vs-textarea[formControlName=${ProdutosNaoConformidadesFormControl.detalhamento}]`).type(strings[2])
    cy.getVsTextArea(ProdutosNaoConformidadesFormControl.operacaoEngenharia).type(strings[0])

    cy.get('rnc-produtos-nao-conformidades-editor-modal vs-button[type=save] button').should('be.enabled')

  })

  it('Salvar e validar', () => {
     const postRequest = createNewProdutoNaoConformidadeRequest()
    cy.intercept(
      postRequest.method,
      postRequest.url,
      postRequest.response
    ).as('postProdutoRequest')

    const refreshGridRequest = getAllProdutosNaoConformidadesRequest()
    refreshGridRequest.response.body.items.push({
      codigo:strings[2],
      descricao:strings[2],
      unidadeMedida: strings[2],
      detalhamento:strings[3],
      quantidade: codes[3]
    })
    cy.intercept(
      refreshGridRequest.method,
      refreshGridRequest.url,
      refreshGridRequest.response).as('getProdutoRequest')

    cy.get('rnc-produtos-nao-conformidades-editor-modal vs-button[type="save"] button').click();

    cy.wait('@postProdutoRequest').then((interception:Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode,200)
      postRequest.expectedBody.id = interception.request.body.id
      interception.request.body.quantidade = Number.parseFloat(interception.request.body.quantidade)
      cy.validateRequestBody(interception.request.body, postRequest.expectedBody)
    })
    cy.wait('@getProdutoRequest')
  })
})
