import { getAllPagelessProductsRequest, getProductByIdRequest } from 'cypress/support/requests/logistics-products/products-requests';
import { SolucaoProdutoFormControls } from '../../../../../apps/rnc/src/app/pages/settings/solucao/solucao-produto/solucao-produto-form-controls';
import { updateProdutoSolucaoRequest, getAllProdutosSolucaoView, getSolucaoRequestForId } from '../../../../support/requests/solucoes/solucoes-requests';
import { ids, strings } from 'cypress/support/test-utils';
import { Interception } from 'cypress/types/net-stubbing';

describe("Atualizar produto solucao com sucesso", () => {
  it("Visitar tela de produto solucao", () => {
    cy.batchRequestStub([getSolucaoRequestForId(),getAllProdutosSolucaoView(ids[0])]).then((alias: any) => {
      cy.visit(`rnc/configuracoes/solucoes/${ids[0]}/produtos`);
      cy.wait(alias);
    })
  })
  it("Clicar na primeira da lista", () => {
    cy.batchRequestStub([getProductByIdRequest(), getAllPagelessProductsRequest()]).then(aliases => {
      cy.get('rnc-solucao-produto vs-grid tbody tr').eq(0).click()
      cy.wait(aliases)
    })
  })
  it("Preencher formulário", () => {
    cy.get("rnc-solucao-produto-editor-modal vs-button[type='save'] button").should('be.disabled')

    cy.getVsInput(SolucaoProdutoFormControls.quantidade).clear({force: true}).type('1', {force: true})
    cy.getVsTextArea(SolucaoProdutoFormControls.operacaoEngenharia).type(strings[0])
    cy.getAutoCompleteItem({
      formControlName:SolucaoProdutoFormControls.idProduto,
      request:getAllPagelessProductsRequest(),
      selector: 'rnc-solucao-produto-editor-modal qa-produto-autocomplete-select',
      eq: 1
    })

    cy.get("rnc-solucao-produto-editor-modal vs-button[type='save'] button").should('be.enabled');
  })
  it("Salvar novo produto na solucao", () => {

    const putRequest = updateProdutoSolucaoRequest(ids[0])

    cy.batchRequestStub(putRequest).then(aliases => {
      cy.get("rnc-solucao-produto-editor-modal vs-button[type='save'] button").click()
      cy.wait(aliases).then((interception:Interception) => {
        cy.validateRequestStatusCode(interception.response?.statusCode, 200)
        interception.request.body.quantidade = Number.parseInt(interception.request.body.quantidade)
        putRequest.expectedBody.id = interception.request.body.id

        cy.validateRequestBody(interception.request.body, putRequest.expectedBody)
      })
    })
    const refreshGridRequest = getAllProdutosSolucaoView(ids[0])
    cy.batchRequestStub(refreshGridRequest).then(aliases => {
      cy.wait(aliases)
    })

  })
})

