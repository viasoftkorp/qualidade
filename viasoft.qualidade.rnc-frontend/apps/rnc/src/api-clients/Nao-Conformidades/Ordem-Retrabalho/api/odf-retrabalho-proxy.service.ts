import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { ensureTrailingSlash } from '@viasoft/http';
import { Observable } from 'rxjs';
import { OrdemRetrabalhoInput } from '../model/ordem-retrabalho-input';
import { OrdemRetrabalhoOutput } from '../model/ordem-retrabalho-output';
import { GerarOrdemRetrabalhoValidationResult } from '../model/gerar-ordem-retrabalho-validation-result';

@Injectable({
  providedIn: 'root'
})
export class OdfRetrabalhoProxyService {
  constructor(
    @Inject(VS_BACKEND_URL) protected gateway: string,
  private httpClient: HttpClient
  ) {}

  public gerarOdf(input: OrdemRetrabalhoInput, idNaoConformidade:string):Observable<OrdemRetrabalhoOutput> {
    return this.httpClient.post<OrdemRetrabalhoOutput>(`${this.baseUrl(idNaoConformidade)}`, input);
  }
  public estornarOdf(idNaoConformidade:string):Observable<OrdemRetrabalhoOutput> {
    return this.httpClient.delete<OrdemRetrabalhoOutput>(`${this.baseUrl(idNaoConformidade)}`, {});
  }
  public getOdf(idNaoConformidade:string):Observable<OrdemRetrabalhoOutput> {
    return this.httpClient.get<OrdemRetrabalhoOutput>(`${this.baseUrl(idNaoConformidade)}`);
  }

  public canGenerateOdf(idNaoConformidade:string, isFullValidation:boolean)
    :Observable<GerarOrdemRetrabalhoValidationResult> {
    let httpParams = new HttpParams();

    httpParams = httpParams.append('isFullValidation', isFullValidation);

    return this.httpClient
      .get<GerarOrdemRetrabalhoValidationResult>(
        `${this.baseUrl(idNaoConformidade)}/can-generate`,
        { params: httpParams }
      );
  }

  private baseUrl(idNaoConformidade:string) {
    // eslint-disable-next-line max-len
    return `${ensureTrailingSlash(this.gateway)}qualidade/rnc/gateway/nao-conformidades/${idNaoConformidade}/retrabalho/ordens`;
  }
}
