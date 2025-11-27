import { updateAcoesPreventivasNaoConformidadeRequest, getAllAcoesPreventivasNaoConformidadeRequest, getAcoesPreventivasList, getAcaoPreventivaById } from "cypress/support/requests/nao-conformidade/acao-preventiva-nao-conformidades/acao-preventiva-nao-conformidade.requests";
import { getUsersList } from "cypress/support/requests/nao-conformidade/solucao-nao-conformidades/solucao-nao-conformidade.requests";
import { strings, codes } from "cypress/support/test-utils";
import { Interception } from "cypress/types/net-stubbing";
import { AcoesPreventivasNaoConformidadesFormControl } from "@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/defeitos-nao-conformidades/acoes-preventivas-nao-conformidades/acoes-preventivas-nao-conformidades-editor-modal/acoes-preventivas-nao-conformidades-form-control"
import { SelectDefeitosGridMock } from "cypress/support/mock/select-defeitos-grid-mock";
import { selectDefeitosGridElement } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { getUserByIdRequest } from "cypress/support/requests/authentication/users-requests";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";

const acaoPreventivaEditorSelector = 'rnc-acoes-preventivas-nao-conformidades-editor-modal'
describe("Atualizar acao preventiva com sucesso",() => {
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
    const acoesPreventivasList = getAcoesPreventivasList();
    const acaoPreventivaById = getAcaoPreventivaById()
    const usersList = getUsersList()
    const userById = getUserByIdRequest()
    cy.batchRequestStub([acoesPreventivasList, acaoPreventivaById, usersList, userById])
    .then(aliasesToWait => {
      cy.get('rnc-acoes-preventivas-nao-conformidades vs-grid tbody tr').eq(0).click()
      cy.get(`${acaoPreventivaEditorSelector} vs-button[type=save] button`).should('be.disabled');

      cy.get(`${acaoPreventivaEditorSelector} vs-datepicker[ng-reflect-control-name=${AcoesPreventivasNaoConformidadesFormControl.dataAnalise}]`).clear().type(`01012022`)
      cy.get(`${acaoPreventivaEditorSelector} vs-datepicker[ng-reflect-control-name=${AcoesPreventivasNaoConformidadesFormControl.dataPrevistaImplantacao}]`).clear().type(`01012022`)
      cy.get(`${acaoPreventivaEditorSelector} vs-datepicker[ng-reflect-control-name=${AcoesPreventivasNaoConformidadesFormControl.dataVerificacao}]`).clear().type(`01012022`)
      cy.get(`${acaoPreventivaEditorSelector} vs-datepicker[ng-reflect-control-name=${AcoesPreventivasNaoConformidadesFormControl.novaData}]`).clear().type(`01012022`)
      cy.get(`${acaoPreventivaEditorSelector} vs-checkbox[formControlName=${AcoesPreventivasNaoConformidadesFormControl.implementada}] mat-checkbox input`).click({ force: true });
      cy.getAutoCompleteItem({
        formControlName:AcoesPreventivasNaoConformidadesFormControl.idAcaoPreventiva,
        selector: `${acaoPreventivaEditorSelector} qa-acao-preventiva-autocomplete-select`,
        request:getAcoesPreventivasList(),
        eq:1
      })
      cy.get(`${acaoPreventivaEditorSelector} vs-textarea[formControlName=${AcoesPreventivasNaoConformidadesFormControl.detalhamento}]`).clear().type(strings[3])
      cy.get(`${acaoPreventivaEditorSelector} vs-button[type=save] button`).should('be.enabled')
    })
  })
  it('validar body do put', () => {
     const putRequest = updateAcoesPreventivasNaoConformidadeRequest()
    cy.intercept(
      putRequest.method,
      putRequest.url,
      putRequest.response
    ).as('putAcaoPreventivaRequest')

    const refreshGridRequest = getAllAcoesPreventivasNaoConformidadeRequest()
    refreshGridRequest.response.body.items.push({
      codigo:codes[2],
      descricao:strings[2],
      detalhamento:strings[3]
    })
    cy.intercept(
      refreshGridRequest.method,
      refreshGridRequest.url,
      refreshGridRequest.response).as('getAcaoPreventivaRequest')

    cy.get(`${acaoPreventivaEditorSelector} vs-button[type="save"] button`).click();
    cy.wait('@putAcaoPreventivaRequest').then((interception:Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode,200)
      putRequest.expectedBody.id = interception.request.body.id
      interception.request.body.dataAnalise = new Date(interception.request.body.dataAnalise)
      interception.request.body.dataPrevistaImplantacao = new Date(interception.request.body.dataPrevistaImplantacao)
      interception.request.body.dataVerificacao = new Date(interception.request.body.dataVerificacao)
      interception.request.body.novaData = new Date(interception.request.body.novaData)
      cy.validateRequestBody(interception.request.body, putRequest.expectedBody)
    })
    cy.wait('@getAcaoPreventivaRequest')
  })
})
