import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import {
  OrdemRetrabalhoInput,
  OrdemRetrabalhoOutput
} from '@viasoft/rnc/api-clients/Nao-Conformidades/Ordem-Retrabalho';
import { TipoLocal } from '@viasoft/rnc/api-clients/Locais/model/tipo-local';
import { finalize } from 'rxjs/operators';
import { EstoquePedidoVendaEstoqueLocalViewOutput } from '@viasoft/rnc/api-clients/Estoque-Pedido-Venda-Estoque-Locais';
import { EstoqueLocalOutput } from '@viasoft/rnc/api-clients/Estoque-Locais/model/estoque-local-output';
import { GerarOdfRetrabalhoFormControls } from './gerarOdfRetrabalhoFormControls';
import { GerarOdfRetrabalhoEditorModalService } from './gerar-odf-retrabalho-editor-modal.service';
import { NaoConformidadesEditorService } from '../../../nao-conformidades-editor.service';

@Component({
  selector: 'rnc-gerar-odf-retrabalho-editor-modal',
  templateUrl: './gerar-odf-retrabalho-editor-modal.component.html',
  styleUrls: ['./gerar-odf-retrabalho-editor-modal.component.scss'],
  providers: [GerarOdfRetrabalhoEditorModalService]
})
export class GerarOdfRetrabalhoEditorModalComponent implements OnInit {
  public form:FormGroup
  public formControls = GerarOdfRetrabalhoFormControls;
  public processando = false;
  public localDestinoTipoLocalEnum = TipoLocal;

  constructor(private formBuilder: FormBuilder,
    private gerarOdfRetrabalhoEditorModalService: GerarOdfRetrabalhoEditorModalService,
    private dialogRef: MatDialogRef<GerarOdfRetrabalhoEditorModalComponent>,
    private naoConformidadesEditorService: NaoConformidadesEditorService) { }

  public get isLoading():boolean {
    return this.gerarOdfRetrabalhoEditorModalService.isLoading;
  }
  public get canSave():boolean {
    return this.form.valid && this.form.dirty && !this.processando;
  }

  public get utilizarReservaDePedidoNaLocalizacaoDeEstoque():boolean {
    return this.gerarOdfRetrabalhoEditorModalService.configuracaoGeral.utilizarReservaDePedidoNaLocalizacaoDeEstoque;
  }

  public get idProduto():string {
    return this.naoConformidadesEditorService.naoConformidadeAtual.idProduto;
  }
  public get numeroLote():string {
    return this.naoConformidadesEditorService.naoConformidadeAtual.numeroLote;
  }
  public get numeroPedido():string {
    return this.naoConformidadesEditorService.naoConformidadeAtual.numeroPedido;
  }
  public get numeroOdfDestino():number {
    return this.gerarOdfRetrabalhoEditorModalService.numeroOdfDestino;
  }

  public ngOnInit(): void {
    this.gerarOdfRetrabalhoEditorModalService.onInit();
    this.createForm();
  }

  public preencherQuantidade(estoqueLocal: EstoquePedidoVendaEstoqueLocalViewOutput | EstoqueLocalOutput):void {
    this.form.get(this.formControls.quantidade).setValue(estoqueLocal.quantidade);
  }

  public preencherSaldoLote(estoqueLocal: EstoquePedidoVendaEstoqueLocalViewOutput | EstoqueLocalOutput):void {
    this.form.get(this.formControls.saldoLote).setValue(estoqueLocal.quantidade);
  }

  public estoquesLocaisSelectCleanedHandle():void {
    this.form.get(this.formControls.saldoLote).reset();
  }

  public gerarOdf():void {
    const input = this.form.getRawValue() as OrdemRetrabalhoInput;
    this.processando = true;
    this.gerarOdfRetrabalhoEditorModalService.gerarOdf(input)
      .pipe(finalize(() => {
        this.processando = false;
      }))
      .subscribe({
        next: (result:OrdemRetrabalhoOutput) => {
          if (result.success) {
            this.dialogRef.close(true);
          }
        }
      });
  }

  private createForm() {
    this.form = this.formBuilder.group({});
    this.form.addControl(GerarOdfRetrabalhoFormControls.quantidade, this.formBuilder
      .control(null, [Validators.min(0), Validators.required]));
    // eslint-disable-next-line max-len
    this.form.addControl(GerarOdfRetrabalhoFormControls.idLocalDestino, this.formBuilder.control(null, [Validators.required]));
    // eslint-disable-next-line max-len
    this.form.addControl(GerarOdfRetrabalhoFormControls.idEstoqueLocalOrigem, this.formBuilder.control(null, [Validators.required]));

    this.form.addControl(
      GerarOdfRetrabalhoFormControls.saldoLote,
      this.formBuilder.control({ value: null, disabled: true })
    );
  }
}
