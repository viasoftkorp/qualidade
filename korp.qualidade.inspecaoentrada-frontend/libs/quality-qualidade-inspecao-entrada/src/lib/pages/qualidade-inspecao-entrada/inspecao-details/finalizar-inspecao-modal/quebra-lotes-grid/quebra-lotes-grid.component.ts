import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import {
  EstoqueLocalPedidoVendaAlocacaoDTO,
} from '../../../../../tokens';
import {
  VsDialog,
  VsGridGetInput, VsGridGetResult,
  VsGridNumberColumn,
  VsGridOptions,
} from '@viasoft/components';
import { MatDialog } from '@angular/material/dialog';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  GetPedidoVendaLoteDto,
  PedidoVendaLoteDto
} from '../../../../../tokens/interfaces/pedido-venda-lote-interface-dto';
import { QuebraLoteEditorComponent } from './quebra-lote-editor/quebra-lote-editor.component';
import { QuebraLoteService } from './quebra-lote.service';

@Component({
  selector: 'inspecao-entrada-quebra-lotes-grid',
  templateUrl: './quebra-lotes-grid.component.html',
  styleUrls: ['./quebra-lotes-grid.component.scss']
})
export class QuebraLotesGridComponent implements OnInit, OnDestroy {
  @Input() public inspecaoDto: EstoqueLocalPedidoVendaAlocacaoDTO;
  @Input() public recnoInspecao: number;
  @Input() public utilizaReserva: boolean;
  @Input() public codigoProduto: string;
  public gridOptions = new VsGridOptions();

  constructor(
    private vsDialog: VsDialog,
    private matDialog: MatDialog,
    private quebraLoteService: QuebraLoteService
  ) {
  }

  public ngOnDestroy(): void {
    this.quebraLoteService.onDestroy()
  }

  public ngOnInit(): void {
    this.quebraLoteService.utilizaReserva = this.utilizaReserva;

    this.quebraLoteService.onInit();
    this.initGrid();
  }

  public adicionarLote() {
    const dialogOptions = this.vsDialog.generateDialogConfig({
      inspecaoDto: this.inspecaoDto,
      codigoProduto: this.codigoProduto
    }, {
      hasBackdrop: true
    });

    this.matDialog.open(QuebraLoteEditorComponent, dialogOptions).afterClosed()
      .subscribe(() => {
        this.gridOptions.refresh();
      });
  }

  private initGrid(): void {
    this.gridOptions.id = '8790F413-C37B-4251-8950-1DE9FA0B8B72';
    this.gridOptions.sizeColumnsToFit = false;
    this.gridOptions.enableFilter = false;
    this.gridOptions.enableSorting = false;
    this.gridOptions.columns = [
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.NumeroLote',
        field: 'numeroLote'
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.Quantidade',
        field: 'quantidade'
      })
    ];

    this.gridOptions.get = (input: VsGridGetInput) => this.getGridData();
    this.gridOptions.select = (rowIndex, data: PedidoVendaLoteDto) => this.editarLote(data);

    this.gridOptions.actions = [
      {
        icon: 'trash-alt',
        tooltip: 'Apagar',
        callback: (rowIndex: number, data: PedidoVendaLoteDto) => {
          this.deletarLote(data)
        }
      }
    ]
  }

  private getGridData(): Observable<VsGridGetResult> {
    return this.quebraLoteService
      .getLotes(this.recnoInspecao, this.inspecaoDto?.id)
      .pipe(
        map((r: GetPedidoVendaLoteDto) => new VsGridGetResult(r.items, r.totalCount))
      );
  }

  private editarLote(lote: PedidoVendaLoteDto): void {
    const data = {
      lote,
      inspecaoDto: this.inspecaoDto
    }
    const dialogOptions = this.vsDialog.generateDialogConfig(data, {
      hasBackdrop: true
    });
    this.matDialog.open(QuebraLoteEditorComponent, dialogOptions).afterClosed()
      .subscribe(() => {
        this.gridOptions.refresh();
      });
  }

  private deletarLote(lote:PedidoVendaLoteDto): void {
    this.quebraLoteService.deleteLote(lote)
    this.gridOptions.refresh();
  }
}
