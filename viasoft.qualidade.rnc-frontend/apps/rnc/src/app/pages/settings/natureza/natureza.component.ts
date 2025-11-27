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
import { Policies } from '../../../services/authorizations/policies/policies';
import { EditorModalData } from '../../../tokens/consts/editor-modal-data';
import { NaturezaEditorModalComponent } from './natureza-editor-modal/natureza-editor-modal.component';
import { NaturezaService } from './natureza.service';
import { NaturezaOutput } from '@viasoft/rnc/api-clients/Naturezas';

@Component({
  selector: 'rnc-natureza',
  templateUrl: './natureza.component.html',
  styleUrls: ['./natureza.component.scss']
})
export class NaturezaComponent implements OnInit {
  public gridOptions: VsGridOptions;
  public permissions: Map<string, boolean> = new Map<string, boolean>();
  public loaded = false;
  public policies = Policies;

  constructor(
    private service: NaturezaService,
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
    this.authorizationService.isGrantedMap([Policies.CreateNatureza, Policies.UpdateNatureza, Policies.DeleteNatureza])
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
        headerName: 'Configuracoes.Natureza.Codigo',
        field: 'codigo',
        kind: 'number',
        filterOptions: {
          operators: [JQQB_OP_EQUAL],
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.Natureza.Descricao',
        field: 'descricao'
      }),
      new VsGridCheckboxColumn({
        headerName: 'Configuracoes.Natureza.Ativo',
        field: 'isAtivo',
        disabled: true
      })
    ];
    this.gridOptions.get = (input: VsGridGetInput) => this.get(input);
    this.gridOptions.select = this.permissions.get(Policies.UpdateNatureza)
      ? (index: number, data: NaturezaOutput) => this.openEditorModal(data, EditorAction.Update) : null;
    this.gridOptions.delete = this.permissions.get(Policies.DeleteNatureza)
      ? (index: number, data: NaturezaOutput) => this.delete(data.id) : null;
    this.gridOptions.deleteBehaviours = {
      enableAutoDeleteConfirm: true,
    };
    this.gridOptions.actions = [
      {
        icon: 'check-circle',
        tooltip: 'Configuracoes.Ativar',
        callback: (_, natureza: NaturezaOutput) => this.ativar(natureza.id),
        condition: (_, natureza: NaturezaOutput) => !natureza.isAtivo && this.permissions.get(Policies.DeleteNatureza)
      },
      {
        icon: 'times-circle',
        tooltip: 'Configuracoes.Inativar',
        callback: (_, natureza: NaturezaOutput) => this.inativar(natureza.id),
        condition: (_, natureza: NaturezaOutput) => natureza.isAtivo && this.permissions.get(Policies.DeleteNatureza)
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
          this.messageService.warn(`Configuracoes.ValidationResult.${error.error}|entidade:Natureza`);
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

  private openEditorModal(data: NaturezaOutput, action: EditorAction): void {
    this.dialog.open(NaturezaEditorModalComponent, {
      action,
      data
    } as EditorModalData<NaturezaOutput>, { maxWidth: '30%' }).afterClosed().subscribe((natureza: NaturezaOutput) => {
      if (natureza) {
        this.gridOptions.refresh();
      }
    });
  }
  private get(input: VsGridGetInput) {
    return this.service.getViewList(input)
      .pipe(
        map((pagedResult: IPagedResultOutputDto<NaturezaOutput>) => new
        VsGridGetResult(pagedResult.items, pagedResult.totalCount))
      );
  }
}
