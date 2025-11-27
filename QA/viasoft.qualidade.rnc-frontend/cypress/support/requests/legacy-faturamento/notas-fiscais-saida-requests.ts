import { IPagedResultOutputDto } from "@viasoft/common";
import { ids } from "cypress/support/test-utils";
import { NotaFiscalSaidaOutput } from '../../../../apps/rnc/src/api-clients/Nota-Fiscal-Saida/model';

export const getListNotasFiscaisSaida = () => {
  return {
    url: `qualidade/rnc/gateway/notas-fiscais-saida?**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        totalCount: 2,
        items: [
          {
            id: ids[0],
            numeroNotaFiscal: 1
          } as NotaFiscalSaidaOutput,
          {
            id: ids[1],
            numeroNotaFiscal: 2
          } as NotaFiscalSaidaOutput,
        ],
      } as IPagedResultOutputDto<NotaFiscalSaidaOutput>,
    },
  } as CypressRequestV2<IPagedResultOutputDto<NotaFiscalSaidaOutput>>;
};
export const getNotasFiscaisSaidaById = () => {
  return {
    url: `qualidade/rnc/gateway/notas-fiscais-saida/**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        id: ids[0],
        numeroNotaFiscal: 1
      } as NotaFiscalSaidaOutput,
    },
  } as CypressRequestV2<NotaFiscalSaidaOutput>;
};
