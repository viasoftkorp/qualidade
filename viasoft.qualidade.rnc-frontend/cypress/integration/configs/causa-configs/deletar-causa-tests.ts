import { IPagedResultOutputDto } from '@viasoft/common';
import { CausaModel } from '@viasoft/rnc/api-clients/Causas/model/Causa-model';
import { deleteCausaRequest, getAllCausasRequest, getCausasViewListRequest } from './../../../support/requests/causas/causas.request';

describe("Deletar causa com sucesso",() => {
  VisitarTelaPrincipal()
  ClicarBotaoDelete()
  DeletarCausaComSucesso()
})

function VisitarTelaPrincipal(){
  it("Visitar tela principal",() => {
    cy.batchRequestStub(getCausasViewListRequest()).then(alias => {
      cy.visit('rnc/configuracoes/causas')
      cy.wait(alias)
    })
  })
}

function ClicarBotaoDelete(){
  it("Clicar no botão de delete",() => {
    cy.get('rnc-causa vs-grid vs-button[ng-reflect-icon="trash-alt"] button').eq(0).click()
  })
}

function DeletarCausaComSucesso(){
  it("Deletar causa com sucesso",() => {
    const deleteRequest = deleteCausaRequest()
    const refreshGridRequest = getCausasViewListRequest()

    const body = refreshGridRequest.response.body as IPagedResultOutputDto<CausaModel>

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
