import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { ensureTrailingSlash } from '@viasoft/http';
// eslint-disable-next-line max-len
import {
  CentroCustoCausaNaoConformidadeOutput
} from '@viasoft/rnc/api-clients/Nao-Conformidades/Centros-Custo-Causas-Nao-Conformidades/model/centro-custo-causa-nao-conformidade-output';
import { HttpClient } from '@angular/common/http';
import { IPagedResultOutputDto } from '@viasoft/common';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CentrosCustoCausasNaoConformidadesProxyService {
  constructor(
    @Inject(VS_BACKEND_URL) protected gateway: string,
    private httpClient: HttpClient
  ) {}

  public getList(idNaoConformidade: string, idCausaNaoConformidade: string):Observable<IPagedResultOutputDto<CentroCustoCausaNaoConformidadeOutput>> {
    return this.httpClient
      // eslint-disable-next-line max-len
      .get<IPagedResultOutputDto<CentroCustoCausaNaoConformidadeOutput>>(`${this.baseUrl(idNaoConformidade, idCausaNaoConformidade)}`);
  }

  private baseUrl(idNaoConformidade: string, idCausaNaoConformidade: string) {
    return `${ensureTrailingSlash(this.gateway)}qualidade/rnc/gateway/nao-conformidades/${idNaoConformidade}/causas/${idCausaNaoConformidade}/centros-custo`;
  }
}
