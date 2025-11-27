import { NaoConformidadesFormControl } from '../../../../apps/rnc/src/app/pages/nao-conformidades/nao-conformidades-editor/nao-conformidades-form-control';
import { codes, ids } from 'cypress/support/test-utils';
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
import { getListNotasFiscaisSaida } from 'cypress/support/requests/legacy-faturamento/notas-fiscais-saida-requests';
import { getListOrdemProducao } from 'cypress/support/requests/producao/ordens-producao-requests';
import { getAllPagelessProductsRequest } from 'cypress/support/requests/logistics-products/products-requests';

const mainSelector = 'rnc-nao-conformidades-editor';
describe.skip('Origem cliente', () => {
  it('Visitar tela de nao-conformidade', () => {
    cy.batchRequestStub(getAllNaoConformidadesRequest()).then((alias) => {
      cy.visit('rnc/nao-conformidades');
      cy.wait(alias);
    });
  });
  it('Clicar botão novo', () => {
    cy.get('vs-header vs-button[icon=plus] button').click();
  });

  it('Selecionar origem cliente', () => {
    cy.get(`vs-select[formcontrolname=${NaoConformidadesFormControl.origem}]`).click();
    cy.get(`mat-option`).contains('Cliente').click();
    botaoSalvarDeveEstarDesabilitado();
  });

  it('Preencher campos estritamente obrigatórios', () => {
    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idNatureza,
      request: getNaturezasList(),
      selector: 'qa-natureza-autocomplete-select',
    });

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idProduto,
      request: getAllPagelessProductsRequest(),
      selector: 'qa-produto-autocomplete-select',
    });
    botaoSalvarDeveEstarDesabilitado();
  });
  it('Preencher pessoa e verificar se botão habilita', () => {
    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idPessoa,
      request: getClientesList(),
      selector: `${mainSelector} person-person-autocomplete-single[personType='Customer']`,
    });

    botaoSalvarDeveEstarHabilitado();
  });
  it('Salvar nova nao-conformidade', () => {
    const criarNaoConformidadeMock = new CreateNaoConformidadeMock();

    createNaoConformidade(criarNaoConformidadeMock).then((createInterception:Interception) => {

      criarNaoConformidadeMock.createNaoConformidade.expectedBody.id = createInterception.request.body.id;
      createInterception.request.body.revisao = Number.parseInt(createInterception.request.body.revisao);
      createInterception.request.body.dataFabricacaoLote = new Date(createInterception.request.body.dataFabricacaoLote);
      cy.validateRequestStatusCode(createInterception?.response?.statusCode, 200);
      cy.validateRequestBody(createInterception?.request.body, criarNaoConformidadeMock.createNaoConformidade.expectedBody);
    });
  });
});

describe.skip('Origem interno', () => {
  it('Visitar tela de nao-conformidade', () => {
    cy.batchRequestStub(getAllNaoConformidadesRequest()).then((alias) => {
      cy.visit('rnc/nao-conformidades');
      cy.wait(alias);
    });
  });
  it('Clicar botão novo', () => {
    cy.get('vs-header vs-button[icon=plus] button').click();
  });

  it('Selecionar origem interno', () => {
    cy.get(`vs-select[formcontrolname=${NaoConformidadesFormControl.origem}]`).click();
    cy.get(`mat-option`).contains('Interno').click();
    botaoSalvarDeveEstarDesabilitado();
  });

  it('Preencher campos estritamente obrigatórios', () => {
    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idNatureza,
      request: getNaturezasList(),
      selector: 'qa-natureza-autocomplete-select',
    });

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idProduto,
      request: getAllPagelessProductsRequest(),
      selector: 'qa-produto-autocomplete-select',
    });
    botaoSalvarDeveEstarHabilitado();
  });
  it('Salvar nova nao-conformidade', () => {
    const criarNaoConformidadeMock = new CreateNaoConformidadeMock();

    createNaoConformidade(criarNaoConformidadeMock).then((createInterception:Interception) => {
      const expectedResult = {
        ...criarNaoConformidadeMock.createNaoConformidade.expectedBody,
        id: createInterception.request.body.id,
        origem: OrigemNaoConformidades.Interno,
      } as NaoConformidadeInput;
      delete expectedResult.idPessoa;
      cy.validateRequestStatusCode(createInterception?.response?.statusCode, 200);
      cy.validateRequestBody(createInterception?.request.body, expectedResult);
    });
  });
});

describe.skip('Origem inspeção de entrada', () => {
  it('Visitar tela de nao-conformidade', () => {
    cy.batchRequestStub(getAllNaoConformidadesRequest()).then((alias) => {
      cy.visit('rnc/nao-conformidades');
      cy.wait(alias);
    });
  });
  it('Clicar botão novo', () => {
    cy.get('vs-header vs-button[icon=plus] button').click();
  });

  it('Selecionar origem inspeção de entrada', () => {
    cy.get(`vs-select[formcontrolname=${NaoConformidadesFormControl.origem}]`).click();
    cy.get(`mat-option`).contains('Inspeção de Entrada').click();
    botaoSalvarDeveEstarDesabilitado();
  });

  it('Preencher campos estritamente obrigatórios', () => {
    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idNatureza,
      request: getNaturezasList(),
      selector: 'qa-natureza-autocomplete-select',
    });

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idProduto,
      request: getAllPagelessProductsRequest(),
      selector: 'qa-produto-autocomplete-select',
    });
    botaoSalvarDeveEstarDesabilitado();
  });

  it('Preencher fornecedor e verificar se botão continua desabilitado', () => {
    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idPessoa,
      request: getClientesList(),
      selector: `${mainSelector} person-person-autocomplete-single[personType='Supplier']`,
    });

    botaoSalvarDeveEstarDesabilitado();
  });

  it('Preencher nota fiscal e verificar se botão habilitou', () => {
    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idNotaFiscal,
      request: getListNotasFiscaisEntrada(),
      selector: 'rnc-notas-fiscais-entrada-autocomplete-select',
    });
    botaoSalvarDeveEstarHabilitado();
  });

  it('Salvar nova nao-conformidade', () => {
    const criarNaoConformidadeMock = new CreateNaoConformidadeMock();

    createNaoConformidade(criarNaoConformidadeMock).then((createInterception:Interception) => {
      const expectedResult = {
        ...criarNaoConformidadeMock.createNaoConformidade.expectedBody,
        id: createInterception.request.body.id,
        origem: OrigemNaoConformidades.InspecaoEntrada,
        idNotaFiscal: ids[0],
      } as NaoConformidadeInput;
      cy.validateRequestStatusCode(createInterception?.response?.statusCode, 200);
      cy.validateRequestBody(createInterception?.request.body, expectedResult);
    });
  });
});
describe.skip('Origem inspeção de saída', () => {
  it('Visitar tela de nao-conformidade', () => {
    cy.batchRequestStub(getAllNaoConformidadesRequest()).then((alias) => {
      cy.visit('rnc/nao-conformidades');
      cy.wait(alias);
    });
  });
  it('Clicar botão novo', () => {
    cy.get('vs-header vs-button[icon=plus] button').click();
  });

  it('Selecionar origem inspeção de saida', () => {
    cy.get(`vs-select[formcontrolname=${NaoConformidadesFormControl.origem}]`).click();
    cy.get(`mat-option`).contains('Inspeção de Saída').click();
    botaoSalvarDeveEstarDesabilitado();
  });

  it('Preencher campos estritamente obrigatórios', () => {
    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idNatureza,
      request: getNaturezasList(),
      selector: 'qa-natureza-autocomplete-select',
    });

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idProduto,
      request: getAllPagelessProductsRequest(),
      selector: 'qa-produto-autocomplete-select',
    });
    botaoSalvarDeveEstarDesabilitado();
  });

  it('Preencher cliente e verificar se botão continua desabilitado', () => {
    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idPessoa,
      request: getClientesList(),
      selector: `${mainSelector} person-person-autocomplete-single[personType='Customer']`,
    });

    botaoSalvarDeveEstarDesabilitado();
  });

  it('Preencher odf e verificar se botão habilitou', () => {
    const getListOrdemProducaoRequest = getListOrdemProducao();
    getListOrdemProducaoRequest.response.body.items[0].idCliente = ids[2]
    cy.batchRequestStub(getListOrdemProducaoRequest).then(alias => {
      cy.getVsInput(NaoConformidadesFormControl.numeroOdf, `${mainSelector}`)
      .type(codes[0].toString())
      .blur()
      cy.wait(alias)
    })

    botaoSalvarDeveEstarHabilitado();
  });

  it('Salvar nova nao-conformidade', () => {
    const criarNaoConformidadeMock = new CreateNaoConformidadeMock();

    createNaoConformidade(criarNaoConformidadeMock).then((createInterception:Interception) => {
      const expectedResult = {
        ...criarNaoConformidadeMock.createNaoConformidade.expectedBody,
        id: createInterception.request.body.id,
        origem: OrigemNaoConformidades.InspecaoSaida,
        numeroOdf: codes[0].toString(),
      } as NaoConformidadeInput;
      cy.validateRequestStatusCode(createInterception?.response?.statusCode, 200);
      cy.validateRequestBody(createInterception?.request.body, expectedResult);
    });
  });
});

describe.skip('Ao trocar origem para inspeção de entrada deve zerar fornecedor', () => {
  it('Visitar tela de nao-conformidade', () => {
    cy.batchRequestStub(getAllNaoConformidadesRequest()).then((alias) => {
      cy.visit('rnc/nao-conformidades');
      cy.wait(alias);
    });
  });
  it('Clicar botão novo', () => {
    cy.get('vs-header vs-button[icon=plus] button').click();
  });

  it('Selecionar origem cliente', () => {
    cy.get(`vs-select[formcontrolname=${NaoConformidadesFormControl.origem}]`).click();
    cy.get(`mat-option`).contains('Cliente').click();
  });

  it('Preencher campos obrigatórios', () => {
    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idNatureza,
      request: getNaturezasList(),
      selector: 'qa-natureza-autocomplete-select',
    });

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idProduto,
      request: getAllPagelessProductsRequest(),
      selector: 'qa-produto-autocomplete-select',
    });

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idPessoa,
      request: getClientesList(),
      selector: `${mainSelector} person-person-autocomplete-single[personType='Customer']`,
    });

    botaoSalvarDeveEstarHabilitado();
  });

  it('Selecionar origem inspeção de entrada', () => {
    cy.get(`vs-select[formcontrolname=${NaoConformidadesFormControl.origem}]`).click();
    cy.get(`mat-option`).contains('Inspeção de Entrada').click();
  });
  it('Verificar se fornecedor está vazio', () => {
    cy.get(`${mainSelector} person-person-autocomplete-single[formControlName='idPessoa'][personType='Supplier'] input`).should('be.empty')
  })
})

describe.skip('Se origem igual a entrada, nota fiscal deve ser buscada das notas de entrada, se não, deve ser buscada das notas de saida', () => {
  it('Visitar tela de nao-conformidade', () => {
    cy.batchRequestStub(getAllNaoConformidadesRequest()).then((alias) => {
      cy.visit('rnc/nao-conformidades');
      cy.wait(alias);
    });
  });
  it('Clicar botão novo', () => {
    cy.get('vs-header vs-button[icon=plus] button').click();
  });

  it('Selecionar origem cliente', () => {
    cy.get(`vs-select[formcontrolname=${NaoConformidadesFormControl.origem}]`).click();
    cy.get(`mat-option`).contains('Cliente').click();
  });

  it('Preencher campos obrigatórios', () => {
    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idNatureza,
      request: getNaturezasList(),
      selector: 'qa-natureza-autocomplete-select',
    });

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idProduto,
      request: getAllPagelessProductsRequest(),
      selector: 'qa-produto-autocomplete-select',
    });

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idPessoa,
      request: getClientesList(),
      selector: `${mainSelector} person-person-autocomplete-single[personType='Customer']`,
    });

    botaoSalvarDeveEstarHabilitado();
  });

  it('Preencher nota fiscal de saída', () => {
    cy.get(`${mainSelector} rnc-notas-fiscais-entrada-autocomplete-select`).should('not.exist')

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idNotaFiscal,
      request: getListNotasFiscaisSaida(),
      selector: 'rnc-notas-fiscais-saida-autocomplete-select',
    });
  });

  it('Selecionar origem interno', () => {
    cy.get(`vs-select[formcontrolname=${NaoConformidadesFormControl.origem}]`).click();
    cy.get(`mat-option`).contains('Cliente').click();
  });

  notaFiscalNaoDeveEstarVazio();

  it('Selecionar origem inspeção de saída', () => {
    cy.get(`vs-select[formcontrolname=${NaoConformidadesFormControl.origem}]`).click();
    cy.get(`mat-option`).contains('Inspeção de Saída').click();
  });

  notaFiscalNaoDeveEstarVazio();

  it('Selecionar origem inspeção de entrada', () => {
    cy.get(`vs-select[formcontrolname=${NaoConformidadesFormControl.origem}]`).click();
    cy.get(`mat-option`).contains('Inspeção de Entrada').click();
  });

  it('Preencher fornecedor', () => {
    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idPessoa,
      request: getClientesList(),
      selector: `${mainSelector} person-person-autocomplete-single[personType='Supplier']`,
    });
  })

  it('Nota fiscal de saída deve estar vazio, e preencher nota fiscal de entrada', () => {
    cy.get(`${mainSelector} rnc-notas-fiscais-saida-autocomplete-select`).should('not.exist')
    cy.get(`${mainSelector} rnc-notas-fiscais-entrada-autocomplete-select input`).should('be.empty')

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idNotaFiscal,
      request: getListNotasFiscaisEntrada(),
      selector: 'rnc-notas-fiscais-entrada-autocomplete-select',
    });
  });
  it('Selecionar origem inspeção de saída', () => {
    cy.get(`vs-select[formcontrolname=${NaoConformidadesFormControl.origem}]`).click();
    cy.get(`mat-option`).contains('Inspeção de Saída').click();
  });

  notaFiscalDeveEstarVazio();

  it('Preencher nota fiscal de saída', () => {
    cy.get(`${mainSelector} rnc-notas-fiscais-entrada-autocomplete-select`).should('not.exist')

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idNotaFiscal,
      request: getListNotasFiscaisSaida(),
      selector: 'rnc-notas-fiscais-saida-autocomplete-select',
    });
  });

  it('Preencher cliente e verificar se nota fiscal é limpo', () => {
    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idPessoa,
      request: getClientesList(),
      selector: `${mainSelector} person-person-autocomplete-single[personType='Customer']`,
    });
    cy.get(`${mainSelector} rnc-notas-fiscais-saida-autocomplete-select input`).should('be.empty')
  })
  it('Preencher odf', () => {
    const getListOrdemProducaoRequest = getListOrdemProducao();
    getListOrdemProducaoRequest.response.body.items[0].idCliente = ids[2];
    cy.batchRequestStub(getListOrdemProducaoRequest).then(alias => {
      cy.getVsInput(NaoConformidadesFormControl.numeroOdf, `${mainSelector}`)
      .type(codes[0].toString())
      .blur()
      cy.wait(alias);
    })

  })

  it('Preencher nota fiscal de saída', () => {
    cy.get(`${mainSelector} rnc-notas-fiscais-entrada-autocomplete-select`).should('not.exist')

    cy.getAutoCompleteItem({
      formControlName: NaoConformidadesFormControl.idNotaFiscal,
      request: getListNotasFiscaisSaida(),
      selector: 'rnc-notas-fiscais-saida-autocomplete-select',
    });
  });

  it('Salvar nova nao-conformidade', () => {
    const criarNaoConformidadeMock = new CreateNaoConformidadeMock();

    createNaoConformidade(criarNaoConformidadeMock).then((createInterception:Interception) => {
      const expectedResult = {
        ...criarNaoConformidadeMock.createNaoConformidade.expectedBody,
        id: createInterception.request.body.id,
        origem: OrigemNaoConformidades.InspecaoSaida,
        idNotaFiscal: ids[0],
        numeroOdf: codes[0].toString()
      } as NaoConformidadeInput;
      cy.validateRequestStatusCode(createInterception?.response?.statusCode, 200);
      cy.validateRequestBody(createInterception?.request.body, expectedResult);
    });
  });
});

function botaoSalvarDeveEstarDesabilitado() {
  it('Botao salvar deve estar desabilitado', () => {
    cy.get(`${mainSelector} vs-header vs-button[type=save] button`).should('be.disabled');
  });
}
function botaoSalvarDeveEstarHabilitado() {
  it('Botao salvar deve estar desabilitado', () => {
    cy.get(`${mainSelector} vs-header vs-button[type=save] button`).should('be.enabled');
  });
}
function notaFiscalNaoDeveEstarVazio() {
  it('Nota fiscal não deve estar vazio', () => {
    cy.get(`${mainSelector} rnc-notas-fiscais-saida-autocomplete-select input`).should('have.value', '1')
  })
}
function notaFiscalDeveEstarVazio() {
  it('Nota fiscal deve estar vazio', () => {
    cy.get(`${mainSelector} rnc-notas-fiscais-saida-autocomplete-select input`).should('be.empty')
  })
}
