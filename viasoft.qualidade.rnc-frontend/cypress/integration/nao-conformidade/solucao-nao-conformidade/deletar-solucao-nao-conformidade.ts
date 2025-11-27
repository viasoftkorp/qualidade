import { IPagedResultOutputDto } from "@viasoft/common";
import { SolucoesNaoConformidadesModel } from "@viasoft/rnc/api-clients/Nao-Conformidades";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { SelectDefeitosGridMock } from "cypress/support/mock/select-defeitos-grid-mock";
import { getAllCausasNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/causa-nao-conformidades/causa-nao-conformidade.requests";
import { deleteSolucoesNaoConformidadeRequest, getAllSolucoesNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/solucao-nao-conformidades/solucao-nao-conformidade.requests";
import { selectDefeitosGridElement } from "cypress/support/user-actions/nao-conformidades-user-actions";

describe('Deletar solucao nao conformidades', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    const selectDefeitosGridRequests = new SelectDefeitosGridMock();
    selectDefeitosGridElement( naoConformidadeRequests, selectDefeitosGridRequests);
  });
  it('Clicar em Soluções', () => {
    cy.batchRequestStub(getAllSolucoesNaoConformidadeRequest()).then(aliases => {
      cy.get('mat-tab-group mat-tab-header div').contains('Soluções').click()
      cy.wait(aliases)
    })
  })
   it("Deletar solucao com sucesso",() => {
    const deleteRequest = deleteSolucoesNaoConformidadeRequest()
    const refreshGridRequest = getAllSolucoesNaoConformidadeRequest()
    const causaList = getAllCausasNaoConformidadeRequest()

    const body = refreshGridRequest.response.body as IPagedResultOutputDto<SolucoesNaoConformidadesModel>
    if(body.items){
      body.items = [body.items[1]]
    }
    refreshGridRequest.response.body = body

    cy.batchRequestStub([deleteRequest, refreshGridRequest, causaList])
    .then(aliasesToWait => {
    cy.get('#mat-tab-label-1-1').click()
    cy.get('rnc-solucoes-nao-conformidades vs-button[ng-reflect-icon=trash-alt] button').eq(0).click()
    cy.get('vs-message-dialog vs-button[type="save"]').click()
    })
  })
})



