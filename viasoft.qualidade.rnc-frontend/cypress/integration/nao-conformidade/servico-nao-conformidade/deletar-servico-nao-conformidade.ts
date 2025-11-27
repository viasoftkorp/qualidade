import { IPagedResultOutputDto } from "@viasoft/common";
import { ServicosNaoConformidadesModel } from "@viasoft/rnc/api-clients/Nao-Conformidades";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { deleteServicosNaoConformidadeRequest, getAllServicosNaoConformidadeRequest, updateServicosNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/servico-nao-conformidades/servico-nao-conformidade.requests";
import { navigateToNaoConformidadeEditor } from "cypress/support/user-actions/nao-conformidades-user-actions";

describe('Deletar servicos solucao nao conformidades', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();

    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Clicar em Serviços', () => {
    cy.batchRequestStub(getAllServicosNaoConformidadeRequest()).then((aliases) => {
      cy.getVsTabGroupItem('Serviços').click()
      cy.wait(aliases);
    });
  });
   it("Deletar servicos não conformidade com sucesso",() => {
    const deleteRequest = deleteServicosNaoConformidadeRequest()
    const refreshGridRequest = getAllServicosNaoConformidadeRequest()

    const body = refreshGridRequest.response.body as IPagedResultOutputDto<ServicosNaoConformidadesModel>
    if(body.items){
      body.items = [body.items[1]]
    }
    refreshGridRequest.response.body = body

    cy.batchRequestStub([deleteRequest, refreshGridRequest])
    .then(aliasesToWait => {
      cy.get('rnc-servicos-nao-conformidades vs-button[ng-reflect-icon=trash-alt] button').eq(0).click()
      cy.get('vs-message-dialog vs-button[type="save"]').click()
      cy.wait(aliasesToWait)
    })
  })
})



