import { codes, dates, ids } from 'cypress/support/test-utils';
import { EstoquePedidoVendaEstoqueLocalViewOutput } from '../../../../apps/rnc/src/api-clients/Estoque-Pedido-Venda-Estoque-Locais/model/estoque-pedido-venda-estoque-local-view-output'
import { IPagedResultOutputDto } from '@viasoft/common';

export const getAllEstoquePedidoVendaEstoqueLocaisViewRequest = () => {
  return {
    url: `qualidade/rnc/gateway/estoque-pedido-venda-estoque-local-views?**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        items: [
          {
            id: ids[0],
            legacyId: codes[0],
            idEstoquePedidoVenda: ids[0],
            idEstoqueLocal: ids[0],
            isLocalBloquearMovimentacao: false,
            numeroLote: codes[0].toString(),
            codigoLocal: codes[0],
            dataFabricacao: dates[0],
            dataValidade: dates[0],
            idProduto: ids[0],
            idPedido: ids[0],
          } as EstoquePedidoVendaEstoqueLocalViewOutput,
          {
            id: ids[1],
            legacyId: codes[1],
            idEstoquePedidoVenda: ids[1],
            idEstoqueLocal: ids[1],
            isLocalBloquearMovimentacao: false,
            numeroLote: codes[1].toString(),
            codigoLocal: codes[1],
            dataFabricacao: dates[1],
            dataValidade: dates[1],
            idProduto: ids[1],
            idPedido: ids[1],
          } as EstoquePedidoVendaEstoqueLocalViewOutput,
        ],
        totalCount: 2,
      } as IPagedResultOutputDto<EstoquePedidoVendaEstoqueLocalViewOutput>,
    },
  } as CypressRequestV2<IPagedResultOutputDto<EstoquePedidoVendaEstoqueLocalViewOutput>>;
};
