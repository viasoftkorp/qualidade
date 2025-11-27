import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  VsAuthorizationService, IPagedResultOutputDto, JQQB_OP_EQUAL, MessageService
} from '@viasoft/common';
import {
  VsGridOptions, VsDialog, VsGridSimpleColumn, VsGridGetInput, VsGridGetResult, VsGridCheckboxColumn
} from '@viasoft/components';
import { SolucaoModel } from '@viasoft/rnc/api-clients/Solucoes/model/solucao-model';
import { Policies } from '@viasoft/rnc/app/services/authorizations/policies/policies';
import { map } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { SolucaoService } from './solucao.service';

@Component({
  selector: 'rnc-solucao',
  templateUrl: './solucao.component.html',
  styleUrls: ['./solucao.component.scss']
})
export class SolucaoComponent implements OnInit {
  public gridOptions: VsGridOptions;
  public permissions: Map<string, boolean> = new Map<string, boolean>();
  public loaded = false;
  public policies = Policies;

  constructor(
    private service: SolucaoService,
    private dialog: VsDialog,
    private authorizationService:VsAuthorizationService,
    private router:Router,
    private route: ActivatedRoute,
    private messageService: MessageService
  ) {
    this.getPermissions();
  }

  ngOnInit(): void { // vazio
  }

  public novo(): void {
    this.openCreateEditor();
  }

  private getPermissions() {
    this.authorizationService.isGrantedMap([Policies.CreateSolucao, Policies.UpdateSolucao, Policies.DeleteSolucao])
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
        headerName: 'Configuracoes.Solucao.Codigo',
        field: 'codigo',
        kind: 'number',
        filterOptions: {
          operators: [JQQB_OP_EQUAL],
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.Solucao.Descricao',
        field: 'descricao'
      }),
      new VsGridCheckboxColumn({
        headerName: 'Configuracoes.Solucao.Imediata',
        field: 'imediata',
      }),
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.Solucao.Detalhamento',
        field: 'detalhamento'
      }),
      new VsGridCheckboxColumn({
        headerName: 'Configuracoes.Solucao.Ativo',
        field: 'isAtivo',
        disabled: true
      })
    ];
    this.gridOptions.get = (input: VsGridGetInput) => this.get(input);
    this.gridOptions.select = this.permissions.get(Policies.UpdateSolucao)
      ? (index: number, data: SolucaoModel) => this.openUpdateEditor(data) : null;
    this.gridOptions.delete = this.permissions.get(Policies.DeleteSolucao)
      ? (index: number, data: SolucaoModel) => this.delete(data.id) : null;
    this.gridOptions.deleteBehaviours = {
      enableAutoDeleteConfirm: true,
    };
    this.gridOptions.actions = [
      {
        icon: 'check-circle',
        tooltip: 'Configuracoes.Ativar',
        callback: (_, solucao: SolucaoModel) => this.ativar(solucao.id),
        condition: (_, solucao: SolucaoModel) => !solucao.isAtivo && this.permissions.get(Policies.DeleteSolucao)
      },
      {
        icon: 'times-circle',
        tooltip: 'Configuracoes.Inativar',
        callback: (_, solucao: SolucaoModel) => this.inativar(solucao.id),
        condition: (_, solucao: SolucaoModel) => solucao.isAtivo && this.permissions.get(Policies.DeleteSolucao)
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
          this.messageService.warn(`Configuracoes.ValidationResult.${error.error}|entidade:Solução`);
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

  private openUpdateEditor(data: SolucaoModel):void {
    this.router.navigate([`${data.id}`], { relativeTo: this.route });
  }

  private openCreateEditor():void {
    this.router.navigate(['new'], { relativeTo: this.route });
  }

  private get(input: VsGridGetInput) {
    return this.service.getViewList(input)
      .pipe(
        map((pagedResult: IPagedResultOutputDto<SolucaoModel>) => new
        VsGridGetResult(pagedResult.items, pagedResult.totalCount))
      );
  }
}
