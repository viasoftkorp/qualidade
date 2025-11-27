import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { ensureTrailingSlash } from '@viasoft/http';
import { Observable } from 'rxjs';
import { OrdemRetrabalhoOutput } from '../model/ordem-retrabalho-output';

@Injectable({
  providedIn: 'root'
})
export class OdfRetrabalhoProxyService {
  constructor(
    @Inject(VS_BACKEND_URL) protected gateway: string,
    private httpClient: HttpClient
  ) { }

  public getOdf(idNaoConformidade: string): Observable<OrdemRetrabalhoOutput> {
    return this.httpClient.get<OrdemRetrabalhoOutput>(`${this.baseUrl(idNaoConformidade)}`);
  }

  private baseUrl(idNaoConformidade: string) {
    return `${ensureTrailingSlash(this.gateway)}qualidade/rnc/gateway/nao-conformidades/${idNaoConformidade}/retrabalho/ordens`;
  }
}
