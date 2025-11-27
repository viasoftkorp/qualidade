import { Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { VsSelectOption } from '@viasoft/components';
import { QualidadeInspecaoEntradaService } from 'libs/quality-qualidade-inspecao-entrada/src/lib/services/qualidade-inspecao-entrada.service';
import {
  AlterarDadosPedidoDTO,
  EstoqueLocalPedidoVendaAlocacaoDTO,
  PlanoInspecaoDTO
} from 'libs/quality-qualidade-inspecao-entrada/src/lib/tokens';
import { TIPO_LOCAL_APROVADO, TIPO_LOCAL_REPROVADO } from 'libs/quality-qualidade-inspecao-entrada/src/lib/tokens/consts';
import { QuebraLoteService } from '../quebra-lotes-grid/quebra-lote.service';
import { UltraMath } from '../../../../../tokens/functions/UltraMath';
import { PedidoVendaLoteDto } from '../../../../../tokens/interfaces/pedido-venda-lote-interface-dto';
import { MessageService } from '@viasoft/common';

@Component({
  selector: 'inspecao-entrada-alterar-dados-finalizacao-modal',
  templateUrl: './alterar-dados-finalizacao-modal.component.html',
  styleUrls: ['./alterar-dados-finalizacao-modal.component.scss']
})
export class AlterarDadosFinalizacaoModalComponent implements OnInit {
  public inspecaoDto: EstoqueLocalPedidoVendaAlocacaoDTO;
  public recnoInspecao: number;
  public codigoProduto: string;
  public formCustomValidatorMessage = '';
  public form: FormGroup;
  public dto: PlanoInspecaoDTO;
  public selectAprovadosOptions: VsSelectOption[] = []
  public selectReprovadosOptions: VsSelectOption[] = []
  private quantidadeInspecao: number;

  quantiadeTotalAlocadaValidator() {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      const quantidadeTotalDistribuida = Number(control.get('quantidadeAprovada').value) + Number(control.get('quantidadeReprovada').value);
      if (quantidadeTotalDistribuida > this.quantidadeInspecao) {
        this.formCustomValidatorMessage = "QualidadeInspecaoEntrada.InspecaoDetails.FinalizarInspecaoModal.Error.AlocacaoQuantidadeSuperiorInspecao";
        return { 'invalidValue': true };
      } else if (quantidadeTotalDistribuida > this.inspecaoDto.quantidadeRestanteInspecionar) {
        this.formCustomValidatorMessage = "QualidadeInspecaoEntrada.InspecaoDetails.FinalizarInspecaoModal.Error.AlocacaoQuantidadeRestante";
        return { 'invalidValue': true };
      }
      this.formCustomValidatorMessage = "";
      return null;
    };
  }

  constructor(
    private formBuilder: FormBuilder,
    private inspecaoEntradaService: QualidadeInspecaoEntradaService,
    private dialogRef: MatDialogRef<AlterarDadosFinalizacaoModalComponent>,
    private quebraLoteService:QuebraLoteService,
    private messageService: MessageService,
    @Inject(MAT_DIALOG_DATA) private data: AlterarDadosPedidoDTO
  ) {
    this.inspecaoDto = data.pedidoVenda;
    this.recnoInspecao = data.recnoInspecaoEntrada;
    this.quantidadeInspecao = data.quantidadeInspecao;
    this.codigoProduto = data.codigoProduto;
  }

  async ngOnInit() {
    this.initSelectOptions().then(() => {
      this.initForm();
    });
  }

  public async salvar():Promise<void> {
    const lotes = await this.quebraLoteService.getLotes(this.recnoInspecao, this.inspecaoDto.id).toPromise();

    const somaQuantidadesLotes = UltraMath.Sum(lotes.items, (lote: PedidoVendaLoteDto) => lote.quantidade);
    const quantidadeAprovada = this.form.get('quantidadeAprovada').value as number;
    if(somaQuantidadesLotes > quantidadeAprovada) {
      this.messageService.warn('QualidadeInspecaoEntrada.InspecaoDetails.SomaLoteInvalida', 'QualidadeInspecaoEntrada.Atencao')
      return;
    }
    this.inspecaoDto.lotes = lotes.items;
    this.inspecaoDto.codigoLocalAprovado = Number(this.form.get('localPrincipal').value);
    this.inspecaoDto.codigoLocalReprovado = Number(this.form.get('localReprovado').value);
    this.inspecaoDto.quantidadeAprovada = Number(this.form.get('quantidadeAprovada').value);
    this.inspecaoDto.quantidadeReprovada = Number(this.form.get('quantidadeReprovada').value);
    await this.inspecaoEntradaService.atualizarDistribuicaoInspecaoEstoquePedidoVenda(this.recnoInspecao, this.inspecaoDto);
    this.dialogRef.close();
  }

  private async initSelectOptions() {
    const locaisReprovados = await this.inspecaoEntradaService.getLocais(TIPO_LOCAL_REPROVADO, this.data.codigoProduto);
    const locaisAprovados = await this.inspecaoEntradaService.getLocais(TIPO_LOCAL_APROVADO, this.data.codigoProduto);
    locaisReprovados.forEach((l) => {
      this.selectReprovadosOptions.push({
        name: l.descricao,
        value: l.codigo
      });
    });
    if (this.selectReprovadosOptions.length == 0) {
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
    if (this.selectAprovadosOptions.length == 0) {
      this.selectAprovadosOptions.push({
        name: 'Local não disponível',
        value: 0
      });
    }
  }

  private initForm(): void {
    this.form = this.formBuilder.group({
      pedido: [{ value: this.inspecaoDto.numeroPedido, disabled: true }],
      numeroOdf: [{ value: this.inspecaoDto.numeroOdf, disabled: true }],
      quantidadeInspecao: [{ value: this.quantidadeInspecao, disabled: true }],
      quantidadeAlocadaPedido: [{ value: this.inspecaoDto.quantidadeAlocadaLoteLocal, disabled: true }],
      quantidadeRestanteInspecionar: [{ value: this.inspecaoDto.quantidadeRestanteInspecionar, disabled: true }],
      quantidadeNecessidadePedido: [{ value: this.inspecaoDto.quantidadeAlocadaLoteLocal, disabled: true }],
      localPrincipal: [this.inspecaoDto.codigoLocalAprovado?.toString()],
      localReprovado: [this.inspecaoDto.codigoLocalReprovado?.toString()],
      quantidadeAprovada: [{ value: this.inspecaoDto.quantidadeAprovada, disabled: !this.inspecaoDto.codigoLocalAprovado }],
      quantidadeReprovada: [{ value: this.inspecaoDto.quantidadeReprovada, disabled: !this.inspecaoDto.codigoLocalReprovado }],
    }, { validators: [this.quantiadeTotalAlocadaValidator()] });
  }
}
