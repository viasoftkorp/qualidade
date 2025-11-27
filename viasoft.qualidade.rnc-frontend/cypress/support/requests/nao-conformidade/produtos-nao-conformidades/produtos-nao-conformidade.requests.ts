import { IPagedResultOutputDto } from "@viasoft/common"
import {ProdutosNaoConformidadesInput, ProdutosNaoConformidadesModel, ProdutosNaoConformidadesOutput } from '../../../../../apps/rnc/src/api-clients/Nao-Conformidades/Produtos-Nao-Conformidades/model'
import { codes, ids, strings } from "cypress/support/test-utils"

export const getAllProdutosNaoConformidadesRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/produtos?**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        totalCount:2,
        items:[
          {
            id:ids[0],
            detalhamento: strings[0],
            idProduto: ids[0],
            idSolucaoNaoConformidade:ids[0],
            idNaoConformidade:ids[0],
            descricao:strings[0],
            codigo:strings[0],
            unidadeMedida: strings[0],
            quantidade: codes[0]
          } as ProdutosNaoConformidadesOutput,
          {
            id:ids[1],
            detalhamento: strings[1],
            idProduto: ids[1],
            idSolucaoNaoConformidade:ids[1],
            idNaoConformidade:ids[1],
            descricao:strings[1],
            codigo:strings[1],
            unidadeMedida: strings[1],
            quantidade: codes[1]
          } as ProdutosNaoConformidadesOutput
        ] as Array<ProdutosNaoConformidadesOutput>,
      } as IPagedResultOutputDto<ProdutosNaoConformidadesOutput>
    }
  } as CypressRequest
}

export const getProdutosNaoConformidadeRequestForId = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/produtos/**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        id:ids[0],
        detalhamento: strings[0],
        idProduto: ids[0],
        idSolucaoNaoConformidade:ids[0],
        idNaoConformidade:ids[0],
        descricao:strings[0],
        codigo:strings[0],
        unidadeMedida: strings[0],
        quantidade: codes[0]
      } as ProdutosNaoConformidadesModel
    }
  } as CypressRequest
}

export const createNewProdutoNaoConformidadeRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/produtos`,
    method:'POST',
    response: {
      statusCode: 200,
    },
    expectedBody: {
        id: '',
        detalhamento: strings[2],
        idNaoConformidade:ids[0],
        idProduto:ids[0],
        quantidade: codes[2]
    } as ProdutosNaoConformidadesInput
  } as CypressRequest
}

export const updateProdutosSolucoesNaoConformidadeRequest = () => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/produtos/**`,
    method:'PUT',
    response: {
      statusCode: 200
    },
    expectedBody: {
        id: '',
        detalhamento: strings[3],
        idProduto:ids[1],
        quantidade: codes[3]
    } as ProdutosNaoConformidadesInput
  } as CypressRequest
}
export const deleteProdutosSolucoesNaoConformidadeRequest = () => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/produtos/**`,
    method:'DELETE',
    response: {
      statusCode: 200
    },
  } as CypressRequest
}
