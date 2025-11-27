/* eslint-disable @typescript-eslint/member-ordering */
import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { IPagedResultOutputDto } from '@viasoft/common';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { SolucaoModel, SolucaoInput } from '../../../../api-clients/Solucoes';
import { Observable } from 'rxjs';
import { SolucaoProdutoInput } from '../../../../api-clients/Solucoes/model/solucao-produto-input';
import { SolucaoServicoInput } from '../../../../api-clients/Solucoes/model/solucao-servico-input';
import { SolucaoServicoModel } from '../../../../api-clients/Solucoes/model/solucao-servico-model';
import { SolucaoProdutoModel } from '../../../../api-clients/Solucoes/model/solucao-produto-model';

@Injectable({
  providedIn: 'root'
})
export class SolucaoService {
  private readonly endpointSolucao: string;
  private readonly endpointProdutoSolucao =
    (idSolucao: string) => `${ensureTrailingSlash(this.gateway)}qualidade/rnc/core/solucoes/${idSolucao}/produtos`;
  private readonly endpointServicoSolucao =
    (idSolucao: string) => `${ensureTrailingSlash(this.gateway)}qualidade/rnc/core/solucoes/${idSolucao}/servicos`;

  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient) {
    this.endpointSolucao = `${ensureTrailingSlash(gateway)}qualidade/rnc/core/solucoes`;
  }

  public get(id: string): Observable<SolucaoModel> {
    return this.httpClient.get<SolucaoModel>(`${this.endpointSolucao}/${id}`);
  }

  public getList(input: VsGridGetInput): Observable<IPagedResultOutputDto<SolucaoModel>> {
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

    return this.httpClient.get<IPagedResultOutputDto<SolucaoModel>>(this.endpointSolucao, { params: httpParams });
  }

  public create(input: SolucaoInput): Observable<SolucaoModel> {
    return this.httpClient.post<SolucaoModel>(this.endpointSolucao, input);
  }

  public update(id: string, input: SolucaoInput): Observable<SolucaoModel> {
    return this.httpClient.put<SolucaoModel>(`${this.endpointSolucao}/${id}`, input);
  }

  public delete(id: string): Observable<void> {
    return this.httpClient.delete<void>(`${this.endpointSolucao}/${id}`);
  }

  // public getProdutoSolucao(id: string, idSolucao: string): Observable<SolucaoProdutoModel> {
  //   return this.httpClient.get<SolucaoProdutoModel>(`${this.endpointProdutoSolucao(idSolucao)}/${id}`);
  // }

  public addProduto(input: SolucaoProdutoInput): Observable<SolucaoProdutoModel> {
    return this.httpClient.post<SolucaoProdutoModel>(this.endpointProdutoSolucao(input.idSolucao), input);
  }

  public updateProduto(input: SolucaoProdutoInput, id : string): Observable<SolucaoProdutoModel> {
    return this.httpClient
      .put<SolucaoProdutoModel>(`${this.endpointProdutoSolucao(input.idSolucao)}/${id}`, input);
  }

  public deleteProduto(id: string, idSolucao: string): Observable<SolucaoProdutoModel> {
    return this.httpClient.delete<SolucaoProdutoModel>(`${this.endpointProdutoSolucao(idSolucao)}/${id}`);
  }

  public addServico(input: SolucaoServicoInput): Observable<SolucaoServicoModel> {
    return this.httpClient.post<SolucaoServicoModel>(this.endpointServicoSolucao(input.idSolucao), input);
  }

  public updateServico(input: SolucaoServicoInput): Observable<SolucaoServicoModel> {
    return this.httpClient.put<SolucaoServicoModel>(`${this.endpointServicoSolucao(input.idSolucao)}/${input.id}`, input);
  }

  public deleteServico(id: string, idSolucao: string): Observable<SolucaoServicoModel> {
    return this.httpClient.delete<SolucaoServicoModel>(`${this.endpointServicoSolucao(idSolucao)}/${id}`);
  }

  public getSolucaoProdutosList(input: VsGridGetInput, idSolucao: string): Observable<IPagedResultOutputDto<SolucaoProdutoModel>> {
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
    return this.httpClient.get<IPagedResultOutputDto<SolucaoProdutoModel>>(this.endpointProdutoSolucao(idSolucao), { params: httpParams });
  }

  public getSolucaoServicosList(input: VsGridGetInput, idSolucao: string): Observable<IPagedResultOutputDto<SolucaoServicoModel>> {
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
    return this.httpClient.get<IPagedResultOutputDto<SolucaoServicoModel>>(this.endpointServicoSolucao(idSolucao), { params: httpParams });
  }
}
