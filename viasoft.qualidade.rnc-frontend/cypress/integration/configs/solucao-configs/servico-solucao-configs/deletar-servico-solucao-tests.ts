import { SolucaoProdutoOutput } from '../../../../../apps/rnc/src/api-clients/Solucoes/model/solucao-produto-output';

import { IPagedResultOutputDto } from '@viasoft/common';
import { deleteProdutoSolucaoRequest, getAllProdutosSolucaoView, getSolucaoRequestForId } from 'cypress/support/requests/solucoes/solucoes-requests';
import { ids } from 'cypress/support/test-utils';


describe("Deletar solucao com sucesso",() => {
  VisitarTelaPrincipal()
  ClicarBotaoDelete()
  DeletarComSucesso()
})

function VisitarTelaPrincipal(){
  it("Visitar tela de solucao",() => {
    cy.batchRequestStub(requestInicialListSolucao).then(alias => {
      cy.visit(`rnc/configuracoes/solucoes/${ids[0]}/produtos`);
      cy.wait(alias);
    })
  })
}

function ClicarBotaoDelete(){
  it("Clicar no botão de delete",() => {
    cy.get('rnc-solucao-produto vs-grid vs-button[ng-reflect-icon="trash-alt"] button').eq(0).click()
  })
}

function DeletarComSucesso(){
  it("Deletar com sucesso",()=> {
    const deleteRequest = deleteProdutoSolucaoRequest()

    const refreshGridRequest = getAllProdutosSolucaoView()
    const body = refreshGridRequest.response.body as IPagedResultOutputDto<SolucaoProdutoOutput>

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


const requestInicialListSolucao = [
  getSolucaoRequestForId(),
  getAllProdutosSolucaoView(ids[0])
]
