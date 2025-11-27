import { IPagedResultOutputDto } from "@viasoft/common";
import { ProdutosNaoConformidadesOutput } from "@viasoft/rnc/api-clients/Nao-Conformidades/Produtos-Nao-Conformidades/model";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { getAllProdutosNaoConformidadesRequest, deleteProdutosSolucoesNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/produtos-nao-conformidades/produtos-nao-conformidade.requests";
import { navigateToNaoConformidadeEditor } from "cypress/support/user-actions/nao-conformidades-user-actions";

describe('Deletar produto nao conformidades', () => {
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
   it("Deletar produto não conformidade com sucesso",() => {
    const deleteRequest = deleteProdutosSolucoesNaoConformidadeRequest()
    const refreshGridRequest = getAllProdutosNaoConformidadesRequest()

    const body = refreshGridRequest.response.body as IPagedResultOutputDto<ProdutosNaoConformidadesOutput>
    if(body.items){
      body.items = [body.items[1]]
    }
    refreshGridRequest.response.body = body

    cy.batchRequestStub([deleteRequest, refreshGridRequest])
    .then(aliasesToWait => {
      cy.get('rnc-produtos-nao-conformidades vs-button[ng-reflect-icon=trash-alt] button').eq(0).click()
      cy.get('vs-message-dialog vs-button[type="save"]').click()
    })
  })
})



