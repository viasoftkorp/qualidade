import { HttpErrorResponse } from '@angular/common/http';
import {
  Component,
  Inject,
  OnDestroy,
  OnInit
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import {
  MessageService,
  UserService,
  VsSubscriptionManager
} from '@viasoft/common';
import {
  VsDialog,
  VsGridGetInput,
  VsGridGetResult,
  VsGridNumberColumn, VsGridOptions, VsGridSimpleColumn, VsSelectOption
} from '@viasoft/components';
import { OrigemNaoConformidades, RncEditorModalService } from '@viasoft/rnc-lib';
import {
  QualidadeInspecaoEntradaService
} from 'libs/quality-qualidade-inspecao-entrada/src/lib/services/qualidade-inspecao-entrada.service';
import {
  AlterarDadosPedidoDTO,
  EstoqueLocalPedidoVendaAlocacaoDTO,
  FinalizarInspecaoInput,
  FinalizarInspecaoModalData,
  GetAllEstoqueLocalPedidoVendaAlocacaoDTO,
  InspecaoEntradaDTO,
  NotaFiscalDTO,
  PedidoVendaInput
} from 'libs/quality-qualidade-inspecao-entrada/src/lib/tokens';
import {
  TIPO_LOCAL_APROVADO,
  TIPO_LOCAL_REPROVADO,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO
} from 'libs/quality-qualidade-inspecao-entrada/src/lib/tokens/consts';
import { getErrorMessage } from 'libs/quality-qualidade-inspecao-entrada/src/lib/tokens/functions';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  AlterarDadosFinalizacaoModalComponent
} from './alterar-dados-finalizacao-modal/alterar-dados-finalizacao-modal.component';

@Component({
  selector: 'qa-finalizar-inspecao-modal',
  templateUrl: './finalizar-inspecao-modal.component.html',
  styleUrls: ['./finalizar-inspecao-modal.component.scss']
})

export class FinalizarInspecaoModalComponent implements OnInit, OnDestroy {

  private inspecaoDto: InspecaoEntradaDTO;
  private notaFiscal: NotaFiscalDTO;
  private codigoProduto: string;
  private subs = new VsSubscriptionManager();
  private quantidadeTotalAlocadaPedido: number;
  public gridOptions = new VsGridOptions();
  public utilizaReserva: boolean
  public form: FormGroup;
  public formQuantidades: FormGroup;
  public formLocais: FormGroup;
  public formReserva: FormGroup;
  public desabilitarSalvar = false;
  public selectAprovadosOptions: VsSelectOption[] = []
  public selectReprovadosOptions: VsSelectOption[] = []
  public itensAlterados = new Map<string, EstoqueLocalPedidoVendaAlocacaoDTO>();
  public formCustomValidatorMessage = "";

  public get salvarDesabilitado(): boolean {
    this.quantiadeTotalAlocadaValidator();
    if (this.utilizaReserva) {
      return (this.quantidadeTotalAlocadaPedido < this.inspecaoDto.quantidadeInspecao)
        || (this.quantidadeTotalAlocadaPedido > this.inspecaoDto.quantidadeLote)
        || this.desabilitarSalvar;
    }
    if (!this.formQuantidades) {
      return true;
    }
    const quantidadeAprovada = Number(this.formQuantidades.get('quantidadeAprovada').value ?? '0');
    const quantidadeRejeitada = Number(this.formQuantidades.get('quantidadeRejeitada').value ?? '0');
    const total = quantidadeAprovada + quantidadeRejeitada;
    return (total < this.inspecaoDto.quantidadeInspecao) || this.desabilitarSalvar || (total > this.inspecaoDto.quantidadeLote);
  }

  quantiadeTotalAlocadaValidator() {
    if (this.utilizaReserva) {
      if (this.quantidadeTotalAlocadaPedido > this.inspecaoDto.quantidadeLote) {
        this.formCustomValidatorMessage = "QualidadeInspecaoEntrada.InspecaoDetails.Error.AlocacaoSuperiorLote";
        return;
      } else if (this.quantidadeTotalAlocadaPedido < this.inspecaoDto.quantidadeInspecao) {
        this.formCustomValidatorMessage = "QualidadeInspecaoEntrada.InspecaoDetails.Error.AlocacaoInferiorInspecao";
        return;
      }
    } else if (this.formQuantidades) {
      const quantidadeAprovada = Number(this.formQuantidades.get('quantidadeAprovada').value ?? '0');
      const quantidadeRejeitada = Number(this.formQuantidades.get('quantidadeRejeitada').value ?? '0');
      const total = quantidadeAprovada + quantidadeRejeitada;

      if (total < this.inspecaoDto.quantidadeInspecao) {
        this.formCustomValidatorMessage = "QualidadeInspecaoEntrada.InspecaoDetails.Error.AlocacaoInferiorInspecao";
        return;
      } else if (total > this.inspecaoDto.quantidadeLote) {
        this.formCustomValidatorMessage = "QualidadeInspecaoEntrada.InspecaoDetails.Error.AlocacaoSuperiorLote";
        return;
      }
    }
    this.formCustomValidatorMessage = "";
  }

  constructor(
    private formBuilder: FormBuilder,
    private vsDialog: VsDialog,
    private matDialog: MatDialog,
    private inspecaoEntradaService: QualidadeInspecaoEntradaService,
    private messageService: MessageService,
    private router: Router,
    private rncEditorModalService: RncEditorModalService,
    private userService: UserService,
    @Inject(MAT_DIALOG_DATA) data: FinalizarInspecaoModalData,
  ) {
    this.inspecaoDto = data.inspecaoEntrada;
    this.notaFiscal = data.notaFiscal;
    this.codigoProduto = data.codigoProduto;
  }

  async ngOnInit(): Promise<void> {
    this.initForm();
    this.initGrid();
    this.utilizaReserva = await this.inspecaoEntradaService.getValorParametro(UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO, UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO);
    if (!this.utilizaReserva) {
      await this.initSelectOptions();
      this.initFormsQuantidades();
    }
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  public async finalizarInspecao(): Promise<void> {
    this.desabilitarSalvar = true;

    const input: FinalizarInspecaoInput = this.formQuantidades
      ? {
        codigoInspecao: this.inspecaoDto.codigoInspecao,
        quantidadeAprovada: Number(this.formQuantidades.get('quantidadeAprovada').value) ?? 0,
        quantidadeRejeitada: Number(this.formQuantidades.get('quantidadeRejeitada').value) ?? 0,
        codigoLocalPrincipal: this.formLocais.get('localPrincipal').value ? Number(this.formLocais.get('localPrincipal').value) : 0,
        codigoLocalReprovado: this.formLocais.get('localReprovado').value ? Number(this.formLocais.get('localReprovado').value) : 0,
      }
      : {
        codigoInspecao: this.inspecaoDto.codigoInspecao,
        quantidadeAprovada: 0,
        quantidadeRejeitada: 0,
        codigoLocalPrincipal: 0,
        codigoLocalReprovado: 0,
      };

    let possuiQuantidadesReprovadas = false;
    if (this.utilizaReserva) {
      const quantidadesAlocadasPedido = await this.inspecaoEntradaService.getQuantidadeTotalAlocadaPedido(this.inspecaoDto.recnoInspecao);
      possuiQuantidadesReprovadas = quantidadesAlocadasPedido.quantidadeReprovada > 0;
    } else {
      possuiQuantidadesReprovadas = input.quantidadeRejeitada > 0;
    }

    if (possuiQuantidadesReprovadas) {
      const informacoesRnc = await this.inspecaoEntradaService.getInformacoesRNC(this.inspecaoDto.recnoInspecao, this.codigoProduto);
      const rncGerada = await this.rncEditorModalService.openCreateRncModal(
        {
          origem: OrigemNaoConformidades.InspecaoEntrada,
          numeroNotaFiscal: informacoesRnc.numeroNota,
          numeroLote: informacoesRnc.numeroLote,
          dataFabricacaoLote: informacoesRnc.dataFabricacaoLote,
          idCriador: this.userService.getCurrentUser().id,
          loteTotal: this.inspecaoDto.quantidadeInspecao == informacoesRnc.quantidadeTotalLote,
          loteParcial: this.inspecaoDto.quantidadeInspecao != informacoesRnc.quantidadeTotalLote,
          idProduto: informacoesRnc.idProduto,
          idNotaFiscal: informacoesRnc.idNotaFiscal,
          idPessoa: informacoesRnc.idFornecedor
        }).toPromise();
      input.idRnc = rncGerada.naoConformidade.id;
    }

    this.inspecaoEntradaService.finalizarInspecao(input).subscribe(() => {
      this.matDialog.closeAll();
      this.router.navigate(['/processamento']);
    }, (err: HttpErrorResponse) => {
      this.desabilitarSalvar = false;
      this.messageService.error(getErrorMessage(err));
    });
  }

  private async initSelectOptions() {
    const locaisReprovados = await this.inspecaoEntradaService.getLocais(TIPO_LOCAL_REPROVADO, this.codigoProduto);
    const locaisAprovados = await this.inspecaoEntradaService.getLocais(TIPO_LOCAL_APROVADO, this.codigoProduto);
    locaisReprovados.forEach((l) => {
      this.selectReprovadosOptions.push({
        name: l.descricao,
        value: l.codigo
      });
    });
    if (this.selectReprovadosOptions.length === 0) {
      this.selectReprovadosOptions.push({
        name: 'Local não disponível',
        value: 0
      });
    }

    locaisAprovados.forEach((l) => {
      this.selectAprovadosOptions.push({
        name: l.descricao,
        value: l.codigo
      });
    });
    if (this.selectAprovadosOptions.length === 0) {
      this.selectAprovadosOptions.push({
        name: 'Local não disponível',
        value: 0
      });
    }
  }

  private initForm(): void {
    this.form = this.formBuilder.group({
      resultado: [{ value: null, disabled: true }],
      quantidadeInspecao: [{ value: this.inspecaoDto.quantidadeInspecao, disabled: true }],
      quantidadeLote: [{ value: this.inspecaoDto.quantidadeLote, disabled: true }],
    });

    this.buscarResultadoInspecao();
  }

  private initFormsQuantidades(): void {
    this.formLocais = this.formBuilder.group({
      localPrincipal: [this.selectAprovadosOptions[0].value, Validators.required],
      localReprovado: [this.selectReprovadosOptions[0].value, Validators.required],
    });

    this.formQuantidades = this.formBuilder.group({
      quantidadeAprovada: [this.form.get('resultado').value == "Aprovado" ? this.inspecaoDto.quantidadeLote : 0],
      quantidadeRejeitada: [0],
    });

    this.subs.add('resultado-inspecao-changes', this.form.get('resultado').valueChanges.subscribe((result: string) => {
      if (result == 'Aprovado') {
        this.formQuantidades.get('quantidadeAprovada').setValue(this.inspecaoDto.quantidadeLote);
      }
    }));
  }

  private initGrid(): void {
    this.gridOptions.id = '4503acdc-cfdd-4fc8-9a08-5fa48546a871';
    this.gridOptions.enableFilter = false;
    this.gridOptions.enableQuickFilter = false;
    this.gridOptions.enableSorting = false;

    this.gridOptions.columns = [
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.FinalizarInspecaoModal.GridFinalizacao.Pedido',
        field: 'numeroPedido',
        width: 50,
      }),
      new VsGridSimpleColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.FinalizarInspecaoModal.GridFinalizacao.QuantidadePedido',
        field: 'quantidadeAlocadaLoteLocal',
        width: 50,
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.FinalizarInspecaoModal.GridFinalizacao.QuantidadeAprovada',
        field: 'quantidadeAprovada',
        width: 50,
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.FinalizarInspecaoModal.GridFinalizacao.QuantidadeReprovada',
        field: 'quantidadeReprovada',
        width: 50,
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.FinalizarInspecaoModal.GridFinalizacao.LocalAprovado',
        field: 'descricaoLocalAprovado',
        width: 50,
      }),
      new VsGridNumberColumn({
        headerName: 'QualidadeInspecaoEntrada.InspecaoDetails.FinalizarInspecaoModal.GridFinalizacao.LocalReprovado',
        field: 'descricaoLocalReprovado',
        width: 50,
      }),
    ];

    this.gridOptions.get = (input) => this.getGridData(input);
    this.gridOptions.edit = (row, data: EstoqueLocalPedidoVendaAlocacaoDTO) => this.onEdit(data);
  }

  private getGridData(input: VsGridGetInput): Observable<VsGridGetResult> {
    return this.inspecaoEntradaService
      .getInspecaoEntradaPedidoVenda(input, {
        recnoInspecao: this.inspecaoDto.recnoInspecao,
        lote: this.notaFiscal.lote,
        codigoProduto: this.notaFiscal.codigoProduto
      } as PedidoVendaInput)
      .pipe(
        map((result: GetAllEstoqueLocalPedidoVendaAlocacaoDTO) => {
          this.inspecaoEntradaService.getQuantidadeTotalAlocadaPedido(this.inspecaoDto.recnoInspecao).then((quantidadesAlocadas) => {
            this.quantidadeTotalAlocadaPedido = quantidadesAlocadas.quantidadeTotalAlocada;
          });
          return new VsGridGetResult(result.items, result.totalCount);
        })
      );
  }

  private async onEdit(data: EstoqueLocalPedidoVendaAlocacaoDTO) {
    await this.matDialog.open(AlterarDadosFinalizacaoModalComponent,
      this.vsDialog.generateDialogConfig({
        pedidoVenda: data,
        recnoInspecaoEntrada: this.inspecaoDto.recnoInspecao,
        quantidadeLote: this.inspecaoDto.quantidadeLote,
        lote: this.notaFiscal.lote,
        codigoProduto: this.notaFiscal.codigoProduto
      } as AlterarDadosPedidoDTO, { hasBackdrop: true })).afterClosed().toPromise();
    this.gridOptions.refresh();
    this.quantidadeTotalAlocadaPedido = (await this.inspecaoEntradaService.getQuantidadeTotalAlocadaPedido(this.inspecaoDto.recnoInspecao)).quantidadeTotalAlocada;
  }

  private buscarResultadoInspecao(): void {
    this.subs.add('buscar-resultado-inspecao', this.inspecaoEntradaService
      .getResultadoInspecao(this.inspecaoDto.codigoInspecao)
      .subscribe((resultado: string) => {
        this.form.get('resultado').setValue(resultado);
      }));
  }
}
