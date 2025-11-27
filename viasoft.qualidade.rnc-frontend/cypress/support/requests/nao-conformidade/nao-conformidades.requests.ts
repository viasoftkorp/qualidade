import { IPagedResultOutputDto } from '@viasoft/common';
import { codes, dates, ids, strings } from 'cypress/support/test-utils';
import {
  NaoConformidadeInput,
  NaoConformidadeModel,
  NaoConformidadeOutput,
} from '@viasoft/rnc/api-clients/Nao-Conformidades/index';
import { ProdutoOutput } from '@viasoft/rnc/app/pages/shared/produto-autocomplete-select/produto-autocomplete-select.component';
import { NaturezaOutput } from '@viasoft/rnc/api-clients/Naturezas/model/natureza-output';
import { NaturezaModel } from '@viasoft/rnc/api-clients/Naturezas/model/natureza-model';
import { PersonOutput } from '@viasoft/person-lib';
import { OrigemNaoConformidades } from '@viasoft/rnc/api-clients/Nao-Conformidades/model/origem-nao-conformidades';

export const getAllNaoConformidadesRequest = () => {
  return {
    url: `qualidade/rnc/gateway/nao-conformidades?**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        totalCount: 2,
        items: [
          {
            id: ids[0],
            idNaoConformidade: ids[0],
            codigo: codes[0],
            origem: codes[0],
            status: codes[0],
            idNotaFiscal: ids[0],
            numeroNotaFiscal: strings[0],
            idNatureza: ids[0],
            descricaoNatureza: strings[0],
            codigoNatureza: codes[0],
            natureza: strings[0],
            idCliente: ids[0],
            nomeCliente: strings[0],
            codigoCliente: 1,
            cliente: strings[0],
            idOdf: ids[0],
            numeroOdf: strings[0],
            idProduto: ids[0],
            descricaoProduto: strings[0],
            codigoProduto: 1,
            produto: strings[0],
            revisao: codes[0],
            equipe: strings[0],
            idLote: ids[0],
            numeroLote: strings[0],
            dataFabricacaoLote: dates[0],
            campoNf: strings[0],
            idCriador: ids[0],
            usuario: strings[0],
            loteTotal: true,
            loteParcial: true,
            rejeitado: true,
            aceitoConcessao: true,
            retrabalhoPeloCliente: true,
            retrabalhoNoCliente: true,
            naoConformidadeEmPotencial: true,
            relatoNaoConformidade: true,
            melhoriaEmPotencial: true,
            descricao: strings[0],
          } as NaoConformidadeOutput,
          {
            id: ids[1],
            idNaoConformidade: ids[1],
            codigo: codes[1],
            origem: codes[1],
            status: codes[1],
            idNotaFiscal: ids[1],
            numeroNotaFiscal: strings[1],
            idNatureza: ids[1],
            descricaoNatureza: strings[1],
            codigoNatureza: codes[1],
            natureza: strings[1],
            idCliente: ids[1],
            nomeCliente: strings[1],
            codigoCliente: 2,
            cliente: strings[1],
            idOdf: ids[1],
            numeroOdf: strings[1],
            idProduto: ids[1],
            descricaoProduto: strings[1],
            codigoProduto: 2,
            produto: strings[1],
            revisao: codes[1],
            equipe: strings[1],
            idLote: ids[1],
            numeroLote: strings[1],
            dataFabricacaoLote: dates[1],
            campoNf: strings[1],
            idCriador: ids[1],
            usuario: strings[1],
            loteTotal: true,
            loteParcial: true,
            rejeitado: true,
            aceitoConcessao: true,
            retrabalhoPeloCliente: true,
            retrabalhoNoCliente: true,
            naoConformidadeEmPotencial: true,
            relatoNaoConformidade: true,
            melhoriaEmPotencial: true,
            descricao: strings[1],
          } as NaoConformidadeOutput,
        ],
      } as IPagedResultOutputDto<NaoConformidadeModel>,
    },
  } as CypressRequest;
};

export const getNaoConformidadeRequestForId = (idNaoConformidade: string = '**') => {
  return {
    url: `qualidade/rnc/gateway/nao-conformidades/${ids[0]}`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        id: ids[0],
        codigo: codes[0],
        origem: codes[0],
        status: codes[0],
        idNotaFiscal: ids[0],
        numeroNotaFiscal: strings[0],
        idNatureza: ids[0],
        descricaoNatureza: strings[0],
        codigoNatureza: codes[0],
        natureza: strings[0],
        idPessoa: ids[0],
        nomeCliente: strings[0],
        codigoCliente: 1,
        cliente: strings[0],
        idOdf: ids[0],
        numeroOdf: codes[0].toString(),
        idProduto: ids[0],
        descricaoProduto: strings[0],
        codigoProduto: 1,
        produto: strings[0],
        revisao: codes[0],
        equipe: strings[0],
        idLote: ids[0],
        numeroLote: strings[0],
        dataFabricacaoLote: dates[0],
        campoNf: strings[0],
        idCriador: ids[0],
        usuario: strings[0],
        loteTotal: true,
        loteParcial: true,
        rejeitado: true,
        aceitoConcessao: true,
        retrabalhoPeloCliente: true,
        retrabalhoNoCliente: true,
        naoConformidadeEmPotencial: true,
        relatoNaoConformidade: true,
        melhoriaEmPotencial: true,
        descricao: strings[0],
        dataCriacao: dates[0],
        companyId: ids[0]
      } as NaoConformidadeOutput,
    },
  } as CypressRequestV2<NaoConformidadeOutput>;
};

export const createNewNaoConformidadeRequest = () => {
  return {
    url: `qualidade/rnc/gateway/nao-conformidades`,
    method: 'POST',
    response: {
      statusCode: 200,
    } as IPagedResultOutputDto<NaoConformidadeModel>,

    expectedBody: {
      id: '',
      origem: OrigemNaoConformidades.Cliente,
      idNatureza: ids[2],
      idPessoa: ids[2],
      idProduto: ids[0]
    } as NaoConformidadeInput,
  } as CypressRequest;
};
export const updateNaoConformidadeRequest = () => {
  return {
    url: `qualidade/rnc/gateway/nao-conformidades/**`,
    method: 'PUT',
    response: {
      statusCode: 200,
    } as IPagedResultOutputDto<NaoConformidadeModel>,

    expectedBody: {
      id: '',
      origem: codes[2],
      idNatureza: ids[2],
      idPessoa: ids[2],
      idProduto: ids[2],
      revisao: codes[2],
      equipe: strings[2],
      dataFabricacaoLote: new Date(2022, 0, 1, 0, 0, 0),
      campoNf: strings[2],
      loteTotal: true,
      loteParcial: true,
      rejeitado: true,
      aceitoConcessao: true,
      retrabalhoPeloCliente: true,
      retrabalhoNoCliente: true,
      naoConformidadeEmPotencial: true,
      relatoNaoConformidade: true,
      melhoriaEmPotencial: true,
      descricao: strings[2],
      numeroLote: `${codes[0]}`,
      numeroNotaFiscal: `${codes[0]}`,
      numeroOdf: `${codes[0]}`,
    } as NaoConformidadeInput,
  } as CypressRequest;
};

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
          } as ProdutoOutput,
          {
            id: ids[1],
            descricao: strings[1],
            codigo: '2',
          } as ProdutoOutput,
        ],
      } as IPagedResultOutputDto<ProdutoOutput>,
    },
  } as CypressRequestV2<IPagedResultOutputDto<ProdutoOutput>>;
};

export const getNaturezasList = () => {
  return {
    url: `qualidade/rnc/gateway/naturezas?**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        totalCount: 2,
        items: [
          {
            id: ids[2],
            descricao: strings[0],
            codigo: '1',
          } as NaturezaModel,
          {
            id: ids[3],
            descricao: strings[1],
            codigo: '2',
          } as NaturezaModel,
        ],
      } as IPagedResultOutputDto<NaturezaModel>,
    },
  } as CypressRequestV2<IPagedResultOutputDto<NaturezaModel>>;
};
export const getNaturezaById = () => {
  return {
    url: `qualidade/rnc/gateway/naturezas/**`,
    method: 'GET',
    response: {
      statusCode: 200,
      body: {
        id: ids[2],
        descricao: strings[0],
        codigo: '1',
      } as NaturezaModel,
    },
  } as CypressRequestV2<NaturezaModel>;
};
