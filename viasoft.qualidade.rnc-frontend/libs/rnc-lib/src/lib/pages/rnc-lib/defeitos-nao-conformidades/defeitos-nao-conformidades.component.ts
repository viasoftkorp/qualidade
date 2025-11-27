import { DecimalPipe } from '@angular/common';
import {
  Component,
  EventEmitter, Input,
  OnDestroy,
  OnInit,
  Output,
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
import { DefeitosNaoConformidadesModel } from '../../../api-clients/Nao-Conformidades';
import { EditorAction } from '../../../tokens/classes/editor-action.enum';
import { EditorModalData } from '../../../tokens/classes/editor-modal-data';
import { formatNumberToDecimal } from '../../../tokens/classes/format-number-to-decimal';
import { map } from 'rxjs/operators';
// eslint-disable-next-line max-len
import { DefeitosNaoConformidadesEditorModalComponent } from './defeitos-nao-conformidades-editor-modal/defeitos-nao-conformidades-editor-modal.component';
import { DefeitosNaoConformidadesService } from './defeitos-nao-conformidades.service';
import { NaoConformidadesEditorService } from '../rnc-editor-modal/nao-conformidades-editor.service';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'rnc-defeitos-nao-conformidades',
  templateUrl: './defeitos-nao-conformidades.component.html',
  styleUrls: ['./defeitos-nao-conformidades.component.scss'],
  providers: [DecimalPipe],
})
export class DefeitosNaoConformidadesComponent implements OnInit, OnDestroy {
  @Input() private idNaoConformidade: string;
  @Output() public idDefeitoNaoConformidade = new EventEmitter<DefeitosNaoConformidadesModel>();
  private subscriptionManager = new VsSubscriptionManager();
  private idDefeitoValue: string;
  public gridOptions: VsGridOptions;
  public permissions: Map<string, boolean> = new Map<string, boolean>();
  public loaded = true;
  public canAddDefeito = true;

  constructor(
    private service: DefeitosNaoConformidadesService,
    private dialog: MatDialog,
    private vsDialog: VsDialog,
    private decimalPipe: DecimalPipe,
    private naoConformidadesEditorService: NaoConformidadesEditorService,
    private messageService:MessageService
  ) {  }

  ngOnInit(): void {
    this.gridInit();
    this.subscribeBloquearAtualizacaoNaoConformidade();
  }

  ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public novo(): void {
    this.openEditorModal(EditorAction.Create, <DefeitosNaoConformidadesModel>{
      idNaoConformidade: this.idNaoConformidade,
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
        field: 'detalhamento',
        filterOptions: { disable: true },
      }),
    ];
    this.gridOptions.get = (input: VsGridGetInput) => this.get(input, this.idNaoConformidade);
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
              this.idDefeitoNaoConformidade.emit(null);
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
    const dialogConfig = this.vsDialog.generateDialogConfig(new EditorModalData(action, defeitoData),
      {
        maxWidth: '50%'
      });
    this.dialog
      .open(DefeitosNaoConformidadesEditorModalComponent, dialogConfig)
      .afterClosed()
      .subscribe((defeito: DefeitosNaoConformidadesModel) => {
        if (defeito) {
          this.gridOptions.refresh();
        }
      });
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
        if (this.naoConformidadesEditorService.somenteLeitura) {
          this.canAddDefeito = false;
        } else {
          this.canAddDefeito = this.naoConformidadesEditorService.naoConformidadeNaoConcluida;
        }

        this.gridOptions.refreshActions();
      })
    );
  }
}
