import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ConfiguracaoGeralService } from '@viasoft/rnc/app/services/configuracoes/configuracao-geral.service';
import { ConfiguracaoGeralOutput } from '@viasoft/rnc/api-clients/Configuracoes/Gerais/configuracao-geral-output';
import { ConfiguracaoGeralInput } from '@viasoft/rnc/api-clients/Configuracoes/Gerais/configuracao-geral-input';
import { GeralFormControls } from './geral-form-controls';

@Injectable()
export class GeralService {
  public form: FormGroup
  public configuracaoGeral: ConfiguracaoGeralOutput;
  public isLoading = true;
  constructor(private formBuilder: FormBuilder,
    private configuracaoGeralService: ConfiguracaoGeralService) { }

  public get canSave():boolean {
    return this.form.valid && this.form.dirty;
  }

  public async onInit():Promise<void> {
    this.configuracaoGeral = await this.configuracaoGeralService.get().toPromise();
    this.createForm();
    this.populateForm();
    this.isLoading = false;
  }
  public save():void {
    const input = this.form.getRawValue() as ConfiguracaoGeralInput;
    this.configuracaoGeralService.update(input).subscribe(() => {
      this.form.markAsPristine();
    });
  }

  private createForm():void {
    this.form = this.formBuilder.group({});
    this.form.addControl(GeralFormControls.considerarApenasSaldoApontado, this.formBuilder.control(false));
  }
  private populateForm() {
    this.form.get(GeralFormControls.considerarApenasSaldoApontado)
      .setValue(this.configuracaoGeral.considerarApenasSaldoApontado);
  }
}
