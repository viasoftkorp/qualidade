import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { CentroCustoOutput } from '@viasoft/rnc/api-clients/Centros-Custo/model/centro-custo-output';
import { Observable } from 'rxjs';

@Injectable()
export class CentroCustoService {
  private readonly endpoint: string;
  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/gateway/centros-custo`;
  }
  public getList(input: VsGridGetInput): Observable<IPagedResultOutputDto<CentroCustoOutput>> {
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

    return this.httpClient.get<IPagedResultOutputDto<CentroCustoOutput>>(this.endpoint, { params: httpParams });
  }
  public get(id: string): Observable<CentroCustoOutput> {
    return this.httpClient.get<CentroCustoOutput>(`${this.endpoint}/${id}`);
  }
}
