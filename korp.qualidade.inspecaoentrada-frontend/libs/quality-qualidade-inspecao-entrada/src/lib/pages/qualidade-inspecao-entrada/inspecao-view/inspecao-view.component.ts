import { HttpErrorResponse } from '@angular/common/http';
import {
  Component,
  OnDestroy,
  OnInit
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {
  MessageService,
  VsSubscriptionManager
} from '@viasoft/common';
import {
  VsDialog,
  VsGridDateColumn,
  VsGridGetInput,
  VsGridGetResult,
  VsGridNumberColumn,
  VsGridOptions,
  VsGridSimpleColumn,
} from '@viasoft/components';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { QualidadeInspecaoEntradaService } from '../../../services/qualidade-inspecao-entrada.service';
import {
  GetInspecaoEntradaDTO, InspecaoDetailsDTO, InspecaoEntradaDTO, NotaFiscalDTO
} from '../../../tokens';
import { getErrorMessage } from '../../../tokens/functions';
import { InspecaoDetailsComponent } from '../inspecao-details/inspecao-details.component';

@Component({
  selector: 'qa-inspecao-view',
  templateUrl: './inspecao-view.component.html',
  styleUrls: ['./inspecao-view.component.scss'],
})
export class InspecaoViewComponent implements OnInit, OnDestroy {
  private subs = new VsSubscriptionManager();
  private notaDto: NotaFiscalDTO;
  public gridOptions = new VsGridOptions();

  constructor(
    private vsDialog: VsDialog,
    private matDialog: MatDialog,
    private inspecaoEntradaService: QualidadeInspecaoEntradaService,
    private messageService: MessageService
  ) {
  }

  ngOnInit(): void {
    this.subs.add('get-notaFiscal', this.inspecaoEntradaService
      .notaFiscalSelecionada
      .subscribe((notaFiscal: NotaFiscalDTO) => {
        this.notaDto = notaFiscal;
        this.gridOptions.refresh();
      }));

    this.initGrid();
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  private initGrid(): void {
    this.gridOptions.id = 'f4a80bcf-ae40-4d88-a393-034509444cdc';
    this.gridOptions.enableFilter = false;
    this.gridOptions.enableQuickFilter = false;
    this.gridOptions.enableSorting = false;
    this.gridOptions.sizeColumnsToFit = false;

    this.gridOptions.columns = [
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.NotaFiscal',
        field: 'notaFiscal',
        width: 80,
      }),
      new VsGridDateColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.DataInspecao',
        field: 'dataInspecao',
        width: 100,
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.Inspetor',
        field: 'inspetor',
        width: 150,
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.Resultado',
        field: 'resultado',
        width: 150,
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.QuantidadeInspecao',
        field: 'quantidadeInspecao',
        width: 100,
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.QuantidadeLote',
        field: 'quantidadeLote',
        width: 100,
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.QuantidadeAceita',
        field: 'quantidadeAceita',
        width: 100,
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.QuantidadeAprovada',
        field: 'quantidadeAprovada',
        width: 100,
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.QuantidadeReprovada',
        field: 'quantidadeReprovada',
        width: 100,
      }),
    ];

    this.gridOptions.get = (input: VsGridGetInput) => this.getGridData(input);
    this.gridOptions.select = (rowIndex, data: InspecaoEntradaDTO) => this.editarInspecao(data);

    this.gridOptions.actions = [{
      icon: 'search',
      tooltip: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.EditarInspecao',
      callback: (rowIndex: number, data: InspecaoEntradaDTO) => this.editarInspecao(data),
      condition: (rowIndex: number, data: InspecaoEntradaDTO) => !data.resultado
    }, {
      icon: 'trash-alt',
      tooltip: 'QualidadeInspecaoEntrada.InspecaoEntradaGrid.ExcluirInspecao',
      callback: (rowIndex: number, data: InspecaoEntradaDTO) => this.excluirInspecao(data.codigoInspecao),
    }];
  }

  private getGridData(input: VsGridGetInput): Observable<VsGridGetResult> {
    if (!this.notaDto) {
      return of(new VsGridGetResult(null, 0));
    }

    return this.inspecaoEntradaService
      .getInspecoesEntrada(input, this.notaDto.notaFiscal, this.notaDto.lote)
      .pipe(
        map((r: GetInspecaoEntradaDTO) => new VsGridGetResult(r.items, r.totalCount)),
      );
  }

  private editarInspecao(dto: InspecaoEntradaDTO): void {
    if (!dto.resultado) {
      const data = {
        notaFiscal: this.notaDto,
        codigoProduto: this.notaDto.codigoProduto,
        novaInspecao: false,
        codigoInspecao: dto.codigoInspecao
      } as InspecaoDetailsDTO;
      const dialogOptions = this.vsDialog.generateDialogConfig(data, {
        hasBackdrop: true
      });
      this.matDialog.open(InspecaoDetailsComponent, dialogOptions).afterClosed().toPromise().then(() => {
        this.gridOptions.refresh();
        this.inspecaoEntradaService.refreshNotaFiscalGrid.next(undefined);
      });
    }
  }

  private excluirInspecao(codigoInspecao: number): void {
    this.inspecaoEntradaService.excluirInspecaoEntrada(codigoInspecao).subscribe(() => {
      this.gridOptions.refresh(true);
      this.inspecaoEntradaService.refreshNotaFiscalGrid.next(undefined);
    }, (err: HttpErrorResponse) => {
      this.messageService.error(getErrorMessage(err));
    });
  }
}
