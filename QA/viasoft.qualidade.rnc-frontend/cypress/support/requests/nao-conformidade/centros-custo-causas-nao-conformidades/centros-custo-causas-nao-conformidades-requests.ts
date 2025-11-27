import { IPagedResultOutputDto } from "@viasoft/common"
import { ids } from "cypress/support/test-utils"
import { CentroCustoCausaNaoConformidadeOutput } from '../../../../../apps/rnc/src/api-clients/Nao-Conformidades/Centros-Custo-Causas-Nao-Conformidades/model/centro-custo-causa-nao-conformidade-output'
export const getAllCentrosCustoCausaNaoConformidadeRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/centros-custo`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        totalCount:2,
        items:[
          {
            id:ids[0],
            idCentroCusto: ids[0],
            idNaoConformidade: ids[0]
          } as CentroCustoCausaNaoConformidadeOutput,
          {
            id:ids[1],
            idCentroCusto: ids[1],
            idNaoConformidade: ids[1]

          } as CentroCustoCausaNaoConformidadeOutput
        ]
      } as IPagedResultOutputDto<CentroCustoCausaNaoConformidadeOutput>
    }
  } as CypressRequestV2<IPagedResultOutputDto<CentroCustoCausaNaoConformidadeOutput>>
}
