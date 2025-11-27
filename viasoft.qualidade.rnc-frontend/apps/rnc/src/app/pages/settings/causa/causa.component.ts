import { Component, OnInit } from '@angular/core';
import {
  VsGridOptions, VsGridSimpleColumn, VsGridGetInput, VsGridGetResult, VsDialog, VsGridCheckboxColumn
} from '@viasoft/components';
import {
  IPagedResultOutputDto,
  JQQB_OP_EQUAL,
  MessageService,
  VsAuthorizationService
} from '@viasoft/common';
import { map } from 'rxjs/operators';

import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { HttpErrorResponse } from '@angular/common/http';
import { CausaOutput } from '@viasoft/rnc/api-clients/Causas';
import { Policies } from '../../../services/authorizations/policies/policies';
import { EditorModalData } from '../../../tokens/consts/editor-modal-data';
import { CausaEditorModalComponent } from './causa-editor-modal/causa-editor-modal.component';
import { CausaService } from './causa.service';

@Component({
  selector: 'rnc-causa',
  templateUrl: './causa.component.html',
  styleUrls: ['./causa.component.scss']
})
export class CausaComponent implements OnInit {
  public gridOptions: VsGridOptions;
  public permissions: Map<string, boolean> = new Map<string, boolean>();
  public loaded = false;
  public policies = Policies;

  constructor(
    private service: CausaService,
    private dialog: VsDialog,
    private authorizationService:VsAuthorizationService,
    private messageService: MessageService
  ) {
    this.getPermissions();
  }

  ngOnInit(): void { // vazio
  }

  public novo(): void {
    this.openEditorModal(null, EditorAction.Create);
  }

  private getPermissions() {
    this.authorizationService.isGrantedMap([Policies.CreateCausa, Policies.UpdateCausa, Policies.DeleteCausa])
      .then((permissions: Map<string, boolean>) => {
        this.permissions = permissions;
        this.gridInit();
        this.loaded = true;
      });
  }

  private gridInit() {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = 'A5E99753-0841-4044-BF96-ED7BDB28095A';
    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.Causa.Codigo',
        field: 'codigo',
        kind: 'number',
        filterOptions: {
          operators: [JQQB_OP_EQUAL],
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.Causa.Descricao',
        field: 'descricao'
      }),
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.Causa.Detalhamento',
        field: 'detalhamento'
      }),
      new VsGridCheckboxColumn({
        headerName: 'Configuracoes.Causa.Ativo',
        field: 'isAtivo',
        disabled: true
      })
    ];
    this.gridOptions.get = (input: VsGridGetInput) => this.get(input);
    this.gridOptions.select = this.permissions.get(Policies.UpdateCausa)
      ? (index: number, data: CausaOutput) => this.openEditorModal(data, EditorAction.Update) : null;
    this.gridOptions.delete = this.permissions.get(Policies.DeleteCausa)
      ? (index: number, data: CausaOutput) => this.delete(data.id) : null;
    this.gridOptions.deleteBehaviours = {
      enableAutoDeleteConfirm: true,
    };
    this.gridOptions.actions = [
      {
        icon: 'check-circle',
        tooltip: 'Configuracoes.Ativar',
        callback: (_, causa: CausaOutput) => this.ativar(causa.id),
        condition: (_, causa: CausaOutput) => !causa.isAtivo && this.permissions.get(Policies.DeleteCausa)
      },
      {
        icon: 'times-circle',
        tooltip: 'Configuracoes.Inativar',
        callback: (_, causa: CausaOutput) => this.inativar(causa.id),
        condition: (_, causa: CausaOutput) => causa.isAtivo && this.permissions.get(Policies.DeleteCausa)
      }
    ];
  }
  private delete(id: string): void {
    this.service.delete(id).subscribe({
      next: () => {
        this.gridOptions.refresh();
      },
      error: (error:HttpErrorResponse) => {
        if (error.status === 422) {
          this.messageService.warn(`Configuracoes.ValidationResult.${error.error}|entidade:Causa`);
        }
      }
    });
  }
  private ativar(id:string): void {
    this.service.ativar(id).subscribe(() => this.gridOptions.refresh());
  }

  private inativar(id:string): void {
    this.service.inativar(id).subscribe(() => this.gridOptions.refresh());
  }
  private openEditorModal(data: CausaOutput, action: EditorAction): void {
    this.dialog.open(CausaEditorModalComponent, {
      action,
      data
    } as EditorModalData<CausaOutput>, { maxWidth: '30%' }).afterClosed().subscribe((causa: CausaOutput) => {
      if (causa) {
        this.gridOptions.refresh();
      }
    });
  }
  private get(input: VsGridGetInput) {
    return this.service.getViewList(input)
      .pipe(
        map((pagedResult: IPagedResultOutputDto<CausaOutput>) => new
        VsGridGetResult(pagedResult.items, pagedResult.totalCount))
      );
  }
}
