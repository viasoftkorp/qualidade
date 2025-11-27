import { IPagedResultOutputDto } from "@viasoft/common";
import {EstoqueLocalOutput} from '../../../../apps/rnc/src/api-clients/Estoque-Locais/model/estoque-local-output'
import { codes, dates, ids, strings } from "cypress/support/test-utils";
export const getAllEstoqueLocaisRequest = () => {
  return {
    url: `qualidade/rnc/gateway/estoque-locais?**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
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
          {
            id: ids[1],
            codigoLocal: codes[1],
            codigoProduto: strings[1],
            idEmpresa:ids[1],
            idLocal: ids[1],
            idProduto: ids[1],
            legacyIdEmpresa: codes[1],
            lote: codes[1].toString(),
            quantidade: 11,
            codigoArmazem: codes[1].toString(),
            legacyId: 11,
            dataFabricacao: dates[1],
            dataValidade: dates[1],
            numeroAlocacao: codes[1]
          } as EstoqueLocalOutput,
        ],
        totalCount: 2,
      } as IPagedResultOutputDto<EstoqueLocalOutput>,
    },
  } as CypressRequestV2<IPagedResultOutputDto<EstoqueLocalOutput>>;
};
