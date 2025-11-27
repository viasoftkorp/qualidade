import {
  getAllSolucoesNaoConformidadeRequest,
   getUsersList,
   updateSolucoesNaoConformidadeRequest
 } from "cypress/support/requests/nao-conformidade/solucao-nao-conformidades/solucao-nao-conformidade.requests"
import { codes, strings } from "cypress/support/test-utils"
import { Interception } from "cypress/types/net-stubbing"
import { SolucoesNaoConformidadesFormControls } from "../../../../apps/rnc/src/app/pages/nao-conformidades/nao-conformidades-editor/defeitos-nao-conformidades/solucoes-nao-conformidades/solucoes-nao-conformidades-editor-modal/solucoes-nao-conformidades-form-controls"
import { getAllSolucoesRequest, getSolucaoRequestForId } from "cypress/support/requests/solucoes/solucoes-requests"
import { SelectDefeitosGridMock } from "cypress/support/mock/select-defeitos-grid-mock"
import { selectDefeitosGridElement } from "cypress/support/user-actions/nao-conformidades-user-actions"
import { getUserByIdRequest } from "cypress/support/requests/authentication/users-requests"
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock"

const solucaoEditorSelector = 'rnc-solucoes-nao-conformidades-editor-modal'
describe("Atualizar solucao com sucesso",() => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    const selectDefeitosGridRequests = new SelectDefeitosGridMock();
    selectDefeitosGridElement( naoConformidadeRequests, selectDefeitosGridRequests);
  });
  it('Clicar em Soluções', () => {
    cy.batchRequestStub(getAllSolucoesNaoConformidadeRequest()).then(aliases => {
      cy.get('mat-tab-group mat-tab-header div').contains('Soluções').click()
      cy.wait(aliases)
    })
  })
  it('Selecionar uma solução', () => {
    const getUserById = getUserByIdRequest();
    const getSolucaoById = getSolucaoRequestForId();
    cy.batchRequestStub([getUserById, getSolucaoById]).then(aliases => {
      cy.get('rnc-solucoes-nao-conformidades vs-grid tbody tr').eq(0).click()
      cy.wait(aliases)
    })
  })
  it('criar nova solucao nao conformidade', () => {
     const solucaoById = getSolucaoRequestForId()
     const usersList = getUsersList()
     const userById = getUserByIdRequest()
    cy.batchRequestStub([solucaoById, usersList, userById])
    .then(aliasesToWait => {
      cy.get(`${solucaoEditorSelector} vs-button[type=save] button`).should('be.disabled');
      cy.get(`${solucaoEditorSelector} vs-datepicker[ng-reflect-control-name=${SolucoesNaoConformidadesFormControls.dataAnalise}]`).clear().type(`01012022`)
      cy.get(`${solucaoEditorSelector} vs-datepicker[ng-reflect-control-name=${SolucoesNaoConformidadesFormControls.dataPrevistaImplantacao}]`).clear().type(`01012022`)
      cy.get(`${solucaoEditorSelector} vs-datepicker[ng-reflect-control-name=${SolucoesNaoConformidadesFormControls.dataVerificacao}]`).clear().type(`01012022`)
      cy.get(`${solucaoEditorSelector} vs-datepicker[ng-reflect-control-name=${SolucoesNaoConformidadesFormControls.novaData}]`).clear().type(`01012022`)
      cy.getVsInput(`${SolucoesNaoConformidadesFormControls.custoEstimado}`, solucaoEditorSelector).clear().type('4')
      cy.getVsCheckbox(SolucoesNaoConformidadesFormControls.solucaoImediata, solucaoEditorSelector).click({ force: true });
      cy.getAutoCompleteItem({
        formControlName:SolucoesNaoConformidadesFormControls.idSolucao,
        selector: `${solucaoEditorSelector} qa-solucao-autocomplete-select`,
        request: getAllSolucoesRequest(),
        eq:1
      })
      cy.getVsTextArea(SolucoesNaoConformidadesFormControls.detalhamento, solucaoEditorSelector)
        .clear()
        .type(strings[3])
      cy.get(`${solucaoEditorSelector} vs-button[type=save] button`).should('be.enabled')
    })
  })
  it('validar body do put', () => {
     const putRequest = updateSolucoesNaoConformidadeRequest()
    cy.intercept(
      putRequest.method,
      putRequest.url,
      putRequest.response
    ).as('putSolucaoRequest')

    const refreshGridRequest = getAllSolucoesNaoConformidadeRequest()
    refreshGridRequest.response.body.items.push({
      codigo:codes[2],
      descricao:strings[2],
      detalhamento:strings[3]
    })
    cy.intercept(
      refreshGridRequest.method,
      refreshGridRequest.url,
      refreshGridRequest.response).as('getSolucaoRequest')

    cy.get(`${solucaoEditorSelector} vs-button[type="save"] button`).click();
    cy.wait('@putSolucaoRequest').then((interception:Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode,200)
      putRequest.expectedBody.id = interception.request.body.id
      interception.request.body.dataAnalise = new Date(interception.request.body.dataAnalise)
      interception.request.body.dataPrevistaImplantacao = new Date(interception.request.body.dataPrevistaImplantacao)
      interception.request.body.dataVerificacao = new Date(interception.request.body.dataVerificacao)
      interception.request.body.novaData = new Date(interception.request.body.novaData)
      interception.request.body.custoEstimado = Number.parseFloat(interception.request.body.custoEstimado)
      cy.validateRequestBody(interception.request.body, putRequest.expectedBody)
    })
    cy.wait('@getSolucaoRequest')
  })
})
