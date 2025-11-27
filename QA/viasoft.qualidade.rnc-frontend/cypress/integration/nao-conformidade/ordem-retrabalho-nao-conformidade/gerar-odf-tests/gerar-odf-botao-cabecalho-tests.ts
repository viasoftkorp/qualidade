import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { navigateToNaoConformidadeEditor } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { NaoConformidadesFormControl } from '../../../../../apps/rnc/src/app/pages/nao-conformidades/nao-conformidades-editor/nao-conformidades-form-control';
import { codes } from "cypress/support/test-utils";
import { updateNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/nao-conformidades.requests";

describe('Se odf de retrabalho não foi gerada e pode gerar odf de retrabalho, botão deve ficar habilitado', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    naoConformidadeRequests.atualizarNaoConformidadeRequestsRestantesMock.getListOrdemProducao?.response.body.items[0].odfFinalizada = true;
    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Verificar se botão "Gerar Odf Retrabalho está habilitado', () => {
    cy.get('rnc-nao-conformidades-editor vs-header vs-button').contains('Gerar Odf Retrabalho').should('be.enabled')
  })
})

describe('Se ao pressionar para gerar odf de retrabalho, rnc ainda não salva, deve salva-la', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    naoConformidadeRequests.atualizarNaoConformidadeRequestsRestantesMock.getListOrdemProducao?.response.body.items[0].odfFinalizada = true;
    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Modificar lote', () => {
    cy.getVsInput(NaoConformidadesFormControl.numeroLote).type(codes[0].toString())
  })
  it('Prencionar para gerar odf de retrabalho e verificar se rnc foi salva', () => {
    cy.batchRequestStub(updateNaoConformidadeRequest()).then(alias => {
      cy.get('rnc-nao-conformidades-editor vs-header vs-button[icon=wrench] button').click()
      cy.wait(alias)
    })
  })
})

