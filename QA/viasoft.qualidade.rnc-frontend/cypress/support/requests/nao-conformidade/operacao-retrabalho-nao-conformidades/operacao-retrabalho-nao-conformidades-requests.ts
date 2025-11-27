import { codes, ids, strings } from 'cypress/support/test-utils';
import { OperacaoViewOutput } from '../../../../../apps/rnc/src/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/operacao-view-output';
import { OperacaoRetrabalhoOutput } from '../../../../../apps/rnc/src/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/operacao-retrabalho-output';
import { OperacaoRetrabalhoInput } from '../../../../../apps/rnc/src/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/operacao-retrabalho-input';

import { IPagedResultOutputDto } from '@viasoft/common'
import {
  OperacaoSaldoOutput
} from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/operacao-saldo-output';
import {
  OperacaoOutput
} from '@viasoft/rnc/app/pages/shared/operacao-autocomplete-select/operacao-autocomplete-select.component';
export const getAllOperacoesView = ()  => {
  return {
    url:`/qualidade/rnc/gateway/nao-conformidades/**/retrabalho/operacoes/**/operacoes?**`,
    method:'GET',
    response: {
      statusCode: 200,
      body: {
        items: [
          {
            descricaoRecurso: strings[0],
            id: ids[0],
            idOperacaoRetrabalhoNaoConformidade: ids[0],
            idRecurso: ids[0],
            numeroOperacao: '010'
          },
          {
            descricaoRecurso: strings[1],
            id: ids[1],
            idOperacaoRetrabalhoNaoConformidade: ids[1],
            idRecurso: ids[1],
            numeroOperacao: '020'
          }
        ],
        totalCount: 2
      }
    }
  } as CypressRequestV2<IPagedResultOutputDto<OperacaoViewOutput>>
}

export const getOperacaoRetrabalhoRequest = ()  => {
  return {
    url:`/qualidade/rnc/gateway/nao-conformidades/**/retrabalho/operacoes`,
    method:'GET',
    response: {
      statusCode: 200,
      body: null
    }
  } as CypressRequestV2<OperacaoRetrabalhoOutput | null>
}

export const createOperacaoRetrabalhoRequest = ()  => {
  return {
    url:`/qualidade/rnc/gateway/nao-conformidades/**/retrabalho/operacoes`,
    method:'POST',
    response: {
      statusCode: 200,
      body: {
        id: ids[0],
        idNaoConformidade: ids[0],
        numeroOdf: codes[0].toString(),
        numeroOperacaoARetrabalhar: codes[0].toString(),
        quantidade: 10,
        message: '',
        success: true,
        operacoes: [],
      }
    },
    expectedBody: {
      numeroOperacaoARetrabalhar: '010',
      quantidade: '10',
      maquinas: []
    } as OperacaoRetrabalhoInput
  } as CypressRequestV2<OperacaoRetrabalhoOutput>
}

export const getOperacoesRequest = ()  => {
  return {
    url:`/qualidade/rnc/gateway/ordens-producao/operacoes?**`,
    method:'GET',
    response: {
      statusCode: 200,
      body: {
        items: [
          {
            idOperacao: 1320,
            operacao: '010',
            maquina: '01',
            descricaoMaquina: 'Maquina Teste',
          },
          {
            idOperacao: 1321,
            operacao: '020',
            maquina: '02',
            descricaoMaquina: strings[1],
          }
        ]
      }
    }
  } as CypressRequestV2<IPagedResultOutputDto<OperacaoOutput>>
}

export const getSaldoOperacaoRequest = ()  => {
  return {
    url:`/qualidade/rnc/gateway/operacoes/**`,
    method:'GET',
    response: {
      statusCode: 200,
      body: {
        saldoUnidadePadrao: 0,
        quantidadeOperacao999UnidadePadrao: 0,
        quantidadeProduzidaOperacao999UnidadePadrao: 0,
        saldoOperacaoToleranciaMaximoUnidadePadrao: 0,
        saldoOperacaoToleranciaMinimoUnidadePadrao: 0,
        quantidadeMaximaEncerrarOdfOperacao999UnidadePadrao: 0,
        quantidadeMinimaEncerrarOdfOperacao999UnidadePadrao: 0,
        saldo: 5,
        saldoOperacaoToleranciaMaximo: 0,
        saldoOperacaoToleranciaMinimo: 0,
        quantidadeOperacao999: 0,
        quantidadeMaximaEncerrarOdfOperacao999: 0,
        quantidadeMinimaEncerrarOdfOperacao999: 0,
        quantidadeProduzidaOperacao999: 0,
        primeiraOperacaoOdf: false,
        divideNaConversao: false,
        tolerancia: 0,
        unidade: '',
        fator: 0,
        quantidadeProduzidaOpSecundaria: 15,
      }
    }
  } as CypressRequestV2<OperacaoSaldoOutput>
}
