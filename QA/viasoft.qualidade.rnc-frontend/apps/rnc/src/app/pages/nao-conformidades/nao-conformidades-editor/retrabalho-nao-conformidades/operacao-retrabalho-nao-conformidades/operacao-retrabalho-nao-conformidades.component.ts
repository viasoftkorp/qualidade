import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import {
  VsColoredTextData,
  VsFilterItem,
  VsFilterOptions,
  VsGridColoredTextColumn,
  VsGridGetInput,
  VsGridGetResult,
  VsGridOptions,
  VsGridSimpleColumn
} from '@viasoft/components';
import { IPagedResultOutputDto, JQQB_OP_EQUAL, VsCommandRunnerService } from '@viasoft/common';
import { map } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
// eslint-disable-next-line max-len
import { OperacaoViewOutput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/operacao-view-output';
// eslint-disable-next-line max-len
import { StatusProducaoRetrabalho } from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/status-producao-retrabalho';
import { TranslatePipe } from '@ngx-translate/core';
import { DefaultSemanticColors } from '@viasoft/rnc/app/tokens/consts/default-semantic-colors.const';
import { OperacaoRetrabalhoNaoConformidadeFormControls } from './operacao-retrabalho-nao-conformidade-form-controls';
import { OperacaoRetrabalhoNaoConformidadesService } from './operacao-retrabalho-nao-conformidades.service';
import { NaoConformidadesEditorService } from '../../nao-conformidades-editor.service';
import { NaoConformidadesFormControl } from '../../nao-conformidades-form-control';

@Component({
  selector: 'rnc-operacao-retrabalho-nao-conformidades',
  templateUrl: './operacao-retrabalho-nao-conformidades.component.html',
  styleUrls: ['./operacao-retrabalho-nao-conformidades.component.scss']
})
export class OperacaoRetrabalhoNaoConformidadesComponent implements OnInit, OnDestroy {
  public form: FormGroup;
  public formControls = OperacaoRetrabalhoNaoConformidadeFormControls;
  public loaded = false;
  public gridOptions: VsGridOptions;
  private naoConformidadeFormGroup:FormGroup;
  private readonly BUSCAR_STATUS_OPERACOES_COMMAND = 'Viasoft.Qualidade.RNC.Core.BuscarStatusOperacoes';

  constructor(
    private formBuilder: FormBuilder,
    private operacaoRetrabalhoNaoConformidadeService: OperacaoRetrabalhoNaoConformidadesService,
    private translatePipe: TranslatePipe,
    private naoConformidadesEditorService: NaoConformidadesEditorService
  ) { }

  public ngOnInit(): void {
    this.createForm();
    this.populateForm();
    this.gridInit();
    this.adicionarComandos();
    this.naoConformidadeFormGroup = this.naoConformidadesEditorService.form;
  }

  public ngOnDestroy(): void {
    VsCommandRunnerService.removeCommand(this.BUSCAR_STATUS_OPERACOES_COMMAND);
  }

  private createForm():void {
    this.form = this.formBuilder.group({});

    this.form.addControl(OperacaoRetrabalhoNaoConformidadeFormControls.numeroOperacaoARetrabalhar,
      this.formBuilder.control(null));

    this.form.addControl(OperacaoRetrabalhoNaoConformidadeFormControls.quantidade,
      this.formBuilder.control(null));
  }
  private populateForm():void {
    const operacaoRetrabalho = { ...this.naoConformidadesEditorService.operacaoRetrabalho };

    this.form.get(OperacaoRetrabalhoNaoConformidadeFormControls.numeroOperacaoARetrabalhar)
      .setValue(operacaoRetrabalho.numeroOperacaoARetrabalhar);
    this.form.get(OperacaoRetrabalhoNaoConformidadeFormControls.quantidade)
      .setValue(operacaoRetrabalho.quantidade);
    this.loaded = true;
  }

  private gridInit() {
    this.gridOptions = new VsGridOptions();
    this.gridOptions.id = '8DA17E08-FB0D-40FD-A2E0-B190477A7471';

    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'NaoConformidades.NaoConformidadesEditor.Retrabalho.OperacaoRetrabalho.OperacaoEngenharia',
        field: 'numeroOperacao',
      }),
      new VsGridSimpleColumn({
        headerName: 'NaoConformidades.NaoConformidadesEditor.Retrabalho.OperacaoRetrabalho.DescricaoRecurso',
        field: 'descricaoRecurso'
      }),
      new VsGridColoredTextColumn({
        headerName: 'NaoConformidades.NaoConformidadesEditor.Retrabalho.OperacaoRetrabalho.Status',
        field: 'coloredStatus',
        filterOptions: this.filterOptionsStatus(),
        sorting: {
          useField: 'status'
        }
      }),
    ];
    this.gridOptions.get = (input: VsGridGetInput) => this.get(input);
  }

  private filterOptionsStatus(): VsFilterOptions {
    const status: Array<VsFilterItem> = [
      {
        key: StatusProducaoRetrabalho.Aberta.toString(),
        value: `NaoConformidades.NaoConformidadesEditor.Retrabalho.OperacaoRetrabalho.StatusOption.${StatusProducaoRetrabalho[StatusProducaoRetrabalho.Aberta]}`,
      },
      {
        key: StatusProducaoRetrabalho.Cancelada.toString(),
        value: `NaoConformidades.NaoConformidadesEditor.Retrabalho.OperacaoRetrabalho.StatusOption.${StatusProducaoRetrabalho[StatusProducaoRetrabalho.Cancelada]}`,
      },
      {
        key: StatusProducaoRetrabalho.Encerrada.toString(),
        value: `NaoConformidades.NaoConformidadesEditor.Retrabalho.OperacaoRetrabalho.StatusOption.${StatusProducaoRetrabalho[StatusProducaoRetrabalho.Encerrada]}`,
      },
      {
        key: StatusProducaoRetrabalho.Produzindo.toString(),
        value: `NaoConformidades.NaoConformidadesEditor.Retrabalho.OperacaoRetrabalho.StatusOption.${StatusProducaoRetrabalho[StatusProducaoRetrabalho.Produzindo]}`,
      },
    ];
    return {
      operators: [
        JQQB_OP_EQUAL
      ],
      blockInput: true,
      useField: 'status',
      mode: 'selection',
      multiple: true,
      getValidItems: (keys) => of(status.filter((item) => keys.includes(item.key))),
      getItems: () => of({
        items: status,
        totalCount: status.length
      })
    };
  }
  private get(input: VsGridGetInput):Observable<VsGridGetResult> {
    return this.operacaoRetrabalhoNaoConformidadeService.getOperacoesView(input)
      .pipe(
        map((pagedResult: IPagedResultOutputDto<OperacaoViewOutput>) => {
          this.setColoredStatusColumn(pagedResult.items);
          return new VsGridGetResult(pagedResult.items, pagedResult.totalCount);
        })
      );
  }

  private setColoredStatusColumn(items: OperacaoViewOutput[]) {
    items.forEach((item: OperacaoViewOutput) => {
      let color: string;

      switch (item.status) {
      case StatusProducaoRetrabalho.Aberta:
        color = DefaultSemanticColors.informative;
        break;
      case StatusProducaoRetrabalho.Cancelada:
        color = DefaultSemanticColors.negative;
        break;
      case StatusProducaoRetrabalho.Encerrada:
        color = DefaultSemanticColors.positive;
        break;
      case StatusProducaoRetrabalho.Produzindo:
        color = DefaultSemanticColors.notice;
        break;
      default:
        color = DefaultSemanticColors.informative;
        break;
      }

      item.coloredStatus = new VsColoredTextData(color,
        this.translatePipe.transform(`NaoConformidades.NaoConformidadesEditor.Retrabalho.OperacaoRetrabalho.StatusOption.${StatusProducaoRetrabalho[item.status]}`))
    });
  }
  private adicionarComandos(): void {
    VsCommandRunnerService.addCommand(this.BUSCAR_STATUS_OPERACOES_COMMAND, (numeroOdf: number) => {
      const numeroOdfAtual = Number(this.naoConformidadeFormGroup.get(NaoConformidadesFormControl.numeroOdf).value);
      if (numeroOdf === numeroOdfAtual) {
        this.gridOptions.refresh();
      }

      return Promise.resolve();
    });
  }
}
