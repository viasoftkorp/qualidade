import { acessarGerarOdfRetrabalho } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { GerarOdfRetrabalhoFormControls } from '../../../../../apps/rnc/src/app/pages/nao-conformidades/nao-conformidades-editor/retrabalho-nao-conformidades/odf-retrabalho-nao-conformidades/gerar-odf-retrabalho-editor-modal/gerarOdfRetrabalhoFormControls'
import { gerarOrdemRetrabalhoRequest } from "cypress/support/requests/nao-conformidade/ordem-retrabalho-nao-conformidades/ordem-retrabalho-nao-conformidades";
import { Interception } from "cypress/types/net-stubbing";
import { OrdemRetrabalhoInput } from "@viasoft/rnc/api-clients/Nao-Conformidades/Ordem-Retrabalho/model/ordem-retrabalho-input";
import { getAllLocaisRequest, getLocalByIdRequest } from "cypress/support/requests/legacy-logistica/locais-requests";
import { codes, dates, ids, strings } from "cypress/support/test-utils";
import { getAllEstoqueLocaisRequest } from "cypress/support/requests/legacy-logistica/estoque-local-requests";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { AcessarGerarOdfRetrabalhoMock } from "cypress/support/mock/acessar-gerar-odf-retrabalho-mock";
import { getAllEstoquePedidoVendaEstoqueLocaisViewRequest } from "cypress/support/requests/legacy-logistica/estoque-pedido-venda-estoque-local-requests";
import { EstoqueLocalOutput } from "@viasoft/rnc/api-clients/Estoque-Locais/model/estoque-local-output";
import { LocalOutput } from "@viasoft/rnc/api-clients/Locais/model/local-output";
const mainSelector = 'rnc-gerar-odf-retrabalho-editor-modal';
const rncSelector = 'rnc-nao-conformidades-editor';
describe('Se fornecido "quantidade" e "estoque local", deve ser possível gerar odf retrabalho', () => {
  it('Navegar até o modal de geração de odf de retrabalho', () => {
    const navigateToUpdateNaoConformidadeUserActionMock = new NavigateToUpdateNaoConformidadeUserActionMock();
    const acessarGerarOdfRetrabalhoMock = new AcessarGerarOdfRetrabalhoMock();
    navigateToUpdateNaoConformidadeUserActionMock.atualizarNaoConformidadeRequestsRestantesMock.getListOrdemProducao?.response.body.items[0].odfFinalizada = true;

    acessarGerarOdfRetrabalho(navigateToUpdateNaoConformidadeUserActionMock, acessarGerarOdfRetrabalhoMock)
  })
  it('Botão "Gerar Odf" deve estar desabilitado', () => {
    cy.get(`${mainSelector} vs-button[type=save] button`).should('be.disabled');
  })
  it('Preencher lote estoque localização origem', () => {
    cy.batchRequestStub(getAllEstoqueLocaisRequest()).then(alias => {
      cy.get(`${mainSelector} rnc-estoque-local-select[formControlName=${GerarOdfRetrabalhoFormControls.idEstoqueLocalOrigem}] vs-button[icon="search"] button`).click()
      cy.wait(alias)
    })
    cy.get('vs-select-modal vs-grid tbody tr').eq(0).click()

  })
  it('Preencher local destino', () => {
    cy.getAutoCompleteItem({
      formControlName:GerarOdfRetrabalhoFormControls.idLocalDestino,
      selector: `${mainSelector} rnc-locais-autocomplete-select`,
      request: getAllLocaisRequest()
    })
  })
  it('Gerar odf retrabalho', () => {
    const gerarOdfRetrabalhoRequest = gerarOrdemRetrabalhoRequest()
    cy.batchRequestStub(gerarOdfRetrabalhoRequest).then(aliases => {
      cy.get(`${mainSelector} vs-button[type=save] button`).click();
      cy.wait(aliases).then((interception: Interception) => {
        const expectedBody = {
          ...gerarOdfRetrabalhoRequest.expectedBody,
          quantidade:1,
          idLocalDestino: ids[0],
          idEstoqueLocalOrigem: ids[0]
        } as OrdemRetrabalhoInput
        cy.validateRequestBody(interception.request.body, expectedBody)
      })
    })
  })
})

describe('Se houver erro ao gerar odf de retrabalho, deve apresentar mensagem de erro', () => {
  it('Navegar até o modal de geração de odf de retrabalho', () => {
    const navigateToUpdateNaoConformidadeUserActionMock = new NavigateToUpdateNaoConformidadeUserActionMock();
    const acessarGerarOdfRetrabalhoMock = new AcessarGerarOdfRetrabalhoMock();
    navigateToUpdateNaoConformidadeUserActionMock.atualizarNaoConformidadeRequestsRestantesMock.getListOrdemProducao?.response.body.items[0].odfFinalizada = true;

    acessarGerarOdfRetrabalho(navigateToUpdateNaoConformidadeUserActionMock, acessarGerarOdfRetrabalhoMock)
  })
  it('Botão "Gerar Odf" deve estar desabilitado', () => {
    cy.get(`${mainSelector} vs-button[type=save] button`).should('be.disabled');
  })
  it('Preencher lote estoque localização origem', () => {
    cy.batchRequestStub(getAllEstoqueLocaisRequest()).then(alias => {
      cy.get(`${mainSelector} rnc-estoque-local-select[formControlName=${GerarOdfRetrabalhoFormControls.idEstoqueLocalOrigem}] vs-button[icon="search"] button`).click()
      cy.wait(alias)
    })
    cy.get('vs-select-modal vs-grid tbody tr').eq(0).click()

  })
  it('Preencher local destino', () => {
    cy.getAutoCompleteItem({
      formControlName:GerarOdfRetrabalhoFormControls.idLocalDestino,
      selector: `${mainSelector} rnc-locais-autocomplete-select`,
      request: getAllLocaisRequest()
    })
  })
  it('Gerar odf retrabalho', () => {
    const gerarOdfRetrabalhoRequest = gerarOrdemRetrabalhoRequest()
    gerarOdfRetrabalhoRequest.response.statusCode = 422;
    gerarOdfRetrabalhoRequest.response.body.message = "Não foi encontrado serviço com operação engenharia 999"
    cy.batchRequestStub([gerarOdfRetrabalhoRequest]).then(aliases => {
      cy.get(`${mainSelector} vs-button[type=save] button`).click();
      cy.wait(aliases)
    })
  })
  it('Verificar mensagem de erro', () => {
    cy.get('vs-message-dialog').contains('Não foi encontrado serviço com operação engenharia 999').should('exist')
  })
})

describe('Se utilizarReservaDePedidoNaLocalizacaoDeEstoque estiver true, deve ser buscado estoque-pedido-venda-estoque-local', () => {
  it('Navegar até o modal de geração de odf de retrabalho', () => {
    const navigateToUpdateNaoConformidadeUserActionMock = new NavigateToUpdateNaoConformidadeUserActionMock();
    const acessarGerarOdfRetrabalhoMock = new AcessarGerarOdfRetrabalhoMock();
    acessarGerarOdfRetrabalhoMock.getAllEstoqueLocaisRequest = getAllEstoquePedidoVendaEstoqueLocaisViewRequest();
    navigateToUpdateNaoConformidadeUserActionMock.atualizarNaoConformidadeRequestsRestantesMock.getListOrdemProducao?.response.body.items[0].odfFinalizada = true;
    navigateToUpdateNaoConformidadeUserActionMock.atualizarNaoConformidadeRequestsIniciaisMock.getConfiguracoesGeraisRequest
      .response.body.utilizarReservaDePedidoNaLocalizacaoDeEstoque = true;

    acessarGerarOdfRetrabalho(navigateToUpdateNaoConformidadeUserActionMock, acessarGerarOdfRetrabalhoMock)
  })
  it('Botão "Gerar Odf" deve estar desabilitado', () => {
    cy.get(`${mainSelector} vs-button[type=save] button`).should('be.disabled');
  })
  it('Preencher lote estoque localização origem', () => {
    cy.batchRequestStub(getAllEstoquePedidoVendaEstoqueLocaisViewRequest()).then(alias => {
      cy.get(`${mainSelector} rnc-estoque-pedido-venda-estoque-local-select[formControlName=${GerarOdfRetrabalhoFormControls.idEstoqueLocalOrigem}] vs-button[icon="search"] button`).click()
      cy.wait(alias)
    })
    cy.get('vs-select-modal vs-grid tbody tr').eq(0).click()

  })
  it('Preencher local destino', () => {
    cy.getAutoCompleteItem({
      formControlName:GerarOdfRetrabalhoFormControls.idLocalDestino,
      selector: `${mainSelector} rnc-locais-autocomplete-select`,
      request: getAllLocaisRequest()
    })
  })
  it('Gerar odf retrabalho', () => {
    const gerarOdfRetrabalhoRequest = gerarOrdemRetrabalhoRequest()
    gerarOdfRetrabalhoRequest.response.statusCode = 422;
    gerarOdfRetrabalhoRequest.response.body.message = "Não foi encontrado serviço com operação engenharia 999"
    cy.batchRequestStub([gerarOdfRetrabalhoRequest]).then(aliases => {
      cy.get(`${mainSelector} vs-button[type=save] button`).click();
      cy.wait(aliases)
    })
  })
})

describe('Se não utilizarReservaDePedidoNaLocalizacaoDeEstoque estiver true, deve ser buscado estoque-local', () => {
  it('Navegar até o modal de geração de odf de retrabalho', () => {
    const navigateToUpdateNaoConformidadeUserActionMock = new NavigateToUpdateNaoConformidadeUserActionMock();
    const acessarGerarOdfRetrabalhoMock = new AcessarGerarOdfRetrabalhoMock();
    navigateToUpdateNaoConformidadeUserActionMock.atualizarNaoConformidadeRequestsRestantesMock.getListOrdemProducao?.response.body.items[0].odfFinalizada = true;
    navigateToUpdateNaoConformidadeUserActionMock.atualizarNaoConformidadeRequestsIniciaisMock.getConfiguracoesGeraisRequest
      .response.body.utilizarReservaDePedidoNaLocalizacaoDeEstoque = false;

    acessarGerarOdfRetrabalho(navigateToUpdateNaoConformidadeUserActionMock, acessarGerarOdfRetrabalhoMock)
  })
  it('Botão "Gerar Odf" deve estar desabilitado', () => {
    cy.get(`${mainSelector} vs-button[type=save] button`).should('be.disabled');
  })
  it('Preencher lote estoque localização origem', () => {
    cy.batchRequestStub(getAllEstoqueLocaisRequest()).then(alias => {
      cy.get(`${mainSelector} rnc-estoque-local-select[formControlName=${GerarOdfRetrabalhoFormControls.idEstoqueLocalOrigem}] vs-button[icon="search"] button`).click()
      cy.wait(alias)
    })
    cy.get('vs-select-modal vs-grid tbody tr').eq(0).click()

  })
  it('Preencher local destino', () => {
    cy.getAutoCompleteItem({
      formControlName:GerarOdfRetrabalhoFormControls.idLocalDestino,
      selector: `${mainSelector} rnc-locais-autocomplete-select`,
      request: getAllLocaisRequest()
    })
  })
  it('Gerar odf retrabalho', () => {
    const gerarOdfRetrabalhoRequest = gerarOrdemRetrabalhoRequest()
    gerarOdfRetrabalhoRequest.response.statusCode = 422;
    gerarOdfRetrabalhoRequest.response.body.message = "Não foi encontrado serviço com operação engenharia 999"
    cy.batchRequestStub([gerarOdfRetrabalhoRequest]).then(aliases => {
      cy.get(`${mainSelector} vs-button[type=save] button`).click();
      cy.wait(aliases)
    })
  })
})

describe('Se houver apenas um estoque, deve preencher informações de estoque local automaticamente', () => {
  it('Navegar até o modal de geração de odf de retrabalho', () => {
    const navigateToUpdateNaoConformidadeUserActionMock = new NavigateToUpdateNaoConformidadeUserActionMock();
    const acessarGerarOdfRetrabalhoMock = new AcessarGerarOdfRetrabalhoMock();
    const getAllEstoqueLocaisMockRequest = getAllEstoqueLocaisRequest();
    getAllEstoqueLocaisMockRequest.response.body = {
      items: [
        {
          id: ids[0],
          codigoLocal: codes[0],
          codigoProduto: strings[0],
          idEmpresa:ids[0],
          idLocal: ids[0],
          idProduto: ids[0],
          legacyIdEmpresa: codes[0],
          lote: codes[0].toString(),
          quantidade: 10,
          codigoArmazem: codes[0].toString(),
          legacyId: 1,
          dataFabricacao: dates[0],
          dataValidade: dates[0],
          numeroAlocacao: codes[0]
        } as EstoqueLocalOutput,
      ],
      totalCount: 1
    }
    acessarGerarOdfRetrabalhoMock.getAllEstoqueLocaisRequest = getAllEstoqueLocaisMockRequest

    const getLocaisRequest = getAllLocaisRequest();
    getLocaisRequest.response.body = {
      items: [
        {
          codigo: codes[0],
          descricao: strings[0],
          id: ids[0],
          isBloquearMovimentacao: true,
        } as LocalOutput,
      ],
      totalCount: 1
    }
    acessarGerarOdfRetrabalhoMock.getLocaisRequest = getLocaisRequest;

    navigateToUpdateNaoConformidadeUserActionMock.atualizarNaoConformidadeRequestsRestantesMock.getListOrdemProducao?.response.body.items[0].odfFinalizada = true;
    navigateToUpdateNaoConformidadeUserActionMock.atualizarNaoConformidadeRequestsIniciaisMock.getConfiguracoesGeraisRequest
      .response.body.utilizarReservaDePedidoNaLocalizacaoDeEstoque = false;

      cy.batchRequestStub(getLocalByIdRequest()).then(alias => {
        acessarGerarOdfRetrabalho(navigateToUpdateNaoConformidadeUserActionMock, acessarGerarOdfRetrabalhoMock)
        cy.wait(alias);
      })

  })

  it('Botão "Gerar Odf" deve estar habilitado', () => {
    cy.get(`${mainSelector} vs-button[type=save] button`).should('be.enabled');
  })

  it('Gerar odf retrabalho', () => {
    const gerarOdfRetrabalhoRequest = gerarOrdemRetrabalhoRequest()
    gerarOdfRetrabalhoRequest.response.statusCode = 422;
    gerarOdfRetrabalhoRequest.response.body.message = "Não foi encontrado serviço com operação engenharia 999"
    cy.batchRequestStub([gerarOdfRetrabalhoRequest]).then(aliases => {
      cy.get(`${mainSelector} vs-button[type=save] button`).click();
      cy.wait(aliases)
    })
  })
})

describe('Se mais de um estoque, não deve preencher informações de estoque local automaticamente', () => {
  it('Navegar até o modal de geração de odf de retrabalho', () => {
    const navigateToUpdateNaoConformidadeUserActionMock = new NavigateToUpdateNaoConformidadeUserActionMock();
    const acessarGerarOdfRetrabalhoMock = new AcessarGerarOdfRetrabalhoMock();
    navigateToUpdateNaoConformidadeUserActionMock.atualizarNaoConformidadeRequestsRestantesMock.getListOrdemProducao?.response.body.items[0].odfFinalizada = true;

    acessarGerarOdfRetrabalho(navigateToUpdateNaoConformidadeUserActionMock, acessarGerarOdfRetrabalhoMock)
  })
  it('Botão "Gerar Odf" deve estar desabilitado', () => {
    cy.get(`${mainSelector} vs-button[type=save] button`).should('be.disabled');
  })
  it('Estoque local deve estar vazio', () => {
    cy.get(`${mainSelector} rnc-estoque-local-select[formControlName=${GerarOdfRetrabalhoFormControls.idEstoqueLocalOrigem}] input`).should('be.empty')
  })
  it('Preencher local destino', () => {
    cy.get(`${mainSelector} rnc-locais-autocomplete-select input`).should('be.empty')
  })
})

