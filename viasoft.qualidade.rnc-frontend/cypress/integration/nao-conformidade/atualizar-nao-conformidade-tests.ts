import { initiateApp, navigateToNaoConformidadeEditor } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { NaoConformidadesFormControl } from '../../../apps/rnc/src/app/pages/nao-conformidades/nao-conformidades-editor/nao-conformidades-form-control';
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { getRequestsFromMock, ids } from "cypress/support/test-utils";
import { getAllNaoConformidadesRequest } from "cypress/support/requests/nao-conformidade/nao-conformidades.requests";
import { StatusNaoConformidade } from "@viasoft/rnc/api-clients/Nao-Conformidades/model/status-nao-conformidades";

const rncSelector = 'rnc-nao-conformidades-editor';

describe('Se não conformidade fechada, retrabalho deve estar desabilitado', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    naoConformidadeRequests.atualizarNaoConformidadeRequestsIniciaisMock.naoConformidadeById.response.body.status = StatusNaoConformidade.Fechado;
    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Verificar botão gerar retrabalho', () => {
    cy.get('rnc-gerar-retrabalho-button[ng-reflect-disabled=true] vs-button button').should('be.disabled')
  })
})
describe('Se odf de retrabalho gerada, formulário deve ficar desabilitado', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    naoConformidadeRequests.atualizarNaoConformidadeRequestsRestantesMock.getOrdemRetrabalhoRequest.response.body = {
      success:true
    };
    delete naoConformidadeRequests.atualizarNaoConformidadeRequestsRestantesMock.getListOrdemProducao

    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Verificar formulário', () => {
    cy.getVsInput(NaoConformidadesFormControl.numeroOdf, `${rncSelector} vs-form`).should('be.disabled')
  })
})

describe('Se UtilizarReservaDePedidoNaLocalizacaoDeEstoque estiver verdadeiro, deve aparecer a tab "Pedido Venda"', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    naoConformidadeRequests.atualizarNaoConformidadeRequestsIniciaisMock.getConfiguracoesGeraisRequest
      .response.body.utilizarReservaDePedidoNaLocalizacaoDeEstoque = true
    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Verificar se a tab "Pedido Venda" está aparecendo', () => {
    cy.getVsTabGroupItem('Pedido Venda').should('exist')
  })
})

describe('Se UtilizarReservaDePedidoNaLocalizacaoDeEstoque estiver falso, não deve aparecer a tab "Pedido Venda"', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    naoConformidadeRequests.atualizarNaoConformidadeRequestsIniciaisMock.getConfiguracoesGeraisRequest
      .response.body.utilizarReservaDePedidoNaLocalizacaoDeEstoque = false
    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Verificar se a tab "Pedido Venda" está aparecendo', () => {
    cy.getVsTabGroupItem('Pedido Venda').should('not.exist')
  })
})

describe('Se empresa atual não for a mesma da nao conformidade, deve apresentar mensagem para trocar empresa', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();

    initiateApp(naoConformidadeRequests.naoConformidadesList);

    naoConformidadeRequests.atualizarNaoConformidadeRequestsIniciaisMock.naoConformidadeById.response.body.companyId = ids[2];

    cy.batchRequestStub([...getRequestsFromMock(naoConformidadeRequests.atualizarNaoConformidadeRequestsIniciaisMock)])
      .then(aliasesToWait => {
        cy.get('rnc-nao-conformidades vs-grid tbody tr').eq(0).click()
        cy.wait(aliasesToWait)
      });
  });

  it('Verificar mensagem de confirmação de troca de empresa', () => {
    cy.get('vs-message-dialog').contains('Deseja trocar de empresa para acessar à não conformidade ?').should('exist')
  })

  it('Se não confirmada troca, deve voltar para lista de rncs', () => {
    cy.batchRequestStub(getAllNaoConformidadesRequest()).then((alias) => {
      cy.get('vs-message-dialog vs-button[type=cancel] button').click()
      cy.wait(alias)
    })
    cy.url().should('match', /\/rnc\/nao-conformidades$/)
  })
})

describe('Se confirmada troca de empresa, deve trocar empresa para a empresa da não conformidade', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();

    initiateApp(naoConformidadeRequests.naoConformidadesList);

    naoConformidadeRequests.atualizarNaoConformidadeRequestsIniciaisMock.naoConformidadeById.response.body.companyId = ids[2];

    cy.batchRequestStub([...getRequestsFromMock(naoConformidadeRequests.atualizarNaoConformidadeRequestsIniciaisMock)])
      .then(aliasesToWait => {
        cy.get('rnc-nao-conformidades vs-grid tbody tr').eq(0).click()
        cy.wait(aliasesToWait)
      });
  });

  it('Se confirmada troca, deve trocar company e carregar nao conformidade', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();

    cy.batchRequestStub([...getRequestsFromMock(naoConformidadeRequests.atualizarNaoConformidadeRequestsRestantesMock)]).then((alias) => {
      cy.get('vs-message-dialog vs-button[type=save] button').click()
      cy.wait(alias)
    })
  })

  after(() => {
    resetarCompany();
  })
})

function resetarCompany() {
  cy.get('vs-navbar vs-select[formcontrolname=company]').click()
  cy.get('div[role=listbox] mat-option').eq(0).click()
}
