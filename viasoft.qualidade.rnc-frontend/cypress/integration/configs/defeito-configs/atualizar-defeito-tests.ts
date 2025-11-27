import { Interception } from 'cypress/types/net-stubbing';
import { codes, strings } from 'cypress/support/test-utils';
import { DefeitoFormControls } from '../../../../apps/rnc/src/app/pages/settings/defeito/defeito-editor-modal/defeito-form-controls';
import { getAllDefeitosRequest, getDefeitoRequestForId, getDefeitosViewListRequest, updateDefeitoRequest } from '../../../support/requests/defeitos/defeitos-requests';
import { getCausasList, getSolucoesList } from 'cypress/support/requests/defeitos/defeitos-requests';
import { getCausaRequestForId } from 'cypress/support/requests/causas/causas.request';
import { getSolucaoRequestForId } from 'cypress/support/requests/solucoes/solucoes-requests';

describe("Atualizar defeito com sucesso",() => {
  it("Visitar tela defeitos",() => {
    cy.batchRequestStub(getDefeitosViewListRequest()).then(alias => {
      cy.visit('rnc/configuracoes/defeitos')
      cy.wait(alias)
    })
  })
  it("Clicar defeito",() => {
    cy.batchRequestStub([getDefeitoRequestForId(), getCausaRequestForId(), getSolucaoRequestForId()]).then(aliases => {
      cy.get('rnc-defeito vs-grid tbody tr').eq(0).click()
      cy.wait(aliases)
    })

  })
  it("Preencher Formulário",() => {
    cy.get('rnc-defeito-editor-modal vs-button[type="save"] button').should('be.disabled')
    cy.getVsInput(DefeitoFormControls.descricao).clear().type(strings[3])

    cy.getAutoCompleteItem({
      formControlName: DefeitoFormControls.idCausa,
      request: getCausasList(),
      selector:'qa-causa-autocomplete-select',
      eq: 1
    })
    cy.getAutoCompleteItem({
      formControlName: DefeitoFormControls.idSolucao,
      request: getSolucoesList(),
      selector:'qa-solucao-autocomplete-select',
      eq: 1
    })

    cy.get(`vs-textarea[formcontrolname=${DefeitoFormControls.detalhamento}]`).clear().type(strings[3])

    cy.get('rnc-defeito-editor-modal vs-button[type="save"] button').should('be.enabled')
  })

  it("Salvar nova defeito",() => {
    const putRequest = updateDefeitoRequest()
    const refreshGridRequest = getDefeitosViewListRequest()
    refreshGridRequest.response.body.items.push({
      codigo:codes[2],
      descricao:strings[2],
      detalhamento: strings[2]
    })

    cy.batchRequestStub([putRequest, refreshGridRequest]).then((alias:string[]) => {
      cy.get('rnc-defeito-editor-modal vs-button[type="save"] button').click()
      cy.wait(alias).then((interceptions:Interception[]) => {
        const defeitoInterception = interceptions.find((interception:Interception) => interception.request.url.includes('defeitos'))
        if(defeitoInterception) {
          cy.validateRequestStatusCode(defeitoInterception.response?.statusCode,200)

          putRequest.expectedBody.id = defeitoInterception.request.body.id

          cy.validateRequestBody(defeitoInterception.request.body,putRequest.expectedBody)
        }
      })
    })
  })
})
