import {Component, Inject} from '@angular/core';
import {VsGridGetInput, VsGridGetResult, VsGridOptions, VsGridSimpleColumn} from '@viasoft/components';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {IPagedResultOutputDto} from "@viasoft/common";
import {NotasSelectModalService} from "./notas-select-modal.service";

export class NotaFiscalDto {
  public nota: number;
  public numeroNota: number;
  public clienteCodigo: string;
  public clienteRazaoSocial: string;
  public quantidadeLote: number;
}

@Component({
  selector: 'qa-notas-select-modal',
  templateUrl: './notas-select-modal.component.html',
  styleUrl: './notas-select-modal.component.scss',
  providers: [NotasSelectModalService]
})
export class NotasSelectModalComponent {
  public gridOptions: VsGridOptions;

  constructor(private service: NotasSelectModalService,
              private dialogRef: MatDialogRef<NotasSelectModalComponent>,
              @Inject(MAT_DIALOG_DATA) private codigoInspecao: number) {
    this.setGridOptions();
  }

  public cancel(): void {
    this.dialogRef.close();
  }

  private setGridOptions(): void {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = '3bf17623-39a9-4adc-9424-f7cf585ac398';
    this.gridOptions.persistentCache = true;
    this.gridOptions.columns = this.getGridColumns();
    this.gridOptions.get = (input: VsGridGetInput) => this.get(input);
    this.gridOptions.select = (rowIndex, nota: NotaFiscalDto) => this.select(nota);
  }

  private getGridColumns(): VsGridSimpleColumn[] {
    return [
      new VsGridSimpleColumn({ headerName: 'Nota Fiscal', field: 'numeroNota', filterOptions: { useField: 'NotaFiscal.NUMERO' }, sorting: { useField: 'NotaFiscal.NUMERO'}, width: 20 }),
      new VsGridSimpleColumn({ headerName: 'Código Cliente', field: 'clienteCodigo', filterOptions: { useField: 'NotaFiscal.CLI' }, sorting: { useField: 'NotaFiscal.CLI'} , width: 20 }),
      new VsGridSimpleColumn({ headerName: 'Cliente', field: 'clienteRazaoSocial', filterOptions: { useField: 'NotaFiscal.RAZAO' }, sorting: { useField: 'NotaFiscal.RAZAO'} , width: 60 }),
    ];
  }

  private get(input: VsGridGetInput): Observable<VsGridGetResult> {
    return this.service
      .getList(this.codigoInspecao, input.advancedFilter, input.sorting, input.skipCount, input.maxResultCount)
      .pipe(
        map((paged: IPagedResultOutputDto<NotaFiscalDto>) => new VsGridGetResult(paged.items, paged.totalCount))
      );
  }

  private select(nota: NotaFiscalDto): void {
    this.dialogRef.close(nota);
  }
}
