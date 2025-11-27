import { Component, OnInit } from '@angular/core';
import {
  VsDialog,
  VsGridGetInput,
  VsGridGetResult,
  VsGridOptions,
  VsGridSimpleColumn
} from '@viasoft/components';
import { SolucaoService } from '@viasoft/rnc/app/pages/settings/solucao/solucao.service';
import { IPagedResultOutputDto, JQQB_NUMBER_OPERATORS, VsAuthorizationService } from '@viasoft/common';
import { ActivatedRoute, Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { SolucaoProdutoModel } from '@viasoft/rnc/api-clients/Solucoes/model/solucao-produto-model';
import {
  SolucaoProdutoEditorModalComponent
} from '@viasoft/rnc/app/pages/settings/solucao/solucao-produto/solucao-produto-editor-modal/solucao-produto-editor-modal.component';
import { DecimalPipe } from '@angular/common';
import { formatNumberToDecimal } from '@viasoft/rnc/app/tokens/consts/format-number-to-decimal';

@Component({
  selector: 'rnc-solucao-produto',
  templateUrl: './solucao-produto.component.html',
  styleUrls: ['./solucao-produto.component.scss'],
  providers: [DecimalPipe]
})
export class SolucaoProdutoComponent implements OnInit {
  private solucaoId: string;
  public gridOptions: VsGridOptions;
  public permissions: Map<string, boolean> = new Map<string, boolean>();
  public loaded = true;

  constructor(
    private service: SolucaoService,
    private dialog: VsDialog,
    private authorizationService:VsAuthorizationService,
    private router:Router,
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
    <SolucaoProdutoModel> {
      idSolucao: this.solucaoId
    });
  }

  private gridInit() {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = '08268198-5A64-475F-811A-8982044B06B7';
    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'Solucao.Produto.Codigo',
        field: 'codigo',
      }),
      new VsGridSimpleColumn({
        headerName: 'Solucao.Produto.Descricao',
        field: 'descricao',
      }),
      new VsGridSimpleColumn({
        headerName: 'Solucao.Produto.Unidade',
        field: 'unidadeMedida'
      }),
      new VsGridSimpleColumn({
        headerName: 'Solucao.Produto.Quantidade',
        field: 'quantidade',
        kind: 'number',
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade, 6),
        filterOptions: {
          operators: JQQB_NUMBER_OPERATORS,
        }
      })
    ];
    this.gridOptions.get = (input: VsGridGetInput) => this.get(input, this.solucaoId);
    this.gridOptions.select = (index: number, data: SolucaoProdutoModel) => this.openEditorModal(EditorAction.Update, data);
    this.gridOptions.delete = (index: number, data: SolucaoProdutoModel) => this.delete(data.id, data.idSolucao);
    this.gridOptions.deleteBehaviours = {
      enableAutoDeleteConfirm: true,
    };
  }

  private openEditorModal(action: EditorAction, solucaoProduto: SolucaoProdutoModel): void {
    this.dialog.open(
      SolucaoProdutoEditorModalComponent,
      new EditorModalData<SolucaoProdutoModel>(action, solucaoProduto),
      { maxWidth: '30%' }
    ).afterClosed().subscribe((solucao: SolucaoProdutoModel) => {
      if (solucao) {
        this.gridOptions.refresh();
      }
    });
  }

  private get(input: VsGridGetInput, id: string) {
    return this.service.getSolucaoProdutosList(input, id)
      .pipe(
        map((pagedResult: IPagedResultOutputDto<SolucaoProdutoModel>) => new
        VsGridGetResult(pagedResult.items, pagedResult.totalCount), id)
      );
  }

  private delete(id: string, idSolucao: string): void {
    this.service.deleteProduto(id, idSolucao).subscribe(() => {
      this.gridOptions.refresh();
    });
  }
}
