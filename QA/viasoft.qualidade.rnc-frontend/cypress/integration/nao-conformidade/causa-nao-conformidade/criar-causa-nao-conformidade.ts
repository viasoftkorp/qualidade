import { CausasNaoConformidadesFormControl } from "@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/defeitos-nao-conformidades/causas-nao-conformidades/causas-nao-conformidade-editor-modal/causas-nao-conformidades-form-control"
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { SelectDefeitosGridMock } from "cypress/support/mock/select-defeitos-grid-mock";
import { createNewCausasNaoConformidadeRequest, getAllCausasNaoConformidadeRequest, getCausasList } from "cypress/support/requests/nao-conformidade/causa-nao-conformidades/causa-nao-conformidade.requests";
import { codes, strings } from "cypress/support/test-utils";
import { selectDefeitosGridElement } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { Interception } from "cypress/types/net-stubbing";

const causaEditorSelector = 'rnc-causas-nao-conformidade-editor-modal'

describe.skip('Criar causa nao conformidades', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    const selectDefeitosGridRequests = new SelectDefeitosGridMock();
    selectDefeitosGridElement(naoConformidadeRequests, selectDefeitosGridRequests);
  });
  it('criar nova causa nao conformidade', () => {
    cy.get('rnc-causas-nao-conformidades vs-button[icon=plus]').click();
    cy.get(`${causaEditorSelector} vs-button[type=save] button`).should('be.disabled');

    cy.getAutoCompleteItem({
      formControlName:CausasNaoConformidadesFormControl.idCausa,
      request: getCausasList(),
      selector: 'qa-causa-autocomplete-select'
    })

    cy.getVsTextArea(CausasNaoConformidadesFormControl.detalhamento, causaEditorSelector).type(strings[2])
    cy.get(`${causaEditorSelector} vs-button[type=save] button`).should('be.enabled')

  })
  it('validar body do post', () => {
     const postRequest = createNewCausasNaoConformidadeRequest()
    cy.intercept(
      postRequest.method,
      postRequest.url,
      postRequest.response
    ).as('postCausaRequest')

    const refreshGridRequest = getAllCausasNaoConformidadeRequest()
    refreshGridRequest.response.body.items.push({
      codigo:codes[2],
      descricao:strings[2],
      detalhamento:strings[2]
    })
    cy.intercept(
      refreshGridRequest.method,
      refreshGridRequest.url,
      refreshGridRequest.response).as('getCausaRequest')

    cy.get(`${causaEditorSelector} vs-button[type="save"] button`).click();
    cy.wait('@postCausaRequest').then((interception:Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode,200)
      postRequest.expectedBody.id = interception.request.body.id
      cy.validateRequestBody(interception.request.body, postRequest.expectedBody)
    })
    cy.wait('@getCausaRequest')
  })
})
