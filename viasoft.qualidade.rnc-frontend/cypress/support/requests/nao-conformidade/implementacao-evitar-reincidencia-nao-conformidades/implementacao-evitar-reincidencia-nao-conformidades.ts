import { IPagedResultOutputDto } from '@viasoft/common'
import { ids, strings } from 'cypress/support/test-utils'
import { ImplementacaoEvitarReincidenciaNaoConformidadesModel } from '../../../../../apps/rnc/src/api-clients/Nao-Conformidades/Implementacao-Evitar-Reincidencia-Nao-Conformidades/model/implementacao-evitar-reincidencia-nao-conformidades-model'
import { ImplementacaoEvitarReincidenciaNaoConformidadesInput } from '../../../../../apps/rnc/src/api-clients/Nao-Conformidades/Implementacao-Evitar-Reincidencia-Nao-Conformidades/model/implementacao-evitar-reincidencia-nao-conformidades-input'
export const getAllImplementacoesEvitarReincidenciaNaoConformidadeRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/implementacao-evitar-reincidencias?**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        totalCount:2,
        items:[
          {
            id:ids[0],
            idDefeitoNaoConformidade: ids[0],
            idNaoConformidade:ids[0],
            descricao:strings[0],
          } as ImplementacaoEvitarReincidenciaNaoConformidadesModel,
          {
            id:ids[1],
            idDefeitoNaoConformidade: ids[1],
            idNaoConformidade:ids[1],
            descricao:strings[1],
          } as ImplementacaoEvitarReincidenciaNaoConformidadesModel
        ]
      } as IPagedResultOutputDto<ImplementacaoEvitarReincidenciaNaoConformidadesModel>
    }
  } as CypressRequestV2<ImplementacaoEvitarReincidenciaNaoConformidadesModel>
}

export const createImplementacoesEvitarReincidenciaNaoConformidadeRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/implementacao-evitar-reincidencias`,
    method:'POST',
    response: {
      statusCode: 200,
      body: {}
    },
    expectedBody: {

    } as ImplementacaoEvitarReincidenciaNaoConformidadesInput
  } as CypressRequestV2<ImplementacaoEvitarReincidenciaNaoConformidadesModel>
}
export const updateImplementacoesEvitarReincidenciaNaoConformidadeRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/implementacao-evitar-reincidencias/**`,
    method:'PUT',
    response: {
      statusCode: 200,
      body: {}
    },
    expectedBody: {

    } as ImplementacaoEvitarReincidenciaNaoConformidadesInput
  } as CypressRequestV2<ImplementacaoEvitarReincidenciaNaoConformidadesModel>
}
export const deleteImplementacoesEvitarReincidenciaNaoConformidadeRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/implementacao-evitar-reincidencias/**`,
    method:'DELETE',
    response: {
      statusCode: 200,
      body: {}
    },
    expectedBody: {

    } as ImplementacaoEvitarReincidenciaNaoConformidadesInput
  } as CypressRequestV2<ImplementacaoEvitarReincidenciaNaoConformidadesModel>
}
