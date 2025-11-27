import { IPagedResultOutputDto } from '@viasoft/common';
import { NaturezaOutput } from '@viasoft/rnc/api-clients/Naturezas/model/natureza-output';
import { getAllNaturezasRequest, deleteNaturezaRequest, getNaturezasViewListRequest } from './../../../support/requests/naturezas/naturezas.request';

describe("Deletar natureza com sucesso",() => {
  VisitarTelaPrincipal()
  ClicarBotaoDelete()
  DeletarComSucesso()
})

function VisitarTelaPrincipal(){
  it("Visitar tela de natureza",() => {
    cy.batchRequestStub(getNaturezasViewListRequest()).then(alias => {
      cy.visit('rnc/configuracoes/naturezas');
      cy.wait(alias);
    })
  })
}

function ClicarBotaoDelete(){
  it("Clicar no botão de delete",() => {
    cy.get('rnc-natureza vs-grid vs-button[ng-reflect-icon="trash-alt"] button').eq(0).click()
  })
}

function DeletarComSucesso(){
  it("Deletar com sucesso",()=> {
    const deleteRequest = deleteNaturezaRequest()

    const refreshGridRequest = getNaturezasViewListRequest()
    const body = refreshGridRequest.response.body as IPagedResultOutputDto<NaturezaOutput>

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
