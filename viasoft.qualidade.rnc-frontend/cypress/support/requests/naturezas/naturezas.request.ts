import { NaturezaOutput } from '@viasoft/rnc/api-clients/Naturezas/model/natureza-output';
import { NaturezaModel } from './../../../../apps/rnc/src/api-clients/Naturezas/model/natureza-model';
import { IPagedResultOutputDto } from '@viasoft/common';
import { ids,strings,codes } from "../../test-utils"

export const getAllNaturezasRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/naturezas?**`,
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
          } as NaturezaOutput,
          {
            id:ids[1],
            descricao:strings[1],
            codigo:codes[1]
          } as NaturezaOutput
        ]
      } as IPagedResultOutputDto<NaturezaModel>
    }
  } as CypressRequest
}

export const getNaturezasViewListRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/naturezas/view?**`,
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
          } as NaturezaOutput,
          {
            id:ids[1],
            descricao:strings[1],
            codigo:codes[1]
          } as NaturezaOutput
        ]
      } as IPagedResultOutputDto<NaturezaModel>
    }
  } as CypressRequest
}

export const getNaturezaRequestForId = (idNatureza:string = '**')  => {
  return {
    url:`qualidade/rnc/gateway/naturezas/${idNatureza}`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        id:ids[0],
        descricao:strings[0],
        codigo: codes[0]

      } as NaturezaOutput
    }
  } as CypressRequest

}

export const createNewNaturezaRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/naturezas`,
    method:'POST',
    response: {
      statusCode: 200,

    } as IPagedResultOutputDto<NaturezaModel>,

    expectedBody: {

      id:'',
      descricao : strings[2],

    } as NaturezaModel
  } as CypressRequest
}

export const updateNaturezaRequest = (idNatureza:string = '**') => {
  return {
    url:`qualidade/rnc/gateway/naturezas/${idNatureza}`,
    method:'PUT',
    response: {
      statusCode: 200
    } as IPagedResultOutputDto<NaturezaModel>,
    expectedBody: {
      descricao : strings[3]
    } as NaturezaModel
  } as CypressRequest
}
export const deleteNaturezaRequest = (idNatureza:string = '**') => {
  return {
    url:`qualidade/rnc/gateway/naturezas/${idNatureza}`,
    method:'DELETE',
    response: {
      statusCode: 200
    } as IPagedResultOutputDto<NaturezaModel>,
  } as CypressRequest
}



