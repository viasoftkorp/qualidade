import {
  Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { JQQB_OP_EQUAL, IPagedResultOutputDto, VsSubscriptionManager, MessageService } from '@viasoft/common';
import {
  VsGridOptions, VsDialog, VsGridSimpleColumn, VsGridGetInput, VsGridGetResult
} from '@viasoft/components';
import {
  CausasNaoConformidadesModel,
  DefeitosNaoConformidadesModel
} from '../../../api-clients/Nao-Conformidades';
import { EditorAction } from '../../../tokens/classes/editor-action.enum';
import { EditorModalData } from '../../../tokens/classes/editor-modal-data';
import { map } from 'rxjs/operators';
import { CausasNaoConformidadeEditorModalComponent }
  from './causas-nao-conformidade-editor-modal/causas-nao-conformidade-editor-modal.component';
import { CausasNaoConformidadesService } from './causas-nao-conformidades.service';
import { NaoConformidadesEditorService } from '../rnc-editor-modal/nao-conformidades-editor.service';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'rnc-causas-nao-conformidades',
  templateUrl: './causas-nao-conformidades.component.html',
  styleUrls: ['./causas-nao-conformidades.component.scss']
})
export class CausasNaoConformidadesComponent implements OnInit, OnDestroy, OnChanges {
  @Input() public defeito: DefeitosNaoConformidadesModel;
  private subscriptionManager = new VsSubscriptionManager();
  public gridOptions: VsGridOptions;
  public permissions: Map<string, boolean> = new Map<string, boolean>();
  public loaded = false;
  public canEditCausasNaoConformidades = true;

  constructor(
    private service: CausasNaoConformidadesService,
    private dialog: MatDialog,
    private vsDialog: VsDialog,
    private route: ActivatedRoute,
    private naoConformidadesEditorService:NaoConformidadesEditorService,
    private messageService: MessageService
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
    <CausasNaoConformidadesModel> {
      idNaoConformidade: this.defeito.idNaoConformidade,
      idDefeitoNaoConformidade: this.defeito.id
    });
  }

  private gridInit() {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = 'F01ED278-2971-46F9-A286-A877AD1940F9';
    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.Causas.Codigo',
        field: 'codigo',
        kind: 'number',
        filterOptions: {
          operators: [JQQB_OP_EQUAL],
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.Causas.Descricao',
        field: 'descricao'
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.Causas.Detalhamento',
        field: 'detalhamento',
        filterOptions: { disable: true }
      }),
    ];

    this.gridOptions.actions = [
      {
        icon: 'trash-alt',
        tooltip: 'NaoConformidades.Deletar',
        callback: (index: number, data: CausasNaoConformidadesModel) => {
          if (!this.canEditCausasNaoConformidades) {
            return;
          }
          this.messageService.confirm('NaoConformidades.DeleteConfirmMessage').subscribe((confirmed:boolean) => {
            if (confirmed) {
              this.delete(data.id, data.idNaoConformidade);
            }
          });
        },
        condition: () => this.canEditCausasNaoConformidades
      },
    ];

    this.gridOptions.get = (input: VsGridGetInput) => this.get(input, this.defeito.idNaoConformidade, this.defeito.id);
    this.gridOptions.select
      = (index: number, data: CausasNaoConformidadesModel) => {
        this.openEditorModal(EditorAction.Update, data);
      };
    this.gridOptions.edit = (index: number, data: CausasNaoConformidadesModel) => this.openEditorModal(EditorAction.Update, data)

    this.gridOptions.deleteBehaviours = {
      enableAutoDeleteConfirm: true,
    };
  }

  private openEditorModal(action: EditorAction, causaData: CausasNaoConformidadesModel): void {
    const dialogConfig = this.vsDialog.generateDialogConfig(new EditorModalData(action, causaData),
      {
        maxWidth: '30%'
      });

    this.dialog.open(CausasNaoConformidadeEditorModalComponent, dialogConfig)
      .afterClosed()
      .subscribe((causa: CausasNaoConformidadesModel) => {
        if (causa) {
          this.gridOptions.refresh();
        }
      });
  }

  private get(input: VsGridGetInput, id: string, idDefeito:string) {
    return this.service.getList(input, id, idDefeito, true)
      .pipe(
        map((pagedResult: IPagedResultOutputDto<CausasNaoConformidadesModel>) => new
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
          this.canEditCausasNaoConformidades = false;
        } else {
          this.canEditCausasNaoConformidades = this.naoConformidadesEditorService.naoConformidadeNaoConcluida;
        }

        this.gridOptions.refreshActions();
      })
    );
  }
}
