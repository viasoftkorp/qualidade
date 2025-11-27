import { SelectDefeitosGridMock } from "cypress/support/mock/select-defeitos-grid-mock";
import { selectDefeitosGridElement } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { ImplementacaoEvitarReincidenciaNaoConformidadesFormControl } from '../../../../apps/rnc/src/app/pages/nao-conformidades/nao-conformidades-editor/defeitos-nao-conformidades/implementacao-evitar-reincidencia-nao-conformidades/implementacao-evitar-reincidencia-nao-conformidades-editor-modal/implementacao-evitar-reincidencia-nao-conformidades-form-control'
import { getAllUsersRequest, getUserByIdRequest } from "cypress/support/requests/authentication/users-requests";
import { ids, strings } from "cypress/support/test-utils";
import { Interception } from "cypress/types/net-stubbing";
import { ImplementacaoEvitarReincidenciaNaoConformidadesInput } from "@viasoft/rnc/api-clients/Nao-Conformidades/Implementacao-Evitar-Reincidencia-Nao-Conformidades/model/implementacao-evitar-reincidencia-nao-conformidades-input";
import { ImplementacaoEvitarReincidenciaNaoConformidadesModel } from "@viasoft/rnc/api-clients/Nao-Conformidades/Implementacao-Evitar-Reincidencia-Nao-Conformidades/model/implementacao-evitar-reincidencia-nao-conformidades-model";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { getAllImplementacoesEvitarReincidenciaNaoConformidadeRequest, updateImplementacoesEvitarReincidenciaNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/implementacao-evitar-reincidencia-nao-conformidades/implementacao-evitar-reincidencia-nao-conformidades";

const mainSelector = 'rnc-implementacao-evitar-reincidencia-nao-conformidades'
const editorModalSelector = 'rnc-implementacao-evitar-reincidencia-nao-conformidades-editor-modal'
describe.skip('Se campos obrigatórios informados, deve chamar atualizar a implementação', () => {
  it('Navegar para defeitos no editor de Nao Conformidade', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    const selectDefeitosGridRequests = new SelectDefeitosGridMock();
    selectDefeitosGridElement(naoConformidadeRequests, selectDefeitosGridRequests);
  });
  it('Clicar Em Implem. Eficácia Evitar Reinc.', () => {
    const getAllImplementacoesEvitarReincidenciaNaoConformidade = getAllImplementacoesEvitarReincidenciaNaoConformidadeRequest();
    getAllImplementacoesEvitarReincidenciaNaoConformidade.response.body.items[0] = {
      ...getAllImplementacoesEvitarReincidenciaNaoConformidadeRequest().response.body.items[0],
      idResponsavel:ids[0],
      idAuditor: ids[0],
      acaoImplementada: true,
      dataAnalise: new Date(2023, 11, 29),
      dataPrevistaImplantacao: new Date(2023, 11, 29),
      dataVerificacao: new Date(2023, 11, 29),
      novaData: new Date(2023, 11, 29),
    } as ImplementacaoEvitarReincidenciaNaoConformidadesModel
    cy.batchRequestStub([getAllImplementacoesEvitarReincidenciaNaoConformidade, getAllUsersRequest()]).then(alias => {
      cy.get('mat-tab-group mat-tab-header div').contains('Implem. Eficácia Evitar Reinc.').click()

      cy.wait(alias)
    })
  })
  it('Acessar implementação nao conformidade já existente', () => {
    cy.batchRequestStub([getUserByIdRequest()]).then(aliases => {
      cy.get(`${mainSelector} vs-grid tbody tr`).eq(0).click();
      cy.wait(aliases)
    })

    cy.getVsTextArea(ImplementacaoEvitarReincidenciaNaoConformidadesFormControl.descricao, editorModalSelector)
      .clear()
      .type(strings[1])
    cy.getAutoCompleteItem({
      formControlName:ImplementacaoEvitarReincidenciaNaoConformidadesFormControl.idResponsavel,
      request: getAllUsersRequest(),
      selector:'admin-user-autocomplete-select',
      eq:1
    })
    cy.getAutoCompleteItem({
      formControlName:ImplementacaoEvitarReincidenciaNaoConformidadesFormControl.idAuditor,
      request: getAllUsersRequest(),
      selector:'admin-user-autocomplete-select',
      eq:1
    })
    cy.getVsDatePicker(ImplementacaoEvitarReincidenciaNaoConformidadesFormControl.dataAnalise, editorModalSelector).clear().type(`01012022`)
    cy.getVsDatePicker(ImplementacaoEvitarReincidenciaNaoConformidadesFormControl.dataPrevistaImplantacao, editorModalSelector).clear().type(`01012022`)
    cy.getVsDatePicker(ImplementacaoEvitarReincidenciaNaoConformidadesFormControl.dataVerificacao, editorModalSelector).clear().type(`01012022`)
    cy.getVsDatePicker(ImplementacaoEvitarReincidenciaNaoConformidadesFormControl.novaData).clear().type(`01012022`)

    cy.get(`vs-checkbox[formControlName=${ImplementacaoEvitarReincidenciaNaoConformidadesFormControl.acaoImplementada}] mat-checkbox input`).click({ force: true });
  })
  it('Salvar e validar dados', () => {
    cy.batchRequestStub([updateImplementacoesEvitarReincidenciaNaoConformidadeRequest(), getAllImplementacoesEvitarReincidenciaNaoConformidadeRequest()]).then(alias => {
      cy.get(`${editorModalSelector} vs-button[type=save] button`).click();
      cy.wait(alias).then((interceptions:Array<Interception>) => {
        const interceptionRequestBody = interceptions[0].request.body as ImplementacaoEvitarReincidenciaNaoConformidadesInput;
        const expectedBody = {
          ...updateImplementacoesEvitarReincidenciaNaoConformidadeRequest().expectedBody,
          id: interceptionRequestBody.id,
          acaoImplementada: false,
          descricao: strings[1],
          idAuditor: ids[1],
          idResponsavel: ids[1],
          idDefeitoNaoConformidade: ids[0],
          idNaoConformidade: ids[0],
        } as ImplementacaoEvitarReincidenciaNaoConformidadesInput
        cy.validateRequestBody(interceptionRequestBody, expectedBody)
      })
    })
  })
})
