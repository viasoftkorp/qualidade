import { CausaOutput } from './../../../../apps/rnc/src/api-clients/Causas/model/causa-output';
import { CausaModel } from './../../../../apps/rnc/src/api-clients/Causas/model/Causa-model';
import { IPagedResultOutputDto } from '@viasoft/common';
import { ids,strings,codes } from "../../test-utils"

export const getAllCausasRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/causas?**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        totalCount:2,
        items:[
          {
            id:ids[0],
            descricao:strings[0],
            codigo:codes[0]
          } as CausaOutput,
          {
            id:ids[1],
            descricao:strings[1],
            codigo:codes[1]
          } as CausaOutput
        ]
      } as IPagedResultOutputDto<CausaModel>
    }
  } as CypressRequest
}

export const getCausasViewListRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/causas/view?**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        totalCount:2,
        items:[
          {
            id:ids[0],
            descricao:strings[0],
            codigo:codes[0]
          } as CausaOutput,
          {
            id:ids[1],
            descricao:strings[1],
            codigo:codes[1]
          } as CausaOutput
        ]
      } as IPagedResultOutputDto<CausaModel>
    }
  } as CypressRequest
}

export const getCausaRequestForId = (idCausa:string = '**')  => {
  return {
    url:`qualidade/rnc/gateway/causas/${idCausa}`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        id:ids[0],
        descricao:strings[3],
        codigo:codes[0],
        detalhamento:strings[3]

      } as CausaOutput
    }
  } as CypressRequest
}

export const createNewCausaRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/causas`,
    method:'POST',
    response: {
      statusCode: 200,
    },

    expectedBody: {
      id: '' ,
      descricao : strings[2],
      detalhamento:strings[2]
    } as CausaModel
  } as CypressRequest
}

export const updateCausaRequest = (idCausa:string = '**') => {
  return {
    url:`qualidade/rnc/gateway/causas/${idCausa}`,
    method:'PUT',
    response: {
      statusCode: 200
    },
    expectedBody: {
      id:'',
      descricao : strings[3],
      detalhamento:strings[3]
    } as CausaModel
  } as CypressRequest
}
export const deleteCausaRequest = (idCausa:string = '**') => {
  return {
    url:`qualidade/rnc/gateway/causas/${idCausa}`,
    method:'DELETE',
    response: {
      statusCode: 200
    },
  } as CypressRequest
}



