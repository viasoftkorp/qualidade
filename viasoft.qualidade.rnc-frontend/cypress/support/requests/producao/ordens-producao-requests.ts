import { IPagedResultOutputDto } from '@viasoft/common';
import { codes, ids } from 'cypress/support/test-utils';
import { OrdemProducaoOutput } from '../../../../apps/rnc/src/api-clients/Ordem-Producao/model/ordem-producao-output'

export const getListOrdemProducao = () => {
  return {
    url: `qualidade/rnc/gateway/ordens-producao?**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        totalCount: 2,
        items: [
          {
            id: ids[0],
            idPedido: ids[0],
            idProduto: ids[0],
            numeroOdf: codes[0],
            revisao: codes[0].toString(),
            odfFinalizada: false,
            idCliente: ids[0]
          } as OrdemProducaoOutput,
          {
            id: ids[0],
            idPedido: ids[0],
            idProduto: ids[0],
            numeroOdf: codes[0],
            revisao: codes[0].toString(),
            odfFinalizada: false,
            idCliente: ids[1]
          } as OrdemProducaoOutput,
        ],
      } as IPagedResultOutputDto<OrdemProducaoOutput>,
    },
  } as CypressRequestV2<IPagedResultOutputDto<OrdemProducaoOutput>>;
};
