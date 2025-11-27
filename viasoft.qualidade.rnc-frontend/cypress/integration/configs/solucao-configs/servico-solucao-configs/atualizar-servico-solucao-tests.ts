import { getAllServicosSolucaoView, getRecursoById, getRecursosList, updateServicoSolucaoRequest } from '../../../../support/requests/solucoes/solucoes-requests';
import { SolucaoServicoFormControls } from '../../../../../apps/rnc/src/app/pages/settings/solucao/solucao-servico/solucao-servico-form-controls';
import {  getProdutosList, getSolucaoRequestForId } from '../../../../support/requests/solucoes/solucoes-requests';
import { codes, ids, strings } from 'cypress/support/test-utils';
import { Interception } from 'cypress/types/net-stubbing';
import { getProductByIdRequest } from 'cypress/support/requests/logistics-products/products-requests';
import { ServicoValidationResult } from '../../../../../apps/rnc/src/api-clients/Nao-Conformidades/Servicos-Solucoes-Nao-Conformidades/model/serviceValidationResult'
import { RecursoOutput } from '@viasoft/rnc/app/pages/shared/recurso-autocomplete-select/recurso-autocomplete-select.component';
describe.skip("Atualizar produto solucao com sucesso", () => {
  it("Visitar tela de produto solucao", () => {
    cy.batchRequestStub([getSolucaoRequestForId(),getAllServicosSolucaoView()]).then((alias: any) => {
      cy.visit(`rnc/configuracoes/solucoes/${ids[0]}`);
      cy.get(`mat-tab-group mat-tab-header div[id=mat-tab-label-0-1]`) .click()
      cy.wait(alias);
    })
  })
  it("Clicar na primeira da lista", () => {
    cy.batchRequestStub([getRecursoById()]).then(aliases => {
      cy.get('rnc-solucao-servico vs-grid tbody tr').eq(0).click()

      cy.wait(aliases)
    })
  })
  it("Preencher formulário", () => {
    cy.get("rnc-solucao-servico-editor-modal vs-button[type='save'] button").should('be.disabled')

    cy.get(`vs-textarea[formcontrolname=${SolucaoServicoFormControls.operacaoEngenharia}]`).clear().type(strings[3]);

    cy.getVsInput(SolucaoServicoFormControls.horas).clear().type('4');

    cy.getVsInput(SolucaoServicoFormControls.minutos).clear().type('4');
    cy.getAutoCompleteItem({
      formControlName:SolucaoServicoFormControls.idRecurso,
      request:getRecursosList(),
      selector:'qa-recurso-autocomplete-select',
      eq:1
    });

    cy.get("rnc-solucao-servico-editor-modal vs-button[type='save'] button").should('be.enabled');
  })

  it('Se horas e minutos forem igual a 0, botão salvar deve estar desabilitado', () => {
    cy.getVsInput(SolucaoServicoFormControls.horas).clear().type('0');
    cy.getVsInput(SolucaoServicoFormControls.minutos).clear().type('0');
    cy.get("rnc-solucao-servico-editor-modal vs-button[type='save'] button").should('be.disabled');
    cy.getVsInput(SolucaoServicoFormControls.horas).clear().type('4');
    cy.get("rnc-solucao-servico-editor-modal vs-button[type='save'] button").should('be.enabled');
    cy.getVsInput(SolucaoServicoFormControls.minutos).clear().type('4');
  })

  it("Salvar novo servico na solucao", () => {

    const putRequest = updateServicoSolucaoRequest(ids[0])

    cy.intercept(
      putRequest.method,
      putRequest.url,
      putRequest.response)
      .as('putSolucoesRequest')

    const refreshGridRequest = getAllServicosSolucaoView(ids[0])

    cy.intercept(
      refreshGridRequest.method,
      refreshGridRequest.url,
      refreshGridRequest.response)
      .as('getSolucaoRequest')

    cy.get("rnc-solucao-servico-editor-modal vs-button[type='save'] button").click()

    cy.wait('@putSolucoesRequest').then((interception: Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode, 200)
      putRequest.expectedBody.idServicoSolucao = interception.request.body.idServicoSolucao
      interception.request.body.quantidade = Number.parseInt(interception.request.body.quantidade)
      interception.request.body.horas = Number.parseInt(interception.request.body.horas)
      interception.request.body.minutos = Number.parseInt(interception.request.body.minutos)
      putRequest.expectedBody.idSolucao = interception.request.body.idSolucao

      cy.validateRequestBody(interception.request.body, putRequest.expectedBody)
    })

    cy.wait('@getSolucaoRequest')

  })
})

describe.skip("Se operação engenharia já em uso deve apresentar mensagem", () => {
  it("Visitar tela de produto solucao", () => {
    cy.batchRequestStub([getSolucaoRequestForId(),getAllServicosSolucaoView()]).then((alias: any) => {
      cy.visit(`rnc/configuracoes/solucoes/${ids[0]}`);
      cy.get(`mat-tab-group mat-tab-header div[id=mat-tab-label-0-1]`) .click()
      cy.wait(alias);
    })
  })
  it("Clicar na primeira da lista", () => {
    const getRecursosListRequest = getRecursosList();
    getRecursosListRequest.response.body.items = [
      {
        id: ids[0],
        descricao: strings[0],
        codigo: codes[0].toString(),
      } as RecursoOutput,
      {
        id: ids[1],
        descricao: strings[1],
        codigo: codes[1].toString(),
      } as RecursoOutput,
    ]
    cy.batchRequestStub([getRecursoById(), getRecursosListRequest]).then(aliases => {
      cy.get('rnc-solucao-servico vs-grid tbody tr').eq(0).click()

      cy.wait(aliases)
    })
  })
  it("Preencher formulário", () => {
    cy.get("rnc-solucao-servico-editor-modal vs-button[type='save'] button").should('be.disabled')

    cy.get(`vs-textarea[formcontrolname=${SolucaoServicoFormControls.operacaoEngenharia}]`).clear().type(strings[3]);

    cy.getVsInput(SolucaoServicoFormControls.horas).clear().type('4');

    cy.getVsInput(SolucaoServicoFormControls.minutos).clear().type('4');
    cy.getAutoCompleteItem({
      formControlName:SolucaoServicoFormControls.idRecurso,
      request:getRecursosList(),
      selector:'qa-recurso-autocomplete-select',
      eq:1
    });

    cy.get("rnc-solucao-servico-editor-modal vs-button[type='save'] button").should('be.enabled');
  })
  it("Salvar novo servico na solucao", () => {

    const putRequest = updateServicoSolucaoRequest(ids[0])
    putRequest.response.statusCode = 422
    putRequest.response.body = ServicoValidationResult.OperacaoEngenhariaJaUtilizada;
    cy.intercept(
      putRequest.method,
      putRequest.url,
      putRequest.response)
      .as('putSolucoesRequest')

    cy.get("rnc-solucao-servico-editor-modal vs-button[type='save'] button").click()

    cy.wait('@putSolucoesRequest').then((interception: Interception) => {
      putRequest.expectedBody.idServicoSolucao = interception.request.body.idServicoSolucao
      interception.request.body.quantidade = Number.parseInt(interception.request.body.quantidade)
      interception.request.body.horas = Number.parseInt(interception.request.body.horas)
      interception.request.body.minutos = Number.parseInt(interception.request.body.minutos)
      putRequest.expectedBody.idSolucao = interception.request.body.idSolucao

      cy.validateRequestBody(interception.request.body, putRequest.expectedBody)
    })

  })
  it('Verificar mensagem de erro', () => {
    cy.get('vs-message-dialog').contains('Operação engenharia já utilizada por outro serviço desta solução').should('exist')
  })
})
