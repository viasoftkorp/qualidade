import { DecimalPipe, Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  IPagedResultOutputDto,
  JQQB_NUMBER_OPERATORS,
  MessageService,
  VsSubscriptionManager
} from '@viasoft/common';
import {
  VsGridOptions,
  VsGridSimpleColumn,
  VsGridGetInput,
  VsGridGetResult,
  VsDialog
} from '@viasoft/components';
import {
  ProdutosNaoConformidadesModel,
  ProdutosNaoConformidadesOutput,
} from '@viasoft/rnc/api-clients/Nao-Conformidades/Produtos-Nao-Conformidades/model';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { formatNumberToDecimal } from '@viasoft/rnc/app/tokens/consts/format-number-to-decimal';
import { map } from 'rxjs/operators';
import { ProdutosNaoConformidadesService } from './produtos-nao-conformidades.service';

// eslint-disable-next-line max-len
import { ProdutosNaoConformidadesEditorModalComponent } from './produtos-nao-conformidades-editor-modal/produtos-nao-conformidades-editor-modal.component';
import { NaoConformidadesEditorService } from '../../nao-conformidades-editor/nao-conformidades-editor.service';

@Component({
  selector: 'rnc-produtos-nao-conformidades',
  templateUrl: './produtos-nao-conformidades.component.html',
  styleUrls: ['./produtos-nao-conformidades.component.scss'],
  providers: [DecimalPipe],
})
export class ProdutosNaoConformidadesComponent implements OnInit, OnDestroy {
  private naoConformidadeId: string;
  private subscriptionManager = new VsSubscriptionManager();
  public gridOptions: VsGridOptions;
  public permissions: Map<string, boolean> = new Map<string, boolean>();
  public loaded = false;
  public canAddProdutoNaoConformidade = true;

  constructor(
    private service: ProdutosNaoConformidadesService,
    private dialog: VsDialog,
    private location: Location,
    private decimalPipe: DecimalPipe,
    private naoConformidadesEditorService: NaoConformidadesEditorService,
    private messageService:MessageService
  ) {
    let url = location.path();
    url = url.substring(url.lastIndexOf('/') + 1);
    this.naoConformidadeId = url;

    this.loaded = true;
  }
  ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  ngOnInit(): void {
    this.gridInit();
    this.subscribeBloquearAtualizacaoNaoConformidade();
  }

  public novo(): void {
    this.openEditorModal(EditorAction.Create, {
      idNaoConformidade: this.naoConformidadeId,
    } as ProdutosNaoConformidadesModel);
  }

  private gridInit() {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = '99D2593B-E821-4E7C-A155-0E811AD180A8';
    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.ProdutosSolucoes.Codigo',
        field: 'codigo',
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.ProdutosSolucoes.Descricao',
        field: 'descricao',
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.ProdutosSolucoes.UnidadeMedida',
        field: 'unidadeMedida',
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.ProdutosSolucoes.Quantidade',
        field: 'quantidade',
        kind: 'number',
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade, 6),
        filterOptions: {
          operators: JQQB_NUMBER_OPERATORS,
        },
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.ProdutosSolucoes.Detalhamento',
        field: 'detalhamento'
      }),
    ];
    this.gridOptions.actions = [
      {
        icon: 'trash-alt',
        tooltip: 'NaoConformidades.Deletar',
        callback: (index: number, data: ProdutosNaoConformidadesModel) => {
          this.messageService.confirm('NaoConformidades.DeleteConfirmMessage').subscribe((confirmed:boolean) => {
            if (confirmed) {
              this.delete(data.id, data.idNaoConformidade);
            }
          });
        },
        condition: () => this.canAddProdutoNaoConformidade
      },
    ];

    this.gridOptions.get = (input: VsGridGetInput) => this.get(this.naoConformidadeId, input);
    this.gridOptions.select = (index: number, data: ProdutosNaoConformidadesModel) => {
      this.openEditorModal(EditorAction.Update, data);
    };
    this.gridOptions.edit = (index: number, data: ProdutosNaoConformidadesModel) => this.openEditorModal(EditorAction.Update, data)

    this.gridOptions.deleteBehaviours = {
      enableAutoDeleteConfirm: true,
    };
  }

  private openEditorModal(action: EditorAction, data: ProdutosNaoConformidadesModel): void {
    this.dialog
      .open(ProdutosNaoConformidadesEditorModalComponent, {
        data,
        action,
      } as EditorModalData<ProdutosNaoConformidadesModel>, { maxWidth: '30%' })
      .afterClosed()
      .subscribe((solucao: ProdutosNaoConformidadesModel) => {
        if (solucao) {
          this.gridOptions.refresh();
        }
      });
  }

  private get(id: string, input: VsGridGetInput) {
    return this.service
      .getList(input, id)
      .pipe(
        map(
          (pagedResult: IPagedResultOutputDto<ProdutosNaoConformidadesOutput>) => new VsGridGetResult(pagedResult.items, pagedResult.totalCount),
          id
        )
      );
  }

  private delete(id: string, naoConformidadeId: string): void {
    this.service.delete(id, naoConformidadeId).subscribe(() => {
      this.gridOptions.refresh();
    });
  }

  private subscribeBloquearAtualizacaoNaoConformidade() {
    this.subscriptionManager.add(
      'bloquearAtualizacaoNaoConformidade',
      this.naoConformidadesEditorService.validarAtualizacaoNaoConformidade.subscribe(() => {
        this.canAddProdutoNaoConformidade = this.naoConformidadesEditorService.naoConformidadeNaoConcluida
          && !this.naoConformidadesEditorService.naoConformidadeTemRetrabalhoGerado;
        this.gridOptions.refreshActions();
      })
    );
  }
}
