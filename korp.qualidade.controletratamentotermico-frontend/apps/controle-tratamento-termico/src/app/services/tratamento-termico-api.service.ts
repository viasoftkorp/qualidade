import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { VsAutocompleteGetInput, VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { firstValueFrom } from 'rxjs';
import { SessionService } from './session.service';
import { PagedResultDto, PagelessResultDto, TratamentoTermicoDtoOutput, TratamentoTermicoInserirInput, TratamentoTermicoAtualizarInput } from '../tokens';

@Injectable({
  providedIn: 'root'
})
export class TratamentoTermicoApiService {

  private get baseUrl() {
    return `${this.session.currentBaseUrl}/producao/gateway`;
  }

  constructor(private http: HttpClient, private session: SessionService) { }

  public buscarListaLote(input: VsAutocompleteGetInput): Promise<PagedResultDto<string>> {
    let queryParameters = new HttpParams();
    if (input.valueToFilter) {
      queryParameters = queryParameters.append('filter', input.valueToFilter);
    }
    if (input.skipCount) {
      queryParameters = queryParameters.append('skipCount', input.skipCount);
    }
    if (input.maxDropSize) {
      queryParameters = queryParameters.append('maxResultCount', input.maxDropSize);
    }

    return firstValueFrom(this.http.get<PagedResultDto<string>>(
      `${ensureTrailingSlash(this.baseUrl)}read-model/tratamentos-termicos/lotes`,
      { params: queryParameters }
    ));
  }

  public buscarLista(input: VsGridGetInput): Promise<PagelessResultDto<TratamentoTermicoDtoOutput>> {
    let queryParameters = new HttpParams();
    if (input.filter) {
      queryParameters = queryParameters.append('filter', input.filter);
    }
    if (input.maxResultCount) {
      queryParameters = queryParameters.append('maxResultCount', input.maxResultCount);
    }
    if (input.skipCount) {
      queryParameters = queryParameters.append('skipCount', input.skipCount);
    }
    if (input.advancedFilter) {
      queryParameters = queryParameters.append('advancedFilter', input.advancedFilter);
    }
    if (input.sorting) {
      queryParameters = queryParameters.append('sorting', input.sorting);
    }

    return firstValueFrom(this.http.get<PagelessResultDto<TratamentoTermicoDtoOutput>>(
      `${ensureTrailingSlash(this.baseUrl)}read-model/tratamentos-termicos`,
      { params: queryParameters }
    ));
  }

  public criar(dto: TratamentoTermicoInserirInput): Promise<any> {
    return firstValueFrom(this.http.post(
      `${ensureTrailingSlash(this.baseUrl)}tratamentos-termicos`,
      dto
    ));
  }

  public atualizar(dto: TratamentoTermicoAtualizarInput): Promise<any> {
    return firstValueFrom(this.http.put(
      `${ensureTrailingSlash(this.baseUrl)}tratamentos-termicos`,
      dto
    ));
  }

  public deletar(id: string): Promise<any> {
    return firstValueFrom(this.http.delete(
      `${ensureTrailingSlash(this.baseUrl)}tratamentos-termicos/${id}`
    ));
  }

  public imprimirRelatorio(idTratamentoTermico: string) {
    return firstValueFrom(this.http.post<never>
      (`${ensureTrailingSlash(this.baseUrl)}read-model/tratamentos-termicos/${idTratamentoTermico}/relatorios/impressao`,
        {}
      ));
  }
}
