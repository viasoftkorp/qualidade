import { IPagedResultOutputDto } from '@viasoft/common';
import { LocalOutput } from '../../../../apps/rnc/src/api-clients/Locais/model/local-output';
import { codes, ids, strings } from 'cypress/support/test-utils';
export const getAllLocaisRequest = () => {
  return {
    url: `qualidade/rnc/gateway/locais?**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        items: [
          {
            codigo: codes[0],
            descricao: strings[0],
            id: ids[0],
            isBloquearMovimentacao: true,
          } as LocalOutput,
          {
            codigo: codes[1],
            descricao: strings[1],
            id: ids[1],
            isBloquearMovimentacao: true,
          } as LocalOutput,
        ],
        totalCount: 2,
      } as IPagedResultOutputDto<LocalOutput>,
    },
  } as CypressRequestV2<IPagedResultOutputDto<LocalOutput>>;
};

export const getLocalByIdRequest = () => {
  return {
    url: `qualidade/rnc/gateway/locais/**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        codigo: codes[0],
        descricao: strings[0],
        id: ids[0],
        isBloquearMovimentacao: true,
      } as LocalOutput,
    },
  } as CypressRequestV2<LocalOutput>;
};
