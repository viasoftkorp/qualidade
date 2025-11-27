import { ServicoValidationResult } from '../../../../apps/rnc/src/api-clients/Nao-Conformidades/Servicos-Solucoes-Nao-Conformidades/model/serviceValidationResult'
import { ServicosNaoConformidadesFormControl } from '@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/servicos-nao-conformidades/servicos-nao-conformidades-editor-modal/servicos-nao-conformidades-form-control';
import { NavigateToUpdateNaoConformidadeUserActionMock } from 'cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock';
import {
  createNewServicosNaoConformidadeRequest,
  getAllServicosNaoConformidadeRequest
} from 'cypress/support/requests/nao-conformidade/servico-nao-conformidades/servico-nao-conformidade.requests';
import { getRecursosList } from 'cypress/support/requests/solucoes/solucoes-requests';
import { strings, codes } from 'cypress/support/test-utils';
import { navigateToNaoConformidadeEditor } from 'cypress/support/user-actions/nao-conformidades-user-actions';
import { Interception } from 'cypress/types/net-stubbing';
const mainSelector = 'rnc-servicos-nao-conformidades-editor-modal'
describe('Criar servico com sucesso', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();

    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Clicar em Serviços', () => {
    cy.batchRequestStub(getAllServicosNaoConformidadeRequest()).then((aliases) => {
      cy.getVsTabGroupItem('Serviços').click()
      cy.wait(aliases);
    });
  });
  it('Clicar botão para adicionar serviço', () => {
    cy.batchRequestStub(getRecursosList()).then((alias) => {
      cy.get('rnc-servicos-nao-conformidades vs-button[icon=plus] button').click();
      cy.wait(alias);
    });
  });
  it('Preencher campos', () => {
    cy.get('rnc-servicos-nao-conformidades-editor-modal vs-button[type=save] button').should('be.disabled');

    cy.get('div.vs-autocomplete-panel vs-button button').eq(0).click();

    cy.getVsTextArea(ServicosNaoConformidadesFormControl.operacaoEngenharia).type(strings[2]);

    cy.getVsInput(`${ServicosNaoConformidadesFormControl.horas}`).type('3');
    cy.getVsInput(`${ServicosNaoConformidadesFormControl.minutos}`).type('3');

    cy.getVsTextArea(ServicosNaoConformidadesFormControl.detalhamento).type(strings[2]);


    cy.get('rnc-servicos-nao-conformidades-editor-modal vs-button[type=save] button').should('be.enabled');
  });

  it('Se horas e minutos forem igual a 0, botão salvar deve estar desabilitado', () => {
    cy.getVsInput(ServicosNaoConformidadesFormControl.horas).clear().type('0');
    cy.getVsInput(ServicosNaoConformidadesFormControl.minutos).clear().type('0');
    cy.get("rnc-servicos-nao-conformidades-editor-modal vs-button[type='save'] button").should('be.disabled');
    cy.getVsInput(ServicosNaoConformidadesFormControl.horas).clear().type('3');
    cy.get("rnc-servicos-nao-conformidades-editor-modal vs-button[type='save'] button").should('be.enabled');
    cy.getVsInput(ServicosNaoConformidadesFormControl.minutos).clear().type('3');
  })

  it('Salvar e validar', () => {
    const postRequest = createNewServicosNaoConformidadeRequest();
    cy.intercept(postRequest.method, postRequest.url, postRequest.response).as('postServicoNaoConformidadeRequest');

    const refreshGridRequest = getAllServicosNaoConformidadeRequest();
    refreshGridRequest.response.body.items.push({
      codigo: strings[2],
      descricao: strings[2],
      unidadeMedida: strings[2],
      detalhamento: strings[3],
      quantidade: codes[3],
    });
    cy.intercept(refreshGridRequest.method, refreshGridRequest.url, refreshGridRequest.response).as(
      'getProdutoSolucaoRequest'
    );

    cy.get('rnc-servicos-nao-conformidades-editor-modal vs-button[type="save"] button').click();
    cy.wait('@postServicoNaoConformidadeRequest').then((interception: Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode, 200);
      postRequest.expectedBody.id = interception.request.body.id;
      interception.request.body.quantidade = Number.parseFloat(interception.request.body.quantidade);
      interception.request.body.horas = Number.parseFloat(interception.request.body.horas);
      interception.request.body.minutos = Number.parseFloat(interception.request.body.minutos);
      cy.validateRequestBody(interception.request.body, postRequest.expectedBody);
    });
    cy.wait('@getProdutoSolucaoRequest');
  });
});

describe('Se operação engenharia já em uso deve apresentar mensagem', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();

    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Clicar em Serviços', () => {
    cy.batchRequestStub(getAllServicosNaoConformidadeRequest()).then((aliases) => {
      cy.getVsTabGroupItem('Serviços').click()
      cy.wait(aliases);
    });
  });
  it('Clicar botão para adicionar serviço', () => {
    cy.batchRequestStub(getRecursosList()).then((alias) => {
      cy.get('rnc-servicos-nao-conformidades vs-button[icon=plus] button').click();
      cy.wait(alias);
    });
  });
  it('Preencher campos', () => {
    cy.get('rnc-servicos-nao-conformidades-editor-modal vs-button[type=save] button').should('be.disabled');

    cy.get('div.vs-autocomplete-panel vs-button button').eq(0).click();

    cy.getVsTextArea(ServicosNaoConformidadesFormControl.operacaoEngenharia).type(strings[2]);

    cy.getVsInput(`${ServicosNaoConformidadesFormControl.horas}`).type('3');
    cy.getVsInput(`${ServicosNaoConformidadesFormControl.minutos}`).type('3');

    cy.getVsTextArea(ServicosNaoConformidadesFormControl.detalhamento).type(strings[2]);


    cy.get('rnc-servicos-nao-conformidades-editor-modal vs-button[type=save] button').should('be.enabled');
  });

  it('Salvar e validar', () => {
    const postRequest = createNewServicosNaoConformidadeRequest();
    postRequest.response.statusCode = 422
    postRequest.response.body = ServicoValidationResult.OperacaoEngenhariaJaUtilizada;
    cy.intercept(postRequest.method, postRequest.url, postRequest.response).as('postServicoNaoConformidadeRequest');

    const refreshGridRequest = getAllServicosNaoConformidadeRequest();
    refreshGridRequest.response.body.items.push({
      codigo: strings[2],
      descricao: strings[2],
      unidadeMedida: strings[2],
      detalhamento: strings[3],
      quantidade: codes[3],
    });
    cy.intercept(refreshGridRequest.method, refreshGridRequest.url, refreshGridRequest.response).as(
      'getProdutoSolucaoRequest'
    );

    cy.get('rnc-servicos-nao-conformidades-editor-modal vs-button[type="save"] button').click();
    cy.wait('@postServicoNaoConformidadeRequest').then((interception: Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode, 422);
      postRequest.expectedBody.id = interception.request.body.id;
      interception.request.body.quantidade = Number.parseFloat(interception.request.body.quantidade);
      interception.request.body.horas = Number.parseFloat(interception.request.body.horas);
      interception.request.body.minutos = Number.parseFloat(interception.request.body.minutos);
      cy.validateRequestBody(interception.request.body, postRequest.expectedBody);
    });
  });
  it('Verificar mensagem de erro', () => {
    cy.get('vs-message-dialog').contains('Operação engenharia já utilizada por outro serviço').should('exist')
  })
});
