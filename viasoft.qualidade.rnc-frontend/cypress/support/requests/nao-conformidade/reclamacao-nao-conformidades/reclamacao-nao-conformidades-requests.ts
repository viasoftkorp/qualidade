import { codes, ids, strings } from "cypress/support/test-utils";
import { ReclamacoesNaoConformidadesInput, ReclamacoesNaoConformidadesOutput } from "@viasoft/rnc/api-clients/Nao-Conformidades"

export const createReclamacaoNaoConformidadeRequest = ()  => ({
  url: `qualidade/rnc/gateway/nao-conformidades/**/reclamacao`,
  method: 'POST',
  response: {
    statusCode: 200,
  },
  expectedBody: {
    id: ids[0],
    procedentes: codes[2],
    improcedentes: codes[2],
    quantidadeLote: codes[2],
    quantidadeNaoConformidade: codes[2],
    disposicaoProdutosAprovados: codes[2],
    disposicaoProdutosConcessao: codes[2],
    rejeitado: codes[2],
    retrabalho: codes[2],
    retrabalhoComOnus: true,
    retrabalhoSemOnus: true,
    devolucaoFornecedor: true,
    recodificar: true,
    sucata: true,
    observacao: strings[2],
    idNaoConformidade: ids[0],
  } as ReclamacoesNaoConformidadesInput
} as CypressRequestV2<ReclamacoesNaoConformidadesInput>)

export const updateReclamacaoNaoConformidadeRequest = ()  => ({
  url: `qualidade/rnc/gateway/nao-conformidades/**/reclamacao`,
  method: 'PUT',
  response: {
    statusCode: 200,
  },
  expectedBody: {
    id: '',
    procedentes: codes[3],
    improcedentes: codes[3],
    quantidadeLote: codes[3],
    quantidadeNaoConformidade: codes[3],
    disposicaoProdutosAprovados: codes[3],
    disposicaoProdutosConcessao: codes[3],
    rejeitado: codes[3],
    retrabalho: codes[3],
    retrabalhoComOnus: true,
    retrabalhoSemOnus: true,
    devolucaoFornecedor: true,
    recodificar: true,
    sucata: true,
    observacao: strings[3],
    idNaoConformidade: ids[0],
  } as ReclamacoesNaoConformidadesInput
} as CypressRequest)


export const getReclamacaoNaoConformidadeRequest = ()  => ({
  url: `qualidade/rnc/gateway/nao-conformidades/**/reclamacao`,
  method: 'GET',
  response: {
    statusCode: 200,
  },
  body: {
    id: ids[0],
    procedentes: codes[0],
    improcedentes: codes[0],
    quantidadeLote: codes[0],
    quantidadeNaoConformidade: codes[0],
    disposicaoProdutosAprovados: codes[0],
    disposicaoProdutosConcessao: codes[0],
    rejeitado: codes[0],
    retrabalho: codes[0],
    retrabalhoComOnus: true,
    retrabalhoSemOnus: true,
    devolucaoFornecedor: true,
    recodificar: true,
    sucata: true,
    observacao: strings[0],
    idNaoConformidade: ids[0],
  } as ReclamacoesNaoConformidadesOutput
} as CypressRequest)
