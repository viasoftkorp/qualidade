import { DecimalPipe, Location } from '@angular/common';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
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
} from '../../../api-clients/Nao-Conformidades/Produtos-Nao-Conformidades/model';
import { EditorAction } from '../../../tokens/classes/editor-action.enum';
import { EditorModalData } from '../../../tokens/classes/editor-modal-data';
import { formatNumberToDecimal } from '../../../tokens/classes/format-number-to-decimal';
import { map } from 'rxjs/operators';
import { ProdutosNaoConformidadesService } from './produtos-nao-conformidades.service';

// eslint-disable-next-line max-len
import { ProdutosNaoConformidadesEditorModalComponent } from './produtos-nao-conformidades-editor-modal/produtos-nao-conformidades-editor-modal.component';
import { NaoConformidadesEditorService } from '../rnc-editor-modal/nao-conformidades-editor.service';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'rnc-produtos-nao-conformidades',
  templateUrl: './produtos-nao-conformidades.component.html',
  styleUrls: ['./produtos-nao-conformidades.component.scss'],
  providers: [DecimalPipe],
})
export class ProdutosNaoConformidadesComponent implements OnInit, OnDestroy {
  @Input() private idNaoConformidade: string;
  private subscriptionManager = new VsSubscriptionManager();
  public gridOptions: VsGridOptions;
  public permissions: Map<string, boolean> = new Map<string, boolean>();
  public loaded = false;
  public canAddProdutoNaoConformidade = true;

  constructor(
    private service: ProdutosNaoConformidadesService,
    private dialog: MatDialog,
    private vsDialog: VsDialog,
    private location: Location,
    private decimalPipe: DecimalPipe,
    private naoConformidadesEditorService: NaoConformidadesEditorService,
    private messageService:MessageService
  ) {
  }
  ngOnInit(): void {
    this.loaded = true;
    this.gridInit();
    this.subscribeBloquearAtualizacaoNaoConformidade();
  }

  ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public novo(): void {
    this.openEditorModal(EditorAction.Create, {
      idNaoConformidade: this.idNaoConformidade,
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
        field: 'detalhamento',
        filterOptions: { disable: true },
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

    this.gridOptions.get = (input: VsGridGetInput) => this.get(this.idNaoConformidade, input);
    this.gridOptions.select = (index: number, data: ProdutosNaoConformidadesModel) => {
      this.openEditorModal(EditorAction.Update, data);
    };
    this.gridOptions.edit = (index: number, data: ProdutosNaoConformidadesModel) => this.openEditorModal(EditorAction.Update, data)

    this.gridOptions.deleteBehaviours = {
      enableAutoDeleteConfirm: true,
    };
  }

  private openEditorModal(action: EditorAction, data: ProdutosNaoConformidadesModel): void {
    const dialogConfig = this.vsDialog.generateDialogConfig(new EditorModalData(action, data),
      {
        maxWidth: '60%'
      });

    this.dialog
      .open(ProdutosNaoConformidadesEditorModalComponent, dialogConfig)
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
        if (this.naoConformidadesEditorService.somenteLeitura) {
          this.canAddProdutoNaoConformidade = false;
        } else {
          this.canAddProdutoNaoConformidade = this.naoConformidadesEditorService.naoConformidadeNaoConcluida
            && !this.naoConformidadesEditorService.naoConformidadeTemRetrabalhoGerado;
        }

        this.gridOptions.refreshActions();
      })
    );
  }
}
