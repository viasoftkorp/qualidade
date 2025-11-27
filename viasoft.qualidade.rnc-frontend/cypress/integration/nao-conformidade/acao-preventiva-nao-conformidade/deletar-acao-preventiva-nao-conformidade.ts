import { IPagedResultOutputDto } from "@viasoft/common";
import { AcoesPreventivasNaoConformidadesModel } from "@viasoft/rnc/api-clients/Nao-Conformidades";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { SelectDefeitosGridMock } from "cypress/support/mock/select-defeitos-grid-mock";
import { deleteAcoesPreventivasNaoConformidadeRequest, getAllAcoesPreventivasNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/acao-preventiva-nao-conformidades/acao-preventiva-nao-conformidade.requests";
import { getAllCausasNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/causa-nao-conformidades/causa-nao-conformidade.requests";
import { selectDefeitosGridElement } from "cypress/support/user-actions/nao-conformidades-user-actions";

describe('Deletar acao preventiva nao conformidades', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    const selectDefeitosGridRequests = new SelectDefeitosGridMock();
    selectDefeitosGridElement(naoConformidadeRequests, selectDefeitosGridRequests);
  });
  it('Clicar Em Açoes preventivas', () => {
    cy.batchRequestStub(getAllAcoesPreventivasNaoConformidadeRequest()).then(alias => {
      cy.get('mat-tab-group mat-tab-header div').contains('Ações Preventivas').click()

      cy.wait(alias)
    })
  })
   it("Deletar acao preventiva com sucesso",() => {
    const deleteRequest = deleteAcoesPreventivasNaoConformidadeRequest()
    const refreshGridRequest = getAllAcoesPreventivasNaoConformidadeRequest()
    const causaList = getAllCausasNaoConformidadeRequest()

    const body = refreshGridRequest.response.body as IPagedResultOutputDto<AcoesPreventivasNaoConformidadesModel>
    if(body.items){
      body.items = [body.items[1]]
    }
    refreshGridRequest.response.body = body

    cy.batchRequestStub([deleteRequest, refreshGridRequest, causaList])
    .then(aliasesToWait => {
    cy.get('#mat-tab-label-1-2').click()
    cy.get('rnc-acoes-preventivas-nao-conformidades vs-button[ng-reflect-icon=trash-alt] button').eq(0).click()
    cy.get('vs-message-dialog vs-button[type="save"]').click()

    })
  })
})



