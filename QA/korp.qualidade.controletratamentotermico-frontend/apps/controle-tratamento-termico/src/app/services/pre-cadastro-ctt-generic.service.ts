import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { VsGridGetInput } from "@viasoft/components";
import { ensureTrailingSlash } from "@viasoft/http";
import { firstValueFrom } from "rxjs";
import { SessionService } from "./session.service";
import { PagedResultDto, PreCadastroCttGenericAtualizarInput, PreCadastroCttGenericDtoOutput, PreCadastroCttGenericInserirInput } from "../tokens";

@Injectable({ providedIn: 'root' })
export class PreCadastroCttGenericService {
    private _baseUrlSuffix = '';

    private get baseUrl() {
        return `${this.session.currentBaseUrl}/producao/gateway`;
    }

    public set baseUrlSuffix(value: string) {
        this._baseUrlSuffix = value;
    }

    constructor(private http: HttpClient, private session: SessionService) { }

    public buscarLista(input: VsGridGetInput): Promise<PagedResultDto<PreCadastroCttGenericDtoOutput>> {
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

        return firstValueFrom(this.http.get<PagedResultDto<PreCadastroCttGenericDtoOutput>>(
            `${ensureTrailingSlash(this.baseUrl)}read-model/tratamentos-termicos/pre-cadastros/${this._baseUrlSuffix}`,
            { params: queryParameters }
        ));
    }

    public criar(dto: PreCadastroCttGenericInserirInput) {
        return firstValueFrom(this.http.post(`${ensureTrailingSlash(this.baseUrl)}tratamentos-termicos/pre-cadastros/${this._baseUrlSuffix}`, dto));
    }

    public atualizar(dto: PreCadastroCttGenericAtualizarInput) {
        return firstValueFrom(this.http.put(`${ensureTrailingSlash(this.baseUrl)}tratamentos-termicos/pre-cadastros/${this._baseUrlSuffix}`, dto));
    }

    public deletar(id: string) {
        return firstValueFrom(this.http.delete(`${ensureTrailingSlash(this.baseUrl)}tratamentos-termicos/pre-cadastros/${this._baseUrlSuffix}/${id}`));
    }
}