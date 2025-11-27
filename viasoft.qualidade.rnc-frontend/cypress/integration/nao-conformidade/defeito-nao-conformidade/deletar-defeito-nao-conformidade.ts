import { IPagedResultOutputDto } from "@viasoft/common";
import { DefeitosNaoConformidadesModel } from "@viasoft/rnc/api-clients/Nao-Conformidades";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { SelectDefeitosGridMock } from "cypress/support/mock/select-defeitos-grid-mock";
import { deleteDefeitosNaoConformidadeRequest, getAllDefeitosNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/defeito-nao-conformidades/defeito-nao-conformidade.requests";
import { selectDefeitosGridElement } from "cypress/support/user-actions/nao-conformidades-user-actions";

describe('Deletar defeito nao conformidades', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    const selectDefeitosGridRequests = new SelectDefeitosGridMock()
    selectDefeitosGridElement(naoConformidadeRequests, selectDefeitosGridRequests);
  });
   it("Deletar defeito com sucesso",() => {
    const deleteRequest = deleteDefeitosNaoConformidadeRequest()
    const refreshGridRequest = getAllDefeitosNaoConformidadeRequest()

    const body = refreshGridRequest.response.body as IPagedResultOutputDto<DefeitosNaoConformidadesModel>
    if(body.items){
      body.items = [body.items[1]]
    }
    refreshGridRequest.response.body = body

    cy.intercept(
      deleteRequest.method,
      deleteRequest.url,
      deleteRequest.response)
      .as('deleteRequest')
    cy.intercept(
      refreshGridRequest.method,
      refreshGridRequest.url,
      refreshGridRequest.response)
      .as('refreshGridRequest')
    cy.get('rnc-defeitos-nao-conformidades vs-button[ng-reflect-icon=trash-alt] button').eq(0).click()
    cy.get('vs-message-dialog vs-button[type="save"]').click()
    cy.wait('@deleteRequest')
    cy.wait('@refreshGridRequest')
  })
})
