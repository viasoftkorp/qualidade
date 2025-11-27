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
import {
  AlterarDadosDTO,
  InspecaoDetailsDTO,
  PlanoInspecaoDTO,
  ResultadosInspecao
} from 'libs/quality-qualidade-inspecao-entrada/src/lib/tokens';

@Component({
  selector: 'qa-alterar-dados-inspecao-modal',
  templateUrl: './alterar-dados-inspecao-modal.component.html',
  styleUrls: ['./alterar-dados-inspecao-modal.component.scss']
})
export class AlterarDadosInspecaoModalComponent implements OnInit {
  
  public form: FormGroup;
  public dto: PlanoInspecaoDTO;
  public resultadoOptions: VsSelectOption[] = [{
    value: ResultadosInspecao.Aprovado,
    name: 'QualidadeInspecaoEntrada.InspecaoDetails.AlterarDadosInspecaoModal.Aprovado'
  },
  {
    value: ResultadosInspecao.NaoAplicavel,
    name: 'QualidadeInspecaoEntrada.InspecaoDetails.AlterarDadosInspecaoModal.NaoAplicavel'
  },
  {
    value: ResultadosInspecao.NaoConforme,
    name: 'QualidadeInspecaoEntrada.InspecaoDetails.AlterarDadosInspecaoModal.NaoConforme'
  }];

  public get listenFormResultadoValueChanges(): boolean {
    if (this.form) {
      if ((this.form.get('menorValorInspecionado').value == null && this.form.get('maiorValorInspecionado').value == null) ||
        (this.form.get('menorValorInspecionado').value == 0 && this.form.get('maiorValorInspecionado').value == 0)) {
        return;
      }
      if (Number(this.form.get('menorValorBase').value) == 0 && Number(this.form.get('maiorValorBase').value) == 0) {
        this.form.get('resultado').setValue(ResultadosInspecao.NaoAplicavel);
      } else if ((this.form.get('menorValorInspecionado').value >= this.form.get('menorValorBase').value)
        && (this.form.get('menorValorInspecionado').value <= this.form.get('maiorValorBase').value)
        && (this.form.get('maiorValorInspecionado').value <= this.form.get('maiorValorBase').value)
        && (this.form.get('maiorValorInspecionado').value >= this.form.get('menorValorBase').value)) {
        this.form.get('resultado').setValue(ResultadosInspecao.Aprovado);
      } else {
        this.form.get('resultado').setValue(ResultadosInspecao.NaoConforme);
      }
    }
    return false;
  }

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

  public async salvarAlteracoes(): Promise<void> {
    const maiorValorInspecionado = Number(this.form.get('maiorValorInspecionado').value);
    const menorValorInspecionado = Number(this.form.get('menorValorInspecionado').value);

    await this.validateResult();
    this.dto.resultado = this.form.get('resultado').value;
    this.dto.maiorValorInspecionado = maiorValorInspecionado || null;
    this.dto.menorValorInspecionado = menorValorInspecionado || null;
    this.dto.observacao = this.form.get('observacao').value;
    this.dialogRef.close(this.dto);
  }

  private initForm(): void {
    this.form = this.formBuilder.group({
      descricao: [{ value: this.dto.descricao, disabled: true }],
      resultado: [this.dto.resultado],
      menorValorInspecionado: [this.dto.menorValorInspecionado],
      menorValorBase: [{ value: this.dto.menorValorBase, disabled: true }],
      maiorValorInspecionado: [this.dto.maiorValorInspecionado],
      maiorValorBase: [{ value: this.dto.maiorValorBase, disabled: true }],
      observacao: [this.dto.observacao]
    });
  }

  private async validateResult() {
    if ((Number(this.form.get('menorValorBase').value) == 0 && Number(this.form.get('maiorValorBase').value) == 0) || this.form.get('resultado').value == ResultadosInspecao.NaoAplicavel) {
      return;
    }

    if (this.form.get('resultado').value == ResultadosInspecao.Aprovado) {
      if ((this.form.get('menorValorInspecionado').value >= this.form.get('menorValorBase').value)
        && (this.form.get('menorValorInspecionado').value <= this.form.get('maiorValorBase').value)
        && (this.form.get('maiorValorInspecionado').value <= this.form.get('maiorValorBase').value)
        && (this.form.get('maiorValorInspecionado').value >= this.form.get('menorValorBase').value)) {
        return;
      } else {
        const confirm = await this.matDialog.open(VsMessageDialogComponent, {
          maxWidth: '60vw',
          panelClass: 'vs-message-dialog-panel',
          data: {
            message: 'Atenção resultado está marcado como aprovado, mas os menores e maiores valores não estão dentro da faixa, deseja realmente marcar como aprovado?',
            messageDialogType: 'confirm',
          }
        }).afterClosed().toPromise();
        if (!confirm) {
          this.form.get('resultado').setValue(ResultadosInspecao.NaoConforme);
        }
      }
    } else if (this.form.get('resultado').value == ResultadosInspecao.NaoConforme) {
      if ((this.form.get('menorValorInspecionado').value >= this.form.get('menorValorBase').value)
        && (this.form.get('menorValorInspecionado').value <= this.form.get('maiorValorBase').value)
        && (this.form.get('maiorValorInspecionado').value <= this.form.get('maiorValorBase').value)
        && (this.form.get('maiorValorInspecionado').value >= this.form.get('menorValorBase').value)) {
        const confirm = await this.matDialog.open(VsMessageDialogComponent, {
          maxWidth: '60vw',
          panelClass: 'vs-message-dialog-panel',
          data: {
            message: 'Atenção resultado está marcado como não conforme, mas os menores e maiores valores estão dentro da faixa, deseja realmente marcar como não conforme?',
            messageDialogType: 'confirm',
          }
        }).afterClosed().toPromise();
        if (!confirm) {
          this.form.get('resultado').setValue(ResultadosInspecao.Aprovado);
        }
      } else {
        return;
      }
    }
  }
}
