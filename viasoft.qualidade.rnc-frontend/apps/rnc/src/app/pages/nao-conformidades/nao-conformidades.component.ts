import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IPagedResultOutputDto, JQQB_NUMBER_OPERATORS, JQQB_OP_EQUAL, VsAuthorizationService } from '@viasoft/common';
import {
  VsDialog,
  VsFilterGetItemsOutput,
  VsFilterItem,
  VsFilterOptions,
  VsGridGetInput,
  VsGridGetResult,
  VsGridOptions,
  VsGridSimpleColumn
} from '@viasoft/components';
import { NaoConformidadeModel } from '@viasoft/rnc/api-clients/Nao-Conformidades/model/nao-conformidade-model';
import { OrigemNaoConformidades } from '@viasoft/rnc/api-clients/Nao-Conformidades/model/origem-nao-conformidades';
import { StatusNaoConformidade } from '@viasoft/rnc/api-clients/Nao-Conformidades/model/status-nao-conformidades';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { Policies } from '../../services/authorizations/policies/policies';
import { NaoConformidadesService } from './nao-conformidades.service';
import { NaoConformidadePolicies } from '../../services/authorizations/policies/nao-conformidade-policies';

@Component({
  selector: 'rnc-nao-conformidades',
  templateUrl: './nao-conformidades.component.html',
  styleUrls: ['./nao-conformidades.component.scss']
})
export class NaoConformidadeComponent implements OnInit {
  public gridOptions: VsGridOptions;
  public permissions: Map<string, boolean> = new Map<string, boolean>();
  public loaded = false;
  public policies = Policies;
  public naoConformidadePolicies = NaoConformidadePolicies;

  constructor(
    private service: NaoConformidadesService,
    private dialog: VsDialog,
    private authorizationService:VsAuthorizationService,
    private router:Router,
    private route: ActivatedRoute,
  ) {
  }

  ngOnInit(): void {
    this.getPermissions();
  }

  public novo(): void {
    this.openCreateEditor();
  }

  private getPermissions() {
    this.authorizationService
      .isGrantedMap([
        NaoConformidadePolicies.CreateNaoConformidade,
        NaoConformidadePolicies.UpdateNaoConformidade,
        NaoConformidadePolicies.DeleteNaoConformidade
      ])
      .then((permissions: Map<string, boolean>) => {
        this.permissions = permissions;
        this.gridInit();
        this.loaded = true;
      });
  }

  private gridInit() {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = 'A5E99753-0841-4044-BF96-ED7BDB28098A';
    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'NaoConformidades.Codigo',
        field: 'codigo',
        kind: 'number',
        filterOptions: {
          operators: [JQQB_OP_EQUAL],
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidades.Origem',
        field: 'origem',
        translate: true,
        format: (data: OrigemNaoConformidades) => `NaoConformidades.OrigensOptions.${OrigemNaoConformidades[data]}`,
        filterOptions: this.filtroOrigem()
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidades.Status',
        field: 'status',
        translate: true,
        format: (data: StatusNaoConformidade) => `NaoConformidades.StatusOptions.${StatusNaoConformidade[data]}`,
        filterOptions: this.filtroStatus()
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidades.NotaFiscal',
        field: 'numeroNotaFiscal',
        kind: 'string',
        filterOptions: {
          operators: [JQQB_OP_EQUAL],
        },
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidades.Natureza',
        field: 'natureza',
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidades.Cliente',
        field: 'cliente',
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidades.Fornecedor',
        field: 'fornecedor',
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidades.Odf',
        field: 'numeroOdf',
        kind: 'number',
        filterOptions: {
          operators: JQQB_NUMBER_OPERATORS,
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidades.Produto',
        field: 'produto',
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidades.Revisao',
        field: 'revisao',
        kind: 'string'
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidades.Equipe',
        field: 'equipe',
      }),
    ];
    this.gridOptions.get = (input: VsGridGetInput) => this.get(input);
    this.gridOptions.select = this.permissions.get(NaoConformidadePolicies.UpdateNaoConformidade)
      ? (index: number, data: NaoConformidadeModel) => this.openUpdateEditor(data) : null;
    this.gridOptions.delete = this.permissions.get(NaoConformidadePolicies.DeleteNaoConformidade)
      ? (index: number, data: NaoConformidadeModel) => this.delete(data.id) : null;
    this.gridOptions.deleteBehaviours = {
      enableAutoDeleteConfirm: true,
    };
  }
  private delete(id: string): void {
    this.service.delete(id).subscribe(() => {
      this.gridOptions.refresh();
    });
  }

  private openUpdateEditor(data: NaoConformidadeModel):void {
    this.router.navigate([`${data.id}`], { relativeTo: this.route });
  }

  private openCreateEditor():void {
    this.router.navigate(['new'], { relativeTo: this.route });
  }

  private get(input: VsGridGetInput) {
    return this.service.getList(input)
      .pipe(
        map((pagedResult: IPagedResultOutputDto<NaoConformidadeModel>) => new
        VsGridGetResult(pagedResult.items, pagedResult.totalCount))
      );
  }
  private filtroOrigem(): VsFilterOptions {
    const origens: Array<VsFilterItem> = [
      {
        key: OrigemNaoConformidades.Cliente.toString(),
        value: `NaoConformidades.OrigensOptions.${OrigemNaoConformidades.Cliente}`,
      },
      {
        key: OrigemNaoConformidades.InspecaoEntrada.toString(),
        value: `NaoConformidades.OrigensOptions.${OrigemNaoConformidades.InspecaoEntrada}`,
      },
      {
        key: OrigemNaoConformidades.InspecaoSaida.toString(),
        value: `NaoConformidades.OrigensOptions.${OrigemNaoConformidades.InspecaoSaida}`,
      },
      {
        key: OrigemNaoConformidades.Interno.toString(),
        value: `NaoConformidades.OrigensOptions.${OrigemNaoConformidades.Interno}`,
      },
    ];

    return {
      operators: [
        JQQB_OP_EQUAL
      ],
      blockInput: true,
      useField: 'origem',
      mode: 'selection',
      multiple: true,
      getValidItems: (keys) => of(origens.filter((item) => keys.includes(item.key))),
      getItems: () => of({
        items: origens,
        totalCount: origens.length
      } as VsFilterGetItemsOutput)
    };
  }
  private filtroStatus(): VsFilterOptions {
    const status: Array<VsFilterItem> = [
      {
        key: StatusNaoConformidade.Aberto.toString(),
        value: `NaoConformidades.StatusOptions.${StatusNaoConformidade.Aberto}`,
      },
      {
        key: StatusNaoConformidade.Fechado.toString(),
        value: `NaoConformidades.StatusOptions.${StatusNaoConformidade.Fechado}`,
      },
      {
        key: StatusNaoConformidade.Pendente.toString(),
        value: `NaoConformidades.StatusOptions.${StatusNaoConformidade.Pendente}`,
      }
    ];

    return {
      operators: [
        JQQB_OP_EQUAL
      ],
      blockInput: true,
      useField: 'status',
      mode: 'selection',
      multiple: true,
      getValidItems: (keys) => of(status.filter((item) => keys.includes(item.key))),
      getItems: () => of({
        items: status,
        totalCount: status.length
      })
    };
  }
}
