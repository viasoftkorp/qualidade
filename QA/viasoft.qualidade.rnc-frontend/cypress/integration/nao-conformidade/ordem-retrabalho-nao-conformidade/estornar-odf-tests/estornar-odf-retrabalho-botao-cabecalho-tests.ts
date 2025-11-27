import { GerarOrdemRetrabalhoValidationResult } from "@viasoft/rnc/api-clients/Nao-Conformidades/Ordem-Retrabalho/model/gerar-ordem-retrabalho-validation-result";
import { OrigemNaoConformidades } from "@viasoft/rnc/api-clients/Nao-Conformidades/model/origem-nao-conformidades";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { navigateToNaoConformidadeEditor } from "cypress/support/user-actions/nao-conformidades-user-actions";

describe('Se origem não conformidade for "Inspeção de saída", botão deve estar desabilitado', () => {
  describe('Se odf de retrabalho não foi gerada mas não pode gerar odf de retrabalho, botão deve ficar desabilitado', () => {
    it('Navegar para Nao Conformidade editor', () => {
      const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
      naoConformidadeRequests.atualizarNaoConformidadeRequestsRestantesMock.getOrdemRetrabalhoRequest.response.body = {
        success: true
      }
      delete naoConformidadeRequests.atualizarNaoConformidadeRequestsRestantesMock.getListOrdemProducao
      naoConformidadeRequests.atualizarNaoConformidadeRequestsIniciaisMock.naoConformidadeById.response.body.origem = OrigemNaoConformidades.InspecaoSaida;
      navigateToNaoConformidadeEditor(naoConformidadeRequests);
    });
    it('Verificar se botão "Gerar Odf Retrabalho está desabilitado', () => {
      cy.get('rnc-nao-conformidades-editor vs-header vs-button[icon=undo] button').should('be.disabled')
    })
  })
})
