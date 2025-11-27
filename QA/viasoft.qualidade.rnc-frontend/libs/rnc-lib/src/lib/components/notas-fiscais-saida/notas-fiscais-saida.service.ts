import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { Observable } from 'rxjs';
import { NotaFiscalSaidaOutput } from '../../api-clients/Nota-Fiscal-Saida/model';

@Injectable({
  providedIn: 'root'
})
export class NotasFiscaisSaidaService {
  private readonly endpoint: string;
  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/gateway/notas-fiscais-saida`;
  }
  public getList(input: VsGridGetInput, idCliente: string): Observable<IPagedResultOutputDto<NotaFiscalSaidaOutput>> {
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
    if (idCliente) {
      httpParams = httpParams.append('idCliente', idCliente.toString());
    }
    return this.httpClient.get<IPagedResultOutputDto<NotaFiscalSaidaOutput>>(this.endpoint, { params: httpParams });
  }
  public get(id: string): Observable<NotaFiscalSaidaOutput> {
    return this.httpClient.get<NotaFiscalSaidaOutput>(`${this.endpoint}/${id}`);
  }
}
