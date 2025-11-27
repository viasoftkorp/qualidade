import { SolucaoOutput } from './../../../../apps/rnc/src/app/pages/shared/solucao-autocomplete-select/solucao-autocomplete-select.component';
import { CausaOutput } from './../../../../apps/rnc/src/app/pages/shared/causa-autocomplete-select/causa-autocomplete-select.component';
import { DefeitoModel } from './../../../../apps/rnc/src/api-clients/Defeitos/model/defeito-model';
import { IPagedResultOutputDto } from '@viasoft/common';
import { ids, strings, codes } from "../../test-utils"
import { DefeitoOutput } from '@viasoft/rnc/api-clients/Defeitos/model/defeito-output';

export const getAllDefeitosRequest = () => {
  return {
    url: `qualidade/rnc/gateway/defeitos?**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        totalCount: 2,
        items: [
          {
            id: ids[0],
            descricao: strings[0],
            codigo: codes[0],
            detalhamento: strings[0],
            idCausa: ids[0],
            idSolucao: ids[0],
          } as DefeitoModel,
          {
            id: ids[1],
            descricao: strings[1],
            codigo: codes[1],
            detalhamento: strings[1],
            idCausa: ids[1],
            idSolucao: ids[1],
          } as DefeitoModel
        ]
      } as IPagedResultOutputDto<DefeitoModel>
    }
  } as CypressRequestV2<IPagedResultOutputDto<DefeitoModel>>
}

export const getDefeitosViewListRequest = () => {
  return {
    url: `qualidade/rnc/gateway/defeitos/view?**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        totalCount: 2,
        items: [
          {
            id: ids[0],
            descricao: strings[0],
            codigo: codes[0],
            detalhamento: strings[0],
            idCausa: ids[0],
            idSolucao: ids[0],
          } as DefeitoOutput,
          {
            id: ids[1],
            descricao: strings[1],
            codigo: codes[1],
            detalhamento: strings[1],
            idCausa: ids[1],
            idSolucao: ids[1],
          } as DefeitoOutput
        ]
      } as IPagedResultOutputDto<DefeitoOutput>
    }
  } as CypressRequestV2<IPagedResultOutputDto<DefeitoOutput>>
}

export const getDefeitoRequestForId = () => {
  return {
    url: `qualidade/rnc/gateway/defeitos/**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        id: ids[0],
        descricao: strings[0],
        codigo: codes[0],
        detalhamento: strings[0],
        idCausa: ids[0],
        idSolucao: ids[0],
      } as DefeitoModel
    }
  } as CypressRequest
}

export const createNewDefeitoRequest = () => {
  return {
    url: `qualidade/rnc/gateway/defeitos`,
    method: 'POST',
    response: {
      statusCode: 200,
    },
    expectedBody: {
      id: '',
      descricao: strings[2],
      detalhamento: strings[2],
      idCausa: ids[0],
      idSolucao: ids[0],
    } as DefeitoModel
  } as CypressRequest
}

export const updateDefeitoRequest = (idDefeito: string = '**') => {
  return {
    url: `qualidade/rnc/gateway/defeitos/${idDefeito}`,
    method: 'PUT',
    response: {
      statusCode: 200
    },
    expectedBody: {
      id: '',
      descricao: strings[3],
      detalhamento: strings[3],
      idCausa: ids[1],
      idSolucao: ids[1],
    } as DefeitoModel
  } as CypressRequest
}
export const deleteDefeitoRequest = (idDefeito: string = '**') => {
  return {
    url: `qualidade/rnc/gateway/defeitos/${idDefeito}`,
    method: 'DELETE',
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
            id: ids[0],
            descricao: strings[0],
            codigo: '1',
          } as CausaOutput,
          {
            id: ids[1],
            descricao: strings[1],
            codigo: '2',
          } as CausaOutput
        ]
      } as IPagedResultOutputDto<CausaOutput>
    }
  } as CypressRequestV2<IPagedResultOutputDto<CausaOutput>>
}
export const getSolucoesList = () => {
  return {
    url: `qualidade/rnc/gateway/solucoes?**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        totalCount: 2,
        items: [
          {
            id: ids[0],
            descricao: strings[0],
            codigo: '1',
          } as SolucaoOutput,
          {
            id: ids[1],
            descricao: strings[1],
            codigo: '2',
          } as SolucaoOutput
        ]
      } as IPagedResultOutputDto<SolucaoOutput>
    }
  } as CypressRequestV2<IPagedResultOutputDto<SolucaoOutput>>
}
