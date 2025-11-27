
import { ProdutosNaoConformidadesFormControl } from "@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/produtos-nao-conformidades/produtos-nao-conformidades-editor-modal/produtos-nao-conformidades-form-control"
import { getAllPagelessProductsRequest, getAllProductsRequest, getProductByIdRequest } from "cypress/support/requests/logistics-products/products-requests";
import { getProdutosList } from "cypress/support/requests/nao-conformidade/nao-conformidades.requests";
import { getAllProdutosNaoConformidadesRequest, updateProdutosSolucoesNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/produtos-nao-conformidades/produtos-nao-conformidade.requests";
import { strings, codes } from "cypress/support/test-utils";
import { navigateToNaoConformidadeEditor } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { Interception } from "cypress/types/net-stubbing";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";

describe("Atualizar produto não conformidade com sucesso",() => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();

    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Clicar Produtos', () => {
    cy.batchRequestStub(getAllProdutosNaoConformidadesRequest()).then(aliases => {
      cy.getVsTabGroupItem('Produtos').click();
      cy.wait(aliases)
    })
  })

  it('Selecionar um produto', () => {
    cy.batchRequestStub([getProductByIdRequest()])
    .then(aliasesToWait => {
      cy.get('rnc-produtos-nao-conformidades vs-grid tbody tr').eq(0).click();
      cy.wait(aliasesToWait)
    })
  })

  it('Atualizar campos', () => {
    cy.get('rnc-produtos-nao-conformidades-editor-modal vs-button[type=save] button').should('be.disabled');

    cy.getVsInput(`${ProdutosNaoConformidadesFormControl.quantidade}`)
    .clear({force: true})
    .type(`${codes[3]}`, { force: true })

    cy.getAutoCompleteItem({
      formControlName: ProdutosNaoConformidadesFormControl.idProduto,
      request: getAllPagelessProductsRequest(),
      selector: 'rnc-produtos-nao-conformidades-editor-modal qa-produto-autocomplete-select',
      eq: 1,
    });

    cy.get(`rnc-produtos-nao-conformidades-editor-modal vs-textarea[formControlName=${ProdutosNaoConformidadesFormControl.detalhamento}]`)
    .clear()
    .type(strings[3])
    cy.getVsTextArea(ProdutosNaoConformidadesFormControl.operacaoEngenharia).type(strings[0])

    cy.get('rnc-produtos-nao-conformidades-editor-modal vs-button[type=save] button').should('be.enabled')

  })

  it('Salvar e validar', () => {
     const putRequest = updateProdutosSolucoesNaoConformidadeRequest()
    cy.intercept(
      putRequest.method,
      putRequest.url,
      putRequest.response
    ).as('putProdutoSolucaoRequest')

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
      refreshGridRequest.response).as('getProdutoSolucaoRequest')

    cy.get('rnc-produtos-nao-conformidades-editor-modal vs-button[type="save"] button').click();
    cy.wait('@putProdutoSolucaoRequest').then((interception:Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode,200)
      putRequest.expectedBody.id = interception.request.body.id
      interception.request.body.quantidade = Number.parseFloat(interception.request.body.quantidade)
      cy.validateRequestBody(interception.request.body, putRequest.expectedBody)
    })
    cy.wait('@getProdutoSolucaoRequest')
  })
})
