import { ensureTrailingSlash } from '@viasoft/http';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable, Inject } from '@angular/core';
import { UserSelectOutput } from '@viasoft/administration';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsFilterGetItemsInput } from '@viasoft/components';

@Injectable()
export class UserProxyService {
  private readonly endpoint: string;
  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/gateway/authentication/users`;
  }

  public getList(input: VsFilterGetItemsInput): Observable<IPagedResultOutputDto<UserSelectOutput>> {
    let httpParams = new HttpParams();

    if (input.filter) {
      httpParams = httpParams.append('advancedFilter', input.filter);
    }
    if (input.skipCount) {
      httpParams = httpParams.append('skipCount', input.skipCount.toString());
    }
    if (input.maxResultCount) {
      httpParams = httpParams.append('maxResultCount', input.maxResultCount.toString());
    }

    return this.httpClient.get<IPagedResultOutputDto<UserSelectOutput>>(`${this.endpoint}`, { params: httpParams });
  }
}
