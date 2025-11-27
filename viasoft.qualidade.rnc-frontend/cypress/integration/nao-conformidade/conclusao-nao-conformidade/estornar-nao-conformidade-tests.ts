import { ConclusoesNaoConformidadesFormControl } from "@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/conclusao-nao-conformidades/conclusao-nao-conformidades-editor-modal/conclusoes-nao-conformidades-form-control";
import { NaoConformidadesFormControl } from "@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/nao-conformidades-form-control";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { getAllUsersRequest } from "cypress/support/requests/authentication/users-requests";
import { calcularCicloTempoRequest, concluirNaoConformidadeRequest, estornarConclusaoNaoConformidadeRequest, getConclusaoNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/conclusao-nao-conformidades/conclusao-nao-conformidades-requests";
import { ids } from "cypress/support/test-utils";
import { navigateToNaoConformidadeEditor } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { Interception } from "cypress/types/net-stubbing";
import { getNaoConformidadeRequestForId } from "../../../support/requests/nao-conformidade/nao-conformidades.requests";
import {
  getOperacaoRetrabalhoRequest
} from "../../../support/requests/nao-conformidade/operacao-retrabalho-nao-conformidades/operacao-retrabalho-nao-conformidades-requests";
import {
  getOrdemRetrabalhoRequest
} from "../../../support/requests/nao-conformidade/ordem-retrabalho-nao-conformidades/ordem-retrabalho-nao-conformidades";

describe('Se não conformidade está fechada, deve permitir estorno da conclusão', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    naoConformidadeRequests.atualizarNaoConformidadeRequestsIniciaisMock.naoConformidadeById.response.body.status = 2;
    navigateToNaoConformidadeEditor(naoConformidadeRequests);
    cy.get('rnc-nao-conformidades-editor vs-button[icon=check-square]').should('not.exist')
  });

  it('Se não conformidade fechada, botão de estorno deve aparecer', () => {
    cy.get('rnc-nao-conformidades-editor div[actions] vs-button[label="NaoConformidades.EstornarConclusao"] button')
      .should('exist')
  })

  it('Se não confirmar estorno conclusão, nada deve acontecer', () => {
    cy.get('rnc-nao-conformidades-editor div[actions] vs-button[label="NaoConformidades.EstornarConclusao"] button')
      .click();

    cy.get('vs-message-dialog vs-button[type="cancel"] button').click()
  })

  it('Se confirmado estorno conclusão, deve mandar estornar conclusão', () => {
    cy.get('rnc-nao-conformidades-editor div[actions] vs-button[label="NaoConformidades.EstornarConclusao"] button')
      .click();

    cy.batchRequestStub([
      estornarConclusaoNaoConformidadeRequest(), getNaoConformidadeRequestForId(),
      getOperacaoRetrabalhoRequest(), getOrdemRetrabalhoRequest()
    ]).then(alias => {
      cy.get('vs-message-dialog vs-button[type="save"] button').click()
      cy.wait(alias);
    })
  })

  it('Se conclusão estornada, deve habilitar edição da não conformidade', () => {
    cy.getVsInput(NaoConformidadesFormControl.numeroOdf ,'rnc-nao-conformidades-editor vs-form form').should('not.be.disabled');
  })
})
