import { GerarOrdemRetrabalhoValidationResult } from "@viasoft/rnc/api-clients/Nao-Conformidades/Ordem-Retrabalho/model/gerar-ordem-retrabalho-validation-result"
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock"
import { canGenerateOrdemRetrabalhoRequest } from "cypress/support/requests/nao-conformidade/ordem-retrabalho-nao-conformidades/ordem-retrabalho-nao-conformidades"
import { navigateToNaoConformidadeEditor } from "cypress/support/user-actions/nao-conformidades-user-actions"

const mainSelector = 'rnc-nao-conformidades-editor'

describe('Se não houver operação de retrabalho gerada e não houver ordem de faturamento gerada, deve ' +
'buscar odf e se a mesma estiver finalizada deve apresentar o botão "Gerar Odf Retrabalho"',
() => {
  it('Navegar para edição de não conformidade', () => {
    const navigateToUpdateNaoConformidadeUserActionMock = new NavigateToUpdateNaoConformidadeUserActionMock()
    navigateToUpdateNaoConformidadeUserActionMock.atualizarNaoConformidadeRequestsRestantesMock.getListOrdemProducao?.response
      .body.items[0].odfFinalizada = true;
    navigateToNaoConformidadeEditor(navigateToUpdateNaoConformidadeUserActionMock)
  })
  it('Verificar botão "Gerar Ordem Retrabalho"', () => {
    cy.get(`${mainSelector} vs-header rnc-gerar-retrabalho-button vs-button`)
    .contains('Gerar Odf Retrabalho')
    .should('exist')
  })
})

describe('Se não houver operação de retrabalho gerada e não houver ordem de faturamento gerada, deve ' +
'chamar buscar odf e se a mesma não estiver finalizada deve apresentar o botão "Gerar Operação Retrabalho"',
() => {
  it('Navegar para edição de não conformidade', () => {
    const navigateToUpdateNaoConformidadeUserActionMock = new NavigateToUpdateNaoConformidadeUserActionMock()

    navigateToNaoConformidadeEditor(navigateToUpdateNaoConformidadeUserActionMock)
  })
  it('Verificar botão', () => {
    cy.get(`${mainSelector} vs-header rnc-gerar-retrabalho-button vs-button`)
    .contains('Gerar Operação Retrabalho')
    .should('exist')
  })
})

describe('Se houver operação de retrabalho gerada, deve apresentar o botão "Gerar Operação Retrabalho" e o mesmo deve estar desabilitado', () => {
  it('Navegar para edição de não conformidade', () => {
    const navigateToUpdateNaoConformidadeUserActionMock = new NavigateToUpdateNaoConformidadeUserActionMock()
    navigateToUpdateNaoConformidadeUserActionMock.atualizarNaoConformidadeRequestsRestantesMock.getOperacaoRetrabalhoRequest.response.body = {}
    delete navigateToUpdateNaoConformidadeUserActionMock.atualizarNaoConformidadeRequestsRestantesMock.getListOrdemProducao;
    navigateToNaoConformidadeEditor(navigateToUpdateNaoConformidadeUserActionMock)
  })
  it('Verificar botão', () => {
    cy.get(`${mainSelector} vs-header rnc-gerar-retrabalho-button vs-button`)
    .contains('Gerar Operação Retrabalho')
    .should('exist')
    .should('be.disabled')
  })
})
describe('Se não houver operação de retrabalho gerada e houver ordem de faturamento gerada,'+
  'deve apresentar o botão "Estornar Odf Retrabalho"',
() => {
  it('Navegar para edição de não conformidade', () => {
    const navigateToUpdateNaoConformidadeUserActionMock = new NavigateToUpdateNaoConformidadeUserActionMock()
    navigateToUpdateNaoConformidadeUserActionMock
      .atualizarNaoConformidadeRequestsRestantesMock.getOrdemRetrabalhoRequest.response.body = {}
    delete navigateToUpdateNaoConformidadeUserActionMock.atualizarNaoConformidadeRequestsRestantesMock.getListOrdemProducao;

    navigateToNaoConformidadeEditor(navigateToUpdateNaoConformidadeUserActionMock)
  })
  it('Verificar botão', () => {
    cy.get(`${mainSelector} vs-header rnc-gerar-retrabalho-button vs-button`)
    .contains('Estornar Odf Retrabalho')
    .should('exist')
  })
})

