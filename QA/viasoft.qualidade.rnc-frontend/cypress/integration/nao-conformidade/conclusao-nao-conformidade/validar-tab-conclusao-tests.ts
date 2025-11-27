import { getConclusaoNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/conclusao-nao-conformidades/conclusao-nao-conformidades-requests";
import { navigateToNaoConformidadeEditor } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { ConclusoesNaoConformidadesFormControl } from "@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/conclusao-nao-conformidades/conclusao-nao-conformidades-editor-modal/conclusoes-nao-conformidades-form-control";
import { getUserByIdRequest } from "cypress/support/requests/authentication/users-requests";
import { strings } from "cypress/support/test-utils";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";

describe('Verificar tab conclusao nao conformidades status fechado', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    naoConformidadeRequests.atualizarNaoConformidadeRequestsIniciaisMock.naoConformidadeById.response.body.status = 2;
    navigateToNaoConformidadeEditor(naoConformidadeRequests);
    cy.get('rnc-nao-conformidades-editor vs-button[icon=check-square]').should('not.exist')
  });
  it('Clicar em Conclusão', () => {
    cy.batchRequestStub([getConclusaoNaoConformidadeRequest(), getUserByIdRequest()]).then(aliases => {
      cy.get('mat-tab-group mat-tab-header div').contains('Conclusão').click()
      cy.wait(aliases)
    });
  })
   it('Verificar form tab conclusao', () => {
    cy.get(`admin-user-autocomplete-select[formControlName=${ConclusoesNaoConformidadesFormControl.idAuditor}] input`)
      .should('have.value', 'Admin Admin')
    cy.getVsInput(`${ConclusoesNaoConformidadesFormControl.evidencia}`).should('have.value', strings[0])
    cy.get(`vs-datepicker[ng-reflect-control-name=${ConclusoesNaoConformidadesFormControl.dataReuniao}] input`).should('have.value', '01.01.2022')
    cy.get(`vs-datepicker[ng-reflect-control-name=${ConclusoesNaoConformidadesFormControl.dataVerificacao}] input`).should('have.value', '01.01.2022')
    cy.getVsCheckbox(ConclusoesNaoConformidadesFormControl.eficaz).should('be.checked')
    cy.getVsCheckbox(ConclusoesNaoConformidadesFormControl.novaReuniao).should('be.checked')
  })

})
