import { GridsDependentesRequest } from "cypress/support/mock/defeitos-nao-conformidade-mock";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { getRequestsFromMock } from "cypress/support/test-utils";
import { navigateToNaoConformidadeEditor } from "cypress/support/user-actions/nao-conformidades-user-actions";

describe("Verificar se as grids dependentes de defeito receberam o idDefeito",() => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Verificar se as grids de causa, solução e ação preventiva não existem', () => {
    cy.get('mat-tab-group mat-tab-header div').contains('Causas').should('not.exist')
  })
  it('Selecionar defeito', () => {
    cy.get('rnc-defeitos-nao-conformidades vs-grid tbody tr').eq(0).click()
  })
  it('verificar as grids de causa, solucao e acao preventiva', () => {
    const getGridsRequest = new GridsDependentesRequest();
    cy.batchRequestStub(getRequestsFromMock(getGridsRequest))
    .then(aliasToWait => {
      cy.get('rnc-causas-nao-conformidades').should('exist').and('not.be.empty')
      cy.get('#mat-tab-label-1-1').click()
      cy.get('rnc-solucoes-nao-conformidades').should('exist').and('not.be.empty')
      cy.get('#mat-tab-label-1-2').click()
      cy.get('rnc-acoes-preventivas-nao-conformidades').should('exist').and('not.be.empty')
      cy.get('#mat-tab-label-1-0').click()
    })
  })
})
