import { SelectDefeitosGridMock } from "cypress/support/mock/select-defeitos-grid-mock";
import { createImplementacoesEvitarReincidenciaNaoConformidadeRequest, getAllImplementacoesEvitarReincidenciaNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/implementacao-evitar-reincidencia-nao-conformidades/implementacao-evitar-reincidencia-nao-conformidades";
import { selectDefeitosGridElement } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { ImplementacaoEvitarReincidenciaNaoConformidadesFormControl } from '../../../../apps/rnc/src/app/pages/nao-conformidades/nao-conformidades-editor/defeitos-nao-conformidades/implementacao-evitar-reincidencia-nao-conformidades/implementacao-evitar-reincidencia-nao-conformidades-editor-modal/implementacao-evitar-reincidencia-nao-conformidades-form-control'
const mainSelector = 'rnc-implementacao-evitar-reincidencia-nao-conformidades'
const editorModalSelector = 'rnc-implementacao-evitar-reincidencia-nao-conformidades-editor-modal'

import { getAllUsersRequest } from "cypress/support/requests/authentication/users-requests";
import { ids, strings } from "cypress/support/test-utils";
import { Interception } from "cypress/types/net-stubbing";
import { ImplementacaoEvitarReincidenciaNaoConformidadesInput } from "@viasoft/rnc/api-clients/Nao-Conformidades/Implementacao-Evitar-Reincidencia-Nao-Conformidades/model/implementacao-evitar-reincidencia-nao-conformidades-input";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
describe.skip('Se campos obrigatórios informados, deve chamar criação da implementação', () => {
  it('Navegar para defeitos no editor de Nao Conformidade', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    const selectDefeitosGridRequests = new SelectDefeitosGridMock();
    selectDefeitosGridElement(naoConformidadeRequests, selectDefeitosGridRequests);
  });
  it('Clicar Em Implem. Eficácia Evitar Reinc.', () => {
    cy.batchRequestStub(getAllImplementacoesEvitarReincidenciaNaoConformidadeRequest()).then(alias => {
      cy.get('mat-tab-group mat-tab-header div').contains('Implem. Eficácia Evitar Reinc.').click()

      cy.wait(alias)
    })
  })
  it('Criar nova implementação nao conformidade', () => {
    cy.get(`${mainSelector} vs-button[icon=plus]`).click();
    cy.get(`${editorModalSelector} vs-button[type=save] button`).should('be.disabled');

    cy.getVsTextArea(ImplementacaoEvitarReincidenciaNaoConformidadesFormControl.descricao, editorModalSelector).type(strings[1])
    cy.get(`${editorModalSelector} vs-button[type=save] button`).should('be.enabled')
    cy.getAutoCompleteItem({
      formControlName:ImplementacaoEvitarReincidenciaNaoConformidadesFormControl.idResponsavel,
      request: getAllUsersRequest(),
      selector:'admin-user-autocomplete-select',
      eq:1
    })
    cy.getAutoCompleteItem({
      formControlName:ImplementacaoEvitarReincidenciaNaoConformidadesFormControl.idAuditor,
      request: getAllUsersRequest(),
      selector:'admin-user-autocomplete-select',
      eq:1
    })
    cy.getVsDatePicker(ImplementacaoEvitarReincidenciaNaoConformidadesFormControl.dataAnalise, editorModalSelector).type(`01012022`)
    cy.getVsDatePicker(ImplementacaoEvitarReincidenciaNaoConformidadesFormControl.dataPrevistaImplantacao, editorModalSelector).type(`01012022`)
    cy.getVsDatePicker(ImplementacaoEvitarReincidenciaNaoConformidadesFormControl.dataVerificacao, editorModalSelector).type(`01012022`)
    cy.getVsDatePicker(ImplementacaoEvitarReincidenciaNaoConformidadesFormControl.novaData).type(`01012022`)

    cy.get(`vs-checkbox[formControlName=${ImplementacaoEvitarReincidenciaNaoConformidadesFormControl.acaoImplementada}] mat-checkbox input`).click({ force: true });
  })
  it('Salvar e validar dados', () => {
    cy.batchRequestStub([createImplementacoesEvitarReincidenciaNaoConformidadeRequest(), getAllImplementacoesEvitarReincidenciaNaoConformidadeRequest()]).then(alias => {
      cy.get(`${editorModalSelector} vs-button[type=save] button`).click();
      cy.wait(alias).then((interceptions:Array<Interception>) => {
        const interceptionRequestBody = interceptions[0].request.body as ImplementacaoEvitarReincidenciaNaoConformidadesInput;
        const expectedBody = {
          ...createImplementacoesEvitarReincidenciaNaoConformidadeRequest().expectedBody,
          id: interceptionRequestBody.id,
          acaoImplementada: true,
          descricao: strings[1],
          idAuditor: ids[1],
          idResponsavel: ids[1],
          idDefeitoNaoConformidade: ids[0],
          idNaoConformidade: ids[0],
        } as ImplementacaoEvitarReincidenciaNaoConformidadesInput
        cy.validateRequestBody(interceptionRequestBody, expectedBody)
      })
    })
  })
})
