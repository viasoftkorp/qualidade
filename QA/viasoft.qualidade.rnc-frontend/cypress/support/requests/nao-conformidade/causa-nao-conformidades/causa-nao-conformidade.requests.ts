import { IPagedResultOutputDto } from "@viasoft/common"
import { CausaModel } from "@viasoft/rnc/api-clients/Causas/model/Causa-model"
import { CausasNaoConformidadesInput, CausasNaoConformidadesModel } from "@viasoft/rnc/api-clients/Nao-Conformidades"
import { codes, ids, strings } from "cypress/support/test-utils"

export const getAllCausasNaoConformidadeRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/defeitos/**/causas?**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        totalCount:2,
        items:[
          {
            id:ids[0],
            detalhamento: strings[0],
            idCausa: ids[0],
            idDefeitoNaoConformidade:ids[0],
            idNaoConformidade:ids[0],
            descricao:strings[0],
            codigo:codes[0]
          } as CausasNaoConformidadesModel,
          {
            id:ids[1],
            detalhamento: strings[1],
            idCausa: ids[1],
            idDefeitoNaoConformidade:ids[1],
            idNaoConformidade:ids[1],
            descricao:strings[1],
            codigo:codes[1]
          } as CausasNaoConformidadesModel
        ]
      } as IPagedResultOutputDto<CausasNaoConformidadesModel>
    }
  } as CypressRequestV2<IPagedResultOutputDto<CausasNaoConformidadesModel>>
}

export const getCausasNaoConformidadeRequestForId = (idCausa:string = '**')  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/causas/${idCausa}`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        id:ids[0],
        detalhamento: strings[0],
        idCausa: ids[0],
        idDefeitoNaoConformidade:ids[0],
        idNaoConformidade:ids[0],
      } as CausasNaoConformidadesModel
    }
  } as CypressRequest
}

export const createNewCausasNaoConformidadeRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/causas`,
    method:'POST',
    response: {
      statusCode: 200,
    },
    expectedBody: {
        id: '',
        detalhamento: strings[2],
        idCausa: ids[2],
        idDefeitoNaoConformidade:ids[0],
        idNaoConformidade:ids[0],
    } as CausasNaoConformidadesInput
  } as CypressRequest
}

export const updateCausasNaoConformidadeRequest = (idCausa:string = '**') => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/causas/${idCausa}`,
    method:'PUT',
    response: {
      statusCode: 200
    },
    expectedBody: {
        id:'',
        detalhamento: strings[3],
        idCausa: ids[3],
    } as CausasNaoConformidadesModel
  } as CypressRequest
}
export const deleteCausasNaoConformidadeRequest = (idCausa:string = '**') => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/causas/${idCausa}`,
    method:'DELETE',
    response: {
      statusCode: 200
    },
  } as CypressRequest
}
export const getCausasList = () => {
  return {
    url: `qualidade/rnc/gateway/causas?**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        totalCount: 2,
        items: [
          {
            id: ids[2],
            descricao: strings[0],
            codigo: '1',
            detalhamento: strings[0]
          } as CausaModel,
          {
            id: ids[3],
            descricao: strings[1],
            codigo: '2',
            detalhamento: strings[1]
          } as CausaModel
        ]
      } as IPagedResultOutputDto<CausaModel>
    }
  } as CypressRequestV2<IPagedResultOutputDto<CausaModel>>
}
