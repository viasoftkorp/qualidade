import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { ensureTrailingSlash } from '@viasoft/http';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { OperacaoRetrabalhoOutput } from '../model/operacao-retrabalho-output';

@Injectable({
  providedIn: 'root'
})
export class OperacaoRetrabalhoProxyService {
  constructor(
    @Inject(VS_BACKEND_URL) protected gateway: string,
    private httpClient: HttpClient
  ) { }

  public getOperacaoRetrabalho(idNaoConformidade: string): Observable<OperacaoRetrabalhoOutput> {
    return this.httpClient.get<OperacaoRetrabalhoOutput>(`${this.baseUrl(idNaoConformidade)}`);
  }

  private baseUrl(idNaoConformidade: string) {
    // eslint-disable-next-line max-len
    return `${ensureTrailingSlash(this.gateway)}qualidade/rnc/gateway/nao-conformidades/${idNaoConformidade}/retrabalho/operacoes`;
  }
}
