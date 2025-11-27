import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { ensureTrailingSlash } from '@viasoft/http';
import { ConfiguracaoGeralInput } from '@viasoft/rnc/api-clients/Configuracoes/Gerais/configuracao-geral-input';
import { ConfiguracaoGeralOutput } from '@viasoft/rnc/api-clients/Configuracoes/Gerais/configuracao-geral-output';
import { Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ConfiguracaoGeralService {
  private baseEndpoint:string;
  private configuracoesGerais: ConfiguracaoGeralOutput
  constructor(@Inject(VS_BACKEND_URL) private gateway: string,
  private httpClient: HttpClient) {
    this.baseEndpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/gateway/configuracoes-gerais`;
  }

  public get():Observable<ConfiguracaoGeralOutput> {
    if (this.configuracoesGerais != null) {
      return of(this.configuracoesGerais);
    }
    return this.getConfiguracaoGeral();
  }
  public update(input: ConfiguracaoGeralInput):Observable<never> {
    return this.httpClient.put<never>(`${this.baseEndpoint}`, input)
      .pipe(tap(() => {
        this.getConfiguracaoGeral().subscribe();
      }));
  }

  private getConfiguracaoGeral():Observable<ConfiguracaoGeralOutput> {
    return this.httpClient.get<ConfiguracaoGeralOutput>(`${this.baseEndpoint}`)
      .pipe(tap((configuracao:ConfiguracaoGeralOutput) => {
        this.configuracoesGerais = configuracao;
      }));
  }
}
