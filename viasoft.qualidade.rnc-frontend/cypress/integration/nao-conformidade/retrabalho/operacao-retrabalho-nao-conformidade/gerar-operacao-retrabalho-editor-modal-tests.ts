import {
  NavigateToUpdateNaoConformidadeUserActionMock
} from 'cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock'
import { navigateToNaoConformidadeEditor } from 'cypress/support/user-actions/nao-conformidades-user-actions';
import {
  GerarOperacaoRetrabalhoFormControls
} from 'apps/rnc/src/app/pages/nao-conformidades/nao-conformidades-editor/retrabalho-nao-conformidades/operacao-retrabalho-nao-conformidades/gerar-operacao-retrabalho-editor-modal/gerar-operacao-retrabalho-form-controls';
import {
  createOperacaoRetrabalhoRequest,
  getOperacoesRequest, getSaldoOperacaoRequest
} from 'cypress/support/requests/nao-conformidade/operacao-retrabalho-nao-conformidades/operacao-retrabalho-nao-conformidades-requests';
import { Interception } from 'cypress/types/net-stubbing';

const mainSelector = 'rnc-nao-conformidades-editor'
const gerarOperacaoRetrabalhoEditorModalSelector = 'rnc-gerar-operacao-retrabalho-editor-modal'

describe.skip('Se campos obrigatórios preenchidos, ao clicar em gerar operação, deve mandar gerar operação retrabalho', () => {
  it('Navegar para edição de não conformidade', () => {
    const navigateToUpdateNaoConformidadeUserActionMock = new NavigateToUpdateNaoConformidadeUserActionMock();
    navigateToNaoConformidadeEditor(navigateToUpdateNaoConformidadeUserActionMock);
  });

  it('Clicar em "Gerar Operação Retrabalho"', () => {
    cy.get(`${mainSelector} vs-header rnc-gerar-retrabalho-button vs-button`)
        .contains('Gerar Operação Retrabalho')
        .click();
  });

  it('Preencher campos obrigatórios', () => {
    cy.batchRequestStub(getSaldoOperacaoRequest()).then((alias) => {
      cy.getAutoCompleteItem({
        selector: `${gerarOperacaoRetrabalhoEditorModalSelector} qa-operacao-autocomplete-select`,
        request: getOperacoesRequest(),
        formControlName: GerarOperacaoRetrabalhoFormControls.numeroOperacaoARetrabalhar
      })
      cy.wait(alias);
    });

    cy.get(`${gerarOperacaoRetrabalhoEditorModalSelector} vs-button`).contains('Gerar Operação').should('be.disabled');

    cy.getVsInput(GerarOperacaoRetrabalhoFormControls.saldoDisponivel, gerarOperacaoRetrabalhoEditorModalSelector)
      .should('be.disabled')
      .should('have.value', 5);

    cy.getVsInput(GerarOperacaoRetrabalhoFormControls.quantidade, gerarOperacaoRetrabalhoEditorModalSelector).type('10');
  });

  it('Se Pressionar o botão "x" e formulário estiver sujo deve aparecer mensagem confirmação e caso confirmado, deve fechar modal', () => {
    cy.get(`${gerarOperacaoRetrabalhoEditorModalSelector} div[actions] vs-button[icon=times] button`).click();
    const mensagemConfirmacao = 'As alterações realizadas ainda não foram salvas e serão perdidas, deseja continuar?'
    cy.get('vs-message-dialog').contains(mensagemConfirmacao).should('exist');
    cy.get('vs-button[type=cancel] button').click()
   })

   it('Se Teclar "esc" e formulário estiver sujo deve aparecer mensagem confirmação e caso confirmado, deve fechar modal', () => {
    cy.get(gerarOperacaoRetrabalhoEditorModalSelector).type('{esc}')
    const mensagemConfirmacao = 'As alterações realizadas ainda não foram salvas e serão perdidas, deseja continuar?'
    cy.get('vs-message-dialog').contains(mensagemConfirmacao).should('exist');
    cy.get('vs-button[type=cancel] button').click()
   })

   it('Se clicar no backdrop e formulário estiver sujo deve aparecer mensagem confirmação e caso confirmado, deve fechar modal', () => {
    const backDrop = 'div.cdk-overlay-backdrop.cdk-overlay-dark-backdrop.cdk-overlay-backdrop-showing'
    cy.get(backDrop).click({force: true})
    const mensagemConfirmacao = 'As alterações realizadas ainda não foram salvas e serão perdidas, deseja continuar?'
    cy.get('vs-message-dialog').contains(mensagemConfirmacao).should('exist');
    cy.get('vs-button[type=cancel] button').click()
   })

  it('Clicar em Gerar Operação', () => {
    cy.get(`${gerarOperacaoRetrabalhoEditorModalSelector} vs-button`).contains('Gerar Operação').should('not.be.disabled')

    cy.batchRequestStub(createOperacaoRetrabalhoRequest()).then(alias => {
      cy.get(`${gerarOperacaoRetrabalhoEditorModalSelector} vs-button`).contains('Gerar Operação').click();
      cy.wait(alias).then((interception: Interception) => {
        cy.validateRequestBody(interception.request.body, createOperacaoRetrabalhoRequest().expectedBody);
      })
    })
  })
})
