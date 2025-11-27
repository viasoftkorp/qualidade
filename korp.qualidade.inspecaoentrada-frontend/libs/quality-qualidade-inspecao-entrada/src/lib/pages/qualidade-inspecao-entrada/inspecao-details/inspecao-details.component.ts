import { HttpErrorResponse } from '@angular/common/http';
import {
  Component,
  Inject,
  OnDestroy,
} from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup, ValidationErrors, ValidatorFn,
  Validators
} from '@angular/forms';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  Router
} from '@angular/router';
import {
  JQQB_COND_OR,
  JQQB_OP_EQUAL,
  MessageService,
  VsSubscriptionManager
} from '@viasoft/common';
import {
  VsDialog,
  VsFilterOptions,
  VsGridGetInput,
  VsGridGetResult,
  VsGridNumberColumn,
  VsGridOptions,
  VsGridSimpleColumn
} from '@viasoft/components';
import {
  Observable,
  of,
  finalize
} from 'rxjs';
import {
  map,
} from 'rxjs/operators';
import { QualidadeInspecaoEntradaService } from '../../../services/qualidade-inspecao-entrada.service';
import {
  AtualizarInspecaoInput,
  FinalizarInspecaoModalData,
  GetInspecaoEntradaItensDTO,
  InspecaoEntradaDTO,
  InspecaoEntradaItemDTO
} from '../../../tokens';
import { ResultadosInspecao } from '../../../tokens/enums/resultados-inspeção.enum';
import { getErrorMessage } from '../../../tokens/functions';
import { GetPlanosInspecaoDTO, PlanoInspecaoDTO } from '../../../tokens/interfaces/get-planos-inspecao-dto.interface';
import { InspecaoDetailsDTO } from '../../../tokens/interfaces/inspecao-details-dto.class';
import { NovaInspecaoInput } from '../../../tokens/interfaces/nova-inspecao-input.interface';
import { AlterarDadosInspecaoModalComponent } from './alterar-dados-inspecao-modal/alterar-dados-inspecao-modal.component';
import { FinalizarInspecaoModalComponent } from './finalizar-inspecao-modal/finalizar-inspecao-modal.component';

@Component({
  selector: 'qa-inspecao-details',
  templateUrl: './inspecao-details.component.html',
  styleUrls: ['./inspecao-details.component.scss']
})
export class InspecaoDetailsComponent implements OnDestroy {
  private subs = new VsSubscriptionManager();
  public inspecaoDetalhes: InspecaoDetailsDTO
  public form: FormGroup;
  public inspecaoDto: InspecaoEntradaDTO;
  public gridOptions = new VsGridOptions();
  public desabilitarFinalizar = true;
  public salvando = false;
  public planosAlterados = new Map<string, PlanoInspecaoDTO>();
  public itensAlterados = new Map<string, InspecaoEntradaItemDTO>();
  public loadingGrid = true;

  public get finalizarDesabilitado(): boolean {
    return this.desabilitarFinalizar || this.inspecaoDetalhes.novaInspecao || this.salvando;
  }

  public get salvarDesabilitado(): boolean {
    if (this.inspecaoDetalhes.novaInspecao) {
      return this.salvando || this.form.invalid;
    }
    return this.salvando || this.form.invalid || !this.itensAlterados.size;
  }

  public get quantidadeValida(): boolean {
    return this.form.get('quantidadeInspecao').value <= this.form.get('quantidadeInspecionar').value
      && this.form.get('quantidadeInspecao').value > 0;
  }

  public get mostrarErroQuantidadeInspecao(): boolean {
    return this.form.get('quantidadeInspecao').dirty && !this.quantidadeValida;
  }

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private inspecaoEntradaService: QualidadeInspecaoEntradaService,
    private vsDialog: VsDialog,
    private matDialog: MatDialog,
    private messageService: MessageService,
    @Inject(MAT_DIALOG_DATA) data: InspecaoDetailsDTO
  ) {
    this.inspecaoDetalhes = data;
    this.initForm();
    if (!this.inspecaoDetalhes.novaInspecao) {
      this.inspecaoDetalhes.codigoInspecao = data.codigoInspecao;
      this.buscarInspecao(this.inspecaoDetalhes.codigoInspecao, true);
    } else {
      this.initGrid();
    }
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  public salvarInspecao(): void {
    if (this.salvando) {
      return;
    }

    this.salvando = true

    if (this.inspecaoDetalhes.novaInspecao) {
      const input: NovaInspecaoInput = {
        recnoItemNotaFiscal: this.inspecaoDetalhes.notaFiscal.recno,
        notaFiscal: this.inspecaoDetalhes.notaFiscal.notaFiscal,
        codigoProduto: this.inspecaoDetalhes.notaFiscal.codigoProduto,
        plano: this.inspecaoDetalhes.notaFiscal.plano,
        planosInspecao: Array.from(this.planosAlterados.values()),
        lote: this.inspecaoDetalhes.notaFiscal.lote,
        quantidade: Number(this.form.get('quantidadeInspecao').value),
        serieNotaFiscal: this.inspecaoDetalhes.notaFiscal.serie
      };
      this.subs.add('nova-inspecao', this.inspecaoEntradaService.criarInspecao(input)
        .subscribe((codigoInspecao: number) => {
          if (codigoInspecao) {
            this.inspecaoDetalhes.codigoInspecao = codigoInspecao;
            this.inspecaoDetalhes.novaInspecao = false;
            const dialogOptions = this.vsDialog.generateDialogConfig(this.inspecaoDetalhes, {
              hasBackdrop: true
            });
            this.inspecaoEntradaService.refreshInspecoesNotaFiscalGrid.next();
            const openedDialog = this.matDialog.open(InspecaoDetailsComponent, dialogOptions);
            return openedDialog.afterClosed();
          }
        }, (err: HttpErrorResponse) => {
          this.salvando = false;
          this.messageService.error(getErrorMessage(err));
        }));
    } else {
      const input: AtualizarInspecaoInput = {
        codigoInspecao: this.inspecaoDto.codigoInspecao,
        itens: Array.from(this.itensAlterados.values()),
        quantidadeInspecao: Number(this.form.get('quantidadeInspecao').value)
      };
      this.subs.add('atualizar-inspecao', this.inspecaoEntradaService.atualizarInspecao(input)
        .pipe(finalize(() => {
          this.salvando = false;
        }))
        .subscribe(() => {
          this.gridOptions.refresh(true);
          this.itensAlterados.clear();
          this.buscarInspecao(input.codigoInspecao, false);
          this.inspecaoEntradaService.refreshInspecoesNotaFiscalGrid.next();
        }, (err: HttpErrorResponse) => {
          this.messageService.error(getErrorMessage(err));
        }));
    }
  }

  public excluirInspecao(): void {
    this.inspecaoEntradaService.excluirInspecaoEntrada(this.inspecaoDto.codigoInspecao).subscribe(() => {
      this.matDialog.closeAll();
    }, (err: HttpErrorResponse) => {
      this.messageService.error(getErrorMessage(err));
    });
  }

  public finalizarInspecao(): void {
    const dialogOptions = this.vsDialog.generateDialogConfig({
      notaFiscal: this.inspecaoDetalhes.notaFiscal,
      inspecaoEntrada: this.inspecaoDto,
      codigoProduto: this.inspecaoDetalhes.codigoProduto,
      codigoFornecedor: this.inspecaoDetalhes.codigoFornecedor,
    } as FinalizarInspecaoModalData, {
      hasBackdrop: true
    });

    this.matDialog.open(FinalizarInspecaoModalComponent, dialogOptions);
  }

  private async initForm(): Promise<void> {
    this.form = this.formBuilder.group({
      notaFiscal: [{ value: this.inspecaoDetalhes.notaFiscal.notaFiscal, disabled: true }],
      codigoProduto: [{ value: this.inspecaoDetalhes.notaFiscal.codigoProduto, disabled: true }],
      quantidadeInspecionada: [{ value: this.inspecaoDetalhes.notaFiscal.quantidadeInspecionada, disabled: true }],
      quantidadeInspecionar: [{ value: this.inspecaoDetalhes.notaFiscal.quantidadeInspecionar, disabled: true }],
      quantidadeInspecao: [{ value: this.inspecaoDto?.quantidadeInspecao, disabled: !this.inspecaoDetalhes.novaInspecao }, [Validators.required, greaterThanZeroValidator(), Validators.max(!this.inspecaoDetalhes.novaInspecao ? this.inspecaoDto?.quantidadeInspecao : this.inspecaoDetalhes.notaFiscal.quantidadeInspecionar)]],
    });
    if (this.inspecaoDetalhes.novaInspecao) {
      this.inspecaoEntradaService.getFaixaInspecao(this.inspecaoDetalhes.notaFiscal.quantidadeInspecionar).then(result => {
        if (result) {
          this.form.get('quantidadeInspecao').setValue(result.quantidadeInspecionar);
        }
      });
    }
  }

  private initGrid(): void {
    this.gridOptions.id = '06992453-2DAF-4C8A-9A05-D110E6990CCF';

    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.Descricao',
        field: 'descricao',
        width: 100,
        filterOptions: {
          useField: 'QA_ITEM_INSPECAO_ENTRADA.DESCRICAO_PLANO '
        },
        sorting: {
          useField: 'QA_ITEM_INSPECAO_ENTRADA.DESCRICAO_PLANO '
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.Resultado',
        field: 'resultado',
        width: 100,
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.MenorValor',
        field: 'menorValorInspecionado',
        width: 50,
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.MaiorValor',
        field: 'maiorValorInspecionado',
        width: 50,
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.MenorValorBase',
        field: 'menorValorBase',
        width: 70,
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.MaiorValorBase',
        field: 'maiorValorBase',
        width: 70,
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.Observacao',
        field: 'observacao',
        width: 100,
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
    ];

    this.gridOptions.get = (input) => this.getGridData(input);
    this.loadingGrid = false;
    this.gridOptions.actions = [{
      icon: 'pen',
      tooltip: 'QualidadeInspecaoEntrada.InspecaoDetails.AlterarDadosInspecaoModal.Titulo',
      callback: (rowIndex, data) => (

        this.inspecaoDetalhes.novaInspecao
          ? this.abrirModalAlterarDadosPlano(data, this.inspecaoDetalhes) : this.abrirModalAlterarDadosItem(data, this.inspecaoDetalhes))
    }];
  }

  private getGridData(input: VsGridGetInput): Observable<VsGridGetResult> {
    if (!this.inspecaoDetalhes.notaFiscal.notaFiscal || !this.inspecaoDetalhes.notaFiscal.codigoProduto) {
      return of(new VsGridGetResult(null, 0));
    }

    return this.getGridItemsMethod(input)
      .pipe(
        map((result: GetPlanosInspecaoDTO | GetInspecaoEntradaItensDTO) => {
          if (result && result.items) {
            this.desabilitarFinalizar = false;
            if (this.inspecaoDto) {
              this.inspecaoDto.resultado = ResultadosInspecao.Aprovado;
            }

            for (let i = 0; i < result.items.length; i++) {
              let itemAlterado: PlanoInspecaoDTO | InspecaoEntradaItemDTO;

              if (this.inspecaoDetalhes.novaInspecao) {
                itemAlterado = this.planosAlterados?.get(result.items[i].id);
              } else {
                itemAlterado = this.itensAlterados?.get(result.items[i].id);
              }

              if (itemAlterado) {
                result.items[i] = itemAlterado;
              }

              if (!result.items[i].resultado) {
                this.desabilitarFinalizar = true;
              }

              if (result.items[i].resultado === ResultadosInspecao.NaoConforme && this.inspecaoDto) {
                this.inspecaoDto.resultado = ResultadosInspecao.NaoConforme;
              }
            }
            return new VsGridGetResult(result.items, result.totalCount);
          }
          return new VsGridGetResult(null, 0);
        }),
      );
  }

  private getGridItemsMethod(input: VsGridGetInput): Observable<GetPlanosInspecaoDTO | GetInspecaoEntradaItensDTO> {
    if (this.inspecaoDetalhes.novaInspecao) {
      return this.inspecaoEntradaService.getPlanosNovaInspecao(
        input, this.inspecaoDetalhes.notaFiscal.plano,
        this.inspecaoDetalhes.notaFiscal.codigoProduto
      );
    }
    return this.inspecaoEntradaService.getInspecaoEntradaItens(this.inspecaoDto?.codigoInspecao, this.inspecaoDetalhes.codigoProduto, input);
  }

  private abrirModalAlterarDadosPlano(dto: PlanoInspecaoDTO, inspecaoDetais: InspecaoDetailsDTO): void {
    this.subs.add('modal-alterar-dados-plano', this.vsDialog.open(
      AlterarDadosInspecaoModalComponent,
      { dto, inspecaoDetais },
      { autoCloseWhenChangeRoute: false, allowReOpenSameObject: true }
    )
      .afterClosed()
      .subscribe((dtoAlterado: PlanoInspecaoDTO) => {
        if (dtoAlterado) {
          this.planosAlterados.set(dtoAlterado.id, dtoAlterado);
          this.gridOptions.refresh();
        }
      }));
  }

  private abrirModalAlterarDadosItem(dto: InspecaoEntradaItemDTO, inspecaoDetais: InspecaoDetailsDTO): void {
    this.subs.add('modal-alterar-dados-item', this.vsDialog.open(
      AlterarDadosInspecaoModalComponent,
      { dto, inspecaoDetais },
      { autoCloseWhenChangeRoute: false, allowReOpenSameObject: true }
    )
      .afterClosed()
      .subscribe((dtoAlterado: InspecaoEntradaItemDTO) => {
        if (dtoAlterado) {
          this.itensAlterados.set(dtoAlterado.id, dtoAlterado);
          this.gridOptions.refresh();
        }
      }));
  }

  private buscarInspecao(codigoInspecao: number, setarValores: boolean): void {
    this.subs.add('buscar-inspecao', this.inspecaoEntradaService.getInspecaoEntrada(codigoInspecao)
      .subscribe((inspecao: InspecaoEntradaDTO) => {
        this.inspecaoDto = inspecao;
        if (setarValores) {
          this.form.get('quantidadeInspecao').setValue(inspecao.quantidadeInspecao);
          this.initGrid();
        }
      }, (err: HttpErrorResponse) => {
        this.subs.add('erro', this.messageService.error(getErrorMessage(err))
          .subscribe(() => {
            this.router.navigateByUrl('');
          }));
      }));
  }

  private get resultadoFilterOptions(): VsFilterOptions {
    return {
      operators: [JQQB_OP_EQUAL],
      conditions: [JQQB_COND_OR],
      blockInput: true,
      useField: 'QA_ITEM_INSPECAO_ENTRADA.RESULTADO',
      mode: 'selection',
      multiple: true,
      getItems: () => of({
        items: [
          {
            key: ResultadosInspecao.Aprovado.toString(),
            value: `QualidadeInspecaoEntrada.Resultados.Aprovado`,
          },
          {
            key: ResultadosInspecao.ParcialmenteAprovado.toString(),
            value: `QualidadeInspecaoEntrada.Resultados.ParcialmenteAprovado`,
          },
          {
            key: ResultadosInspecao.NaoAplicavel.toString(),
            value: `QualidadeInspecaoEntrada.Resultados.NaoAplicavel`,
          },
          {
            key: ResultadosInspecao.NaoConforme.toString(),
            value: `QualidadeInspecaoEntrada.Resultados.NaoConforme`,
          }
        ],
        totalCount: 3
      })
    };
  }
}

export function greaterThanZeroValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (value != null && value <= 0) {
      return { greaterThanZero: true };
    }
    return null;
  };
}
