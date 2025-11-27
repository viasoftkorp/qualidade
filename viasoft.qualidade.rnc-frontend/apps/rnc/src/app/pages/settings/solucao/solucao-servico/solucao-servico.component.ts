import { DecimalPipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { VsAuthorizationService, IPagedResultOutputDto, JQQB_NUMBER_OPERATORS } from '@viasoft/common';
import {
  VsGridOptions, VsDialog, VsGridSimpleColumn, VsGridGetInput, VsGridGetResult
} from '@viasoft/components';
import { SolucaoServicoModel } from '@viasoft/rnc/api-clients/Solucoes/model/solucao-servico-model';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { formatNumberToDecimal } from '@viasoft/rnc/app/tokens/consts/format-number-to-decimal';
import { map } from 'rxjs/operators';
import { SolucaoService } from '../solucao.service';
import { SolucaoServicoEditorModalComponent } from './solucao-servico-editor-modal/solucao-servico-editor-modal.component';

@Component({
  selector: 'rnc-solucao-servico',
  templateUrl: './solucao-servico.component.html',
  styleUrls: ['./solucao-servico.component.scss'],
  providers: [DecimalPipe]
})
export class SolucaoServicoComponent implements OnInit {
  public gridOptions: VsGridOptions;
  public permissions: Map<string, boolean> = new Map<string, boolean>();
  public loaded = true;
  public solucaoId: string;

  constructor(
    private service: SolucaoService,
    private dialog: VsDialog,
    private authorizationService: VsAuthorizationService,
    private router: Router,
    private route: ActivatedRoute,
    private decimalPipe: DecimalPipe

  ) {
    this.gridInit();
    this.solucaoId = route.snapshot.parent.paramMap.get('id');
  }

  ngOnInit(): void { // vazio
  }

  public novo(): void {
    this.openEditorModal(EditorAction.Create,
      <SolucaoServicoModel>{
        idSolucao: this.solucaoId
      });
  }

  private gridInit() {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = 'B8BB360F-BB6A-4ECA-9C04-DCD0B50E4714';
    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'Solucao.Servico.Recurso',
        field: 'recurso',
      }),
      new VsGridSimpleColumn({
        headerName: 'Solucao.Servico.HorasPrevistas',
        kind: 'number',
        field: 'horas',
        filterOptions: {
          operators: JQQB_NUMBER_OPERATORS
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'Solucao.Servico.MinutosPrevistos',
        field: 'minutos',
        kind: 'number',
        filterOptions: {
          operators: JQQB_NUMBER_OPERATORS
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'Solucao.Servico.OpEngenharia',
        field: 'operacaoEngenharia'
      }),
    ];
    this.gridOptions.get = (input: VsGridGetInput) => this.get(input, this.solucaoId);
    this.gridOptions.select = (index: number, data: SolucaoServicoModel) => this.openEditorModal(EditorAction.Update, data);
    this.gridOptions.delete = (index: number, data: SolucaoServicoModel) => this.delete(data.id, data.idSolucao);
    this.gridOptions.deleteBehaviours = {
      enableAutoDeleteConfirm: true,
    };
  }

  private openEditorModal(action: EditorAction, solucaoServico: SolucaoServicoModel): void {
    this.dialog.open(
      SolucaoServicoEditorModalComponent,
      new EditorModalData<SolucaoServicoModel>(action, solucaoServico),
      { maxWidth: '30%' }
    ).afterClosed().subscribe((solucao: SolucaoServicoModel) => {
      if (solucao) {
        this.gridOptions.refresh();
      }
    });
  }

  private get(input: VsGridGetInput, id: string) {
    return this.service.getSolucaoServicosList(input, id)
      .pipe(
        map((pagedResult: IPagedResultOutputDto<SolucaoServicoModel>) => new
        VsGridGetResult(pagedResult.items, pagedResult.totalCount), id)
      );
  }

  private delete(id: string, idSolucao: string): void {
    this.service.deleteServico(id, idSolucao).subscribe(() => {
      this.gridOptions.refresh();
    });
  }
}
