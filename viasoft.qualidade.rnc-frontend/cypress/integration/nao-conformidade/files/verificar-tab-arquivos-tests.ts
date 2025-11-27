import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { deleteFile, downloadFileRequest, getFilesList } from "cypress/support/requests/file-provider/file";
import { navigateToNaoConformidadeEditor } from "cypress/support/user-actions/nao-conformidades-user-actions";

describe("Verificar tab de arquivos",() => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Popular e testar tab de arquivos', () => {
    const getFiles = getFilesList();
    const deleteFiles = deleteFile();
    const downloadFiles = downloadFileRequest();
     cy.batchRequestStub([getFiles, deleteFiles, downloadFiles])
    .then(aliasesToWait => {
      cy.getVsTabGroupItem('Arquivos').click()
      cy.wait(aliasesToWait[0])
      cy.get('rnc-nao-conformidades-files vs-button[ng-reflect-icon=file-download]').eq(0).click()
      cy.wait(aliasesToWait[2])
      cy.get('rnc-nao-conformidades-files vs-button[ng-reflect-icon=trash-alt]').eq(0).click()
      cy.get('vs-message-dialog vs-button[type=save]').click()
      cy.wait(aliasesToWait[1])
    })
  })
})
