import { codes, ids, strings } from 'cypress/support/test-utils'
import { CentroCustoOutput } from '../../../../apps/rnc/src/api-clients/Centros-Custo/model'
import { IPagedResultOutputDto } from '@viasoft/common'
export const getAllCentrosCustoRequest = ()  => {
  return {
    url:`/qualidade/rnc/gateway/centros-custo?**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        totalCount:2,
        items:[
          {
            id:ids[0],
            descricao:strings[0],
            codigo: codes[0].toString(),
            isSintetico: false
          } as CentroCustoOutput,
          {
            id:ids[1],
            descricao:strings[1],
            codigo: codes[1].toString(),
            isSintetico: false
          } as CentroCustoOutput
        ]
      } as IPagedResultOutputDto<CentroCustoOutput>
    }
  } as CypressRequestV2<IPagedResultOutputDto<CentroCustoOutput>>
}
