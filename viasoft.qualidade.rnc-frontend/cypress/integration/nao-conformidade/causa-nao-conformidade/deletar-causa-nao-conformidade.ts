import { IPagedResultOutputDto } from "@viasoft/common";
import { CausasNaoConformidadesModel } from "@viasoft/rnc/api-clients/Nao-Conformidades";
import { SelectDefeitosGridMock } from "cypress/support/mock/select-defeitos-grid-mock";
import { deleteCausasNaoConformidadeRequest, getAllCausasNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/causa-nao-conformidades/causa-nao-conformidade.requests";
import { selectDefeitosGridElement } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";

describe('Deletar causa nao conformidades', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    const selectDefeitosGridRequests = new SelectDefeitosGridMock();
    selectDefeitosGridElement(naoConformidadeRequests, selectDefeitosGridRequests);
  });
   it("Deletar causa com sucesso",() => {
    const deleteRequest = deleteCausasNaoConformidadeRequest()
    const refreshGridRequest = getAllCausasNaoConformidadeRequest()
    const body = refreshGridRequest.response.body as IPagedResultOutputDto<CausasNaoConformidadesModel>
    if(body.items){
      body.items = [body.items[1]]
    }
    refreshGridRequest.response.body = body
    cy.batchRequestStub([deleteRequest]).then(aliases => {
      cy.get('rnc-causas-nao-conformidades vs-button[ng-reflect-icon=trash-alt] button').eq(0).click()
      cy.get('vs-message-dialog vs-button[type="save"]').click()
      cy.wait(aliases)
    })
  })
})
