import { DefeitoModel } from './../../../../apps/rnc/src/api-clients/Defeitos/model/defeito-model';
import { getAllDefeitosRequest, getDefeitosViewListRequest } from './../../../support/requests/defeitos/defeitos-requests';
import { IPagedResultOutputDto } from '@viasoft/common';
import { deleteDefeitoRequest } from 'cypress/support/requests/defeitos/defeitos-requests';



describe("Deletar defeito com sucesso",() => {
  VisitarTelaPrincipal()
  ClicarBotaoDelete()
  DeletarDefeitoComSucesso()
})

function VisitarTelaPrincipal(){
  it("Visitar tela principal",() => {
    cy.batchRequestStub(getDefeitosViewListRequest()).then(alias => {
      cy.visit('rnc/configuracoes/defeitos')
      cy.wait(alias)
    })
  })
}

function ClicarBotaoDelete(){
  it("Clicar no botão de delete",() => {
    cy.get('rnc-defeito vs-grid vs-button[ng-reflect-icon="trash-alt"] button').eq(0).click()
  })
}

function DeletarDefeitoComSucesso(){
  it("Deletar defeito com sucesso",() => {
    const deleteRequest = deleteDefeitoRequest()
    const refreshGridRequest = getDefeitosViewListRequest()

    const body = refreshGridRequest.response.body as IPagedResultOutputDto<DefeitoModel>

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
