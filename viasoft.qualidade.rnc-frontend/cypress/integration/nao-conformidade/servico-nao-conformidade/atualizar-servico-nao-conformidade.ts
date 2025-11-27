import { ServicosNaoConformidadesFormControl } from '@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/servicos-nao-conformidades/servicos-nao-conformidades-editor-modal/servicos-nao-conformidades-form-control';
import {
  getAllServicosNaoConformidadeRequest, updateServicosNaoConformidadeRequest,
} from 'cypress/support/requests/nao-conformidade/servico-nao-conformidades/servico-nao-conformidade.requests';
import { getRecursoById, getRecursosList } from 'cypress/support/requests/solucoes/solucoes-requests';
import { strings, codes } from 'cypress/support/test-utils';
import { navigateToNaoConformidadeEditor } from 'cypress/support/user-actions/nao-conformidades-user-actions';
import { Interception } from 'cypress/types/net-stubbing';
import { ServicoValidationResult } from '../../../../apps/rnc/src/api-clients/Nao-Conformidades/Servicos-Solucoes-Nao-Conformidades/model/serviceValidationResult'
import { NavigateToUpdateNaoConformidadeUserActionMock } from 'cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock';
describe.skip('Atualizar serviço com sucesso', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();

    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Clicar em Serviços', () => {
    cy.batchRequestStub(getAllServicosNaoConformidadeRequest()).then(
      (aliases) => {
        cy.getVsTabGroupItem('Serviços').click()
        cy.wait(aliases);
      }
    );
  });
  it('Selecionar um serviço', () => {
    cy.batchRequestStub([getRecursoById()]).then((aliasesToWait) => {
      cy.get('rnc-servicos-nao-conformidades vs-grid tbody tr').eq(0).click();
      cy.wait(aliasesToWait);
    });
  });

  it('Atualizar campos', () => {
    cy.get('rnc-servicos-nao-conformidades-editor-modal vs-button[type=save] button').should('be.disabled');

    cy.getVsTextArea(ServicosNaoConformidadesFormControl.operacaoEngenharia).clear().type(strings[3]);

    cy.getAutoCompleteItem({
      formControlName: ServicosNaoConformidadesFormControl.idRecurso,
      request: getRecursosList(),
      selector: 'qa-recurso-autocomplete-select',
      eq: 1,
    });


    cy.getVsInput(`${ServicosNaoConformidadesFormControl.horas}`).clear().type('4');
    cy.getVsInput(`${ServicosNaoConformidadesFormControl.minutos}`).clear().type('4');

    cy.getVsTextArea(ServicosNaoConformidadesFormControl.detalhamento).clear().type(strings[3]);

    cy.get('rnc-servicos-nao-conformidades-editor-modal vs-button[type=save] button').should('be.enabled');
  });

  it('Se horas e minutos forem igual a 0, botão salvar deve estar desabilitado', () => {
    cy.getVsInput(ServicosNaoConformidadesFormControl.horas).clear().type('0');
    cy.getVsInput(ServicosNaoConformidadesFormControl.minutos).clear().type('0');
    cy.get("rnc-servicos-nao-conformidades-editor-modal vs-button[type='save'] button").should('be.disabled');
    cy.getVsInput(ServicosNaoConformidadesFormControl.horas).clear().type('4');
    cy.get("rnc-servicos-nao-conformidades-editor-modal vs-button[type='save'] button").should('be.enabled');
    cy.getVsInput(ServicosNaoConformidadesFormControl.minutos).clear().type('4');
  })

  it('Salvar e validar', () => {
    const putRequest = updateServicosNaoConformidadeRequest();
    cy.intercept(putRequest.method, putRequest.url, putRequest.response).as('putServicoRequest');

    const refreshGridRequest = getAllServicosNaoConformidadeRequest();
    refreshGridRequest.response.body.items.push({
      codigo: strings[2],
      descricao: strings[2],
      unidadeMedida: strings[2],
      detalhamento: strings[3],
      quantidade: codes[3],
    });
    cy.intercept(refreshGridRequest.method, refreshGridRequest.url, refreshGridRequest.response).as('getServicoRequest');

    cy.get('rnc-servicos-nao-conformidades-editor-modal vs-button[type="save"] button').click();
    cy.wait('@putServicoRequest').then((interception: Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode, 200);
      putRequest.expectedBody.id = interception.request.body.id;
      interception.request.body.quantidade = Number.parseFloat(interception.request.body.quantidade);
      interception.request.body.horas = Number.parseFloat(interception.request.body.horas);
      interception.request.body.minutos = Number.parseFloat(interception.request.body.minutos);
      cy.validateRequestBody(interception.request.body, putRequest.expectedBody);
    });
    cy.wait('@getServicoRequest');
  });
});

describe.skip('Se operação engenharia já em uso deve apresentar mensagem', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();

    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Clicar em Serviços', () => {
    cy.batchRequestStub(getAllServicosNaoConformidadeRequest()).then(
      (aliases) => {
        cy.getVsTabGroupItem('Serviços').click()
        cy.wait(aliases);
      }
    );
  });
  it('Selecionar um serviço', () => {
    cy.batchRequestStub([getRecursoById()]).then((aliasesToWait) => {
      cy.get('rnc-servicos-nao-conformidades vs-grid tbody tr').eq(0).click();
      cy.wait(aliasesToWait);
    });
  });

  it('Atualizar campos', () => {
    cy.get('rnc-servicos-nao-conformidades-editor-modal vs-button[type=save] button').should('be.disabled');

    cy.getVsTextArea(ServicosNaoConformidadesFormControl.operacaoEngenharia).clear().type(strings[3]);

    cy.getAutoCompleteItem({
      formControlName: ServicosNaoConformidadesFormControl.idRecurso,
      request: getRecursosList(),
      selector: 'qa-recurso-autocomplete-select',
      eq: 1,
    });


    cy.getVsInput(`${ServicosNaoConformidadesFormControl.horas}`).clear().type('4');
    cy.getVsInput(`${ServicosNaoConformidadesFormControl.minutos}`).clear().type('4');

    cy.getVsTextArea(ServicosNaoConformidadesFormControl.detalhamento).clear().type(strings[3]);

    cy.get('rnc-servicos-nao-conformidades-editor-modal vs-button[type=save] button').should('be.enabled');
  });

  it('Salvar e validar', () => {
    const putRequest = updateServicosNaoConformidadeRequest();
    putRequest.response.statusCode = 422
    putRequest.response.body = ServicoValidationResult.OperacaoEngenhariaJaUtilizada;
    cy.intercept(putRequest.method, putRequest.url, putRequest.response).as('putServicoRequest');

    const refreshGridRequest = getAllServicosNaoConformidadeRequest();
    refreshGridRequest.response.body.items.push({
      codigo: strings[2],
      descricao: strings[2],
      unidadeMedida: strings[2],
      detalhamento: strings[3],
      quantidade: codes[3],
    });

    cy.get('rnc-servicos-nao-conformidades-editor-modal vs-button[type="save"] button').click();
    cy.wait('@putServicoRequest').then((interception: Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode, 422);
      putRequest.expectedBody.id = interception.request.body.id;
      interception.request.body.quantidade = Number.parseFloat(interception.request.body.quantidade);
      interception.request.body.horas = Number.parseFloat(interception.request.body.horas);
      interception.request.body.minutos = Number.parseFloat(interception.request.body.minutos);
      cy.validateRequestBody(interception.request.body, putRequest.expectedBody);
    });
  });
  it('Verificar mensagem de erro', () => {
    cy.get('vs-message-dialog').contains('Operação engenharia já utilizada por outro serviço').should('exist')
  })
});
