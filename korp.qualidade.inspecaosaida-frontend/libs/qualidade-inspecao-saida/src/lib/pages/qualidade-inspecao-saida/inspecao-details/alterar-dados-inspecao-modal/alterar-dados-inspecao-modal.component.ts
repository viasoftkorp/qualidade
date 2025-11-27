import {
  Component,
  Inject,
  OnInit
} from '@angular/core';
import {
  FormBuilder,
  FormGroup
} from '@angular/forms';
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogRef
} from '@angular/material/dialog';
import { VsMessageDialogComponent, VsSelectOption } from '@viasoft/components';
import { AlterarDadosDTO } from 'libs/qualidade-inspecao-saida/src/lib/tokens/interfaces/alterar-dados-input.interface';
import {
  InspecaoDetailsDTO,
  PlanoInspecaoDTO,
  ResultadosInspecao
} from '../../../../tokens';
import { VsSubscriptionManager } from '@viasoft/common';

@Component({
  selector: 'qa-alterar-dados-inspecao-modal',
  templateUrl: './alterar-dados-inspecao-modal.component.html',
  styleUrls: ['./alterar-dados-inspecao-modal.component.scss']
})
export class AlterarDadosInspecaoModalComponent implements OnInit {

  public form: FormGroup;
  public dto: PlanoInspecaoDTO;
  public subscriptionManager = new VsSubscriptionManager();
  public resultadoOptions: VsSelectOption[] = [{
    value: ResultadosInspecao.Aprovado,
    name: 'QualidadeInspecaoSaida.InspecaoDetails.AlterarDadosInspecaoModal.Aprovado'
  },
  {
    value: ResultadosInspecao.ParcialmenteAprovado,
    name: 'QualidadeInspecaoSaida.InspecaoDetails.AlterarDadosInspecaoModal.ParcialmenteAprovado'
  },
  {
    value: ResultadosInspecao.NaoAplicavel,
    name: 'QualidadeInspecaoSaida.InspecaoDetails.AlterarDadosInspecaoModal.NaoAplicavel'
  },
  {
    value: ResultadosInspecao.NaoConforme,
    name: 'QualidadeInspecaoSaida.InspecaoDetails.AlterarDadosInspecaoModal.NaoConforme'
  }];

  constructor(
    private formBuilder: FormBuilder,
    private dialogRef: MatDialogRef<AlterarDadosInspecaoModalComponent>,
    @Inject(MAT_DIALOG_DATA) data: AlterarDadosDTO,
    private matDialog: MatDialog
  ) {
    this.dto = data.dto;
  }

  ngOnInit(): void {
    this.initForm();
  }

  public get valorBaseAprovado(): boolean {
    const valorBaseDesconsideravel = Number(this.form.get('menorValorBase').value) == 0 && Number(this.form.get('maiorValorBase').value) == 0
      && !this.form.get('menorValor').value && !this.form.get('maiorValor').value;

    const valorBaseAprovado = valorBaseDesconsideravel
        || ((this.form.get('menorValor').value >= this.form.get('menorValorBase').value)
        && (this.form.get('menorValor').value <= this.form.get('maiorValorBase').value)
        && (this.form.get('maiorValor').value <= this.form.get('maiorValorBase').value)
        && (this.form.get('maiorValor').value >= this.form.get('menorValorBase').value));

    return valorBaseAprovado;
  }

  public get valorBaseNaoAplicavel(): boolean {
    const valorBaseNaoAplicavel = Number(this.form.get('menorValorBase').value) == 0 && Number(this.form.get('maiorValorBase').value) == 0
      && (this.form.get('menorValor').value || this.form.get('maiorValor').value);

    return valorBaseNaoAplicavel;
  }

  public async salvarAlteracoes(): Promise<void> {
    const maiorValor = this.form.get('maiorValor').value as string;
    const menorValor = this.form.get('menorValor').value as string;
    const observacao = this.form.get('observacao').value as string;
    const resultado = this.form.get('resultado').value as ResultadosInspecao;

    await this.validarAtualizacao();
    this.dto.resultado = resultado;
    this.dto.maiorValor = Number(maiorValor)
    this.dto.menorValor = Number(menorValor)
    this.dto.observacao = observacao;
    this.dialogRef.close(this.dto);
  }

  private initForm(): void {
    this.form = this.formBuilder.group({
      descricao: [{ value: this.dto.descricao, disabled: true }],
      resultado: [this.dto.resultado],
      menorValor: [this.dto.menorValor],
      menorValorBase: [{ value: this.dto.menorValorBase, disabled: true }],
      maiorValor: [this.dto.maiorValor],
      maiorValorBase: [{ value: this.dto.maiorValorBase, disabled: true }],
      observacao: [this.dto.observacao]
    });

    this.subscriptionManager.add('menor-valor-value-changes', this.form.get('menorValor').valueChanges
      .subscribe(() => this.validarValorBase()));

    this.subscriptionManager.add('maior-valor-value-changes', this.form.get('maiorValor').valueChanges
      .subscribe(() => this.validarValorBase()));
  }

  private async validarAtualizacao(): Promise<void> {
    const resultado = this.form.get('resultado').value as ResultadosInspecao;

    if (resultado === ResultadosInspecao.Aprovado && !this.valorBaseAprovado) {
      const confirm = await this.matDialog.open(VsMessageDialogComponent, {
        maxWidth: '60vw',
        panelClass: 'vs-message-dialog-panel',
        data: {
          message: 'Atenção resultado está marcado como aprovado, mas os menores e maiores valores não estão dentro da faixa, deseja realmente marcar como aprovado?',
          messageDialogType: 'confirm',
        }
      }).afterClosed().toPromise();
      if (!confirm) {
        this.form.get('resultado').setValue(ResultadosInspecao.NaoConforme, { emitEvent: false });
      }

      return;
    }

    if (resultado === ResultadosInspecao.NaoAplicavel && this.valorBaseAprovado) {
      const confirm = await this.matDialog.open(VsMessageDialogComponent, {
        maxWidth: '60vw',
        panelClass: 'vs-message-dialog-panel',
        data: {
          message: 'Atenção resultado está marcado como não aplicável, mas os menores e maiores valores estão dentro da faixa, deseja realmente marcar como não aplicável?',
          messageDialogType: 'confirm',
        }
      }).afterClosed().toPromise();
      if (!confirm) {
        this.form.get('resultado').setValue(ResultadosInspecao.Aprovado, { emitEvent: false });
      }

      return;
    }

    if (resultado === ResultadosInspecao.NaoConforme && this.valorBaseAprovado) {
      const confirm = await this.matDialog.open(VsMessageDialogComponent, {
        maxWidth: '60vw',
        panelClass: 'vs-message-dialog-panel',
        data: {
          message: 'Atenção resultado está marcado como não conforme, mas os menores e maiores valores estão dentro da faixa, deseja realmente marcar como não conforme?',
          messageDialogType: 'confirm',
        }
      }).afterClosed().toPromise();
      if (!confirm) {
        this.form.get('resultado').setValue(ResultadosInspecao.Aprovado, { emitEvent: false });
      }

      return;
    }
  }

  private validarValorBase(): void {
    if (this.valorBaseAprovado) {
      this.form.get('resultado').setValue(ResultadosInspecao.Aprovado, { emitEvent: false });
    } else if (this.valorBaseNaoAplicavel) {
      this.form.get('resultado').setValue(ResultadosInspecao.NaoAplicavel, { emitEvent: false });
    } else {
      this.form.get('resultado').setValue(ResultadosInspecao.NaoConforme, { emitEvent: false });
    }
  }
}
