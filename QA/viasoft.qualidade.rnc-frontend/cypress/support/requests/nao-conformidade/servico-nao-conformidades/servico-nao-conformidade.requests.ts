import { IPagedResultOutputDto } from "@viasoft/common"
import { ServicosNaoConformidadesInput, ServicosNaoConformidadesModel, ServicosNaoConformidadesOutput } from "@viasoft/rnc/api-clients/Nao-Conformidades"
import { ServicoValidationResult } from '../../../../../apps/rnc/src/api-clients/Nao-Conformidades/Servicos-Solucoes-Nao-Conformidades/model/serviceValidationResult'
import { ids, strings, codes } from "cypress/support/test-utils"

export const getAllServicosNaoConformidadeRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/servicos?**`,
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
            idRecurso:ids[0],
            idNaoConformidade:ids[0],
            descricao:strings[0],
            codigo:strings[0],
            descricaoRecurso: strings[0],
            horas: codes[0],
            minutos: codes[0],
            operacaoEngenharia: strings[0],
            quantidade: codes[0],
          } as ServicosNaoConformidadesOutput,
          {
            id:ids[1],
            detalhamento: strings[1],
            idProduto: ids[1],
            idSolucaoNaoConformidade:ids[1],
            idRecurso:ids[1],
            idNaoConformidade:ids[1],
            descricao:strings[1],
            codigo:strings[1],
            descricaoRecurso: strings[1],
            horas: codes[1],
            minutos: codes[1],
            operacaoEngenharia: strings[1],
            quantidade: codes[1],
          } as ServicosNaoConformidadesOutput
        ]
      } as IPagedResultOutputDto<ServicosNaoConformidadesOutput>
    }
  } as CypressRequest
}

export const getServicosNaoConformidadeRequestForId = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/servicos/**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        id:ids[0],
        detalhamento: strings[0],
        idProduto: ids[0],
        idRecurso: ids[0],
        idNaoConformidade:ids[0],
        horas: codes[0],
        minutos: codes[0],
        operacaoEngenharia: strings[0],
        quantidade: codes[0]
      } as ServicosNaoConformidadesOutput
    }
  } as CypressRequest
}

export const createNewServicosNaoConformidadeRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/servicos`,
    method:'POST',
    response: {
      statusCode: 200,
      body: ServicoValidationResult.Ok
    },
    expectedBody: {
        id:'',
        detalhamento: strings[2],
        idRecurso: ids[0],
        idNaoConformidade:ids[0],
        horas: codes[2],
        minutos: codes[2],
        operacaoEngenharia: strings[2],
    } as ServicosNaoConformidadesInput
  } as CypressRequestV2<ServicoValidationResult>
}

export const updateServicosNaoConformidadeRequest = () => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/servicos/**`,
    method:'PUT',
    response: {
      statusCode: 200,
      body: ServicoValidationResult.Ok
    },
    expectedBody: {
        id:'',
        detalhamento: strings[3],
        idRecurso: ids[1],
        horas: codes[3],
        minutos: codes[3],
        operacaoEngenharia: strings[3],
    } as ServicosNaoConformidadesModel
  } as CypressRequestV2<ServicoValidationResult>
}
export const deleteServicosNaoConformidadeRequest = () => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/servicos/**`,
    method:'DELETE',
    response: {
      statusCode: 200
    },
  } as CypressRequest
}
