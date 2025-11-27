import { codes, ids, strings } from 'cypress/support/test-utils';
import { PedidoVendaOutput } from '../../../../apps/rnc/src/api-clients/Pedido-Venda/model'

export const getPedidoVendaByIdRequest = () => {
  return {
    url: `qualidade/rnc/gateway/pedidos-venda/**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        id: ids[0],
        numeroPedido: strings[0]
      } as PedidoVendaOutput,
    },
  } as CypressRequestV2<PedidoVendaOutput>;
};
