import { SelectDefeitosGridMock } from "cypress/support/mock/select-defeitos-grid-mock";
import { selectDefeitosGridElement } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { IPagedResultOutputDto } from "@viasoft/common";
import { ImplementacaoEvitarReincidenciaNaoConformidadesModel } from "@viasoft/rnc/api-clients/Nao-Conformidades/Implementacao-Evitar-Reincidencia-Nao-Conformidades/model/implementacao-evitar-reincidencia-nao-conformidades-model";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { deleteImplementacoesEvitarReincidenciaNaoConformidadeRequest, getAllImplementacoesEvitarReincidenciaNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/implementacao-evitar-reincidencia-nao-conformidades/implementacao-evitar-reincidencia-nao-conformidades";

const mainSelector = 'rnc-implementacao-evitar-reincidencia-nao-conformidades'
const editorModalSelector = 'rnc-implementacao-evitar-reincidencia-nao-conformidades-editor-modal'

describe('Se formulário não desabilitado ao clicar para deletar, deve apagar implementação', () => {
  it('Navegar para defeitos no editor de Nao Conformidade', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    const selectDefeitosGridRequests = new SelectDefeitosGridMock();
    selectDefeitosGridElement( naoConformidadeRequests, selectDefeitosGridRequests);
  });
  it('Clicar Em Implem. Eficácia Evitar Reinc.', () => {
    cy.batchRequestStub(getAllImplementacoesEvitarReincidenciaNaoConformidadeRequest()).then(alias => {
      cy.get('mat-tab-group mat-tab-header div').contains('Implem. Eficácia Evitar Reinc.').click()

      cy.wait(alias)
    })
  })
  it("Deletar produto não conformidade com sucesso",() => {
    const deleteRequest = deleteImplementacoesEvitarReincidenciaNaoConformidadeRequest()
    const refreshGridRequest = getAllImplementacoesEvitarReincidenciaNaoConformidadeRequest()

    const body = refreshGridRequest.response.body as IPagedResultOutputDto<ImplementacaoEvitarReincidenciaNaoConformidadesModel>
    if(body.items){
      body.items = [body.items[1]]
    }
    refreshGridRequest.response.body = body

    cy.batchRequestStub([deleteRequest, refreshGridRequest])
    .then(aliasesToWait => {
      cy.get(`${mainSelector} vs-button[ng-reflect-icon=trash-alt] button`).eq(0).click()
      cy.get(`vs-message-dialog vs-button[type="save"]`).click()
    })
  })
})
