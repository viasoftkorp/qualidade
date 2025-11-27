import { Component, OnInit } from '@angular/core';
import { VsDialog, VsGridGetInput, VsGridGetResult, VsGridOptions, VsGridSimpleColumn } from '@viasoft/components';
import { ConfiguracoesService } from '../configuracoes.service';
import { map } from 'rxjs/operators';
import { GetAllPlanoAmostragem } from '../../../tokens/interfaces/planos-amostragem-dto.interface';
import { PlanoAmostragemEditorComponent } from './plano-amostragem-editor/plano-amostragem-editor.component';
import { JQQB_NUMBER_OPERATORS, JQQB_OP_GREATER_OR_EQUAL } from '@viasoft/common';

@Component({
  selector: 'inspecao-entrada-plano-amostragem',
  templateUrl: './plano-amostragem.component.html',
  styleUrls: ['./plano-amostragem.component.css']
})
export class PlanoAmostragemComponent implements OnInit {

  public gridOptions: VsGridOptions = new VsGridOptions();

  constructor(private configuracoesService: ConfiguracoesService, private vsDialog: VsDialog) { }

  ngOnInit(): void {
    this.iniciarGrid();
  }

  private iniciarGrid(): void {
    this.gridOptions.id = '08A53CD1-07F8-4264-B50C-7226474F6FC9';
    this.gridOptions.sizeColumnsToFit = false;

    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.PlanoAmostragem.QuantidadeMinima',
        field: 'quantidadeMinima',
        kind: 'number',
        filterOptions: {
          operators: JQQB_NUMBER_OPERATORS,
          defaultOperator: JQQB_OP_GREATER_OR_EQUAL,
          useField: 'QTD_MIN'
        },
        sorting: {
          useField:  'QTD_MIN'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.PlanoAmostragem.QuantidadeMaxima',
        field: 'quantidadeMaxima',
        kind: 'number',
        filterOptions: {
          operators: JQQB_NUMBER_OPERATORS,
          defaultOperator: JQQB_OP_GREATER_OR_EQUAL,
          useField: 'QTD_MAX'
        },
        sorting: {
          useField:  'QTD_MAX'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'Configuracoes.PlanoAmostragem.QuantidadeInspecionar',
        field: 'quantidadeInspecionar',
        kind: 'number',
        filterOptions: {
          operators: JQQB_NUMBER_OPERATORS,
          defaultOperator: JQQB_OP_GREATER_OR_EQUAL,
          useField: 'QTD_INSPEC'
        },
        sorting: {
          useField:  'QTD_INSPEC'
        }
      }),
    ];

    this.gridOptions.get = (input: VsGridGetInput) => this.getGridData(input);
    this.gridOptions.edit = (index: number, data) => {
      this.vsDialog.open(PlanoAmostragemEditorComponent, data).afterClosed().toPromise().then(() => {
        this.gridOptions.refresh();
      });
    };
    this.gridOptions.select = (index: number, data) => {
      this.vsDialog.open(PlanoAmostragemEditorComponent, data).afterClosed().toPromise().then(() => {
        this.gridOptions.refresh();
      });
    };
    this.gridOptions.delete = async (index: number, data) => {
      await this.configuracoesService.removePlanoAmostragem(data.id);
      this.gridOptions.refresh();
    }
  }

  private getGridData(input: VsGridGetInput) {
    return this.configuracoesService.getPlanosAmostragem(input)
      .pipe(
        map((pagedHistorico: GetAllPlanoAmostragem) => new VsGridGetResult(pagedHistorico.items, pagedHistorico.totalCount))
      );
  }

  public novoPlanoAmostragem() {
    this.vsDialog.open(PlanoAmostragemEditorComponent, null).afterClosed().toPromise().then(() => {
      this.gridOptions.refresh();
    });
  }

}
