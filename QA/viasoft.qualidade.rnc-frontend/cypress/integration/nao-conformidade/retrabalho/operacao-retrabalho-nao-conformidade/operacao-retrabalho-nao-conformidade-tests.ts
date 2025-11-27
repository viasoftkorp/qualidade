import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { getAllOperacoesView } from "cypress/support/requests/nao-conformidade/operacao-retrabalho-nao-conformidades/operacao-retrabalho-nao-conformidades-requests";
import { navigateToNaoConformidadeEditor } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { OperacaoRetrabalhoNaoConformidadeFormControls } from 'apps/rnc/src/app/pages/nao-conformidades/nao-conformidades-editor/retrabalho-nao-conformidades/operacao-retrabalho-nao-conformidades/operacao-retrabalho-nao-conformidade-form-controls'
import { codes, ids } from "cypress/support/test-utils";
const mainSelector = 'rnc-nao-conformidades-editor'
const operacaoRetrabalhoTabSelector = `${mainSelector} rnc-operacao-retrabalho-nao-conformidades vs-form form`
describe('Se houver operação retrabalho cadastrada, deve apresentar a tab "Operação Retrabalho"', () => {
  it('Navegar para edição de não conformidade', () => {
    const navigateToUpdateNaoConformidadeUserActionMock = new NavigateToUpdateNaoConformidadeUserActionMock()
    navigateToUpdateNaoConformidadeUserActionMock.atualizarNaoConformidadeRequestsRestantesMock.getOperacaoRetrabalhoRequest.response.body = {
      id:ids[0],
      idNaoConformidade: ids[0],
      numeroOperacaoARetrabalhar: codes[0].toString(),
      quantidade: 10,
      message: "",
      operacoes: [],
      success: true,
    }
    delete navigateToUpdateNaoConformidadeUserActionMock.atualizarNaoConformidadeRequestsRestantesMock.getListOrdemProducao;
    navigateToNaoConformidadeEditor(navigateToUpdateNaoConformidadeUserActionMock)
  })
  it('Acessar tab "Ordem Retrabalho"', () => {
    cy.batchRequestStub(getAllOperacoesView()).then(aliases => {
      cy.getVsTabGroupItem('Operação Retrabalho', mainSelector).click();
      cy.wait(aliases)
    })
  })
  it('Verificar campos', () => {
    cy.getVsInput(OperacaoRetrabalhoNaoConformidadeFormControls.numeroOperacaoARetrabalhar, operacaoRetrabalhoTabSelector)
      .should('have.value', codes[0])

    cy.getVsInput(OperacaoRetrabalhoNaoConformidadeFormControls.quantidade, operacaoRetrabalhoTabSelector)
    .should('have.value', 10)

    cy.get(`${operacaoRetrabalhoTabSelector} vs-grid tbody tr`).should('have.length', 2)
  })
})

describe('Se não houver operação retrabalho cadastrada, não deve apresentar a tab "Operação Retrabalho"', () => {
  it('Navegar para edição de não conformidade', () => {
    const navigateToUpdateNaoConformidadeUserActionMock = new NavigateToUpdateNaoConformidadeUserActionMock()
    navigateToUpdateNaoConformidadeUserActionMock.atualizarNaoConformidadeRequestsRestantesMock.getOperacaoRetrabalhoRequest.response.body = null;
    navigateToNaoConformidadeEditor(navigateToUpdateNaoConformidadeUserActionMock)
  })
  it('Verificar se tab não existe', () => {
    cy.getVsTabGroupItem('Operação Retrabalho', mainSelector).should('not.exist');
  })
})

