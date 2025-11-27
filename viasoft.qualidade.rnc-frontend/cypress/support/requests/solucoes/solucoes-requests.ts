import { RecursoOutput } from './../../../../apps/rnc/src/app/pages/shared/recurso-autocomplete-select/recurso-autocomplete-select.component';
import { ProdutoOutput } from './../../../../apps/rnc/src/app/pages/shared/produto-autocomplete-select/produto-autocomplete-select.component';
import { SolucaoServicoModel } from './../../../../apps/rnc/src/api-clients/Solucoes/model/solucao-servico-model';
import { SolucaoProdutoOutput } from './../../../../apps/rnc/src/api-clients/Solucoes/model/solucao-produto-output';
import { SolucaoProdutoModel } from '@viasoft/rnc/api-clients/Solucoes/model/solucao-produto-model';
import { SolucaoModel } from './../../../../apps/rnc/src/api-clients/Solucoes/model/solucao-model';
import { SolucaoOutput } from './../../../../apps/rnc/src/api-clients/Solucoes/model/solucao-output';
import { IPagedResultOutputDto } from '@viasoft/common';
import { ids, strings, codes } from "../../test-utils"

export const getAllSolucoesRequest = () => {
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
            codigo: codes[0],
            detalhamento: strings[1],
            imediata: true
          } as SolucaoOutput,
          {
            id: ids[1],
            descricao: strings[1],
            codigo: codes[1],
            detalhamento: strings[2],
            imediata: false
          } as SolucaoOutput
        ]
      } as IPagedResultOutputDto<SolucaoModel>
    }
  } as CypressRequestV2<IPagedResultOutputDto<SolucaoModel>>
}

export const getSolucoesViewListRequest = () => {
  return {
    url: `qualidade/rnc/gateway/solucoes/view?**`,
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
            detalhamento: strings[1],
            imediata: true
          } as SolucaoOutput,
          {
            id: ids[1],
            descricao: strings[1],
            codigo: codes[1],
            detalhamento: strings[2],
            imediata: false
          } as SolucaoOutput
        ]
      } as IPagedResultOutputDto<SolucaoModel>
    }
  } as CypressRequestV2<IPagedResultOutputDto<SolucaoModel>>
}

export const getSolucaoRequestForId = (idSolucao: string = '**') => {
  return {
    url: `qualidade/rnc/gateway/solucoes/${idSolucao}`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        id: ids[0],
        descricao: strings[0],
        codigo: codes[0],
        detalhamento: strings[1],
        imediata: true

      } as SolucaoOutput
    }
  } as CypressRequest
}

export const createNewSolucaoRequest = () => {
  return {
    url: `qualidade/rnc/gateway/solucoes`,
    method: 'POST',
    response: {
      statusCode: 200,
    },

    expectedBody: {
      id: '',
      descricao: strings[2],
      detalhamento: strings[2],
      imediata: true
    } as SolucaoModel
  } as CypressRequest
}

export const updateSolucaoRequest = (idSolucao: string = '**') => {
  return {
    url: `qualidade/rnc/gateway/solucoes/${idSolucao}`,
    method: 'PUT',
    response: {
      statusCode: 200
    },
    expectedBody: {
      id: '',
      descricao: strings[3],
      detalhamento: strings[3],
      imediata: false
    } as SolucaoModel
  } as CypressRequest
}

export const deleteSolucaoRequest = (idSolucao: string = '**') => {
  return {
    url: `qualidade/rnc/gateway/solucoes/${idSolucao}`,
    method: 'DELETE',
    response: {
      statusCode: 200
    },
  } as CypressRequest
}

export const getAllProdutosSolucaoView = (idSolucao: string = ids[0]) => {
  return {
    url: `qualidade/rnc/gateway/solucoes/${idSolucao}/produtos?**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        totalCount: 2,
        items: [
          {
            id: ids[0],
            idProduto: ids[0],
            idSolucao: ids[0],
            descricao: strings[0],
            codigo: '1',
            quantidade: codes[0],
            unidadeMedida: 'UN'
          } as SolucaoProdutoModel,
          {
            id: ids[1],
            idProduto: ids[1],
            idSolucao: ids[1],
            descricao: strings[1],
            codigo: '2',
            quantidade: codes[2],
            unidadeMedida: 'UN'
          } as SolucaoProdutoModel
        ]
      } as IPagedResultOutputDto<SolucaoProdutoModel>
    }
  } as CypressRequest
}

export const getAllServicosSolucaoView = (idSolucao: string = ids[0]) => {
  return {
    url: `qualidade/rnc/gateway/solucoes/${idSolucao}/servicos?**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        totalCount: 2,
        items: [
          {
            id: ids[0],
            idProduto: ids[0],
            idRecurso: ids[0],
            codigoRecurso: '1',
            descricaoRecurso: strings[0],
            horas: codes[0],
            minutos: codes[0],
            operacaoEngenharia: strings[0],
            idSolucao: ids[0],
            descricao: strings[0],
            codigo: '1',
            quantidade: codes[0],
          } as SolucaoServicoModel,
          {
            id: ids[1],
            idProduto: ids[1],
            idRecurso: ids[1],
            codigoRecurso: '2',
            descricaoRecurso: strings[1],
            horas: codes[1],
            minutos: codes[1],
            operacaoEngenharia: strings[1],
            idSolucao: ids[1],
            descricao: strings[1],
            codigo: '2',
            quantidade: codes[2],
          } as SolucaoServicoModel
        ]
      } as IPagedResultOutputDto<SolucaoServicoModel>
    }
  } as CypressRequest
}

export const getProdutosSolucaoViewById = (idSolucao: string = '**', id: string = '**') => {
  return {
    url: `qualidade/rnc/gateway/solucoes/${idSolucao}/produtos/${id}`,
    method: 'GET',
    response: {
      statusCode: 200,
    },
    expectedBody: {
      id: '',
      idProduto: ids[0],
      idSolucao: ids[0],
      descricao: strings[0],
      quantidade: codes[0],
      unidadeMedida: 'UN'
    } as SolucaoProdutoModel
  } as CypressRequest
}

export const getServicosSolucaoViewById = (idSolucao: string = ids[0], idServicoSolucao: string = '**') => {
  return {
    url: `qualidade/rnc/gateway/solucoes/${idSolucao}/produtos/${idServicoSolucao}`,
    method: 'GET',
    response: {
      statusCode: 200,
    },
    expectedBody: {
      idServicoSolucao: '',
      idProduto: ids[0],
      idRecurso: ids[0],
      codigoRecurso: '1',
      descricaoRecurso: strings[0],
      horas: codes[0],
      minutos: codes[0],
      operacaoEngenharia: strings[0],
      idSolucao: ids[0],
      descricao: strings[0],
      quantidade: codes[0],
    } as SolucaoServicoModel
  } as CypressRequest
}

export const createProdutoSolucaoRequest = (idSolucao: string = ids[0]) => {
  return {
    url: `qualidade/rnc/gateway/solucoes/${idSolucao}/produtos`,
    method: 'POST',
    response: {
      statusCode: 200,
    },
    expectedBody: {
      id: '',
      idProduto: ids[0],
      idSolucao: ids[0],
      quantidade: codes[2],
    } as SolucaoProdutoModel
  } as CypressRequest
}

export const updateProdutoSolucaoRequest = (idSolucao: string = ids[0], id: string = '**') => {
  return {
    url: `qualidade/rnc/gateway/solucoes/${idSolucao}/produtos/${id}`,
    method: 'PUT',
    response: {
      statusCode: 200
    },
    expectedBody: {
      id: '',
      idProduto: ids[1],
      idSolucao: ids[0],
      quantidade: codes[0],
    } as SolucaoProdutoModel
  } as CypressRequest
}

export const deleteProdutoSolucaoRequest = (idSolucao: string = ids[0], id: string = '**') => {
  return {
    url: `qualidade/rnc/gateway/solucoes/${idSolucao}/produtos/${id}`,
    method: 'DELETE',
    response: {
      statusCode: 200
    },
  } as CypressRequest
}

export const createServicoSolucaoRequest = (idSolucao: string = ids[0]) => {
  return {
    url: `qualidade/rnc/gateway/solucoes/${idSolucao}/servicos`,
    method: 'POST',
    response: {
      statusCode: 200,
    },
    expectedBody: {
      idServicoSolucao: '',
      idRecurso: ids[0] || null,
      horas: codes[2],
      minutos: codes[2],
      operacaoEngenharia: strings[2],
      idSolucao: ids[0],
    } as SolucaoServicoModel,
  } as CypressRequest
}

export const updateServicoSolucaoRequest = (idSolucao: string = ids[0], idServicoSolucao: string = '**') => {
  return {
    url: `qualidade/rnc/gateway/solucoes/${idSolucao}/servicos/${idServicoSolucao}`,
    method: 'PUT',
    response: {
      statusCode: 200
    },
    expectedBody: {
      id: ids[0],
      idRecurso: ids[1],
      horas: codes[3],
      minutos: codes[3],
      operacaoEngenharia: strings[3],
      idSolucao: ids[0],
    } as SolucaoServicoModel,
  } as CypressRequest
}

export const deleteServicoSolucaoRequest = (idSolucao: string = ids[0], idServicoSolucao: string = '**') => {
  return {
    url: `qualidade/rnc/gateway/solucoes/${idSolucao}/servicos/${idServicoSolucao}`,
    method: 'DELETE',
    response: {
      statusCode: 200
    },
  } as CypressRequest
}
export const getProdutosList = () => {
  return {
    url: `qualidade/rnc/gateway/produtos?**`,
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
            idCategoria: ids[0],
          } as ProdutoOutput,
          {
            id: ids[1],
            descricao: strings[1],
            codigo: '2',
            idCategoria: ids[1],
          } as ProdutoOutput
        ]
      } as IPagedResultOutputDto<ProdutoOutput>
    }
  } as CypressRequestV2<IPagedResultOutputDto<ProdutoOutput>>

}

export const getRecursosList = () => {
  return {
    url: `qualidade/rnc/gateway/recursos?**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        totalCount: 2,
        items: [
          {
            id: ids[0],
            descricao: strings[0],
            codigo: codes[0].toString(),
          } as RecursoOutput,
          {
            id: ids[1],
            descricao: strings[1],
            codigo: codes[1].toString(),
          } as RecursoOutput,
          {
            id: ids[2],
            descricao: strings[2],
            codigo: codes[2].toString(),
          } as RecursoOutput,
          {
            id: ids[3],
            descricao: strings[3],
            codigo: codes[3].toString(),
          } as RecursoOutput,
          {
            id: ids[4],
            descricao: strings[4],
            codigo: codes[4].toString(),
          } as RecursoOutput,
          {
            id: ids[5],
            descricao: strings[5],
            codigo: codes[5].toString(),
          } as RecursoOutput,
          {
            id: ids[6],
            descricao: strings[6],
            codigo: codes[6].toString(),
          } as RecursoOutput,
          {
            id: ids[7],
            descricao: strings[7],
            codigo: codes[7].toString(),
          } as RecursoOutput,
          {
            id: ids[8],
            descricao: strings[8],
            codigo: codes[8].toString(),
          } as RecursoOutput,
          {
            id: ids[9],
            descricao: strings[9],
            codigo: codes[9].toString(),
          } as RecursoOutput
        ]
      } as IPagedResultOutputDto<RecursoOutput>
    }
  } as CypressRequestV2<IPagedResultOutputDto<RecursoOutput>>
}
export const getRecursoById = () => {
  return {
    url: `qualidade/rnc/gateway/recursos/**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        id: ids[0],
        descricao: strings[0],
        codigo: '1',
      } as RecursoOutput
    }
  } as CypressRequest
}
