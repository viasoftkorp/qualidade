import { AtualizarNaoConformidadeMock } from "../mock/atualizar-nao-conformidade-mock";
import { CreateNaoConformidadeMock } from "../mock/criar-nao-conformidade-mock";
import { NaoConformidadesList } from "../mock/nao-conformidades-list-mock";
import { SelectDefeitosGridMock } from "../mock/select-defeitos-grid-mock";
import { getRequestsFromMock } from "../test-utils";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "../mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { AcessarGerarOdfRetrabalhoMock } from "../mock/acessar-gerar-odf-retrabalho-mock";

export function initiateApp(listMock: NaoConformidadesList) {
  return cy.batchRequestStub([...getRequestsFromMock(listMock)])
    .then(aliasesToWait => {
      cy.visit('/rnc/nao-conformidades');
      cy.wait(aliasesToWait);
    });
}
export function navigateToNaoConformidadeEditor(mock: NavigateToUpdateNaoConformidadeUserActionMock){
    initiateApp(mock.naoConformidadesList);

    const canGenerateOrdemRetrabalhoRequest = mock.atualizarNaoConformidadeRequestsRestantesMock.canGenerateOrdemRetrabalhoRequest;
    delete mock.atualizarNaoConformidadeRequestsRestantesMock.canGenerateOrdemRetrabalhoRequest

    if (canGenerateOrdemRetrabalhoRequest) {
      cy.intercept(
        canGenerateOrdemRetrabalhoRequest.method,
        canGenerateOrdemRetrabalhoRequest.url,
        canGenerateOrdemRetrabalhoRequest.response
      ).as('canGenerateOrdemRetrabalhoRequest')
    }
    cy.batchRequestStub([...getRequestsFromMock(mock.atualizarNaoConformidadeRequestsIniciaisMock),
    ...getRequestsFromMock(mock.atualizarNaoConformidadeRequestsRestantesMock)])
    .then(aliasesToWait => {
      cy.get('rnc-nao-conformidades vs-grid tbody tr').eq(0).click()
      cy.wait(aliasesToWait)
    });

    if (canGenerateOrdemRetrabalhoRequest) {
      cy.wait('@canGenerateOrdemRetrabalhoRequest')
    }
}

export function selectDefeitosGridElement(navigateToUpdateNaoConformidadeUserActionMock: NavigateToUpdateNaoConformidadeUserActionMock,
  selectDefeitosGridMock:SelectDefeitosGridMock) {
    navigateToNaoConformidadeEditor(navigateToUpdateNaoConformidadeUserActionMock)
    cy.batchRequestStub(getRequestsFromMock(selectDefeitosGridMock))
    .then(aliasesToWait => {
      cy.get('rnc-defeitos-nao-conformidades vs-grid tbody tr').eq(0).click()
      cy.wait(aliasesToWait);
    });
}

export function createNaoConformidade(createNaoConformidadeMock:CreateNaoConformidadeMock){
  return cy.batchRequestStub(getRequestsFromMock(createNaoConformidadeMock)).then(alias => {
    cy.get("rnc-nao-conformidades-editor header vs-button[type=save] button").click();
    cy.wait(alias);
  })
}

export function atualizarNaoConformidade(atualizarNaoConformidadeMock:AtualizarNaoConformidadeMock){
  return cy.batchRequestStub(getRequestsFromMock(atualizarNaoConformidadeMock)).then(alias => {
    cy.get("rnc-nao-conformidades-editor header vs-button[type=save] button").click();
    cy.wait(alias);
  })
}

export function acessarGerarOdfRetrabalho(navigateToUpdateNaoConformidadeUserActionMock: NavigateToUpdateNaoConformidadeUserActionMock,
  acessarGerarOdfRetrabalhoMock: AcessarGerarOdfRetrabalhoMock) {
  navigateToNaoConformidadeEditor(navigateToUpdateNaoConformidadeUserActionMock)
  cy.batchRequestStub(getRequestsFromMock(acessarGerarOdfRetrabalhoMock)).then(aliases => {
    cy.get('rnc-nao-conformidades-editor vs-header vs-button[icon=wrench] button').click();
    cy.wait(aliases)
  })
}
