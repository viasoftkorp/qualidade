import { HttpErrorResponse } from '@angular/common/http';
import {
  Component,
  Inject,
  OnDestroy,
  OnInit
} from '@angular/core';
import {
  FormBuilder,
  FormGroup
} from '@angular/forms';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import {
  MessageService,
  VsUserService,
  VsSubscriptionManager
} from '@viasoft/common';
import {
  VsGridOptions,
  VsSelectOption
} from '@viasoft/components';
import { QualidadeInspecaoSaidaService } from '../../../../services/qualidade-inspecao-saida.service';
import {
  FinalizarInspecaoInput,
  getErrorMessage,
  InspecaoSaidaDTO,
  LocalOutput,
  TIPO_LOCAL_APROVADO,
  TIPO_LOCAL_REPROVADO,
  TIPO_LOCAL_RETRABALHO,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO,
  UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO
} from '../../../../tokens';
import { OrigemNaoConformidades, RncEditorModalService } from '@viasoft/rnc-lib';

@Component({
  selector: 'qa-finalizar-inspecao-modal',
  templateUrl: './finalizar-inspecao-modal.component.html',
  styleUrls: ['./finalizar-inspecao-modal.component.scss']
})

export class FinalizarInspecaoModalComponent implements OnInit, OnDestroy {
  private inspecaoDto: InspecaoSaidaDTO;
  private subs = new VsSubscriptionManager();
  public form: FormGroup;
  public formQuantidades: FormGroup;
  public desabilitarSalvar = false;
  public gridOptions = new VsGridOptions();
  public utilizarReservaPedidoBool = false;
  public localReprovadoOptions: VsSelectOption[] = [];
  public localAprovadoOptions: VsSelectOption[] = [];
  public localRetrabalhoOptions: VsSelectOption[] = [];
  public formCustomValidatorMessage = '';
  public desabilitarLocaisAprovados = false;

  public get salvarDesabilitado(): boolean {
    this.quantiadeTotalAlocadaValidator();
    if (!this.formQuantidades) {
      return true;
    }

    const quantidadeAprovada = Number(this.formQuantidades.get('quantidadeAprovada').value ?? '0');
    const quantidadeReprovada = Number(this.formQuantidades.get('quantidadeReprovada').value ?? '0');
    const quantidadeRetrabalhada = Number(this.formQuantidades.get('quantidadeRetrabalhada').value ?? '0');
    const total = quantidadeAprovada + quantidadeReprovada + quantidadeRetrabalhada;

    return (total < this.inspecaoDto.quantidadeInspecao) || this.desabilitarSalvar || (total > this.inspecaoDto.quantidadeLote);
  }

  quantiadeTotalAlocadaValidator() {
    if (this.formQuantidades) {
      const quantidadeAprovada = Number(this.formQuantidades.get('quantidadeAprovada').value ?? '0');
      const quantidadeReprovada = Number(this.formQuantidades.get('quantidadeReprovada').value ?? '0');
      const quantidadeRetrabalhada = Number(this.formQuantidades.get('quantidadeRetrabalhada').value ?? '0');
      const total = quantidadeAprovada + quantidadeReprovada + quantidadeRetrabalhada;
      if (this.utilizarReservaPedidoBool && total > this.inspecaoDto.quantidadeOrdem) {
        this.formCustomValidatorMessage = 'QualidadeInspecaoSaida.InspecaoDetails.Error.AlocacaoSuperiorPedido';
        return;
      }
      if (total < this.inspecaoDto.quantidadeInspecao) {
        this.formCustomValidatorMessage = 'QualidadeInspecaoSaida.InspecaoDetails.Error.AlocacaoInferiorInspecao';
        return;
      } if (total > this.inspecaoDto.quantidadeLote) {
        this.formCustomValidatorMessage = 'QualidadeInspecaoSaida.InspecaoDetails.Error.AlocacaoSuperiorLote';
        return;
      }
      this.formCustomValidatorMessage = '';
    }
  }

  constructor(
    private formBuilder: FormBuilder,
    private inspecaoSaidaService: QualidadeInspecaoSaidaService,
    private messageService: MessageService,
    private router: Router,
    private matDialog: MatDialog,
    private rncEditorModalService: RncEditorModalService,
    private userService: VsUserService,
    @Inject(MAT_DIALOG_DATA) data: InspecaoSaidaDTO,
  ) {
    this.inspecaoDto = data;
  }

  async ngOnInit(): Promise<void> {
    this.initForm();
    this.utilizarReservaPedidoBool = await this.inspecaoSaidaService.getParametroBool(UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO, UTILIZAR_RESERVA_PEDIDO_LOCALIZACAO_SECAO);
    await this.initSelectOptions();
    this.initFormQuantidaes();
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  private async initSelectOptions() {
    const locaisAprovados = await this.getLocaisAprovados();
    const locaisReprovados = await this.inspecaoSaidaService.getLocais(TIPO_LOCAL_REPROVADO);
    const locaisRetrabalhos = await this.inspecaoSaidaService.getLocais(TIPO_LOCAL_RETRABALHO);

    locaisReprovados.forEach((l) => {
      this.localReprovadoOptions.push({
        name: l.descricao,
        value: l.codigo
      });
    });
    if (this.localReprovadoOptions.length == 0) {
      this.localReprovadoOptions.push({
        name: 'Local não disponível',
        value: 0
      });
    }

    locaisAprovados.forEach((l) => {
      this.localAprovadoOptions.push({
        name: l.descricao,
        value: l.codigo
      });
    });
    if (this.localAprovadoOptions.length == 0) {
      this.localAprovadoOptions.push({
        name: 'Local não disponível',
        value: 0
      });
    }

    locaisRetrabalhos.forEach((l) => {
      this.localRetrabalhoOptions.push({
        name: l.descricao,
        value: l.codigo
      });
    });
    if (this.localRetrabalhoOptions.length == 0) {
      this.localRetrabalhoOptions.push({
        name: 'Local não disponível',
        value: 0
      });
    }
  }

  public async finalizarInspecao(): Promise<void> {
    this.desabilitarSalvar = true;

    const input: FinalizarInspecaoInput = this.formQuantidades
      ? {
        codInspecao: this.inspecaoDto.codigoInspecao,
        quantidadeAprovada: Number(this.formQuantidades.get('quantidadeAprovada').value) ?? 0,
        quantidadeReprovada: Number(this.formQuantidades.get('quantidadeReprovada').value) ?? 0,
        quantidadeRetrabalhada: Number(this.formQuantidades.get('quantidadeRetrabalhada').value) ?? 0,
        codigoLocalAprovado: this.formQuantidades.get('localAprovado').value ?? 0,
        codigoLocalReprovado: this.formQuantidades.get('localReprovado').value ?? 0,
        codigoLocalRetrabalho: this.formQuantidades.get('localRetrabalho').value ?? 0
      }
      : {
        codInspecao: this.inspecaoDto.codigoInspecao,
        quantidadeAprovada: 0,
        quantidadeReprovada: 0,
        quantidadeRetrabalhada: 0,
        codigoLocalAprovado: 0,
        codigoLocalReprovado: 0,
        codigoLocalRetrabalho: 0,
      };

    if (input.quantidadeRetrabalhada > 0) {
      const somaQuantidadeInspecionada = input.quantidadeRetrabalhada + input.quantidadeReprovada + input.quantidadeAprovada;
      const informacoesRnc = await this.inspecaoSaidaService.getInformacoesRNC(this.inspecaoDto.recnoInspecaoSaida);
      this.rncEditorModalService.openCreateRncModal({
        origem: OrigemNaoConformidades.InspecaoSaida,
        idPessoa: informacoesRnc.idCliente,
        idProduto: informacoesRnc.idProduto,
        revisao: informacoesRnc.revisao.toString() as any,
        dataFabricacaoLote: informacoesRnc.dataFabricacaoLote,
        loteTotal: somaQuantidadeInspecionada == informacoesRnc.quantidadeTotalLote,
        loteParcial: somaQuantidadeInspecionada != informacoesRnc.quantidadeTotalLote,
        numeroLote: this.inspecaoDto.lote,
        numeroOdf: informacoesRnc.numeroOdf,
        idCriador: this.userService.getCurrentUser().id
      }).toPromise().then(r => {
        input.rnc = {
          idRnc: r.naoConformidade.id,
          codigoRnc: r.naoConformidade.codigo,
          materiais: [],
          recursos: []
        };

        r.servicoNaoConformidades.forEach(r => {
          input.rnc.recursos.push({
            detalhamento: r.detalhamento,
            idRecurso: r.idRecurso,
            horas: r.horas,
            operacaoEngenharia: r.operacaoEngenharia
          });
        });
        r.produtoNaoConformidades.forEach(p => {
          input.rnc.materiais.push({
            idProduto: p.idProduto,
            quantidade: p.quantidade,
            operacaoEngenharia: p.operacaoEngenharia
          });
        });

        this.inspecaoSaidaService.finalizarInspecao(input).subscribe(() => {
          this.matDialog.closeAll();
          this.router.navigate(['/processamento']);
        }, (err: HttpErrorResponse) => {
          this.desabilitarSalvar = false;
          this.messageService.error(getErrorMessage(err));
        });
      });
    } else {
      this.inspecaoSaidaService.finalizarInspecao(input).subscribe(() => {
        this.matDialog.closeAll();
        this.router.navigate(['/processamento']);
      }, (err: HttpErrorResponse) => {
        this.desabilitarSalvar = false;
        this.messageService.error(getErrorMessage(err));
      });
    }
  }

  private initForm(): void {
    this.form = this.formBuilder.group({
      resultado: [{ value: null, disabled: true }],
      quantidadeInspecao: [{ value: this.inspecaoDto.quantidadeInspecao, disabled: true }],
      quantidadeLote: [{ value: this.inspecaoDto.quantidadeLote, disabled: true }],
      quantidadePedidoVenda: [{ value: this.inspecaoDto.quantidadeOrdem, disabled: true }],
      pedidoVenda: [{ value: this.inspecaoDto.numeroPedido, disabled: true }],
      odf: [{ value: this.inspecaoDto.odf, disabled: true }],
      odfApontada: [{ value: this.inspecaoDto.odfApontada, disabled: true }]
    });

    this.buscarResultadoInspecao();
  }

  private initFormQuantidaes() {
    this.formQuantidades = this.formBuilder.group({
      quantidadeAprovada: [{ value: 0, disabled: !this.localAprovadoOptions[0].value }],
      quantidadeReprovada: [{ value: 0, disabled: !this.localReprovadoOptions[0].value }],
      quantidadeRetrabalhada: [{ value: 0, disabled: !this.localRetrabalhoOptions[0].value }],
      localAprovado: [{ value: this.localAprovadoOptions[0].value, disabled: this.desabilitarLocaisAprovados || !this.localAprovadoOptions[0].value }],
      localReprovado: [{ value: this.localReprovadoOptions[0].value, disabled: !this.localReprovadoOptions[0].value }],
      localRetrabalho: [{ value: this.localRetrabalhoOptions[0].value, disabled: !this.localRetrabalhoOptions[0].value }],
    });
  }

  private buscarResultadoInspecao(): void {
    this.subs.add('buscar-resultado-inspecao', this.inspecaoSaidaService
      .getResultadoInspecao(this.inspecaoDto.codigoInspecao)
      .subscribe((resultado: string) => {
        this.form.get('resultado').setValue(resultado);
      }));
  }

  private async getLocaisAprovados(): Promise<LocalOutput[]> {
    let locaisAprovados: LocalOutput[];
    const processoEngenharia = await this.inspecaoSaidaService.getProcessoEngenharia(this.inspecaoDto.codigoProduto);

    if (processoEngenharia.codigoLocalDestino) {
      this.desabilitarLocaisAprovados = true;

      locaisAprovados = [{
        codigo: processoEngenharia.codigoLocalDestino,
        descricao: processoEngenharia.descricaoLocalDestino
      }];
    } else {
      locaisAprovados = await this.inspecaoSaidaService.getLocais(TIPO_LOCAL_APROVADO);
    }

    return locaisAprovados;
  }
}
