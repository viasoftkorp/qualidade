import { IPagedResultOutputDto } from '@viasoft/common';
import { PersonOutput } from '@viasoft/person-lib';
import { strings, ids } from 'cypress/support/test-utils';

export const getClientesList = () => {
  return {
    url: 'api/ERP/Person/Person/GetAll?**',
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        totalCount: 2,
        items: [
          {
            tradingName: strings[0],
            id: ids[2],
            code: '1',
            companyName: strings[0],
          } as PersonOutput,
          { tradingName: strings[1], id: ids[3], code: '2', companyName: strings[1] } as PersonOutput,
        ],
      } as IPagedResultOutputDto<PersonOutput>,
    },
  } as CypressRequestV2<IPagedResultOutputDto<PersonOutput>>;
};

export const getClienteById = () => {
  return {
    url: 'api/ERP/Person/Person/GetById/**',
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        tradingName: strings[0],
        id: ids[2],
        code: '1',
        companyName: strings[0],
      } as PersonOutput,
    },
  } as CypressRequestV2<PersonOutput>;
};
