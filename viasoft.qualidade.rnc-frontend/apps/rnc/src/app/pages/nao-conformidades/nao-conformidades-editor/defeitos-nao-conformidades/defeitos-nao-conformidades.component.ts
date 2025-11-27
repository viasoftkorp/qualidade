import { DecimalPipe } from '@angular/common';
import {
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  IPagedResultOutputDto,
  JQQB_OP_EQUAL,
  JQQB_NUMBER_OPERATORS,
  VsSubscriptionManager,
  MessageService
} from '@viasoft/common';
import {
  VsGridOptions,
  VsDialog,
  VsGridSimpleColumn,
  VsGridGetInput,
  VsGridGetResult
} from '@viasoft/components';
import { DefeitosNaoConformidadesModel } from '@viasoft/rnc/api-clients/Nao-Conformidades';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { formatNumberToDecimal } from '@viasoft/rnc/app/tokens/consts/format-number-to-decimal';
import { map } from 'rxjs/operators';
// eslint-disable-next-line max-len
import { DefeitosNaoConformidadesEditorModalComponent } from './defeitos-nao-conformidades-editor-modal/defeitos-nao-conformidades-editor-modal.component';
import { DefeitosNaoConformidadesService } from './defeitos-nao-conformidades.service';
import { NaoConformidadesEditorService } from '../nao-conformidades-editor.service';

@Component({
  selector: 'rnc-defeitos-nao-conformidades',
  templateUrl: './defeitos-nao-conformidades.component.html',
  styleUrls: ['./defeitos-nao-conformidades.component.scss'],
  providers: [DecimalPipe],
})
export class DefeitosNaoConformidadesComponent implements OnInit, OnDestroy {
  private subscriptionManager = new VsSubscriptionManager();
  private naoConformidadeId: string;
  private idDefeitoValue: string;
  public gridOptions: VsGridOptions;
  public permissions: Map<string, boolean> = new Map<string, boolean>();
  public loaded = true;
  public canAddDefeito = true;

  constructor(
    private service: DefeitosNaoConformidadesService,
    private dialog: VsDialog,
    private decimalPipe: DecimalPipe,
    private naoConformidadesEditorService: NaoConformidadesEditorService,
    private route: ActivatedRoute,
    private messageService:MessageService
  ) {
    this.gridInit();
    this.naoConformidadeId = route.snapshot.parent.paramMap.get('id');
    this.subscribeBloquearAtualizacaoNaoConformidade();
    this.subscribeAtualizarGrid();
  }
  ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  ngOnInit(): void {
    // vazio
  }

  public novo(): void {
    this.openEditorModal(EditorAction.Create, <DefeitosNaoConformidadesModel>{
      idNaoConformidade: this.naoConformidadeId,
    });
  }

  private gridInit() {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = 'F01ED278-2971-46F9-A286-A877AD1940F7';
    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.Defeitos.Codigo',
        field: 'codigo',
        kind: 'number',
        filterOptions: {
          operators: [JQQB_OP_EQUAL],
        },
        width: 140,
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.Defeitos.Descricao',
        field: 'descricao',
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.Defeitos.Quantidade',
        field: 'quantidade',
        kind: 'number',
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade, 6),
        filterOptions: {
          operators: JQQB_NUMBER_OPERATORS,
        },
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.Defeitos.Detalhamento',
        field: 'detalhamento'
      }),
    ];
    this.gridOptions.get = (input: VsGridGetInput) => this.get(input, this.naoConformidadeId);
    this.gridOptions.select = (index: number, data: DefeitosNaoConformidadesModel) => {
      this.atualizarListaItens(data);
    };
    this.gridOptions.actions = [
      {
        icon: 'trash-alt',
        tooltip: 'NaoConformidades.Deletar',
        callback: (index: number, data: DefeitosNaoConformidadesModel) => {
          this.messageService.confirm('NaoConformidades.DeleteConfirmMessage').subscribe((confirmed:boolean) => {
            if (confirmed) {
              this.delete(data.id, data.idNaoConformidade);
            }
          });
        },
        condition: () => this.canAddDefeito
      },
      {
        icon: 'search',
        tooltip: 'NaoConformidades.visualizarItensDefeitos',
        callback: (index: number, data: DefeitosNaoConformidadesModel) => {
          this.atualizarListaItens(data);
          if (!this.canAddDefeito) {
            return;
          }
          this.openEditorModal(EditorAction.Update, data);
        },
        condition: (index: number, data: DefeitosNaoConformidadesModel) => data.id !== this.idDefeitoValue,
      },
      {
        icon: 'search',
        iconColor: '#2680EB',
        iconStyle: 'regular',
        tooltip: 'NaoConformidades.visualizarItensDefeitos',
        callback: (index: number, data: DefeitosNaoConformidadesModel) => {
          this.atualizarListaItens(data);
          this.openEditorModal(EditorAction.Update, data);
        },
        condition: (index: number, data: DefeitosNaoConformidadesModel) => data.id === this.idDefeitoValue,
      }
    ];
    this.gridOptions.deleteBehaviours = {
      enableAutoDeleteConfirm: true,
    };
  }

  private openEditorModal(action: EditorAction, defeitoData: DefeitosNaoConformidadesModel): void {
    this.dialog
      .open(DefeitosNaoConformidadesEditorModalComponent, new EditorModalData(action, defeitoData), { maxWidth: '60%' });
  }
  private atualizarListaItens(defeito: DefeitosNaoConformidadesModel): void {
    this.openEditorModal(EditorAction.Update, defeito);
    this.idDefeitoValue = defeito.id;
  }

  private get(input: VsGridGetInput, id: string) {
    return this.service
      .getList(input, id)
      .pipe(
        map(
          // eslint-disable-next-line max-len
          (pagedResult: IPagedResultOutputDto<DefeitosNaoConformidadesModel>) => new VsGridGetResult(pagedResult.items, pagedResult.totalCount),
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
        this.canAddDefeito = this.naoConformidadesEditorService.naoConformidadeNaoConcluida;
        this.gridOptions.refreshActions();
      })
    );
  }

  private subscribeAtualizarGrid(): void {
    this.subscriptionManager.add('atualizar-grid-defeitos',
      this.service.atualizarGridDefeitos.subscribe(() => {
        this.gridOptions.refresh();
      }));
  }
}
