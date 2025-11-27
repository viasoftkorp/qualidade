import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Policy } from '@viasoft/controle-tratamento-termico/app/authorization/policy';

export const PreCadastrosPolicies: string[] = [
  Policy.preCadastrosCalcos,
  Policy.preCadastrosParametros,
  Policy.preCadastrosTiposTratamento
];

@Component({
  selector: 'app-pre-cadastros',
  templateUrl: './pre-cadastros.component.html',
  styleUrls: ['./pre-cadastros.component.scss']
})
export class PreCadastrosComponent {
  public readonly Policy = Policy;
  constructor(private router: Router) { }

  public navigateTo(path: string): void {
    this.router.navigateByUrl(path);
  }
}
