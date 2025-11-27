import { ConclusoesNaoConformidadesFormControl } from "@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/conclusao-nao-conformidades/conclusao-nao-conformidades-editor-modal/conclusoes-nao-conformidades-form-control";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { getAllUsersRequest } from "cypress/support/requests/authentication/users-requests";

import { calcularCicloTempoRequest, concluirNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/conclusao-nao-conformidades/conclusao-nao-conformidades-requests";
import { strings, ids } from "cypress/support/test-utils";
import { navigateToNaoConformidadeEditor } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { Interception } from "cypress/types/net-stubbing";

describe('Concluir nao conformidades', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Verificar se tab de conclusão não existe', () => {
    cy.get('mat-tab-group header div').contains('Conclusão').should('not.exist')
  })
  it('Abrir modal de conclusao de não conformidade', () => {
    cy.batchRequestStub([getAllUsersRequest(), calcularCicloTempoRequest()]).then(aliases => {
      cy.get('rnc-nao-conformidades-editor vs-button[icon=check-square]').click();

      cy.wait(aliases)
    })
  })
  it('Preencher campos', () => {
    cy.get('mat-dialog-container vs-button[type=save] button').should('be.disabled');
    cy.get('div.vs-autocomplete-panel vs-button button').eq(0).click()
    cy.getVsInput(ConclusoesNaoConformidadesFormControl.evidencia).clear().type(strings[2])
    cy.getVsCheckbox(ConclusoesNaoConformidadesFormControl.eficaz).click({ force: true })
    cy.getVsCheckbox(ConclusoesNaoConformidadesFormControl.novaReuniao).click({ force: true })
    cy.get(`vs-datepicker[ng-reflect-control-name=${ConclusoesNaoConformidadesFormControl.dataReuniao}]`).type(`01012022`)
    cy.get(`vs-datepicker[ng-reflect-control-name=${ConclusoesNaoConformidadesFormControl.dataVerificacao}]`).type(`01012022`)
    cy.get('mat-dialog-container vs-button[type=save] button').should('be.enabled')
  })

  it('validar body do post', () => {
     const postRequest = concluirNaoConformidadeRequest()
    cy.intercept(
      postRequest.method,
      postRequest.url,
      postRequest.response
    ).as('postRequest')

    cy.get('mat-dialog-container vs-button[type="save"] button').click();
    cy.wait('@postRequest').then((interception:Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode,200)
      postRequest.expectedBody.id = interception.request.body.id
      interception.request.body.idAuditor = ids[0];
      interception.request.body.dataVerificacao = new Date(interception.request.body.dataVerificacao)
      interception.request.body.dataReuniao = new Date(interception.request.body.dataReuniao)
      cy.validateRequestBody(interception.request.body, postRequest.expectedBody)
    })
  })
})
describe('Validar se data da reunião desabilita se não tiver nova reunião', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Abrir modal de conclusao de não conformidade', () => {
    cy.batchRequestStub([getAllUsersRequest(), calcularCicloTempoRequest()]).then(aliases => {
      cy.get('rnc-nao-conformidades-editor vs-button[icon=check-square]').click();

      cy.wait(aliases)
    })
  })
  it('Preencher campos', () => {
    cy.get('mat-dialog-container vs-button[type=save] button').should('be.disabled');
    cy.get('div.vs-autocomplete-panel vs-button button').eq(0).click()
    cy.get(`vs-datepicker[ng-reflect-control-name=${ConclusoesNaoConformidadesFormControl.dataVerificacao}]`).type(`01012022`)

    cy.get('mat-dialog-container vs-button[type=save] button').should('be.enabled')

    cy.get(`vs-datepicker[ng-reflect-control-name=${ConclusoesNaoConformidadesFormControl.dataReuniao}] input`).should('be.disabled')

    cy.getVsCheckbox(ConclusoesNaoConformidadesFormControl.novaReuniao).click({ force: true })
    cy.get(`vs-datepicker[ng-reflect-control-name=${ConclusoesNaoConformidadesFormControl.dataReuniao}] input`).should('be.enabled')
    cy.get('mat-dialog-container vs-button[type=save] button').should('be.disabled');
    cy.get(`vs-datepicker[ng-reflect-control-name=${ConclusoesNaoConformidadesFormControl.dataReuniao}]`).type(`01012022`)
    cy.get('mat-dialog-container vs-button[type=save] button').should('be.enabled')

  })

  it('validar body do post', () => {
    const postRequest = concluirNaoConformidadeRequest()
    postRequest.expectedBody.eficaz = false
    delete postRequest.expectedBody.evidencia;
    cy.batchRequestStub(postRequest).then(alias => {
      cy.get('mat-dialog-container vs-button[type="save"] button').click();

      cy.wait(alias).then((interception:Interception) => {
        cy.validateRequestStatusCode(interception.response?.statusCode,200)
        postRequest.expectedBody.id = interception.request.body.id
        interception.request.body.idAuditor = ids[0];
        interception.request.body.dataVerificacao = new Date(interception.request.body.dataVerificacao)
        interception.request.body.dataReuniao = new Date(interception.request.body.dataReuniao)
        cy.validateRequestBody(interception.request.body, postRequest.expectedBody)
      })
    })
  })
})
