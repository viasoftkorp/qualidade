import { SolucaoOutput } from './../../../../apps/rnc/src/api-clients/Solucoes/model/solucao-output';
import { getAllSolucoesRequest, getSolucoesViewListRequest } from './../../../support/requests/solucoes/solucoes-requests';
import { IPagedResultOutputDto } from '@viasoft/common';
import { deleteSolucaoRequest } from 'cypress/support/requests/solucoes/solucoes-requests';


describe("Deletar solucao com sucesso",() => {
  VisitarTelaPrincipal()
  ClicarBotaoDelete()
  DeletarComSucesso()
})

function VisitarTelaPrincipal(){
  it("Visitar tela de solucao",() => {
    cy.batchRequestStub(getSolucoesViewListRequest()).then(alias => {
      cy.visit('rnc/configuracoes/solucoes');
      cy.wait(alias);
    })
  })
}

function ClicarBotaoDelete(){
  it("Clicar no botão de delete",() => {
    cy.get('rnc-solucao vs-grid vs-button[ng-reflect-icon="trash-alt"] button').eq(0).click()
  })
}

function DeletarComSucesso(){
  it("Deletar com sucesso",()=> {
    const deleteRequest = deleteSolucaoRequest()

    const refreshGridRequest = getSolucoesViewListRequest()
    const body = refreshGridRequest.response.body as IPagedResultOutputDto<SolucaoOutput>

    if(body.items){
      body.items = [body.items[1]]
    }
    refreshGridRequest.response.body = body

    cy.intercept(
      deleteRequest.method,
      deleteRequest.url,
      deleteRequest.response)
      .as('deleteRequest')
    cy.intercept(
      refreshGridRequest.method,
      refreshGridRequest.url,
      refreshGridRequest.response)
      .as('refreshGridRequest')

    cy.get('vs-message-dialog vs-button[type="save"]').click()
    cy.wait('@deleteRequest')
    cy.wait('@refreshGridRequest')
  })
}
