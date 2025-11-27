import { getConfiguracoesGeraisRequest, updateConfiguracoesGeraisRequest } from 'cypress/support/requests/configuracao/configuracao-requests';
import { GeralFormControls } from '../../../../apps/rnc/src/app/pages/settings/geral/geral-form-controls';
import { Interception } from 'cypress/types/net-stubbing';
import { ConfiguracaoGeralInput } from '@viasoft/rnc/api-clients/Configuracoes/Gerais/configuracao-geral-input';

const mainSelector = 'rnc-geral'
describe('Atualizar defeito com sucesso', () => {
  it('Visitar tela configuracoesGerais', () => {
    cy.batchRequestStub(getConfiguracoesGeraisRequest()).then((alias) => {
      cy.visit('rnc/configuracoes/gerais');
      cy.wait(alias);
    });
  });
  it('Preencher Formulário', () => {
    cy.getVsCheckbox(GeralFormControls.considerarApenasSaldoApontado).click({force: true})
  });
  it('Salvar e validar resultado', () => {
    const putRequest = updateConfiguracoesGeraisRequest()
    const getRequest = getConfiguracoesGeraisRequest();
    cy.batchRequestStub([putRequest, getRequest]).then((alias:string[]) => {
      cy.get(`${mainSelector} vs-button[icon="save"] button`).click()
      cy.wait(alias).then((interceptions: Array<Interception>) => {
        const interception = interceptions[0]
        const expectedBody = {
          ...putRequest.expectedBody,
          considerarApenasSaldoApontado: true
        } as ConfiguracaoGeralInput
        cy.validateRequestBody(interception.request.body, expectedBody)
      })
    })
  })
})

