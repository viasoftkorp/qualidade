import { IPagedResultOutputDto } from "@viasoft/common"
import { AcoesPreventivasNaoConformidadesModel } from "@viasoft/rnc/api-clients/Nao-Conformidades"
import { ids, strings, codes } from "cypress/support/test-utils"
import { AcaoPreventivaOutput } from '../../../../../apps/rnc/src/api-clients/Nao-Conformidades/Acoes-Preventivas-Nao-Conformidades/model/acao-preventiva-output'

export const getAllAcoesPreventivasNaoConformidadeRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/acoes-preventivas?**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        totalCount:2,
        items:[
          {
            id:ids[0],
            idAcaoPreventiva: ids[0],
            idAuditor: ids[0],
            implementada: false,
            idResponsavel: ids[0],
            dataAnalise: new Date(2022,0,1,0,0,0),
            dataPrevistaImplantacao: new Date(2022,0,1,0,0,0),
            dataVerificacao: new Date(2022,0,1,0,0,0),
            novaData: new Date(2022,0,1,0,0,0),
            detalhamento: strings[0],
            idDefeitoNaoConformidade: ids[0],
            idNaoConformidade:ids[0],
            descricao:strings[0],
            codigo:codes[0]
          } as AcoesPreventivasNaoConformidadesModel,
          {
            id:ids[1],
            idAcaoPreventiva: ids[1],
            idAuditor: ids[1],
            implementada: false,
            idResponsavel: ids[1],
            dataAnalise: new Date(2022,0,1,0,0,0),
            dataPrevistaImplantacao: new Date(2022,0,1,0,0,0),
            dataVerificacao: new Date(2022,0,1,0,0,0),
            novaData: new Date(2022,0,1,0,0,0),
            detalhamento: strings[1],
            idDefeitoNaoConformidade: ids[1],
            idNaoConformidade:ids[1],
            descricao:strings[1],
            codigo:codes[1]
          } as AcoesPreventivasNaoConformidadesModel
        ]
      } as IPagedResultOutputDto<AcoesPreventivasNaoConformidadesModel>
    }
  } as CypressRequest
}

export const getAcoesPreventivasNaoConformidadeRequestForId = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/acoes-preventivas/**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
            id:ids[0],
            idAcaoPreventiva: ids[0],
            idAuditor: ids[0],
            implementada: false,
            idResponsavel: ids[0],
            dataAnalise: new Date(2022,0,1,0,0,0),
            dataPrevistaImplantacao: new Date(2022,0,1,0,0,0),
            dataVerificacao: new Date(2022,0,1,0,0,0),
            novaData: new Date(2022,0,1,0,0,0),
            detalhamento: strings[0],
            idDefeitoNaoConformidade: ids[0],
            idNaoConformidade:ids[0],
            descricao:strings[0],
            codigo:codes[0]
      } as AcoesPreventivasNaoConformidadesModel
    }
  } as CypressRequest
}

export const createNewAcoesPreventivasNaoConformidadeRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/acoes-preventivas`,
    method:'POST',
    response: {
      statusCode: 200,
    },

    expectedBody: {
            id:'',
            idAcaoPreventiva: ids[0],
            idAuditor: ids[0],
            implementada: true,
            idResponsavel: ids[0],
            dataAnalise: new Date(2022,0,1,0,0,0),
            dataPrevistaImplantacao: new Date(2022,0,1,0,0,0),
            dataVerificacao: new Date(2022,0,1,0,0,0),
            novaData: new Date(2022,0,1,0,0,0),
            detalhamento: strings[2],
            idDefeitoNaoConformidade: ids[0],
            idNaoConformidade:ids[0],
    } as AcoesPreventivasNaoConformidadesModel
  } as CypressRequest
}

export const updateAcoesPreventivasNaoConformidadeRequest = () => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/acoes-preventivas/**`,
    method:'PUT',
    response: {
      statusCode: 200
    },
    expectedBody: {
            id:'',
            idAcaoPreventiva: ids[1],
            idAuditor: ids[0],
            implementada: true,
            idResponsavel: ids[0],
            dataAnalise: new Date(2022,0,1,0,0,0),
            dataPrevistaImplantacao: new Date(2022,0,1,0,0,0),
            dataVerificacao: new Date(2022,0,1,0,0,0),
            novaData: new Date(2022,0,1,0,0,0),
            detalhamento: strings[3],
            idDefeitoNaoConformidade: ids[0],
            idNaoConformidade:ids[0],
    } as AcoesPreventivasNaoConformidadesModel
  } as CypressRequest
}
export const deleteAcoesPreventivasNaoConformidadeRequest = () => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/acoes-preventivas/**`,
    method:'DELETE',
    response: {
      statusCode: 200
    },
  } as CypressRequest
}

export const getAcoesPreventivasList = () => {
  return {
    url: `qualidade/rnc/gateway/acoes-preventivas?**`,
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
            detalhamento: strings[0]
          } as AcaoPreventivaOutput,
          {
            id: ids[1],
            descricao: strings[1],
            codigo: '1',
            detalhamento: strings[1]
          } as AcaoPreventivaOutput
        ]
      } as IPagedResultOutputDto<AcaoPreventivaOutput>
    }
  } as CypressRequestV2<IPagedResultOutputDto<AcaoPreventivaOutput>>
}
export const getAcaoPreventivaById = () => {
  return {
    url: `qualidade/rnc/gateway/acoes-preventivas/**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        id: ids[2],
        descricao: strings[0],
        codigo: '1',
      }
    }
  } as CypressRequest
}
