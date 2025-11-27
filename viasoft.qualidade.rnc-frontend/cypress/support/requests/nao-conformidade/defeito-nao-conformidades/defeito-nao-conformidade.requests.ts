import { IPagedResultOutputDto } from "@viasoft/common"
import { DefeitoModel } from "@viasoft/rnc/api-clients/Defeitos/model/defeito-model"
import { DefeitoOutput } from "@viasoft/rnc/api-clients/Defeitos/model/defeito-output"
import { DefeitosNaoConformidadesModel } from "@viasoft/rnc/api-clients/Nao-Conformidades"
import { ids, strings, codes } from "cypress/support/test-utils"

export const getAllDefeitosNaoConformidadeRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/defeitos?**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        totalCount:2,
        items:[
          {
            id:ids[0],
            quantidade: codes[0],
            detalhamento: strings[0],
            idDefeito: ids[0],
            idNaoConformidade:ids[0],
            descricao:strings[0],
            codigo:codes[0]
          } as DefeitosNaoConformidadesModel,
          {
            id:ids[1],
            quantidade: codes[1],
            detalhamento: strings[1],
            idDefeito: ids[1],
            idNaoConformidade:ids[1],
            descricao:strings[1],
            codigo:codes[1]
          } as DefeitosNaoConformidadesModel
        ]
      } as IPagedResultOutputDto<DefeitosNaoConformidadesModel>
    }
  } as CypressRequest
}

export const getDefeitosNaoConformidadeRequestForId = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/defeitos/**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        id:ids[0],
        quantidade: codes[0],
        detalhamento: strings[0],
        idDefeito: ids[0],
        idNaoConformidade:ids[0],
        descricao:strings[0],
        codigo:codes[0]
      } as DefeitosNaoConformidadesModel
    }
  } as CypressRequest
}

export const createNewDefeitosNaoConformidadeRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/defeitos`,
    method:'POST',
    response: {
      statusCode: 200,
    },

    expectedBody: {
        id: '' ,
        detalhamento: strings[2],
        quantidade: codes[2],
        idDefeito: ids[0],
        idNaoConformidade:ids[0],
    } as DefeitosNaoConformidadesModel
  } as CypressRequest
}

export const updateDefeitosNaoConformidadeRequest = () => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/defeitos/**`,
    method:'PUT',
    response: {
      statusCode: 200
    },
    expectedBody: {
        id:'',
        detalhamento: strings[3],
        quantidade: codes[2],
        idDefeito: ids[1],
    } as DefeitosNaoConformidadesModel
  } as CypressRequest
}
export const deleteDefeitosNaoConformidadeRequest = () => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/defeitos/**`,
    method:'DELETE',
    response: {
      statusCode: 200
    },
  } as CypressRequest
}
