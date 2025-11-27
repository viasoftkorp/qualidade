import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { VsGridGetInput } from '@viasoft/components';
import { ensureTrailingSlash } from '@viasoft/http';
import { firstValueFrom } from 'rxjs';
import { PagelessResultDto, TratamentoTermicoPecaDtoOutput, TratamentoTermicoPecaAtualizarInput } from '../tokens';
import { SessionService } from './session.service';

@Injectable({
    providedIn: 'root'
})
export class TratamentoTermicoPecaApiService {

    private get baseUrl() {
        return `${this.session.currentBaseUrl}/producao/gateway`;
    }

    constructor(private http: HttpClient, private session: SessionService) { }

    public buscarLista(tratamentoTermicoId: string, input: VsGridGetInput): Promise<PagelessResultDto<TratamentoTermicoPecaDtoOutput>> {
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

        queryParameters = queryParameters.append('tratamentoTermicoId', tratamentoTermicoId);

        return firstValueFrom(this.http.get<PagelessResultDto<TratamentoTermicoPecaDtoOutput>>(
            `${ensureTrailingSlash(this.baseUrl)}read-model/tratamentos-termicos/pecas`,
            { params: queryParameters }
        ));
    }

    public buscarListaPorLote(numeroLote: string): Promise<PagelessResultDto<TratamentoTermicoPecaDtoOutput>> {
        return firstValueFrom(this.http.get<PagelessResultDto<TratamentoTermicoPecaDtoOutput>>(
            `${ensureTrailingSlash(this.baseUrl)}read-model/tratamentos-termicos/pecas/${numeroLote}`
        ));
    }

    public atualizar(dto: TratamentoTermicoPecaAtualizarInput[]): Promise<any> {
        return firstValueFrom(this.http.put(
            `${ensureTrailingSlash(this.baseUrl)}tratamentos-termicos/pecas`,
            dto
        ));
    }
}
