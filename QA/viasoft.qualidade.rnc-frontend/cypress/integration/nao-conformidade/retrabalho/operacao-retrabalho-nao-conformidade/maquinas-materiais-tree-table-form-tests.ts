import { MaquinaInput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/maquina-input';
import { OperacaoRetrabalhoInput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/operacao-retrabalho-input';
import { GerarOperacaoRetrabalhoFormControls } from '@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/retrabalho-nao-conformidades/operacao-retrabalho-nao-conformidades/gerar-operacao-retrabalho-editor-modal/gerar-operacao-retrabalho-form-controls';
import { MaquinasFormControl } from '@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/retrabalho-nao-conformidades/operacao-retrabalho-nao-conformidades/gerar-operacao-retrabalho-editor-modal/maquinas-materiais-tree-table-form/maquinas-editor-modal/maquinas-form-control';
import { MateriaisFormControl } from '@viasoft/rnc/app/pages/nao-conformidades/nao-conformidades-editor/retrabalho-nao-conformidades/operacao-retrabalho-nao-conformidades/gerar-operacao-retrabalho-editor-modal/maquinas-materiais-tree-table-form/materiais-editor-modal/materiais-form-control';
import { NavigateToUpdateNaoConformidadeUserActionMock } from 'cypress/support/mock/navigate-to-update-nao-conformidade-user-actions-mock';
import { getAllPagelessProductsRequest } from 'cypress/support/requests/logistics-products/products-requests';
import {
  createOperacaoRetrabalhoRequest,
  getOperacoesRequest,
  getSaldoOperacaoRequest,
} from 'cypress/support/requests/nao-conformidade/operacao-retrabalho-nao-conformidades/operacao-retrabalho-nao-conformidades-requests';
import {  getRecursoById, getRecursosList } from 'cypress/support/requests/solucoes/solucoes-requests';
import { codes, ids, strings } from 'cypress/support/test-utils';
import { navigateToNaoConformidadeEditor } from 'cypress/support/user-actions/nao-conformidades-user-actions';
import { Interception } from 'cypress/types/net-stubbing';

const mainSelector = 'rnc-nao-conformidades-editor';
const gerarOperacaoRetrabalhoEditorModalSelector = 'rnc-gerar-operacao-retrabalho-editor-modal';

describe.skip('Se maquina inserida, formulário deve ser sujo', () => {
  it('Navegar para edição de não conformidade', () => {
    const navigateToUpdateNaoConformidadeUserActionMock = new NavigateToUpdateNaoConformidadeUserActionMock();
    navigateToNaoConformidadeEditor(navigateToUpdateNaoConformidadeUserActionMock);
  });

  it('Clicar em "Gerar Operação Retrabalho"', () => {
    cy.get(`${mainSelector} vs-header rnc-gerar-retrabalho-button vs-button`)
        .contains('Gerar Operação Retrabalho')
        .click();
  });

  it('Adicionar uma máquina', () => {
    cy.wait(50)
    adicionarMaquina(0);
  });

  it('Se Pressionar o botão "x" e formulário estiver sujo deve aparecer mensagem confirmação e caso confirmado, deve fechar modal', () => {
    cy.get(`${gerarOperacaoRetrabalhoEditorModalSelector} div[actions] vs-button[icon=times] button`).click();
    const mensagemConfirmacao = 'As alterações realizadas ainda não foram salvas e serão perdidas, deseja continuar?'
    cy.get('vs-message-dialog').contains(mensagemConfirmacao).should('exist');
    cy.get('vs-button[type=cancel] button').click()
   })
});

describe.skip('Se preenchido maquinas e materiais, deve envia-los ao salvar', () => {
  it('Navegar para edição de não conformidade', () => {
    const navigateToUpdateNaoConformidadeUserActionMock = new NavigateToUpdateNaoConformidadeUserActionMock();
    navigateToNaoConformidadeEditor(navigateToUpdateNaoConformidadeUserActionMock);
  });

  it('Clicar em "Gerar Operação Retrabalho"', () => {
    cy.get(`${mainSelector} vs-header rnc-gerar-retrabalho-button vs-button`)
        .contains('Gerar Operação Retrabalho')
        .click();
  });

  it('Preencher campos obrigatórios', () => {
    cy.batchRequestStub(getSaldoOperacaoRequest()).then((alias) => {
      cy.getAutoCompleteItem({
        selector: `${gerarOperacaoRetrabalhoEditorModalSelector} qa-operacao-autocomplete-select`,
        request: getOperacoesRequest(),
        formControlName: GerarOperacaoRetrabalhoFormControls.numeroOperacaoARetrabalhar
      })
      cy.wait(alias);
    });

    cy.get(`${gerarOperacaoRetrabalhoEditorModalSelector} vs-button`).contains('Gerar Operação').should('be.disabled');

    cy.getVsInput(GerarOperacaoRetrabalhoFormControls.quantidade, gerarOperacaoRetrabalhoEditorModalSelector).type('10');
  });

  it('Adicionar três máquinas', () => {
    const numeroMaquinasParaAdicionar = 3;

    for (let index = 0; index < numeroMaquinasParaAdicionar; index++) {
      adicionarMaquina(index);
      cy.wait(50);
    }
  });

  it('Adicionar um material para cada máquina', () => {
    const numeroMaquinas = 3;
    for (let index = 0; index < numeroMaquinas; index++) {
      adicionarMaterial(index);
      cy.wait(50);
    }
  });

  it('Atualizar primeira maquina', () => {
    cy.batchRequestStub(getRecursoById()).then((aliasToWait) => {
      cy.get('rnc-maquinas-materiais-tree-table-form table tbody vs-tree-table-body-cell').contains(strings[0]).click();
      cy.wait(aliasToWait);
    });

    cy.getAutoCompleteItem({
      formControlName: MaquinasFormControl.idRecurso,
      selector: `rnc-maquinas-editor-modal qa-recurso-autocomplete-select`,
      request: getRecursosList(),
      eq: 1,
    });
    cy.getVsInput(MaquinasFormControl.horas, 'rnc-maquinas-editor-modal').clear().type(codes[1].toString());
    cy.getVsInput(MaquinasFormControl.minutos, 'rnc-maquinas-editor-modal').clear().type(codes[1].toString());
    cy.getVsTextArea(MaquinasFormControl.detalhamento, 'rnc-maquinas-editor-modal').clear().type(strings[1]);
    const saveButtonSelector = 'rnc-maquinas-editor-modal vs-button[type=save] button';

    cy.get(saveButtonSelector).click();

  });

  it('Gerar Operação', () => {
    cy.batchRequestStub(createOperacaoRetrabalhoRequest()).then((aliasToWait) => {
      cy.get(`${gerarOperacaoRetrabalhoEditorModalSelector} vs-button[type=save] button`).click();
      cy.wait(aliasToWait).then((interception: Interception) => {
        const interceptionBody = interception.request.body as OperacaoRetrabalhoInput;

        const expectedBody = {
          ...createOperacaoRetrabalhoRequest().expectedBody,
          maquinas: [
            {
              descricao: interceptionBody.maquinas[0].descricao,
              detalhamento: strings[1],
              horas: codes[1].toString(),
              minutos: codes[1].toString(),
              idRecurso: ids[1],
              id: interceptionBody.maquinas[0].id,
              materiais: [
                {
                  descricao: interceptionBody.maquinas[0].materiais[0].descricao,
                  detalhamento: strings[0],
                  idProduto: ids[0],
                  idMaquina: interceptionBody.maquinas[0].id,
                  quantidade: codes[0],
                  id: interceptionBody.maquinas[0].materiais[0].id
                }
              ],
            },
            {
              descricao: interceptionBody.maquinas[1].descricao,
              detalhamento: strings[1],
              horas: codes[1].toString(),
              minutos: codes[1].toString(),
              idRecurso: ids[1],
              id: interceptionBody.maquinas[1].id,
              materiais: [
                {
                  descricao: interceptionBody.maquinas[1].materiais[0].descricao,
                  detalhamento: strings[1],
                  idProduto: ids[1],
                  idMaquina: interceptionBody.maquinas[1].id,
                  quantidade: codes[1],
                  id: interceptionBody.maquinas[1].materiais[0].id
                }
              ],
            },
            {
              descricao: interceptionBody.maquinas[2].descricao,
              detalhamento: strings[2],
              horas: codes[2].toString(),
              minutos: codes[2].toString(),
              idRecurso: ids[2],
              id: interceptionBody.maquinas[2].id,
              materiais: [
                {
                  descricao: interceptionBody.maquinas[2].materiais[0].descricao,
                  detalhamento: strings[2],
                  idProduto: ids[2],
                  idMaquina: interceptionBody.maquinas[2].id,
                  quantidade: codes[2],
                  id: interceptionBody.maquinas[2].materiais[0].id
                }
              ],
            },
          ] as Array<MaquinaInput>,
        };

        cy.validateRequestBody(interceptionBody, expectedBody);
      });
    });
  });
});

describe.skip('Se houver mais de 5 maquinas deve aprensentar páginação e a mesma deve funcionar corretamente', () => {
  it('Navegar para edição de não conformidade', () => {
    const navigateToUpdateNaoConformidadeUserActionMock = new NavigateToUpdateNaoConformidadeUserActionMock();
    navigateToNaoConformidadeEditor(navigateToUpdateNaoConformidadeUserActionMock);
  });

  it('Clicar em "Gerar Operação Retrabalho"', () => {
    cy.get(`${mainSelector} vs-header rnc-gerar-retrabalho-button vs-button`)
        .contains('Gerar Operação Retrabalho')
        .click();
  });

  it('Adicionar dez máquinas', () => {
    const numeroMaquinasParaAdicionar = 10;

    for (let index = 0; index < numeroMaquinasParaAdicionar; index++) {
      adicionarMaquina(index);
      cy.wait(50)
    }
  });

  it('Verificar se paginador existe', () =>  {
    cy.get('vs-tree-table p-paginator').should('exist')
  })

  it('Modificar para visualizar apenas 5 itens', () => {
    cy.get('vs-tree-table p-paginator p-dropdown').click()

    cy.get('div p-dropdownitem').contains('5').click();
  })

  it('Verificar se apenas 5 itens são apresentados', () => {
    cy.get('div.vs-tree-table-row').should('have.length', 5)
  })
  it('Avançar página e verificar se há mais 5 itens', () => {
    cy.get('div.vs-tree-table-row').should('have.length', 5)
    cy.get('vs-tree-table p-paginator button[pripple] span.pi-angle-right').click()

    cy.get('div.vs-tree-table-row').should('have.length', 5)})
});

function adicionarMaterial(indexElemento = 0) {
  const botaoAdicionarMaterial =
    'rnc-maquinas-materiais-tree-table-form table tbody vs-tree-table-action-cell vs-button[ng-reflect-icon=plus] button';
  const saveButton = 'rnc-materiais-editor-modal vs-button[type=save] button';

  cy.get(botaoAdicionarMaterial).eq(indexElemento).click();
  cy.get(saveButton).should('be.disabled');

  cy.getVsInput(MateriaisFormControl.quantidade, 'rnc-materiais-editor-modal').clear().type(codes[indexElemento].toString());
  cy.getVsTextArea(MateriaisFormControl.detalhamento, 'rnc-materiais-editor-modal').type(strings[indexElemento]);

  cy.getAutoCompleteItem({
    formControlName: MateriaisFormControl.idProduto,
    request: getAllPagelessProductsRequest(),
    selector: 'rnc-materiais-editor-modal qa-produto-autocomplete-select',
    eq: indexElemento,
  });

  cy.get(saveButton).should('be.enabled');

  cy.get(saveButton).click();
}

function adicionarMaquina(indexElemento = 0) {
  cy.get('rnc-maquinas-materiais-tree-table-form vs-tree-table table thead vs-button[ng-reflect-icon=plus] button').click();

  const saveButtonSelector = 'rnc-maquinas-editor-modal vs-button[type=save] button';
  cy.get(saveButtonSelector).should('be.disabled');

  cy.getVsInput(MaquinasFormControl.horas, 'rnc-maquinas-editor-modal').clear().type(codes[indexElemento].toString());
  cy.getVsInput(MaquinasFormControl.minutos, 'rnc-maquinas-editor-modal').clear().type(codes[indexElemento].toString());
  cy.getVsTextArea(MaquinasFormControl.detalhamento, 'rnc-maquinas-editor-modal').type(strings[indexElemento]);
  cy.getAutoCompleteItem({
    formControlName: MaquinasFormControl.idRecurso,
    selector: `rnc-maquinas-editor-modal qa-recurso-autocomplete-select`,
    request: getRecursosList(),
    eq: indexElemento,
  });
  cy.get(saveButtonSelector).should('be.enabled');

  cy.get(saveButtonSelector).click();
}
