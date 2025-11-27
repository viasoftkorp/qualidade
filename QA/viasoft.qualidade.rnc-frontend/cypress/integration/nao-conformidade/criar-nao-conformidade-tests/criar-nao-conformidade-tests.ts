import { NaoConformidadesFormControl } from '../../../../apps/rnc/src/app/pages/nao-conformidades/nao-conformidades-editor/nao-conformidades-form-control';
import { codes, ids, strings } from 'cypress/support/test-utils';
import { Interception } from 'cypress/types/net-stubbing';
import {
  getAllNaoConformidadesRequest,
  getNaturezasList,
  getProdutosList,
} from 'cypress/support/requests/nao-conformidade/nao-conformidades.requests';
import { CreateNaoConformidadeMock } from 'cypress/support/mock/criar-nao-conformidade-mock';
import { createNaoConformidade } from 'cypress/support/user-actions/nao-conformidades-user-actions';
import { getClientesList } from 'cypress/support/requests/erp-person/erp-person-requests';
import {
  getListNotasFiscaisEntrada,
} from 'cypress/support/requests/legacy-compras/notas-fiscais-entrada-requests';
import { OrigemNaoConformidades } from '@viasoft/rnc/api-clients/Nao-Conformidades/model/origem-nao-conformidades';
import { NaoConformidadeInput } from '@viasoft/rnc/api-clients/Nao-Conformidades';
import { OrdemProducaoOutput } from '@viasoft/rnc/api-clients/Ordem-Producao/model';
import { getAllPagelessProductsRequest, getProductByIdRequest } from 'cypress/support/requests/logistics-products/products-requests';
import { getListOrdemProducao } from 'cypress/support/requests/producao/ordens-producao-requests';

const mainSelector = 'rnc-nao-conformidades-editor';
describe.skip('Criar nao-conformidade com sucesso', () => {
  it('Visitar tela de nao-conformidade', () => {
    cy.batchRequestStub(getAllNaoConformidadesRequest()).then((alias) => {
      cy.visit('rnc/nao-conformidades');
      cy.wait(alias);
    });
  });
  it('Clicar botão novo', () => {
    cy.get('vs-header vs-button[icon=plus] button').click();
  });
  it('Preencher formulário', () => {
    cy.get("rnc-nao-conformidades-editor vs-button[type='save'] button").should('be.disabled');

    cy.get(`vs-select[formcontrolname=${NaoConformidadesFormControl.origem}]`).click();
    cy.get(`mat-option`).contains('Inspeção de Entrada').click();

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idPessoa,
      request: getClientesList(),
      selector: `${mainSelector} person-person-autocomplete-single[personType='Supplier']`,
    });

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idNotaFiscal,
      request: getListNotasFiscaisEntrada(),
      selector: 'rnc-notas-fiscais-entrada-autocomplete-select',
    });
    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idNatureza,
      request: getNaturezasList(),
      selector: 'qa-natureza-autocomplete-select',
    });

    const getItemPedidoVendaByNumeroOdfRequest = getListOrdemProducao();
    getItemPedidoVendaByNumeroOdfRequest.response.body.items = [
      {
        id: ids[0],
        numeroOdf: codes[0],
      } as OrdemProducaoOutput
    ]
    cy.batchRequestStub(getItemPedidoVendaByNumeroOdfRequest).then(alias => {
      cy.getVsInput(NaoConformidadesFormControl.numeroOdf, `${mainSelector}`)
      .type(codes[0].toString())
      .blur()
      cy.wait(alias)
    })

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idProduto,
      request: getAllPagelessProductsRequest(),
      selector: 'qa-produto-autocomplete-select',
    });

    cy.getVsInput(`${NaoConformidadesFormControl.numeroLote}`).type(`${codes[0]}`);

    cy.get(`vs-datepicker[ng-reflect-control-name=${NaoConformidadesFormControl.dataFabricacaoLote}]`).type(`01012022`);

    cy.getVsInput(`${NaoConformidadesFormControl.campoNf}`).type(`${strings[2]}`);

    cy.getVsInput(`${NaoConformidadesFormControl.revisao}`).type(`${codes[2]}`);
    cy.getVsTextArea(`${NaoConformidadesFormControl.equipe}`).type(`${strings[2]}`);
    cy.getVsTextArea(NaoConformidadesFormControl.descricao).type(`${strings[2]}`);

    cy.getVsCheckbox(NaoConformidadesFormControl.naoConformidadeEmPotencial).click({ force: true });
    cy.getVsCheckbox(NaoConformidadesFormControl.loteTotal).click({ force: true });
    cy.getVsCheckbox(NaoConformidadesFormControl.rejeitado).click({ force: true });
    cy.getVsCheckbox(NaoConformidadesFormControl.retrabalhoNoCliente).click({ force: true });
    cy.getVsCheckbox(NaoConformidadesFormControl.relatoNaoConformidade).click({ force: true });
    cy.getVsCheckbox(NaoConformidadesFormControl.loteParcial).click({ force: true });
    cy.getVsCheckbox(NaoConformidadesFormControl.aceitoConcessao).click({ force: true });
    cy.getVsCheckbox(NaoConformidadesFormControl.retrabalhoPeloCliente).click({ force: true });
    cy.getVsCheckbox(NaoConformidadesFormControl.melhoriaEmPotencial).click({ force: true });

    cy.get("rnc-nao-conformidades-editor vs-button[type='save'] button").should('be.enabled');
  });
  it('Salvar nova nao-conformidade', () => {
    const criarNaoConformidadeMock = new CreateNaoConformidadeMock();

    createNaoConformidade(criarNaoConformidadeMock).then((createInterception:Interception) => {
      const expectedBody = {
        ...criarNaoConformidadeMock.createNaoConformidade.expectedBody,
        id: createInterception.request.body.id,
        origem: OrigemNaoConformidades.InspecaoEntrada,
        idNatureza: ids[2],
        idPessoa: ids[2],
        idProduto: ids[0],
        revisao: codes[2].toString(),
        equipe: strings[2],
        dataFabricacaoLote: createInterception.request.body.dataFabricacaoLote,
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
        idNotaFiscal: ids[0],
        numeroOdf: codes[0].toString(),
      } as NaoConformidadeInput;

      cy.validateRequestStatusCode(createInterception?.response?.statusCode, 200);
      cy.validateRequestBody(createInterception?.request.body, expectedBody);
    });
  });
});

describe.skip('Se preencher numero odf e dar blur no campo, deve preencher campos com base no itemPedidoVenda', () => {
  it('Visitar tela de nao-conformidade', () => {
    cy.batchRequestStub(getAllNaoConformidadesRequest()).then((alias) => {
      cy.visit('rnc/nao-conformidades');
      cy.wait(alias);
    });
  });
  it('Clicar botão novo', () => {
    cy.get('vs-header vs-button[icon=plus] button').click();
  });

  it('Preencher formulário', () => {
    cy.get("rnc-nao-conformidades-editor vs-button[type='save'] button").should('be.disabled');

    cy.get(`vs-select[formcontrolname=${NaoConformidadesFormControl.origem}]`).click();
    cy.get(`mat-option`).contains('Cliente').click();

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idPessoa,
      request: getClientesList(),
      selector: `${mainSelector} person-person-autocomplete-single[personType='Customer']`,
    });

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idNatureza,
      request: getNaturezasList(),
      selector: 'qa-natureza-autocomplete-select',
    });

    const getItemPedidoVendaByNumeroOdfRequest = getListOrdemProducao();
    getItemPedidoVendaByNumeroOdfRequest.response.body.items = [
      {
        id: ids[0],
        numeroOdf: codes[0],
        idPedido: ids[0],
        idProdutoFaturamento: ids[0],
        numeroOdfFaturamento: codes[0],
        idProduto: ids[0],
        revisao: codes[0].toString(),
        numeroLote: codes[0].toString(),
        idCliente: ids[0]
      } as OrdemProducaoOutput
    ]
    cy.batchRequestStub(getItemPedidoVendaByNumeroOdfRequest).then(alias => {
      cy.getVsInput(NaoConformidadesFormControl.numeroOdf, `${mainSelector}`)
      .type(codes[0].toString())
      .blur()
      cy.wait(alias).then(() => {
        cy.batchRequestStub(getProductByIdRequest()).then(alias => {
          cy.wait(alias)
        })
      })
    })

    cy.get("rnc-nao-conformidades-editor vs-button[type='save'] button").should('be.enabled');
  });

  it('Salvar nova nao-conformidade', () => {
    const criarNaoConformidadeMock = new CreateNaoConformidadeMock();

    createNaoConformidade(criarNaoConformidadeMock).then((createInterception:Interception) => {
      const expectedBody = {
        ...criarNaoConformidadeMock.createNaoConformidade.expectedBody,
        id: createInterception.request.body.id,
        origem: OrigemNaoConformidades.Cliente,
        idNatureza: ids[2],
        idPessoa: ids[0],
        idProduto: ids[0],
        revisao: codes[0].toString(),
        dataFabricacaoLote: createInterception.request.body.dataFabricacaoLote,
        numeroLote: codes[0].toString(),
        numeroOdf: codes[0].toString(),
        numeroOdfFaturamento: codes[0],
        idProdutoFaturamento: ids[0],
        idPedido: ids[0]
      } as NaoConformidadeInput;

      cy.validateRequestStatusCode(createInterception?.response?.statusCode, 200);
      cy.validateRequestBody(createInterception?.request.body, expectedBody);
    });
  });
});
