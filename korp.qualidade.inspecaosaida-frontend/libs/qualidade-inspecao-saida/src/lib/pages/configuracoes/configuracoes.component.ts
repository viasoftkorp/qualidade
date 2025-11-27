import {Component} from '@angular/core';
import {FormBuilder, FormGroup} from "@angular/forms";
import {ConfiguracoesService} from "./configuracoes.service";

export class ConfiguracoesDto {
  public id: string;
  public usarNotaImpressaoRelatorio: boolean;
}

@Component({
  selector: 'qa-configuracoes',
  templateUrl: './configuracoes.component.html',
  styleUrl: './configuracoes.component.css'
})
export class ConfiguracoesComponent {
  public form: FormGroup;
  public carregando = true;

  public get podeSalvar(): boolean {
    return this.form && this.form.valid && this.form.dirty;
  }

  constructor(private formBuilder: FormBuilder, private service: ConfiguracoesService) {
    this.createForm();
    this.getConfiguracao();
  }

  public updateConfiguracoes(): void {
    if (!this.podeSalvar) {
      return;
    }

    const input: ConfiguracoesDto = this.form.getRawValue();
    this.service.updateConfiguracao(input)
      .subscribe(() => {
        this.form.markAsPristine();
      })
  }

  private createForm(): void {
    this.form = this.formBuilder.group({
      id: [],
      usarNotaImpressaoRelatorio: []
    });
  }

  private getConfiguracao(): void {
    this.service.getConfiguracao()
      .subscribe((configuracoes: ConfiguracoesDto) => {
        this.form.patchValue(configuracoes);
        this.carregando = false;
      });
  }
}
