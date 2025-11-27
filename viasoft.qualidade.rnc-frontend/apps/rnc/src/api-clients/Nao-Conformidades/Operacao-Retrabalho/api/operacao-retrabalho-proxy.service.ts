import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { ensureTrailingSlash } from '@viasoft/http';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { OperacaoRetrabalhoOutput } from '../model/operacao-retrabalho-output';
import { OperacaoRetrabalhoInput } from '../model/operacao-retrabalho-input';
import { OperacaoViewOutput } from '../model/operacao-view-output';
import {
  OperacaoSaldoOutput
} from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/operacao-saldo-output';

@Injectable({
  providedIn: 'root'
})
export class OperacaoRetrabalhoProxyService {
  constructor(
    @Inject(VS_BACKEND_URL) protected gateway: string,
  private httpClient: HttpClient
  ) {}

  public gerarOperacaoRetrabalho(input: OperacaoRetrabalhoInput, idNaoConformidade:string)
    :Observable<OperacaoRetrabalhoOutput> {
    return this.httpClient.post<OperacaoRetrabalhoOutput>(`${this.baseUrl(idNaoConformidade)}`, input);
  }

  public getOperacaoRetrabalho(idNaoConformidade:string):Observable<OperacaoRetrabalhoOutput> {
    return this.httpClient.get<OperacaoRetrabalhoOutput>(`${this.baseUrl(idNaoConformidade)}`);
  }

  public getOperacoesView(idNaoConformidade:string, idOperacaoRetrabalho:string, input: VsGridGetInput)
    :Observable<IPagedResultOutputDto<OperacaoViewOutput>> {
    let httpParams = new HttpParams();

    if (input.filter) {
      httpParams = httpParams.append('filter', input.filter);
    }
    if (input.advancedFilter) {
      httpParams = httpParams.append('advancedFilter', input.advancedFilter);
    }
    if (input.sorting) {
      httpParams = httpParams.append('sorting', input.sorting);
    }
    if (input.skipCount) {
      httpParams = httpParams.append('skipCount', input.skipCount.toString());
    }
    if (input.maxResultCount) {
      httpParams = httpParams.append('maxResultCount', input.maxResultCount.toString());
    }
    return this.httpClient
      .get<IPagedResultOutputDto<OperacaoViewOutput>>(
        `${this.baseUrl(idNaoConformidade)}/${idOperacaoRetrabalho}/operacoes`, { params: httpParams }
      );
  }

  public getOperacaoSaldo(legacyIdOperacao: number): Observable<OperacaoSaldoOutput> {
    return this.httpClient.get<OperacaoSaldoOutput>(`${ensureTrailingSlash(this.gateway)}qualidade/rnc/gateway/operacoes/${legacyIdOperacao}`);
  }

  private baseUrl(idNaoConformidade:string) {
    // eslint-disable-next-line max-len
    return `${ensureTrailingSlash(this.gateway)}qualidade/rnc/gateway/nao-conformidades/${idNaoConformidade}/retrabalho/operacoes`;
  }
}
