import { createNewSolucaoRequest, getAllSolucoesRequest, getSolucaoRequestForId, getSolucoesViewListRequest } from 'cypress/support/requests/solucoes/solucoes-requests';
import { strings } from 'cypress/support/test-utils';
import { Interception } from 'cypress/types/net-stubbing';
import { SolucaoFormControls } from '../../../../apps/rnc/src/app/pages/settings/solucao/solucao-form-controls'

describe("Criar solucao com sucesso", () => {
  VisitarTelaPrincipal()
  ClicarBotaoNovo()
  PreencherFormulario()
  Salvar()
  VisitarSolucao()
})

function VisitarTelaPrincipal() {
  it("Visitar tela de solucao", () => {
    cy.batchRequestStub(getSolucoesViewListRequest()).then(alias => {
      cy.visit('rnc/configuracoes/solucoes');
      cy.wait(alias);
    })
  })
}

function ClicarBotaoNovo() {
  it("Clicar botão novo", () => {
    cy.get('vs-header vs-button[icon=plus] button').click()
  })
}

function PreencherFormulario() {
  it("Preencher formulário", () => {
    cy.get("rnc-solucao-editor vs-button[type='save'] button").should('be.disabled')
    cy.getVsInput(SolucaoFormControls.descricao).type(strings[2])
    cy.get('vs-textarea[formcontrolname="detalhamento"]').clear().type(strings[2]);
    cy.get('vs-checkbox[formcontrolname="imediata"] input[type="checkbox"]').click({ force: true });
    cy.get("rnc-solucao-editor vs-button[type='save'] button").should('be.enabled')
  })
}

function Salvar() {
  it("Salvar nova solucao", () => {
    const postRequest = createNewSolucaoRequest()

    cy.intercept(
      postRequest.method,
      postRequest.url,
      postRequest.response)
      .as('postSolucoesRequest')

    cy.get("rnc-solucao-editor vs-button[type='save'] button").click()

    cy.wait('@postSolucoesRequest').then((interception: Interception) => {
      cy.validateRequestStatusCode(interception.response?.statusCode, 200)

      postRequest.expectedBody.id = interception.request.body.id

      cy.validateRequestBody(interception.request.body, postRequest.expectedBody)
    })

  })
}
function VisitarSolucao() {
  it("Visitar solucao", () => {
    cy.batchRequestStub(getSolucaoRequestForId()).then(alias => {
      cy.visit(`rnc/configuracoes/solucoes/**`);
      cy.wait(alias);
    })
  })
}
