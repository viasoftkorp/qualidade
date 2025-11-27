import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

import {
  VsAuthorizationService,
  IPagedResultOutputDto,
  JQQB_OP_CONTAINS,
  JQQB_COND_OR,
  JQQB_OP_EQUAL,
  MessageService,
} from '@viasoft/common';
import { Component } from '@angular/core';

import { UserSelectOutput } from '@viasoft/administration';

import {
  VsDialog,
  VsFilterGetItemsInput,
  VsFilterGetItemsOutput,
  VsFilterItem,
  VsFilterOptions,
  VsGridCheckboxColumn,
  VsGridGetInput,
  VsGridGetResult,
  VsGridOptions,
  VsGridSimpleColumn
} from '@viasoft/components';

import { HttpErrorResponse } from '@angular/common/http';
import {
  AcaoPreventivaEditorModalComponent
} from './acao-preventiva-editor-modal/acao-preventiva-editor-modal/acao-preventiva-editor-modal.component';

import { Policies } from '../../../services/authorizations/policies/policies';
import { AcaoPreventivaService } from './services/acao-preventiva.service';
import { AcaoPreventivaOutput } from '../../../../api-clients/Acoes-Preventivas/model/acao-preventiva-model';
import { UserProxyService } from '../../../../api-clients/Authentication/Users/api/user-proxy.service';

@Component({
  selector: 'rnc-acao-preventiva',
  templateUrl: './acao-preventiva.component.html',
  styleUrls: ['./acao-preventiva.component.scss']
})
export class AcaoPreventivaComponent {
  public gridOptions:VsGridOptions
  public permissions: Map<string, boolean> = new Map<string, boolean>();
  public loaded = false;
  public policies = Policies;

  constructor(
    private acaoPreventivaService:AcaoPreventivaService,
    private dialog:VsDialog,
    private authorizationService:VsAuthorizationService,
    private userProxyService:UserProxyService,
    private messageService: MessageService
  ) {
    this.getPermissions();
  }
  public getPermissions():void {
    this.authorizationService.isGrantedMap([
      Policies.CreateAcaoPreventiva,
      Policies.UpdateAcaoPreventiva,
      Policies.DeleteAcaoPreventiva
    ])
      .then((permissions: Map<string, boolean>) => {
        this.permissions = permissions;
        this.gridInit();
        this.loaded = true;
      });
  }
  public novo(): void {
    this.openEditorModal(null, EditorAction.Create);
  }
  private gridInit() {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = '8049A518-C799-47D6-ABF9-860F6775E4E0';

    this.gridOptions.get = (input:VsGridGetInput) => this.get(input);
    this.gridOptions.select = this.permissions.get(Policies.UpdateAcaoPreventiva)
      ? (index: number, data: AcaoPreventivaOutput) => this.openEditorModal(data, EditorAction.Update) : null;
    this.gridOptions.delete = this.permissions.get(Policies.DeleteAcaoPreventiva)
      ? (index: number, data: AcaoPreventivaOutput) => this.delete(data.id) : null;
    this.gridOptions.deleteBehaviours = {
      enableAutoDeleteConfirm: true,
    };

    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.AcaoPreventiva.Codigo',
        field: 'codigo',
        kind: 'number',
        filterOptions: {
          operators: [JQQB_OP_EQUAL],
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.AcaoPreventiva.Descricao',
        field: 'descricao'
      }),
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.AcaoPreventiva.NomeResponsavel',
        field: 'nomeResponsavel',
        sorting: {
          disable: true,
        },
        filterOptions: this.filterResponsavelOptions(),
      }),
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.AcaoPreventiva.Detalhamento',
        field: 'detalhamento'
      }),
      new VsGridCheckboxColumn({
        headerName: 'Configuracoes.AcaoPreventiva.Ativo',
        field: 'isAtivo',
        disabled: true
      })
    ];
    this.gridOptions.actions = [
      {
        icon: 'check-circle',
        tooltip: 'Configuracoes.Ativar',
        callback: (_, acaoPreventiva: AcaoPreventivaOutput) => this.ativar(acaoPreventiva.id),
        condition: (_, acaoPreventiva: AcaoPreventivaOutput) => !acaoPreventiva.isAtivo && this.permissions.get(Policies.DeleteAcaoPreventiva)
      },
      {
        icon: 'times-circle',
        tooltip: 'Configuracoes.Inativar',
        callback: (_, acaoPreventiva: AcaoPreventivaOutput) => this.inativar(acaoPreventiva.id),
        condition: (_, acaoPreventiva: AcaoPreventivaOutput) => acaoPreventiva.isAtivo && this.permissions.get(Policies.DeleteAcaoPreventiva)
      }
    ];
  }

  private ativar(id:string): void {
    this.acaoPreventivaService.ativar(id).subscribe(() => this.gridOptions.refresh());
  }

  private inativar(id:string): void {
    this.acaoPreventivaService.inativar(id).subscribe(() => this.gridOptions.refresh());
  }

  private filterResponsavelOptions(): VsFilterOptions {
    return {
      operators: [JQQB_OP_CONTAINS],
      mode: 'selection',
      multiple: true,
      getItems: (input: VsFilterGetItemsInput) => this.getResponsaveisForFilter(input),
      useField: 'idResponsavel',
      getItemsFilterFields: ['userName'],
      getItemsFilterOperator: JQQB_OP_CONTAINS,
      conditions: [JQQB_COND_OR]
    };
  }

  private getResponsaveisForFilter(input:VsFilterGetItemsInput): Observable<VsFilterGetItemsOutput> {
    return this.userProxyService.getList(input)
      .pipe(
        map((responsaveis : IPagedResultOutputDto<UserSelectOutput>) => {
          const gridItems = { items: [], totalCount: 0 } as VsFilterGetItemsOutput;
          if (responsaveis.items) {
            gridItems.totalCount = responsaveis.totalCount;
            gridItems.items = responsaveis.items.map((responsavel:UserSelectOutput) => (
                {
                  key: responsavel.id,
                  value: `${responsavel.firstName} ${responsavel.secondName}`
                } as VsFilterItem
            ));
          }
          return gridItems;
        })
      );
  }
  private get(input:VsGridGetInput):Observable<VsGridGetResult> {
    return this.acaoPreventivaService.getViewList(input)
      .pipe(
        map((pagedResult : IPagedResultOutputDto<AcaoPreventivaOutput>) => new
        VsGridGetResult(pagedResult.items, pagedResult.totalCount))
      );
  }
  private delete(id: string): void {
    this.acaoPreventivaService.delete(id).subscribe({
      next: () => {
        this.gridOptions.refresh();
      },
      error: (error:HttpErrorResponse) => {
        if (error.status === 422) {
          this.messageService.warn(`Configuracoes.ValidationResult.${error.error}|entidade:Ação Preventiva`);
        }
      }
    });
  }
  private openEditorModal(data: AcaoPreventivaOutput, action: EditorAction): void {
    this.dialog.open(AcaoPreventivaEditorModalComponent, {
      action,
      data
    } as EditorModalData<AcaoPreventivaOutput>,
    { maxWidth: '30%' }).afterClosed().subscribe((acaoPreventiva: AcaoPreventivaOutput) => {
      if (acaoPreventiva) {
        this.gridOptions.refresh();
      }
    });
  }
}
