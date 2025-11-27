import { getAllDefeitosRequest, getDefeitoRequestForId } from 'cypress/support/requests/defeitos/defeitos-requests';
import {
  getDefeitosNaoConformidadeRequestForId,
  updateDefeitosNaoConformidadeRequest,
  getAllDefeitosNaoConformidadeRequest,
} from 'cypress/support/requests/nao-conformidade/defeito-nao-conformidades/defeito-nao-conformidade.requests';
import { strings, codes } from 'cypress/support/test-utils';
import { navigateToNaoConformidadeEditor } from 'cypress/support/user-actions/nao-conformidades-user-actions';
import { Interception } from 'cypress/types/net-stubbing';
import { DefeitosNaoConformidadesFormControl } from '../../../../apps/rnc/src/app/pages/nao-conformidades/nao-conformidades-editor/defeitos-nao-conformidades/defeitos-nao-conformidades-editor-modal/defeitos-nao-conformidades-form-control';
import { NavigateToUpdateNaoConformidadeUserActionMock } from 'cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock';

describe.skip('Atualizar defeito com sucesso', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Atualizar nova defeito nao conformidade', () => {
    const defeitosList = getAllDefeitosRequest();
    const defeitoNaoConformidade = getDefeitosNaoConformidadeRequestForId();
    const defeitoById = getDefeitoRequestForId();
    cy.batchRequestStub([defeitoNaoConformidade, defeitoById]).then((aliasToWait) => {
      cy.get('rnc-defeitos-nao-conformidades vs-button[ng-reflect-icon=search] button').eq(0).click();
      cy.wait(aliasToWait[0]);
      cy.get('rnc-defeitos-nao-conformidades-editor-modal vs-button[type=save] button').should('be.disabled');

      cy.getVsInput(`${DefeitosNaoConformidadesFormControl.quantidade}`).clear({ force: true }).type('3', { force: true });

      cy.getAutoCompleteItem({
        formControlName: DefeitosNaoConformidadesFormControl.idDefeito,
        request: defeitosList,
        selector: 'qa-defeito-autocomplete-select',
        eq: 1,
      });

      cy.get(`vs-textarea[formControlName=${DefeitosNaoConformidadesFormControl.detalhamento}]`).clear().type(strings[3]);

      cy.get('rnc-defeitos-nao-conformidades-editor-modal vs-button[type=save] button').should('be.enabled');
    });
  });
  it('Salvar e validar', () => {
    const putRequest = updateDefeitosNaoConformidadeRequest();
    cy.intercept(putRequest.method, putRequest.url, putRequest.response).as('putDefeitoRequest');

    const refreshGridRequest = getAllDefeitosNaoConformidadeRequest();
    refreshGridRequest.response.body.items.push({
      codigo: codes[2],
      descricao: strings[2],
      detalhamento: strings[3],
    });
    cy.intercept(refreshGridRequest.method, refreshGridRequest.url, refreshGridRequest.response).as('getDefeitoRequest');

    cy.get('rnc-defeitos-nao-conformidades-editor-modal vs-button[type="save"] button').click();
    cy.wait('@putDefeitoRequest').then((interception: Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode, 200);
      putRequest.expectedBody.id = interception.request.body.id;
      putRequest.expectedBody.quantidade = interception.request.body.quantidade as Number;
      cy.validateRequestBody(interception.request.body, putRequest.expectedBody);
    });
  });
});
