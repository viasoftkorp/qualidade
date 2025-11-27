import { DecimalPipe, Location } from '@angular/common';
import {
  Component,
  Input,
  OnDestroy,
  OnInit
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IPagedResultOutputDto, JQQB_NUMBER_OPERATORS, MessageService, VsSubscriptionManager } from '@viasoft/common';
import { ServicosNaoConformidadesModel } from '@viasoft/rnc/api-clients/Nao-Conformidades';
import {
  VsGridOptions,
  VsGridSimpleColumn,
  VsGridGetInput,
  VsGridGetResult,
  VsDialog, VsGridCheckboxColumn
} from '@viasoft/components';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { formatNumberToDecimal } from '@viasoft/rnc/app/tokens/consts/format-number-to-decimal';
import { map } from 'rxjs/operators';
import { ServicosNaoConformidadesService } from './servicos-nao-conformidades.service';
// eslint-disable-next-line max-len
import { ServicosNaoConformidadesEditorModalComponent } from './servicos-nao-conformidades-editor-modal/servicos-nao-conformidades-editor-modal.component';
import { NaoConformidadesEditorService } from '../../nao-conformidades-editor/nao-conformidades-editor.service';

@Component({
  selector: 'rnc-servicos-nao-conformidades',
  templateUrl: './servicos-nao-conformidades.component.html',
  styleUrls: ['./servicos-nao-conformidades.component.scss'],
  providers: [DecimalPipe],
})
export class ServicosNaoConformidadesComponent implements OnDestroy {
  @Input() public idSolucaoNaoConformidade: string;
  private naoConformidadeId: string;
  private subscriptionManager = new VsSubscriptionManager();
  public gridOptions: VsGridOptions;
  public permissions: Map<string, boolean> = new Map<string, boolean>();
  public loaded = false;
  public canAddServicoNaoConformidade = true;

  constructor(
    private service: ServicosNaoConformidadesService,
    private dialog: VsDialog,
    private route: ActivatedRoute,
    private location: Location,
    private decimalPipe: DecimalPipe,
    private naoConformidadesEditorService : NaoConformidadesEditorService,
    private messageService: MessageService
  ) {
    let url = location.path();
    url = url.substring(url.lastIndexOf('/') + 1);
    this.naoConformidadeId = url;
    this.gridInit();
    this.loaded = true;
    this.subscribeBloquearAtualizacaoNaoConformidade();
  }
  ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public novo(): void {
    this.openEditorModal(EditorAction.Create, <ServicosNaoConformidadesModel>{
      idNaoConformidade: this.naoConformidadeId,
    });
  }

  private gridInit() {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = '3E7EC0D4-25D3-414B-9709-48F7774F9183';
    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.ServicosSolucoes.DescricaoRecurso',
        field: 'descricaoRecurso',
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.ServicosSolucoes.HorasPrevistas',
        field: 'horas',
        kind: 'number',
        filterOptions: {
          operators: JQQB_NUMBER_OPERATORS
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.ServicosSolucoes.MinutosPrevistos',
        field: 'minutos',
        kind: 'number',
        filterOptions: {
          operators: JQQB_NUMBER_OPERATORS
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.ServicosSolucoes.OpEngenharia',
        field: 'operacaoEngenharia',
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidade.ServicosSolucoes.Detalhamento',
        field: 'detalhamento'
      }),
      new VsGridCheckboxColumn({
        headerName: 'NaoConformidade.ServicosSolucoes.ControlarApontamento',
        field: 'controlarApontamento',
        disabled: true
      }),
    ];
    this.gridOptions.actions = [
      {
        icon: 'trash-alt',
        tooltip: 'NaoConformidades.Deletar',
        callback: (index: number, data: ServicosNaoConformidadesModel) => {
          if (!this.canAddServicoNaoConformidade) {
            return;
          }
          this.messageService.confirm('NaoConformidades.DeleteConfirmMessage').subscribe((confirmed:boolean) => {
            if (confirmed) {
              this.delete(data.id, data.idNaoConformidade);
            }
          });
        },
        condition: () => this.canAddServicoNaoConformidade
      },
    ];
    this.gridOptions.get = (input: VsGridGetInput) => this.get(input, this.naoConformidadeId);
    this.gridOptions.select = (index: number, data: ServicosNaoConformidadesModel) => {
      this.openEditorModal(EditorAction.Update, data);
    };
    this.gridOptions.edit = (index: number, data: ServicosNaoConformidadesModel) => this.openEditorModal(EditorAction.Update, data)

    this.gridOptions.deleteBehaviours = {
      enableAutoDeleteConfirm: true,
    };
  }

  private openEditorModal(action: EditorAction, data: ServicosNaoConformidadesModel): void {
    this.dialog
      .open(ServicosNaoConformidadesEditorModalComponent, {
        action,
        data
      } as EditorModalData<ServicosNaoConformidadesModel>, { maxWidth: '30%' })
      .afterClosed()
      .subscribe((solucao: ServicosNaoConformidadesModel) => {
        if (solucao) {
          this.gridOptions.refresh();
        }
      });
  }

  private get(input: VsGridGetInput, id: string) {
    return this.service
      .getList(input, id)
      .pipe(
        map(
          (pagedResult: IPagedResultOutputDto<ServicosNaoConformidadesModel>) => new VsGridGetResult(pagedResult.items, pagedResult.totalCount),id
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
        this.canAddServicoNaoConformidade = this.naoConformidadesEditorService.naoConformidadeNaoConcluida
          && !this.naoConformidadesEditorService.naoConformidadeTemRetrabalhoGerado;
        this.gridOptions.refreshActions();
      })
    );
  }
}
