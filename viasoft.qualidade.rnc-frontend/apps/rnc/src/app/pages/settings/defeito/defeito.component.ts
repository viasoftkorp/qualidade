import {
  IPagedResultOutputDto,
  JQQB_OP_EQUAL,
  MessageService,
  VsAuthorizationService
} from '@viasoft/common';
import { Policies } from '@viasoft/rnc/app/services/authorizations/policies/policies';
import { Component } from '@angular/core';
import {
  VsDialog, VsGridCheckboxColumn, VsGridGetInput, VsGridGetResult, VsGridOptions, VsGridSimpleColumn
} from '@viasoft/components';
import { DefeitoOutput } from '@viasoft/rnc/api-clients/Defeitos';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { map } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { DefeitoService } from './defeito.service';
import { DefeitoEditorModalComponent } from './defeito-editor-modal/defeito-editor-modal.component';

@Component({
  selector: 'rnc-defeito',
  templateUrl: './defeito.component.html',
  styleUrls: ['./defeito.component.scss']
})
export class DefeitoComponent {
  public gridOptions: VsGridOptions;
  public permissions: Map<string, boolean> = new Map<string, boolean>();
  public loaded = false;
  public policies = Policies;

  constructor(private service: DefeitoService,
    private dialog: VsDialog,
    private authorizationService: VsAuthorizationService,
    private messageService: MessageService) {
    this.getPermissions();
  }

  public novo(): void {
    this.openEditorModal(null, EditorAction.Create);
  }

  private getPermissions() {
    this.authorizationService.isGrantedMap([Policies.CreateDefeito, Policies.UpdateDefeito, Policies.DeleteDefeito])
      .then((permissions: Map<string, boolean>) => {
        this.permissions = permissions;
        this.gridInit();
        this.loaded = true;
      });
  }

  private gridInit() {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = 'A5E99753-0654-4044-BF96-ED7BDB28095A';
    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.Defeito.Codigo',
        field: 'codigo',
        kind: 'number',
        filterOptions: {
          operators: [JQQB_OP_EQUAL],
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.Defeito.Descricao',
        field: 'descricao'
      }),
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.Defeito.Causa',
        field: 'causa',
        filterOptions: {
          useField: () => 'descricaoCausa'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.Defeito.Solucao',
        field: 'solucao',
        filterOptions: {
          useField: () => 'descricaoSolucao'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.Defeito.Detalhamento',
        field: 'detalhamento'
      }),
      new VsGridCheckboxColumn({
        headerName: 'Configuracoes.Defeito.Ativo',
        field: 'isAtivo',
        disabled: true
      })
    ];
    this.gridOptions.get = (input: VsGridGetInput) => this.get(input);
    this.gridOptions.select = this.permissions.get(Policies.UpdateDefeito)
      ? (index: number, data: DefeitoOutput) => this.openEditorModal(data, EditorAction.Update) : null;
    this.gridOptions.delete = this.permissions.get(Policies.DeleteDefeito)
      ? (index: number, data: DefeitoOutput) => this.delete(data.id) : null;
    this.gridOptions.deleteBehaviours = {
      enableAutoDeleteConfirm: true,
    };
    this.gridOptions.actions = [
      {
        icon: 'check-circle',
        tooltip: 'Configuracoes.Ativar',
        callback: (_, defeito: DefeitoOutput) => this.ativar(defeito.id),
        condition: (_, defeito: DefeitoOutput) => !defeito.isAtivo && this.permissions.get(Policies.DeleteDefeito)
      },
      {
        icon: 'times-circle',
        tooltip: 'Configuracoes.Inativar',
        callback: (_, defeito: DefeitoOutput) => this.inativar(defeito.id),
        condition: (_, defeito: DefeitoOutput) => defeito.isAtivo && this.permissions.get(Policies.DeleteDefeito)
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
          this.messageService.warn(`Configuracoes.ValidationResult.${error.error}|entidade:Defeito`);
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

  private openEditorModal(data: DefeitoOutput, action: EditorAction): void {
    this.dialog.open(DefeitoEditorModalComponent, {
      action,
      data
    } as EditorModalData<DefeitoOutput>, { maxWidth: '30%' }).afterClosed().subscribe((defeito: DefeitoOutput) => {
      if (defeito) {
        this.gridOptions.refresh();
      }
    });
  }
  private get(input: VsGridGetInput) {
    return this.service.getViewList(input)
      .pipe(
        map((pagedResult: IPagedResultOutputDto<DefeitoOutput>) => {
          pagedResult.items.map((item) => {
            if (item.codigoCausa && item.descricaoCausa) {
              item.causa = `${item.codigoCausa} - ${item.descricaoCausa}`;
            }
            if (item.codigoSolucao && item.descricaoSolucao) {
              item.solucao = `${item.codigoSolucao} - ${item.descricaoSolucao}`;
            }
            return item;
          });
          return new VsGridGetResult(pagedResult.items, pagedResult.totalCount);
        })
      );
  };
}
