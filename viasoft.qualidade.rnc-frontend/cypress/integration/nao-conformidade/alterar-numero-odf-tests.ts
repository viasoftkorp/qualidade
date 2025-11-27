import { NaoConformidadeInput } from "@viasoft/rnc/api-clients/Nao-Conformidades";
import { OrigemNaoConformidades } from "@viasoft/rnc/api-clients/Nao-Conformidades/model/origem-nao-conformidades";
import { OrdemProducaoOutput } from "@viasoft/rnc/api-clients/Ordem-Producao/model";
import { NaoConformidadesFormControl } from "@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/nao-conformidades-form-control";
import { AtualizarNaoConformidadeMock } from "cypress/support/mock/atualizar-nao-conformidade-mock";
import { NavigateToUpdateNaoConformidadeUserActionMock } from "cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock";
import { getPedidoVendaByIdRequest } from "cypress/support/requests/legacy-vendas/pedido-venda-requests";
import { getProductByIdRequest } from "cypress/support/requests/logistics-products/products-requests";
import { getOperacaoRetrabalhoRequest } from "cypress/support/requests/nao-conformidade/operacao-retrabalho-nao-conformidades/operacao-retrabalho-nao-conformidades-requests";
import { getOrdemRetrabalhoRequest } from "cypress/support/requests/nao-conformidade/ordem-retrabalho-nao-conformidades/ordem-retrabalho-nao-conformidades";
import { getListOrdemProducao } from "cypress/support/requests/producao/ordens-producao-requests";
import { ids, codes, strings } from "cypress/support/test-utils";
import { navigateToNaoConformidadeEditor, atualizarNaoConformidade } from "cypress/support/user-actions/nao-conformidades-user-actions";
import { Interception } from "cypress/types/net-stubbing";

const rncSelector = 'rnc-nao-conformidades-editor';

describe('Se numero odf alterado, deve preencher campos com base no itemPedidoVenda', () => {
  it('Navegar para Nao Conformidade editor', () => {
    const naoConformidadeRequests = new NavigateToUpdateNaoConformidadeUserActionMock();
    naoConformidadeRequests.atualizarNaoConformidadeRequestsIniciaisMock.getConfiguracoesGeraisRequest
      .response.body.utilizarReservaDePedidoNaLocalizacaoDeEstoque = true

    navigateToNaoConformidadeEditor(naoConformidadeRequests);
  });

  it('Alterar número odf', () => {
    const getItemPedidoVendaByNumeroOdfRequest = getListOrdemProducao();
    getItemPedidoVendaByNumeroOdfRequest.response.body.items = [
      {
        id: ids[0],
        numeroOdf: codes[0],
        idProdutoFaturamento: ids[0],
        numeroOdfFaturamento: codes[0],
        idProduto: ids[0],
        revisao: codes[0].toString(),
        numeroLote: codes[0].toString(),
        idCliente: ids[1],
        numeroPedido: strings[0]
      } as OrdemProducaoOutput
    ]

    cy.getVsInput(NaoConformidadesFormControl.numeroOdf, `${rncSelector}`)
    .clear()
    .type(codes[0].toString())

    cy.batchRequestStub(getItemPedidoVendaByNumeroOdfRequest).then(alias => {
      cy.getVsInput(NaoConformidadesFormControl.numeroOdf, `${rncSelector}`)
      .blur()
      cy.wait(alias)
    })
  });

  it('Verificar aba "Pedido Venda"', () => {
    cy.batchRequestStub([
      getProductByIdRequest()
    ]).then(aliases => {
      cy.getVsTabGroupItem('Pedido Venda').click()
      cy.wait(aliases)
    })
    const pedidoVendaTabGroupSelector = 'rnc-pedido-venda-nao-conformidades'
    cy.getVsInput(NaoConformidadesFormControl.numeroPedido).should('have.value', strings[0])
    cy.get(`${pedidoVendaTabGroupSelector} qa-produto-autocomplete-select input`).should('have.value', `${codes[0]} - ${strings[0]}`)
    cy.getVsInput(NaoConformidadesFormControl.numeroOdfFaturamento).should('have.value', codes[0])
  })

  it('Salvar nao-conformidade', () => {
    const atualizarNaoConformidadeMock = new AtualizarNaoConformidadeMock();

    atualizarNaoConformidade(atualizarNaoConformidadeMock).then((updateInterception:Interception) => {
      const expectedBody = {
        origem: OrigemNaoConformidades.Interno,
        id: ids[0],
        idNatureza: ids[0],
        idPessoa: ids[1],
        idProduto: ids[0],
        revisao: codes[0].toString(),
        dataFabricacaoLote: updateInterception.request.body.dataFabricacaoLote,
        campoNf: strings[0],
        numeroLote: codes[0].toString(),
        numeroOdf: codes[0],
        numeroOdfFaturamento: codes[0],
        idProdutoFaturamento: ids[0],
        numeroPedido: strings[0]
      } as NaoConformidadeInput;

      cy.validateRequestStatusCode(updateInterception?.response?.statusCode, 200);
      cy.validateRequestBody(updateInterception?.request.body, expectedBody);
    });
  });
});
