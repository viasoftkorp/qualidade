import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { DefeitoOutput, DefeitoInput } from '@viasoft/rnc/api-clients/Defeitos';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DefeitoService {
  private readonly endpoint: string;

  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/gateway/defeitos`;
  }

  public get(id: string): Observable<DefeitoOutput> {
    return this.httpClient.get<DefeitoOutput>(`${this.endpoint}/${id}`);
  }

  public getViewList(input: VsGridGetInput): Observable<IPagedResultOutputDto<DefeitoOutput>> {
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

    return this.httpClient.get<IPagedResultOutputDto<DefeitoOutput>>(`${this.endpoint}/view`, { params: httpParams });
  }

  public create(input: DefeitoInput): Observable<DefeitoOutput> {
    return this.httpClient.post<DefeitoOutput>(this.endpoint, input);
  }

  public update(id: string, input: DefeitoInput): Observable<DefeitoOutput> {
    return this.httpClient.put<DefeitoOutput>(`${this.endpoint}/${id}`, input);
  }

  public delete(id: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.endpoint}/${id}`);
  }

  public ativar(id: string): Observable<void> {
    return this.httpClient.patch<void>(`${this.endpoint}/${id}/ativacao`, {});
  }
  public inativar(id: string): Observable<void> {
    return this.httpClient.patch<void>(`${this.endpoint}/${id}/inativacao`, {});
  }
}
