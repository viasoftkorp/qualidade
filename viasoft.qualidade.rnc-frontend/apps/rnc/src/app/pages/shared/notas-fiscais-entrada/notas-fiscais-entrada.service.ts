import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { NotaFiscalEntradaOutput } from '@viasoft/rnc/api-clients/Nota-Fiscal-Entrada/model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotasFiscaisEntradaService {
  private readonly endpoint: string;
  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/gateway/notas-fiscais-entrada`;
  }
  public getList(input: VsGridGetInput, idFornecedor: string): Observable<IPagedResultOutputDto<NotaFiscalEntradaOutput>> {
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
    if (idFornecedor) {
      httpParams = httpParams.append('idFornecedor', idFornecedor.toString());
    }

    return this.httpClient.get<IPagedResultOutputDto<NotaFiscalEntradaOutput>>(this.endpoint, { params: httpParams });
  }
  public get(id: string): Observable<NotaFiscalEntradaOutput> {
    return this.httpClient.get<NotaFiscalEntradaOutput>(`${this.endpoint}/${id}`);
  }
}
