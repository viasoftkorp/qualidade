import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { NaturezaInput, NaturezaModel } from '../../../api-clients/Naturezas';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NaturezaService {
  private readonly endpoint: string;

  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/core/naturezas`;
  }
  public get(id: string): Observable<NaturezaModel> {
    return this.httpClient.get<NaturezaModel>(`${this.endpoint}/${id}`);
  }

  public getList(input: VsGridGetInput): Observable<IPagedResultOutputDto<NaturezaModel>> {
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

    return this.httpClient.get<IPagedResultOutputDto<NaturezaModel>>(this.endpoint, { params: httpParams });
  }

  public create(input: NaturezaInput): Observable<NaturezaModel> {
    return this.httpClient.post<NaturezaModel>(this.endpoint, input);
  }

  public update(id: string, input: NaturezaInput): Observable<NaturezaModel> {
    return this.httpClient.put<NaturezaModel>(`${this.endpoint}/${id}`, input);
  }

  public delete(id: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.endpoint}/${id}`);
  }
}
