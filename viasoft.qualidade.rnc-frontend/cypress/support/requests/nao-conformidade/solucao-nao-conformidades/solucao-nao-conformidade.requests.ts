import { IPagedResultOutputDto } from "@viasoft/common"
import { SolucoesNaoConformidadesInput, SolucoesNaoConformidadesModel } from "@viasoft/rnc/api-clients/Nao-Conformidades"
import { ids, strings, codes } from "cypress/support/test-utils"

export const getAllSolucoesNaoConformidadeRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/solucoes?**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        totalCount:2,
        items:[
          {
            id:ids[0],
            auditor: strings[0],
            idAuditor: ids[0],
            responsavel: strings[0],
            idResponsavel: ids[0],
            solucaoImediata: false,
            idDefeitoNaoConformidade: ids[0],
            dataAnalise: new Date(2022,0,1,0,0,0),
            custoEstimado: codes[0],
            codigo: codes[0],
            descricao: strings[0],
            idSolucao: ids[0],
            dataPrevistaImplantacao: new Date(2022,0,1,0,0,0),
            dataVerificacao: new Date(2022,0,1,0,0,0),
            detalhamento: strings[0],
            novaData: new Date(2022,0,1,0,0,0),
            idNaoConformidade:ids[0],
          } as SolucoesNaoConformidadesModel,
          {
            id:ids[1],
            auditor: strings[1],
            idAuditor: ids[1],
            responsavel: strings[1],
            idResponsavel: ids[1],
            solucaoImediata: false,
            idDefeitoNaoConformidade: ids[1],
            dataAnalise: new Date(2022,0,1,0,0,0),
            custoEstimado: codes[1],
            codigo: codes[1],
            descricao: strings[1],
            idSolucao: ids[1],
            dataPrevistaImplantacao: new Date(2022,0,1,0,0,0),
            dataVerificacao: new Date(2022,0,1,0,0,0),
            detalhamento: strings[1],
            novaData: new Date(2022,0,1,0,0,0),
            idNaoConformidade:ids[1],
          } as SolucoesNaoConformidadesModel
        ]
      } as IPagedResultOutputDto<SolucoesNaoConformidadesModel>
    }
  } as CypressRequest
}

export const getSolucoesNaoConformidadeRequestForId = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/solucoes/**`,
    method:'GET',
    response: {
      statusCode: 200,
      body:{
        id:ids[0],
        auditor: strings[0],
        idAuditor: ids[0],
        responsavel: strings[0],
        idResponsavel: ids[0],
        solucaoImediata: true,
        idDefeitoNaoConformidade: ids[0],
        dataAnalise: new Date(2022,0,1,0,0,0),
        custoEstimado: codes[0],
        codigo: codes[0],
        descricao: strings[0],
        idSolucao: ids[0],
        dataPrevistaImplantacao: new Date(2022,0,1,0,0,0),
        dataVerificacao: new Date(2022,0,1,0,0,0),
        detalhamento: strings[0],
        novaData: new Date(2022,0,1,0,0,0),
        idNaoConformidade:ids[0],
      } as SolucoesNaoConformidadesModel
    }
  } as CypressRequest
}

export const createNewSolucoesNaoConformidadeRequest = ()  => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/solucoes`,
    method:'POST',
    response: {
      statusCode: 200,
    },
    expectedBody: {
            id:'',
            idAuditor: ids[0],
            idResponsavel: ids[0],
            solucaoImediata: true,
            idDefeitoNaoConformidade: ids[0],
            dataAnalise: new Date(2022,0,1,0,0,0),
            custoEstimado: codes[2],
            idSolucao: ids[1],
            dataPrevistaImplantacao: new Date(2022,0,1,0,0,0),
            dataVerificacao: new Date(2022,0,1,0,0,0),
            detalhamento: strings[2],
            novaData: new Date(2022,0,1,0,0,0),
            idNaoConformidade:ids[0],
    } as SolucoesNaoConformidadesInput
  } as CypressRequest
}

export const updateSolucoesNaoConformidadeRequest = () => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/solucoes/**`,
    method:'PUT',
    response: {
      statusCode: 200
    },
    expectedBody: {
            id:'',
            idAuditor: ids[0],
            idResponsavel: ids[0],
            solucaoImediata: true,
            idDefeitoNaoConformidade: ids[0],
            dataAnalise: new Date(2022,0,1,0,0,0),
            custoEstimado: codes[3],
            idSolucao: ids[1],
            dataPrevistaImplantacao: new Date(2022,0,1,0,0,0),
            dataVerificacao: new Date(2022,0,1,0,0,0),
            detalhamento: strings[3],
            novaData: new Date(2022,0,1,0,0,0),
            idNaoConformidade:ids[0],
    } as SolucoesNaoConformidadesInput
  } as CypressRequest
}
export const deleteSolucoesNaoConformidadeRequest = () => {
  return {
    url:`qualidade/rnc/gateway/nao-conformidades/**/solucoes/**`,
    method:'DELETE',
    response: {
      statusCode: 200
    },
  } as CypressRequest
}

export const getUsersList = () => {
  return {
   url: 'qualidade/rnc/gateway/authentication/users?**',
  method: 'GET',
  response: {
    statusCode: 200,
  },
  body: {
    totalCount: 2,
    items: [
      {
      id: ids[0],
      firstName: 'Admin',
      secondName: 'Admin',
      email: 'admin@korp.com.br',
      login: "admin",
      isActive: true,
      },
      {
      id: ids[1],
      firstName: 'Admin',
      secondName: 'User',
      email: 'admin.user@korp.com.br',
      login: "admin.user",
      isActive: true,
      }
    ]
  },
} as CypressRequest
}
