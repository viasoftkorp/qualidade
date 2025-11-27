import { SelectDefeitosGridMock } from "cypress/support/mock/select-defeitos-grid-mock"
import { getCausaRequestForId } from "cypress/support/requests/causas/causas.request"
import { getAllCausasNaoConformidadeRequest, getCausasList, getCausasNaoConformidadeRequestForId, updateCausasNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/causa-nao-conformidades/causa-nao-conformidade.requests"
import { codes, strings } from "cypress/support/test-utils"
import { selectDefeitosGridElement } from "cypress/support/user-actions/nao-conformidades-user-actions"
import { Interception } from "cypress/types/net-stubbing"
import { CausasNaoConformidadesFormControl } from "@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/defeitos-nao-conformidades/causas-nao-conformidades/causas-nao-conformidade-editor-modal/causas-nao-conformidades-form-control"
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock"

const causaEditorSelector = 'rnc-causas-nao-conformidade-editor-modal'

describe("Atualizar causa com sucesso",() => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    const selectDefeitosGridRequests = new SelectDefeitosGridMock();
    selectDefeitosGridElement(naoConformidadeRequests, selectDefeitosGridRequests);
  });
  it('criar nova causa nao conformidade', () => {
    const causasList = getCausasList();
     const causaNaoConformidade = getCausasNaoConformidadeRequestForId();
     const causaById = getCausaRequestForId()
    cy.batchRequestStub([causasList, causaNaoConformidade, causaById])
    .then(aliasToWait => {
      cy.get('rnc-causas-nao-conformidades vs-grid tbody tr').eq(0).click();
      cy.wait(aliasToWait[2]);

      cy.get(`${causaEditorSelector} vs-button[type=save] button`).should('be.disabled');

      cy.getVsTextArea(CausasNaoConformidadesFormControl.detalhamento, causaEditorSelector)
        .clear({force: true})
        .type(strings[3], {force: true})

      cy.getAutoCompleteItem({
        formControlName:CausasNaoConformidadesFormControl.idCausa,
        request:causasList,
        selector: 'qa-causa-autocomplete-select',
        eq:1
      })

      cy.get(`${causaEditorSelector} vs-button[type=save] button`).should('be.enabled')
    })
  })
  it('validar body do put', () => {
     const putRequest = updateCausasNaoConformidadeRequest()
    cy.intercept(
      putRequest.method,
      putRequest.url,
      putRequest.response
    ).as('putCausaRequest')

    const refreshGridRequest = getAllCausasNaoConformidadeRequest()
    refreshGridRequest.response.body.items.push({
      codigo:codes[2],
      descricao:strings[2],
      detalhamento:strings[3]
    })
    cy.intercept(
      refreshGridRequest.method,
      refreshGridRequest.url,
      refreshGridRequest.response).as('getCausaRequest')

    cy.get(`${causaEditorSelector} vs-button[type="save"] button`).click();
    cy.wait('@putCausaRequest').then((interception:Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode,200)
      putRequest.expectedBody.id = interception.request.body.id
      cy.validateRequestBody(interception.request.body, putRequest.expectedBody)
    })
    cy.wait('@getCausaRequest')
  })
})
