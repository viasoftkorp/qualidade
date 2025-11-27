import { DefeitosNaoConformidadesFormControl } from '../../../../apps/rnc/src/app/pages/nao-conformidades/nao-conformidades-editor/defeitos-nao-conformidades/defeitos-nao-conformidades-editor-modal/defeitos-nao-conformidades-form-control';
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { getAllDefeitosRequest } from "cypress/support/requests/defeitos/defeitos-requests";
import { createNewDefeitosNaoConformidadeRequest, getAllDefeitosNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/defeito-nao-conformidades/defeito-nao-conformidade.requests";
import { codes, strings } from "cypress/support/test-utils";
import { navigateToNaoConformidadeEditor } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { Interception } from "cypress/types/net-stubbing";

describe('Criar defeito nao conformidades', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('criar nova defeito nao conformidade', () => {
    const defeitosRequests = getAllDefeitosRequest();

    cy.batchRequestStub(defeitosRequests)
    .then(aliasToWait => {
      cy.get('rnc-defeitos-nao-conformidades vs-button[icon=plus]').click();
      cy.get('rnc-defeitos-nao-conformidades-editor-modal vs-button[type=save] button').should('be.disabled');
      cy.get('div[class="cdk-overlay-pane vs-autocomplete-panel"] vs-button').eq(0).click();
      cy.wait(aliasToWait);
      cy.getVsInput(`${DefeitosNaoConformidadesFormControl.quantidade}`).clear().type('3')
      cy.get(`vs-textarea[formControlName=${DefeitosNaoConformidadesFormControl.detalhamento}]`).type(strings[2])
      cy.get('rnc-defeitos-nao-conformidades-editor-modal vs-button[type=save] button').should('be.enabled')

    })
  })
  it('validar body do post', () => {
     const postRequest = createNewDefeitosNaoConformidadeRequest()
    cy.intercept(
      postRequest.method,
      postRequest.url,
      postRequest.response
    ).as('postDefeitoRequest')

    const refreshGridRequest = getAllDefeitosNaoConformidadeRequest()
    refreshGridRequest.response.body.items.push({
      codigo:codes[2],
      descricao:strings[2],
      detalhamento:strings[2]
    })
    cy.intercept(
      refreshGridRequest.method,
      refreshGridRequest.url,
      refreshGridRequest.response).as('getDefeitoRequest')

    cy.get('rnc-defeitos-nao-conformidades-editor-modal vs-button[type="save"] button').click();
    cy.wait('@postDefeitoRequest').then((interception:Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode,200)
      postRequest.expectedBody.id = interception.request.body.id
      postRequest.expectedBody.quantidade = interception.request.body.quantidade as Number
      cy.validateRequestBody(interception.request.body, postRequest.expectedBody)
    })
    cy.wait('@getDefeitoRequest')
  })
})
