import { SolucoesNaoConformidadesFormControls } from "../../../../apps/rnc/src/app/pages/nao-conformidades/nao-conformidades-editor/defeitos-nao-conformidades/solucoes-nao-conformidades/solucoes-nao-conformidades-editor-modal/solucoes-nao-conformidades-form-controls"
import { getAllSolucoesRequest } from "cypress/support/requests/solucoes/solucoes-requests";
import { createNewSolucoesNaoConformidadeRequest, getAllSolucoesNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/solucao-nao-conformidades/solucao-nao-conformidade.requests";
import { codes, ids, strings } from "cypress/support/test-utils";
import { Interception } from "cypress/types/net-stubbing";
import { SelectDefeitosGridMock } from "cypress/support/mock/select-defeitos-grid-mock";
import { selectDefeitosGridElement } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";

const solucaoEditorSelector = 'rnc-solucoes-nao-conformidades-editor-modal'

describe.skip('Criar solucao nao conformidades', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    const selectDefeitosGridRequests = new SelectDefeitosGridMock();
    selectDefeitosGridElement(naoConformidadeRequests, selectDefeitosGridRequests);
  });
  it('Clicar em Soluções', () => {
    cy.batchRequestStub(getAllSolucoesNaoConformidadeRequest()).then(aliases => {
      cy.get('mat-tab-group mat-tab-header div').contains('Soluções').click()
      cy.wait(aliases)
    })
  })
  it('criar nova solucao nao conformidade', () => {
    cy.get('rnc-solucoes-nao-conformidades vs-button[icon=plus]').click();
    cy.get(`${solucaoEditorSelector} vs-button[type=save] button`).should('be.disabled');

    cy.getAutoCompleteItem({
      formControlName:SolucoesNaoConformidadesFormControls.idSolucao,
      request: getAllSolucoesRequest(),
      selector:`${solucaoEditorSelector} qa-solucao-autocomplete-select`,
      eq:1
    })

    cy.getVsDatePicker(SolucoesNaoConformidadesFormControls.dataAnalise, solucaoEditorSelector).type(`01012022`)
    cy.getVsDatePicker(SolucoesNaoConformidadesFormControls.dataPrevistaImplantacao, solucaoEditorSelector).type(`01012022`)
    cy.getVsDatePicker(SolucoesNaoConformidadesFormControls.dataVerificacao, solucaoEditorSelector).type(`01012022`)
    cy.getVsDatePicker(SolucoesNaoConformidadesFormControls.novaData, solucaoEditorSelector).type(`01012022`)
    cy.getVsInput(SolucoesNaoConformidadesFormControls.custoEstimado, solucaoEditorSelector).type('3')
    cy.getVsCheckbox(SolucoesNaoConformidadesFormControls.solucaoImediata, solucaoEditorSelector).click({ force: true });
    cy.getVsTextArea(SolucoesNaoConformidadesFormControls.detalhamento, solucaoEditorSelector).clear()
    cy.getVsTextArea(SolucoesNaoConformidadesFormControls.detalhamento, solucaoEditorSelector).type(strings[2])
    cy.get(`${solucaoEditorSelector} vs-button[type=save] button`).should('be.enabled')

  })
  it('validar body do post', () => {
     const postRequest = createNewSolucoesNaoConformidadeRequest()
    cy.intercept(
      postRequest.method,
      postRequest.url,
      postRequest.response
    ).as('postSolucaoRequest')

    const refreshGridRequest = getAllSolucoesNaoConformidadeRequest()
    refreshGridRequest.response.body.items.push({
      codigo:codes[2],
      descricao:strings[2],
      detalhamento:strings[2]
    })
    cy.intercept(
      refreshGridRequest.method,
      refreshGridRequest.url,
      refreshGridRequest.response).as('getSolucaoRequest')

    cy.get('rnc-solucoes-nao-conformidades-editor-modal vs-button[type="save"] button').click();
    cy.wait('@postSolucaoRequest').then((interception:Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode,200)
      postRequest.expectedBody.id = interception.request.body.id
      interception.request.body.idAuditor = ids[0]
      interception.request.body.idResponsavel = ids[0]
      interception.request.body.dataAnalise = new Date(interception.request.body.dataAnalise)
      interception.request.body.dataPrevistaImplantacao = new Date(interception.request.body.dataPrevistaImplantacao)
      interception.request.body.dataVerificacao = new Date(interception.request.body.dataVerificacao)
      interception.request.body.novaData = new Date(interception.request.body.novaData)
      interception.request.body.custoEstimado = Number.parseFloat(interception.request.body.custoEstimado)
      cy.validateRequestBody(interception.request.body, postRequest.expectedBody)
    })
    cy.wait('@getSolucaoRequest')
  })
})
