import { getProdutosList,
  getSolucaoRequestForId,
  getAllServicosSolucaoView,
  getRecursosList,
  createServicoSolucaoRequest
} from '../../../../support/requests/solucoes/solucoes-requests';
import { codes, ids, strings } from 'cypress/support/test-utils';
import { Interception } from 'cypress/types/net-stubbing';
import { SolucaoServicoFormControls } from '@viasoft/rnc/app/pages/settings/solucao/solucao-servico/solucao-servico-form-controls';
import { ServicoValidationResult } from '../../../../../apps/rnc/src/api-clients/Nao-Conformidades/Servicos-Solucoes-Nao-Conformidades/model/serviceValidationResult'

const mainSelector = 'rnc-solucao-servico-editor-modal'

describe("Criar solucao com sucesso", () => {
  it("Visitar tela de servico solucao", () => {
    cy.batchRequestStub([getSolucaoRequestForId(),getAllServicosSolucaoView()]).then(alias => {
      cy.visit(`rnc/configuracoes/solucoes/${ids[0]}`);
      cy.get(`mat-tab-group mat-tab-header div[id=mat-tab-label-0-1]`) .click()
      cy.wait(alias);
    })
  })
  it("Clicar botão novo", () => {
    cy.batchRequestStub(getRecursosList()).then(aliases => {
      cy.get('rnc-solucao-servico vs-button[icon=plus] button').click()

      cy.wait(aliases)
    })
  })
  it("Preencher formulário", () => {
    cy.get("rnc-solucao-servico-editor-modal vs-button[type='save'] button").should('be.disabled')

    cy.get('div.vs-autocomplete-panel vs-button button').eq(0).click()
    cy.getVsInput(SolucaoServicoFormControls.horas).clear().type('3');
    cy.getVsInput(SolucaoServicoFormControls.minutos).clear().type('3');
    cy.get(`vs-textarea[formcontrolname=${SolucaoServicoFormControls.operacaoEngenharia}]`).clear().type(strings[2]);

    cy.get("rnc-solucao-servico-editor-modal vs-button[type='save'] button").should('be.enabled');
  })

  it('Se horas e minutos forem igual a 0, botão salvar deve estar desabilitado', () => {
    cy.getVsInput(SolucaoServicoFormControls.horas).clear().type('0');
    cy.getVsInput(SolucaoServicoFormControls.minutos).clear().type('0');
    cy.get("rnc-solucao-servico-editor-modal vs-button[type='save'] button").should('be.disabled');
    cy.getVsInput(SolucaoServicoFormControls.horas).clear().type('3');
    cy.get("rnc-solucao-servico-editor-modal vs-button[type='save'] button").should('be.enabled');
    cy.getVsInput(SolucaoServicoFormControls.minutos).clear().type('3');
  })

  it("Salvar novo servico na solucao", () => {
    const postRequest = createServicoSolucaoRequest()

    cy.intercept(
      postRequest.method,
      postRequest.url,
      postRequest.response)
      .as('postSolucoesRequest')

      const refreshGridRequest = getAllServicosSolucaoView()

      refreshGridRequest.response.body.items.push({
        codigo:'2',
        descricao:strings[3],
        quantidade: codes[2],
        horas:codes[2],
        minutos:codes[2],
        codigoRecurso:'3',
        descricaoRecurso:strings[3],
        operacaoEngenharia:strings[2]
      })

      cy.intercept(
        refreshGridRequest.method,
        refreshGridRequest.url,
        refreshGridRequest.response)
        .as('getSolucaoRequest')

    cy.get("rnc-solucao-servico-editor-modal vs-button[type='save'] button").click()

    cy.wait('@postSolucoesRequest').then((interception: Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode, 200)

      postRequest.expectedBody.idServicoSolucao = interception.request.body.idServicoSolucao
      postRequest.expectedBody.idSolucao = interception.request.body.idSolucao
      interception.request.body.quantidade = Number.parseInt(interception.request.body.quantidade)
      interception.request.body.horas = Number.parseInt(interception.request.body.horas)
      interception.request.body.minutos = Number.parseInt(interception.request.body.minutos)

      cy.validateRequestBody(interception.request.body, postRequest.expectedBody)
    })

  })
})
describe("Se operação engenharia já em uso deve apresentar mensagem", () => {
  it("Visitar tela de servico solucao", () => {
    cy.batchRequestStub([getSolucaoRequestForId(),getAllServicosSolucaoView()]).then(alias => {
      cy.visit(`rnc/configuracoes/solucoes/${ids[0]}`);
      cy.get(`mat-tab-group mat-tab-header div[id=mat-tab-label-0-1]`) .click()
      cy.wait(alias);
    })
  })
  it("Clicar botão novo", () => {
    cy.batchRequestStub(getRecursosList()).then(aliases => {
      cy.get('rnc-solucao-servico vs-button[icon=plus] button').click()

      cy.wait(aliases)
    })

  })
  it("Preencher formulário", () => {
    cy.get("rnc-solucao-servico-editor-modal vs-button[type='save'] button").should('be.disabled')

    cy.get('div.vs-autocomplete-panel vs-button button').eq(0).click()

    cy.getVsInput(SolucaoServicoFormControls.horas).clear().type('3');
    cy.getVsInput(SolucaoServicoFormControls.minutos).clear().type('3');
    cy.get(`vs-textarea[formcontrolname=${SolucaoServicoFormControls.operacaoEngenharia}]`).clear().type(strings[2]);

    cy.get("rnc-solucao-servico-editor-modal vs-button[type='save'] button").should('be.enabled');
  })
  it("Salvar novo servico na solucao", () => {
    const postRequest = createServicoSolucaoRequest()
    postRequest.response.statusCode = 400
    postRequest.response.body = ServicoValidationResult.OperacaoEngenhariaJaUtilizada;
    cy.intercept(
      postRequest.method,
      postRequest.url,
      postRequest.response)
      .as('postSolucoesRequest')

      const refreshGridRequest = getAllServicosSolucaoView()

      refreshGridRequest.response.body.items.push({
        codigo:'2',
        descricao:strings[3],
        quantidade: codes[2],
        horas:codes[2],
        minutos:codes[2],
        codigoRecurso:'3',
        descricaoRecurso:strings[3],
        operacaoEngenharia:strings[2]
      })

      cy.intercept(
        refreshGridRequest.method,
        refreshGridRequest.url,
        refreshGridRequest.response)
        .as('getSolucaoRequest')

    cy.get("rnc-solucao-servico-editor-modal vs-button[type='save'] button").click()

    cy.wait('@postSolucoesRequest').then((interception: Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode, 400)

      postRequest.expectedBody.idServicoSolucao = interception.request.body.idServicoSolucao
      postRequest.expectedBody.idSolucao = interception.request.body.idSolucao
      interception.request.body.quantidade = Number.parseInt(interception.request.body.quantidade)
      interception.request.body.horas = Number.parseInt(interception.request.body.horas)
      interception.request.body.minutos = Number.parseInt(interception.request.body.minutos)

      cy.validateRequestBody(interception.request.body, postRequest.expectedBody)
    })
  })
  it('Verificar mensagem de erro', () => {
    cy.get('vs-message-dialog').contains('Operação engenharia já utilizada por outro serviço desta solução').should('exist')
  })
})
