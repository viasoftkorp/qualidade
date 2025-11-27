import { getAcoesPreventivasList, createNewAcoesPreventivasNaoConformidadeRequest, getAllAcoesPreventivasNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/acao-preventiva-nao-conformidades/acao-preventiva-nao-conformidade.requests";
import { strings, codes, ids } from "cypress/support/test-utils";
import { Interception } from "cypress/types/net-stubbing";
import { AcoesPreventivasNaoConformidadesFormControl } from "@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/defeitos-nao-conformidades/acoes-preventivas-nao-conformidades/acoes-preventivas-nao-conformidades-editor-modal/acoes-preventivas-nao-conformidades-form-control"
import { SelectDefeitosGridMock } from "cypress/support/mock/select-defeitos-grid-mock";
import { selectDefeitosGridElement } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { getAllUsersRequest } from "cypress/support/requests/authentication/users-requests";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";

const acaoPreventivaEditorSelector = 'rnc-acoes-preventivas-nao-conformidades-editor-modal'
describe.skip('Criar acao preventiva nao conformidades', () => {
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
  it('criar nova acao preventiva nao conformidade', () => {
    cy.get('rnc-acoes-preventivas-nao-conformidades vs-button[icon=plus]').click();
    cy.get(`${acaoPreventivaEditorSelector} vs-button[type=save] button`).should('be.disabled');

    cy.getAutoCompleteItem({
      formControlName:AcoesPreventivasNaoConformidadesFormControl.idAcaoPreventiva,
      request: getAcoesPreventivasList(),
      selector:`${acaoPreventivaEditorSelector} qa-acao-preventiva-autocomplete-select`
    })

    cy.get(`${acaoPreventivaEditorSelector} vs-datepicker[ng-reflect-control-name=${AcoesPreventivasNaoConformidadesFormControl.dataAnalise}]`).type(`01012022`)
    cy.get(`${acaoPreventivaEditorSelector} vs-datepicker[ng-reflect-control-name=${AcoesPreventivasNaoConformidadesFormControl.dataPrevistaImplantacao}]`).type(`01012022`)
    cy.get(`${acaoPreventivaEditorSelector} vs-datepicker[ng-reflect-control-name=${AcoesPreventivasNaoConformidadesFormControl.dataVerificacao}]`).type(`01012022`)
    cy.get(`${acaoPreventivaEditorSelector} vs-datepicker[ng-reflect-control-name=${AcoesPreventivasNaoConformidadesFormControl.novaData}]`).type(`01012022`)
    cy.get(`${acaoPreventivaEditorSelector} vs-checkbox[formControlName=${AcoesPreventivasNaoConformidadesFormControl.implementada}] mat-checkbox input`).click({ force: true });
    cy.get(`${acaoPreventivaEditorSelector} vs-textarea[formControlName=${AcoesPreventivasNaoConformidadesFormControl.detalhamento}]`).type(strings[2])
    cy.getAutoCompleteItem({
      formControlName:AcoesPreventivasNaoConformidadesFormControl.idResponsavel,
      request:getAllUsersRequest(),
      selector:`${acaoPreventivaEditorSelector} admin-user-autocomplete-select`
    })
    cy.get(`${acaoPreventivaEditorSelector} vs-button[type=save] button`).should('be.enabled')

  })
  it('validar body do post', () => {
     const postRequest = createNewAcoesPreventivasNaoConformidadeRequest()
    cy.intercept(
      postRequest.method,
      postRequest.url,
      postRequest.response
    ).as('postAcaoPreventivaRequest')

    const refreshGridRequest = getAllAcoesPreventivasNaoConformidadeRequest()
    refreshGridRequest.response.body.items.push({
      codigo:codes[2],
      descricao:strings[2],
      detalhamento:strings[2]
    })
    cy.intercept(
      refreshGridRequest.method,
      refreshGridRequest.url,
      refreshGridRequest.response).as('getAcaoPreventivaRequest')

    cy.get(`${acaoPreventivaEditorSelector} vs-button[type="save"] button`).click();
    cy.wait('@postAcaoPreventivaRequest').then((interception:Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode,200)
      postRequest.expectedBody.id = interception.request.body.id
      interception.request.body.idAuditor = ids[0]
      interception.request.body.idResponsavel = ids[0]
      interception.request.body.dataAnalise = new Date(interception.request.body.dataAnalise)
      interception.request.body.dataPrevistaImplantacao = new Date(interception.request.body.dataPrevistaImplantacao)
      interception.request.body.dataVerificacao = new Date(interception.request.body.dataVerificacao)
      interception.request.body.novaData = new Date(interception.request.body.novaData)
      cy.validateRequestBody(interception.request.body, postRequest.expectedBody)
    })
    cy.wait('@getAcaoPreventivaRequest')
  })
})
