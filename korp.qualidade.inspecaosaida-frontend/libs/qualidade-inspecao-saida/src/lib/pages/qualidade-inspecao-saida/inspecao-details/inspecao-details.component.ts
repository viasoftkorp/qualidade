import { HttpErrorResponse } from '@angular/common/http';
import {
  Component,
  Inject,
  OnDestroy,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {
  Router
} from '@angular/router';
import { DecimalPipe } from '@angular/common';
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
  finalize,
  Observable,
  of
} from 'rxjs';
import {
  map,
} from 'rxjs/operators';
import { QualidadeInspecaoSaidaService } from '../../../services/qualidade-inspecao-saida.service';
import {
  getErrorMessage,
  GetPlanosInspecaoDTO,
  InspecaoSaidaDTO,
  NovaInspecaoInput,
  PlanoInspecaoDTO,
  AtualizarInspecaoInput,
  ResultadosInspecao,
  InspecaoDetailsDTO, formatNumberToDecimal
} from '../../../tokens';
import {
  GetInspecaoSaidaItensDTO,
  InspecaoSaidaItemDTO
} from '../../../tokens/interfaces/get-inspecao-saida-itens-dto.interface';
import { AlterarDadosInspecaoModalComponent } from './alterar-dados-inspecao-modal/alterar-dados-inspecao-modal.component';
import { FinalizarInspecaoModalComponent } from './finalizar-inspecao-modal/finalizar-inspecao-modal.component';
import { IVsDialogBaseOptions } from '@viasoft/components/dialog/src/dialog.tokens';

@Component({
  selector: 'qa-inspecao-details',
  templateUrl: './inspecao-details.component.html',
  styleUrls: ['./inspecao-details.component.scss'],
  providers: [DecimalPipe]
})
export class InspecaoDetailsComponent implements OnDestroy {
  private subs = new VsSubscriptionManager();
  public inspecaoDetalhes: InspecaoDetailsDTO;
  public form: FormGroup;
  public inspecaoDto: InspecaoSaidaDTO;
  public gridOptions = new VsGridOptions();
  public desabilitarFinalizar = true;
  public salvando = false;
  public planosAlterados = new Map<string, PlanoInspecaoDTO>();
  public itensAlterados = new Map<string, InspecaoSaidaItemDTO>();
  public loadingGrid = true;
  public selectedTab: number;

  public get finalizarDesabilitado(): boolean {
    return this.desabilitarFinalizar || this.inspecaoDetalhes.novaInspecao || this.salvando;
  }

  public get salvarDesabilitado(): boolean {
    if (this.inspecaoDetalhes.novaInspecao) {
      return this.salvando || this.form.invalid;
    }
    return this.salvando || this.form.invalid || !this.itensAlterados.size;
  }

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private inspecaoSaidaService: QualidadeInspecaoSaidaService,
    private vsDialog: VsDialog,
    private matDialog: MatDialog,
    private dialogRef: MatDialogRef<InspecaoDetailsComponent>,
    private messageService: MessageService,
    @Inject(MAT_DIALOG_DATA) data: InspecaoDetailsDTO,
    private decimalPipe: DecimalPipe
  ) {
    this.inspecaoDetalhes = data;
    this.initForm();
    if (!this.inspecaoDetalhes.novaInspecao) {
      this.inspecaoDetalhes.codInspecao = data.codInspecao;
      this.buscarInspecao(this.inspecaoDetalhes.codInspecao, true);
    } else {
      this.initGrid();
    }
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  public onSelectedTabIndexChanged(index: number): void {
    this.selectedTab = index;
  }

  public salvarInspecao(): void {
    if (this.salvando) {
      return;
    }

    this.salvando = true

    if (this.inspecaoDetalhes.novaInspecao) {
      const input: NovaInspecaoInput = {
        odf: this.inspecaoDetalhes.odf,
        codProduto: this.inspecaoDetalhes.codProduto,
        plano: this.inspecaoDetalhes.plano,
        planosInspecao: Array.from(this.planosAlterados.values()),
        quantidade: Number(this.form.get('quantidadeInspecao').value),
        lote: this.inspecaoDetalhes.lote,
      };

      this.subs.add('nova-inspecao', this.inspecaoSaidaService.criarInspecao(input)
        .subscribe((inspecao: InspecaoSaidaDTO) => {
          if (inspecao.codigoInspecao) {
            this.inspecaoDetalhes.id = inspecao.id;
            this.inspecaoDetalhes.codInspecao = inspecao.codigoInspecao;
            this.inspecaoDetalhes.novaInspecao = false;

            const dialogConfig =  this.vsDialog.generateDialogConfig(this.inspecaoDetalhes,
              { autoCloseWhenChangeRoute: true, hasBackdrop: true } as IVsDialogBaseOptions)

            this.matDialog.open(InspecaoDetailsComponent, dialogConfig)
              .afterOpened()
              .subscribe(() => this.dialogRef.close());
          }
        }, (err: HttpErrorResponse) => {
          this.salvando = false;
          this.messageService.error(getErrorMessage(err));
        }));
    } else {
      const input: AtualizarInspecaoInput = {
        codInspecao: this.inspecaoDto.codigoInspecao,
        itens: Array.from(this.itensAlterados.values()),
        quantidadeInspecao: Number(this.form.get('quantidadeInspecao').value),
        lote: this.inspecaoDto.lote
      };
      this.subs.add('atualizar-inspecao', this.inspecaoSaidaService.atualizarInspecao(input)
        .pipe(finalize(() => {
          this.salvando = false;
        }))
        .subscribe(() => {
          this.gridOptions.refresh(true);
          this.itensAlterados.clear();
          this.buscarInspecao(input.codInspecao, false);
        }, (err: HttpErrorResponse) => {
          this.messageService.error(getErrorMessage(err));
        }));
    }
  }

  public excluirInspecao(): void {
    this.inspecaoSaidaService.excluirInspecaoSaida(this.inspecaoDto.codigoInspecao).subscribe(() => {
      this.matDialog.closeAll();
    }, (err: HttpErrorResponse) => {
      this.messageService.error(getErrorMessage(err));
    });
  }

  public finalizarInspecao(): void {
    this.vsDialog.open(FinalizarInspecaoModalComponent, this.inspecaoDto, {
      hasBackdrop: true,
      autoCloseWhenChangeRoute: true,
    });
  }

  private initForm(): void {
    this.form = this.formBuilder.group({
      odf: [{ value: this.inspecaoDetalhes.odf, disabled: true }],
      odfApontada: [{ value: this.inspecaoDetalhes.odfApontada, disabled: true }],
      codProduto: [{ value: this.inspecaoDetalhes.codProduto, disabled: true }],
      quantidadeInspecionada: [{ value: this.inspecaoDetalhes.quantidadeInspecionada, disabled: true }],
      quantidadeInspecionar: [{ value: this.inspecaoDetalhes.quantidadeInspecionar, disabled: true }],
      quantidadeInspecao: [{ value: this.inspecaoDto?.quantidadeInspecao, disabled: !this.inspecaoDetalhes.novaInspecao }, [Validators.required, Validators.max(!this.inspecaoDetalhes.novaInspecao ? this.inspecaoDto?.quantidadeInspecao : this.inspecaoDetalhes.quantidadeInspecionar)]],
    });
  }

  private initGrid(): void {
    this.gridOptions.id = '4B8EC73D-4FF8-4563-9528-0603B8B65002';

    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.Descricao',
        field: 'descricao',
        width: 100,
        filterOptions: {
          useField: 'QA_ITEM_INSPECAO_SAIDA.DESCRICAO'
        },
        sorting: {
          useField: 'QA_ITEM_INSPECAO_SAIDA.DESCRICAO'
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.Resultado',
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
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.MenorValor',
        field: 'menorValor',
        width: 50,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.MaiorValor',
        field: 'maiorValor',
        width: 50,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.MenorValorBase',
        field: 'menorValorBase',
        width: 70,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.MaiorValorBase',
        field: 'maiorValorBase',
        width: 70,
        format: (quantidade: number) => formatNumberToDecimal(this.decimalPipe, quantidade),
        filterOptions: {
          disable: true
        },
        sorting: {
          disable: true
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoSaida.InspecaoDetails.Observacao',
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
      tooltip: 'QualidadeInspecaoSaida.InspecaoDetails.AlterarDadosInspecaoModal.Titulo',
      callback: (rowIndex, data) => (
        this.inspecaoDetalhes.novaInspecao
          ? this.abrirModalAlterarDadosPlano(data, this.inspecaoDetalhes) : this.abrirModalAlterarDadosItem(data, this.inspecaoDetalhes))
    }];
  }

  private getGridData(input: VsGridGetInput): Observable<VsGridGetResult> {
    if (!this.inspecaoDetalhes.odf || !this.inspecaoDetalhes.plano) {
      return of(new VsGridGetResult(null, 0));
    }

    return this.getGridItemsMethod(input)
      .pipe(
        map((result: GetPlanosInspecaoDTO | GetInspecaoSaidaItensDTO) => {
          if (result && result.items) {
            this.desabilitarFinalizar = false;
            if (this.inspecaoDto) {
              this.inspecaoDto.resultado = ResultadosInspecao.Aprovado;
            }

            for (let i = 0; i < result.items.length; i++) {
              let itemAlterado: PlanoInspecaoDTO | InspecaoSaidaItemDTO;

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

  private getGridItemsMethod(input: VsGridGetInput): Observable<GetPlanosInspecaoDTO | GetInspecaoSaidaItensDTO> {
    if (this.inspecaoDetalhes.novaInspecao) {
      return this.inspecaoSaidaService.getPlanosNovaInspecao(input, this.inspecaoDetalhes.recnoProcesso, this.inspecaoDetalhes.plano);
    }
    return this.inspecaoSaidaService.getInspecaoSaidaItens(this.inspecaoDto?.codigoInspecao, input);
  }

  private abrirModalAlterarDadosPlano(dto: PlanoInspecaoDTO, inspecaoDetais: InspecaoDetailsDTO): void {
    this.subs.add('modal-alterar-dados-plano', this.vsDialog.open(
      AlterarDadosInspecaoModalComponent,
      { dto, inspecaoDetais },
      { autoCloseWhenChangeRoute: true, allowReOpenSameObject: true, hasBackdrop: true }
    )
      .afterClosed()
      .subscribe((dtoAlterado: PlanoInspecaoDTO) => {
        if (dtoAlterado) {
          this.planosAlterados.set(dtoAlterado.id, dtoAlterado);
          this.gridOptions.refresh();
        }
      }));
  }

  private abrirModalAlterarDadosItem(dto: InspecaoSaidaItemDTO, inspecaoDetais: InspecaoDetailsDTO): void {
    this.subs.add('modal-alterar-dados-item', this.vsDialog.open(
      AlterarDadosInspecaoModalComponent,
      { dto, inspecaoDetais },
      { autoCloseWhenChangeRoute: true, allowReOpenSameObject: true, hasBackdrop: true }
    )
      .afterClosed()
      .subscribe((dtoAlterado: InspecaoSaidaItemDTO) => {
        if (dtoAlterado) {
          this.itensAlterados.set(dtoAlterado.id, dtoAlterado);
          this.gridOptions.refresh();
        }
      }));
  }

  private buscarInspecao(codInspecao: number, setarValores: boolean): void {
    this.subs.add('buscar-inspecao', this.inspecaoSaidaService.getInspecaoSaida(codInspecao)
      .subscribe((inspecao: InspecaoSaidaDTO) => {
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
      useField: 'QA_ITEM_INSPECAO_SAIDA.RESULTADO',
      mode: 'selection',
      multiple: true,
      getItems: () => of({
        items: [
          {
            key: ResultadosInspecao.Aprovado.toString(),
            value: `QualidadeInspecaoSaida.Resultados.Aprovado`,
          },
          {
            key: ResultadosInspecao.ParcialmenteAprovado.toString(),
            value: `QualidadeInspecaoSaida.Resultados.ParcialmenteAprovado`,
          },
          {
            key: ResultadosInspecao.NaoAplicavel.toString(),
            value: `QualidadeInspecaoSaida.Resultados.NaoAplicavel`,
          },
          {
            key: ResultadosInspecao.NaoConforme.toString(),
            value: `QualidadeInspecaoSaida.Resultados.NaoConforme`,
          }
        ],
        totalCount: 4
      })
    };
  }
}
