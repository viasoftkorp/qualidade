import { ids, strings } from "cypress/support/test-utils";
import { ConclusoesNaoConformidadesInput, ConclusoesNaoConformidadesOutput } from "@viasoft/rnc/api-clients/Nao-Conformidades"

export const concluirNaoConformidadeRequest = ()  => ({
  url: `qualidade/rnc/gateway/nao-conformidades/**/conclusao`,
  method: 'POST',
  response: {
    statusCode: 200,
  },
  expectedBody: {
    id: '',
    idAuditor: ids[0],
    dataReuniao: new Date(2022,0,1,0,0,0),
    dataVerificacao: new Date(2022,0,1,0,0,0),
    eficaz: true,
    novaReuniao: true,
    evidencia: strings[2],
    idNaoConformidade: ids[0],
  } as ConclusoesNaoConformidadesInput
} as CypressRequest)

export const estornarConclusaoNaoConformidadeRequest = ()  => ({
  url: `qualidade/rnc/gateway/nao-conformidades/**/conclusao`,
  method: 'DELETE',
  response: {
    statusCode: 200,
  }
} as CypressRequest)

export const getConclusaoNaoConformidadeRequest = ()  => ({
  url: `qualidade/rnc/gateway/nao-conformidades/**/conclusao`,
  method: 'GET',
  response: {
    statusCode: 200,
    body: {
      id: ids[0],
      idAuditor: ids[0],
      dataReuniao: new Date(2022,0,1,0,0,0),
      dataVerificacao: new Date(2022,0,1,0,0,0),
      eficaz: true,
      novaReuniao: true,
      evidencia: strings[0],
      idNaoConformidade: ids[0],
      cicloDeTempo:5
    } as ConclusoesNaoConformidadesOutput
  },
} as CypressRequestV2<ConclusoesNaoConformidadesOutput>)

export const calcularCicloTempoRequest = ()  => ({
  url: `qualidade/rnc/gateway/nao-conformidades/**/calcular-ciclo-tempo`,
  method: 'GET',
  response: {
    statusCode: 200,
    body:1
  },
} as CypressRequestV2<number>)
