import {
  Component,
  Input,
  OnInit,
  SimpleChanges
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  IPagedResultOutputDto,
  JQQB_COND_OR,
  JQQB_OP_CONTAINS,
  MessageService,
  VsSubscriptionManager
} from '@viasoft/common';
import {
  VsDialog,
  VsFilterGetItemsInput,
  VsFilterGetItemsOutput,
  VsFilterItem,
  VsFilterOptions,
  VsGridGetInput,
  VsGridGetResult,
  VsGridOptions,
  VsGridSimpleColumn,
} from '@viasoft/components';
import { map } from 'rxjs/operators';
import { UserSelectOutput } from '@viasoft/administration';
import { Observable } from 'rxjs';
// eslint-disable-next-line max-len
import { ImplementacaoEvitarReincidenciaNaoConformidadesService } from './implementacao-evitar-reincidencia-nao-conformidades.service';
// eslint-disable-next-line max-len
import { ImplementacaoEvitarReincidenciaNaoConformidadesEditorModalComponent } from './implementacao-evitar-reincidencia-nao-conformidades-editor-modal/implementacao-evitar-reincidencia-nao-conformidades-editor-modal.component';
import { MatDialog } from '@angular/material/dialog';
import { NaoConformidadesEditorService } from '../rnc-editor-modal/nao-conformidades-editor.service';
import { UserProxyService } from '../../../api-clients/Authentication/Users/api/user-proxy.service';
import { EditorModalData } from '../../../tokens/classes/editor-modal-data';
import { EditorAction } from '../../../tokens/classes/editor-action.enum';
import { DefeitosNaoConformidadesModel } from '../../../api-clients/Nao-Conformidades';
import { ImplementacaoEvitarReincidenciaNaoConformidadesModel } from '../../../api-clients/Implementacao-Evitar-Reincidencia-Nao-Conformidades/model/implementacao-evitar-reincidencia-nao-conformidades-model';

@Component({
  selector: 'rnc-implementacao-evitar-reincidencia-nao-conformidades',
  templateUrl: './implementacao-evitar-reincidencia-nao-conformidades.component.html',
  styleUrls: ['./implementacao-evitar-reincidencia-nao-conformidades.component.scss'],
})
export class ImplementacaoEvitarReincidenciaNaoConformidadesComponent implements OnInit {
  @Input() public defeito: DefeitosNaoConformidadesModel;
  private subscriptionManager = new VsSubscriptionManager();
  public gridOptions: VsGridOptions;
  public permissions: Map<string, boolean> = new Map<string, boolean>();
  public loaded = false;
  public canEditAcaoPreventivaNaoConformidades = true;

  constructor(
    private service: ImplementacaoEvitarReincidenciaNaoConformidadesService,
    private vsDialog: VsDialog,
    private matDialog: MatDialog,
    private route: ActivatedRoute,
    private naoConformidadesEditorService: NaoConformidadesEditorService,
    private messageService: MessageService,
    private userProxyService: UserProxyService
  ) {
  }

  public ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public ngOnChanges(changes: SimpleChanges): void {
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

  public ngOnInit(): void {
    this.gridInit();
    this.subscribeBloquearAtualizacaoNaoConformidade();
  }

  public novo(): void {
    this.openEditorModal(EditorAction.Create, <ImplementacaoEvitarReincidenciaNaoConformidadesModel>{
      idNaoConformidade: this.defeito.idNaoConformidade,
      idDefeitoNaoConformidade: this.defeito.id,
    });
  }
  private gridInit() {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = '9040DBA6-FE55-439D-8B8C-1AC50E22C491';
    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.ImplementacaoEvitarReincidencia.Descricao',
        field: 'descricao',
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.ImplementacaoEvitarReincidencia.Responsavel',
        field: 'responsavel',
        sorting: {
          disable: true,
        },
        filterOptions: this.filterResponsavelOptions(),
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.ImplementacaoEvitarReincidencia.Auditor',
        field: 'auditor',
        sorting: {
          disable: true
        },
        filterOptions: this.filterAuditorOptions()
      }),
    ];
    this.gridOptions.actions = [
      {
        icon: 'trash-alt',
        tooltip: 'NaoConformidades.Deletar',
        callback: (index: number, data: ImplementacaoEvitarReincidenciaNaoConformidadesModel) => {
          if (!this.canEditAcaoPreventivaNaoConformidades) {
            return;
          }
          this.messageService.confirm('NaoConformidades.DeleteConfirmMessage').subscribe((confirmed: boolean) => {
            if (confirmed) {
              this.delete(data.id, data.idNaoConformidade);
            }
          });
        },
        condition: () => this.canEditAcaoPreventivaNaoConformidades,
      },
    ];

    this.gridOptions.get = (input: VsGridGetInput) => this.get(input);
    this.gridOptions.select = (index: number, data: ImplementacaoEvitarReincidenciaNaoConformidadesModel) => {
      this.openEditorModal(EditorAction.Update, data);
    };
    // eslint-disable-next-line max-len
    this.gridOptions.edit = (index: number, data: ImplementacaoEvitarReincidenciaNaoConformidadesModel) => this.openEditorModal(EditorAction.Update, data);

    this.gridOptions.deleteBehaviours = {
      enableAutoDeleteConfirm: true,
    };
  }

  private filterResponsavelOptions(): VsFilterOptions {
    return {
      operators: [JQQB_OP_CONTAINS],
      mode: 'selection',
      multiple: true,
      getItems: (input: VsFilterGetItemsInput) => this.getUsersForFilter(input),
      useField: 'idResponsavel',
      getItemsFilterFields: ['userName'],
      getItemsFilterOperator: JQQB_OP_CONTAINS,
      conditions: [JQQB_COND_OR]
    };
  }

  private filterAuditorOptions(): VsFilterOptions {
    return {
      operators: [JQQB_OP_CONTAINS],
      mode: 'selection',
      multiple: true,
      getItems: (input: VsFilterGetItemsInput) => this.getUsersForFilter(input),
      useField: 'idAuditor',
      getItemsFilterFields: ['userName'],
      getItemsFilterOperator: JQQB_OP_CONTAINS,
      conditions: [JQQB_COND_OR]
    };
  }

  private getUsersForFilter(input:VsFilterGetItemsInput): Observable<VsFilterGetItemsOutput> {
    return this.userProxyService.getList(input)
      .pipe(
        map((responsaveis : IPagedResultOutputDto<UserSelectOutput>) => {
          const gridItems = { items: [], totalCount: 0 } as VsFilterGetItemsOutput;
          if (responsaveis.items) {
            gridItems.totalCount = responsaveis.totalCount;
            gridItems.items = responsaveis.items.map((responsavel:UserSelectOutput) => (
                {
                  key: responsavel.id,
                  value: `${responsavel.firstName} ${responsavel.secondName}`
                } as VsFilterItem
            ));
          }
          return gridItems;
        })
      );
  }

  private openEditorModal(action: EditorAction, data: ImplementacaoEvitarReincidenciaNaoConformidadesModel): void {
    const dialogConfig = this.vsDialog.generateDialogConfig(new EditorModalData(action, data),
      {
        maxWidth: '30%'
      });
    this.matDialog
      .open(ImplementacaoEvitarReincidenciaNaoConformidadesEditorModalComponent,
        dialogConfig)
      .afterClosed()
      .subscribe((implementacao: ImplementacaoEvitarReincidenciaNaoConformidadesModel) => {
        if (implementacao) {
          this.gridOptions.refresh();
        }
      });
  }

  private get(input: VsGridGetInput) {
    return this.service
      .getList(input, this.defeito.idNaoConformidade, this.defeito.id)
      .pipe(
        map(
          // eslint-disable-next-line max-len
          (pagedResult: IPagedResultOutputDto<ImplementacaoEvitarReincidenciaNaoConformidadesModel>) => new VsGridGetResult(pagedResult.items, pagedResult.totalCount),
          this.defeito.idNaoConformidade
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
          this.canEditAcaoPreventivaNaoConformidades = false;
        } else {
          this.canEditAcaoPreventivaNaoConformidades = this.naoConformidadesEditorService.naoConformidadeNaoConcluida;
        }

        this.gridOptions.refreshActions();
      })
    );
  }
}
