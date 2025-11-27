import {GerarOrdemRetrabalhoValidationResult} from '../../../../../apps/rnc/src/api-clients/Nao-Conformidades/Ordem-Retrabalho/model/gerar-ordem-retrabalho-validation-result'
import { OrdemRetrabalhoOutput } from '../../../../../apps/rnc/src/api-clients/Nao-Conformidades/Ordem-Retrabalho/model/ordem-retrabalho-output'
import { OrdemRetrabalhoInput } from '../../../../../apps/rnc/src/api-clients/Nao-Conformidades/Ordem-Retrabalho/model/ordem-retrabalho-input'
import { codes, dates, ids } from 'cypress/support/test-utils'
export const canGenerateOrdemRetrabalhoRequest = ()  => {
  return {
    url:`/qualidade/rnc/gateway/nao-conformidades/**/retrabalho/ordens/can-generate?**`,
    method:'GET',
    response: {
      statusCode: 200,
      body: GerarOrdemRetrabalhoValidationResult.Ok
    }
  } as CypressRequestV2<GerarOrdemRetrabalhoValidationResult>
}
export const gerarOrdemRetrabalhoRequest = ()  => {
  return {
    url:`/qualidade/rnc/gateway/nao-conformidades/**/retrabalho/ordens`,
    method:'POST',
    response: {
      statusCode: 200,
      body: {
        success: true
      } as OrdemRetrabalhoOutput,
    },
    expectedBody: {

    } as OrdemRetrabalhoInput
  } as CypressRequestV2<OrdemRetrabalhoOutput>
}
export const estornarOrdemRetrabalhoRequest = ()  => {
  return {
    url:`/qualidade/rnc/gateway/nao-conformidades/**/retrabalho/ordens`,
    method:'DELETE',
    response: {
      statusCode: 200,
      body: {
        success: true
      } as OrdemRetrabalhoOutput,
    },
    expectedBody: {

    } as OrdemRetrabalhoInput
  } as CypressRequestV2<OrdemRetrabalhoOutput>
}

export const getOrdemRetrabalhoRequest = ()  => {
  return {
    url:`/qualidade/rnc/gateway/nao-conformidades/**/retrabalho/ordens`,
    method:'GET',
    response: {
      statusCode: 200,
      body: null
    },
    expectedBody: {

    } as OrdemRetrabalhoInput
  } as CypressRequestV2<OrdemRetrabalhoOutput | null>
}
