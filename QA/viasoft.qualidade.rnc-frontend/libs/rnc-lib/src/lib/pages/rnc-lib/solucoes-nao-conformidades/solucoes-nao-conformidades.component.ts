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
import { DefeitosNaoConformidadesModel } from '../../../api-clients/Nao-Conformidades';
import { SolucoesNaoConformidadesModel }
  from '../../../api-clients/Nao-Conformidades/Solucoes-Nao-Conformidades/model/solucoes-nao-conformidades-model';
import { EditorAction } from '../../../tokens/classes/editor-action.enum';
import { EditorModalData } from '../../../tokens/classes/editor-modal-data';
import { map } from 'rxjs/operators';
import { SolucoesNaoConformidadesEditorModalComponent }
  from './solucoes-nao-conformidades-editor-modal/solucoes-nao-conformidades-editor-modal.component';
import { SolucoesNaoConformidadesService } from './solucoes-nao-conformidades.service';
import { NaoConformidadesEditorService } from '../../rnc-lib/rnc-editor-modal/nao-conformidades-editor.service';
import { MatDialog } from '@angular/material/dialog';

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
    private dialog: MatDialog,
    private vsDialog: VsDialog,
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

    this.gridOptions.get = (input: VsGridGetInput) => this.get(input, this.defeito.idNaoConformidade, this.defeito.id);
    this.gridOptions.select
     = (index: number, data: SolucoesNaoConformidadesModel) => {
        this.openEditorModal(EditorAction.Update, data);
      };
    this.gridOptions.edit = (index: number, data: SolucoesNaoConformidadesModel) => this.openEditorModal(EditorAction.Update, data)

  }

  private openEditorModal(action: EditorAction, data: SolucoesNaoConformidadesModel): void {
    const dialogConfig = this.vsDialog.generateDialogConfig(new EditorModalData(action, data),
      {
        maxWidth: '30%'
      });

    this.dialog.open(SolucoesNaoConformidadesEditorModalComponent, dialogConfig)
      .afterClosed()
      .subscribe((solucao: SolucoesNaoConformidadesModel) => {
        if (solucao) {
          this.gridOptions.refresh();
        }
      });
  }

  private get(input: VsGridGetInput, id: string, idDefeito:string) {
    return this.service.getList(input, id, idDefeito)
      .pipe(
        map((pagedResult: IPagedResultOutputDto<SolucoesNaoConformidadesModel>) => new
        VsGridGetResult(pagedResult.items, pagedResult.totalCount), id)
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
          this.canEditSolucaoNaoConformidades = false;
        } else {
          this.canEditSolucaoNaoConformidades = this.naoConformidadesEditorService.naoConformidadeNaoConcluida;
        }

        this.gridOptions.refreshActions();
      })
    );
  }
}
