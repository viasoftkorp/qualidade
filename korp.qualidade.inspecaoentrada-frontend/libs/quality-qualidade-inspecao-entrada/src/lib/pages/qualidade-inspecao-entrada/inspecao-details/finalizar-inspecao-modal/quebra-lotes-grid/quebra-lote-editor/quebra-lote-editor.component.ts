import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PedidoVendaLoteDto } from '../../../../../../tokens/interfaces/pedido-venda-lote-interface-dto';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { EditorAction } from '../../../../../../tokens/enums/editor-action';
import { QuebraLoteService } from '../quebra-lote.service';
import { EstoqueLocalPedidoVendaAlocacaoDTO, GerarNumeroLoteInput } from '../../../../../../tokens';
import { UUID } from 'angular2-uuid';
import { VsSubscriptionManager } from '@viasoft/common';

@Component({
  selector: 'inspecao-entrada-quebra-lote-editor',
  templateUrl: './quebra-lote-editor.component.html',
  styleUrls: ['./quebra-lote-editor.component.scss']
})
export class QuebraLoteEditorComponent implements OnInit, OnDestroy {
  public form: FormGroup;
  public action: EditorAction;
  public lote: PedidoVendaLoteDto;
  public inspecaoDto: EstoqueLocalPedidoVendaAlocacaoDTO;
  public processando = false;
  private codigoProduto: string;

  public get canSave():boolean {
    return this.form.valid && this.form.dirty && !this.processando;
  }

  private subscriptionManager = new VsSubscriptionManager();

  constructor(
    private formBuilder: FormBuilder, @Inject(MAT_DIALOG_DATA)
    private readonly data: { lote: PedidoVendaLoteDto | null, inspecaoDto: EstoqueLocalPedidoVendaAlocacaoDTO, codigoProduto: string },
    private dialogRef: MatDialogRef<QuebraLoteEditorComponent>,
    private quebraLoteService: QuebraLoteService) {
    this.inspecaoDto = data?.inspecaoDto;
    this.lote = data?.lote;
    this.codigoProduto = data?.codigoProduto;
    this.action = data?.lote ? EditorAction.Update : EditorAction.Create;
  }

  public ngOnInit(): void {
    this.createForm();

    if (this.action === EditorAction.Create) {
      this.gerarNumeroLote();
    } else {
      this.populateForm(this.lote);
    }
  }

  public ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public salvar() {
    if(!this.canSave) {
      return;
    }
    this.processando = true;
    const input = this.form.getRawValue() as PedidoVendaLoteDto;
    input.idInspecaoEntradaPedidoVenda = this.inspecaoDto?.id;
    input.quantidade = Number(input.quantidade);
    if(this.action === EditorAction.Create) {
      this.quebraLoteService.addLote(input)
    } else {
      this.quebraLoteService.updateLote(input)
    }
    this.dialogRef.close(null);
  }

  private createForm() {
    this.form = this.formBuilder.group({});
    this.form.addControl('numeroLote', this.formBuilder.control(null, [Validators.required, Validators.maxLength(50)]));
    this.form.addControl('quantidade', this.formBuilder.control(null, Validators.required));
    this.form.addControl('id', this.formBuilder.control(UUID.UUID(), Validators.required));
  }

  private populateForm(lote: PedidoVendaLoteDto) {
    this.form.get('numeroLote').setValue(lote.numeroLote);
    this.form.get('quantidade').setValue(lote.quantidade);
    this.form.get('id').setValue(lote.id);
  }

  private gerarNumeroLote(): void {
    const input = {
      codigoProduto: this.codigoProduto,
      odf: this.inspecaoDto.numeroOdf
    } as GerarNumeroLoteInput;

    this.subscriptionManager.add('gerar-numero-lote', this.quebraLoteService.gerarNumeroLote(input)
      .subscribe((numeroLote) => {
        this.form.get('numeroLote').setValue(numeroLote);
      }));
  }
}
