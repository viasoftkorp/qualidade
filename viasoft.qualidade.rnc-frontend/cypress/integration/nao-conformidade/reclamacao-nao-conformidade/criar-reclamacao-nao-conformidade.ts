import { ReclamacoesNaoConformidadesInput } from "@viasoft/rnc/api-clients/Nao-Conformidades";
import { ReclamacoesNaoConformidadesFormControl } from "@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/reclamacoes-nao-conformidades/reclamacoes-nao-conformidades-form-control";
import { AtualizarNaoConformidadeMock } from "cypress/support/mock/atualizar-nao-conformidade-mock";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { createReclamacaoNaoConformidadeRequest, getReclamacaoNaoConformidadeRequest } from "cypress/support/requests/nao-conformidade/reclamacao-nao-conformidades/reclamacao-nao-conformidades-requests";
import { strings } from "cypress/support/test-utils";
import { atualizarNaoConformidade, navigateToNaoConformidadeEditor } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { Interception } from "cypress/types/net-stubbing";

describe('Criar reclamacao nao conformidades', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();

    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Navegar para reclamações', () => {
    cy.batchRequestStub(getReclamacaoNaoConformidadeRequest()).then(alias => {
      cy.get('mat-tab-group mat-tab-header div').contains('Reclamação').click();
      cy.wait(alias)
    })
  })
  it('Preencher campos', () => {
    // No caso de clientes
    cy.getVsInput(`${ReclamacoesNaoConformidadesFormControl.improcedentes}`).type('3')
    cy.getVsInput(`${ReclamacoesNaoConformidadesFormControl.procedentes}`).type('3')
    // Qtds
    cy.getVsInput(`${ReclamacoesNaoConformidadesFormControl.quantidadeLote}`).type('3')
    cy.getVsInput(`${ReclamacoesNaoConformidadesFormControl.quantidadeNaoConformidade}`).type('3')
    // Disposição Produtos
    cy.getVsInput(`${ReclamacoesNaoConformidadesFormControl.disposicaoProdutosAprovados}`).type('3')
    cy.getVsInput(`${ReclamacoesNaoConformidadesFormControl.disposicaoProdutosConcessao}`).type('3')
    cy.getVsInput(`${ReclamacoesNaoConformidadesFormControl.rejeitado}`).type('3')
    cy.getVsInput(`${ReclamacoesNaoConformidadesFormControl.retrabalho}`).type('3')

    // Destino Peças Rejeitadas
    cy.getVsCheckbox(ReclamacoesNaoConformidadesFormControl.recodificar).click({force:true})
    cy.getVsCheckbox(ReclamacoesNaoConformidadesFormControl.sucata).click({force:true})

    // Caso Reclamação Improcedente
    cy.getVsCheckbox(ReclamacoesNaoConformidadesFormControl.devolucaoFornecedor).click({force:true})
    cy.getVsCheckbox(ReclamacoesNaoConformidadesFormControl.retrabalhoComOnus).click({force:true})
    cy.getVsCheckbox(ReclamacoesNaoConformidadesFormControl.retrabalhoSemOnus).click({force:true})

    cy.get(`rnc-reclamacoes-nao-conformidades vs-textarea[formControlName=${ReclamacoesNaoConformidadesFormControl.observacao}]`).type(strings[2])

  })
  it('Salvar', () => {
    const atualizarNaoConformidadeMock = new AtualizarNaoConformidadeMock()
    atualizarNaoConformidadeMock.createReclamacaoNaoConformidade = createReclamacaoNaoConformidadeRequest()

    atualizarNaoConformidade(atualizarNaoConformidadeMock).then((interceptions:Interception[]) => {
      const reclamacaoInterception = interceptions.find((interception: Interception) => interception.request.url.includes('reclamacao'));
      if(reclamacaoInterception) {
        atualizarNaoConformidadeMock.createReclamacaoNaoConformidade.expectedBody.id = reclamacaoInterception.request.body.id
        reclamacaoInterception.request.body.revisao = Number.parseInt(reclamacaoInterception.request.body.revisao)
        reclamacaoInterception.request.body.dataFabricacaoLote = new Date(reclamacaoInterception.request.body.dataFabricacaoLote)

        cy.validateRequestStatusCode(reclamacaoInterception?.response?.statusCode, 200)
        cy.validateRequestBody(reclamacaoInterception?.request.body, atualizarNaoConformidadeMock.createReclamacaoNaoConformidade.expectedBody);
      }
    })
  })
})

describe('Criar reclamacao nao conformidades sem valores', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();

    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });
  it('Navegar para reclamações', () => {
    cy.batchRequestStub(getReclamacaoNaoConformidadeRequest()).then(alias => {
      cy.get('mat-tab-group mat-tab-header div').contains('Reclamação').click();
      cy.wait(alias)
    })
  })
  it('Preencher reclamação procedentes', () => {
    cy.getVsInput(`${ReclamacoesNaoConformidadesFormControl.procedentes}`).type('0')
  })
  it('Salvar', () => {
    const atualizarNaoConformidadeMock = new AtualizarNaoConformidadeMock()
    atualizarNaoConformidadeMock.createReclamacaoNaoConformidade = createReclamacaoNaoConformidadeRequest()
    const newExpectedBody = {
      procedentes: 0,
      improcedentes: 0,
      quantidadeLote: 0,
      quantidadeNaoConformidade: 0,
      disposicaoProdutosAprovados: 0,
      disposicaoProdutosConcessao: 0,
      rejeitado: 0,
      retrabalho: 0,
      retrabalhoComOnus: false,
      retrabalhoSemOnus: false,
      devolucaoFornecedor: false,
      recodificar: false,
      sucata: false,
    } as ReclamacoesNaoConformidadesInput
    atualizarNaoConformidadeMock.createReclamacaoNaoConformidade?.expectedBody = newExpectedBody
    atualizarNaoConformidade(atualizarNaoConformidadeMock).then((interceptions:Interception[]) => {
      const reclamacaoInterception = interceptions.find((interception: Interception) => interception.request.url.includes('reclamacao'));
      if(reclamacaoInterception) {
        atualizarNaoConformidadeMock.createReclamacaoNaoConformidade.expectedBody.id = reclamacaoInterception.request.body.id
        reclamacaoInterception.request.body.revisao = Number.parseInt(reclamacaoInterception.request.body.revisao)
        reclamacaoInterception.request.body.dataFabricacaoLote = new Date(reclamacaoInterception.request.body.dataFabricacaoLote)

        cy.validateRequestStatusCode(reclamacaoInterception?.response?.statusCode, 200)
        cy.validateRequestBody(reclamacaoInterception?.request.body, atualizarNaoConformidadeMock.createReclamacaoNaoConformidade.expectedBody);
      }
    })
  })
})
