import {
  Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  JQQB_OP_EQUAL,
  IPagedResultOutputDto,
  VsSubscriptionManager,
  MessageService
} from '@viasoft/common';
import {
  VsGridOptions, VsDialog, VsGridSimpleColumn, VsGridGetInput, VsGridGetResult
} from '@viasoft/components';
import { DefeitosNaoConformidadesModel } from '@viasoft/rnc/api-clients/Nao-Conformidades';
import { SolucoesNaoConformidadesModel }
  from '@viasoft/rnc/api-clients/Nao-Conformidades/Solucoes-Nao-Conformidades/model/solucoes-nao-conformidades-model';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { map } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { SolucoesNaoConformidadesEditorModalComponent }
  from './solucoes-nao-conformidades-editor-modal/solucoes-nao-conformidades-editor-modal.component';
import { SolucoesNaoConformidadesService } from './solucoes-nao-conformidades.service';
import { NaoConformidadesEditorService } from '../../nao-conformidades-editor.service';

@Component({
  selector: 'rnc-solucoes-nao-conformidades',
  templateUrl: './solucoes-nao-conformidades.component.html',
  styleUrls: ['./solucoes-nao-conformidades.component.scss']
})
export class SolucoesNaoConformidadesComponent implements OnInit, OnDestroy, OnChanges {
  @Input() public defeito: DefeitosNaoConformidadesModel;
  private subscriptionManager = new VsSubscriptionManager();
  public gridOptions: VsGridOptions;
  public permissions: Map<string, boolean> = new Map<string, boolean>();
  public loaded = false;
  public canEditSolucaoNaoConformidades = true;

  constructor(
    private service: SolucoesNaoConformidadesService,
    private vsDialog: VsDialog,
    private matDialog: MatDialog,
    private route: ActivatedRoute,
    private naoConformidadesEditorService: NaoConformidadesEditorService,
    private messageService:MessageService
  ) {
  }
  ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes.defeito.currentValue) {
      this.loaded = true;
    }
    if (!changes.defeito.firstChange) {
      this.gridOptions.refresh();
    }
    if (changes.defeito.currentValue === null) {
      this.loaded = false;
    }
  }
  ngOnInit(): void {
    this.gridInit();
    this.subscribeBloquearAtualizacaoNaoConformidade();
  }

  public novo(): void {
    this.openEditorModal(EditorAction.Create,
    <SolucoesNaoConformidadesModel> {
      idNaoConformidade: this.defeito.idNaoConformidade,
      idDefeitoNaoConformidade: this.defeito.id
    });
  }

  private gridInit() {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = '946CB2A1-C060-4BD5-83D4-6304655C9C5A';
    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.Solucoes.Codigo',
        field: 'codigo',
        kind: 'number',
        filterOptions: {
          operators: [JQQB_OP_EQUAL],
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.Solucoes.Descricao',
        field: 'descricao'
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.Solucoes.Detalhamento',
        field: 'detalhamento',
      }),
    ];
    this.gridOptions.actions = [
      {
        icon: 'trash-alt',
        tooltip: 'NaoConformidades.Deletar',
        callback: (index: number, data: SolucoesNaoConformidadesModel) => {
          if (!this.canEditSolucaoNaoConformidades) {
            return;
          }
          this.messageService.confirm('NaoConformidades.DeleteConfirmMessage').subscribe((confirmed:boolean) => {
            if (confirmed) {
              this.delete(data.id, data.idNaoConformidade);
            }
          });
        },
        condition: () => this.canEditSolucaoNaoConformidades
      },
    ];

    this.gridOptions.get = (input: VsGridGetInput) => this.get(input);
    this.gridOptions.select
     = (index: number, data: SolucoesNaoConformidadesModel) => {
        this.openEditorModal(EditorAction.Update, data);
      };
    // eslint-disable-next-line max-len
    this.gridOptions.edit = (index: number, data: SolucoesNaoConformidadesModel) => this.openEditorModal(EditorAction.Update, data);
  }

  private openEditorModal(action: EditorAction, data: SolucoesNaoConformidadesModel): void {
    const dialogConfig = this.vsDialog.generateDialogConfig(new EditorModalData(action, data),
      {
        maxWidth: '30%'
      });
    this.matDialog.open(
      SolucoesNaoConformidadesEditorModalComponent,
      dialogConfig
    ).afterClosed().subscribe((solucao: SolucoesNaoConformidadesModel) => {
      if (solucao) {
        this.gridOptions.refresh();
      }
    });
  }

  private get(input: VsGridGetInput) {
    return this.service.getList(input, this.defeito.idNaoConformidade, this.defeito.id)
      .pipe(
        map((pagedResult: IPagedResultOutputDto<SolucoesNaoConformidadesModel>) => new
        VsGridGetResult(pagedResult.items, pagedResult.totalCount), this.defeito.idNaoConformidade)
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
        this.canEditSolucaoNaoConformidades = this.naoConformidadesEditorService.naoConformidadeNaoConcluida;
        this.gridOptions.refreshActions();
      })
    );
  }
}
