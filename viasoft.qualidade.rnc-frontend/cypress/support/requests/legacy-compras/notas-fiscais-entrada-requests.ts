import { IPagedResultOutputDto } from '@viasoft/common';
import { codes, ids, strings } from 'cypress/support/test-utils';
import { NotaFiscalEntradaOutput } from '../../../../apps/rnc/src/api-clients/Nota-Fiscal-Entrada/model';

export const getListNotasFiscaisEntrada = () => {
  return {
    url: `qualidade/rnc/gateway/notas-fiscais-entrada?**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        totalCount: 2,
        items: [
          {
            id: ids[0],
            codigoProduto: codes[0].toString(),
            idProduto: ids[0],
            lote: '1',
            numeroNotaFiscal: 1,
          } as NotaFiscalEntradaOutput,
          {
            id: ids[1],
            codigoProduto: codes[1].toString(),
            idProduto: ids[1],
            lote: '2',
            numeroNotaFiscal: 2,
          } as NotaFiscalEntradaOutput,
        ],
      } as IPagedResultOutputDto<NotaFiscalEntradaOutput>,
    },
  } as CypressRequestV2<IPagedResultOutputDto<NotaFiscalEntradaOutput>>;
};
export const getNotasFiscaisEntradaById = () => {
  return {
    url: `qualidade/rnc/gateway/notas-fiscais-entrada/**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        id: ids[0],
        codigoProduto: codes[0].toString(),
        idProduto: ids[0],
        lote: '1',
        numeroNotaFiscal: 1,
      } as NotaFiscalEntradaOutput,
    },
  } as CypressRequestV2<NotaFiscalEntradaOutput>;
};
