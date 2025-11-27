import { Injectable } from "@angular/core";
import { GetAllPlanoAmostragem, PlanoAmostragem } from "../../tokens/interfaces/planos-amostragem-dto.interface";
import { VsGridGetInput } from "@viasoft/components";
import { Observable } from "rxjs";
import { HttpClient, HttpParams } from "@angular/common/http";
import { SessionService } from "../../services/session.service";
import { ensureTrailingSlash } from "@viasoft/http";

@Injectable()
export class ConfiguracoesService {

    private get basePath(): string {
        return `${ensureTrailingSlash(this.sessionService.currentBaseUrl)}planos-amostragem`;
    }

    constructor(protected httpClient: HttpClient, private sessionService: SessionService) {
    }

    public getPlanosAmostragem(input: VsGridGetInput): Observable<GetAllPlanoAmostragem> {
        let params = new HttpParams();
        if (input.filter !== undefined && input.filter !== null && input.filter !== '') {
            params = params.set('filter', input.filter);
        }
        if (input.skipCount !== undefined && input.skipCount !== null) {
            params = params.set('skip', input.skipCount.toString());
        }
        if (input.maxResultCount !== undefined && input.maxResultCount !== null) {
            params = params.set('pageSize', input.maxResultCount.toString());
        }

        return this.httpClient.get<GetAllPlanoAmostragem>(`${this.basePath}`, {
            params,
            headers: this.sessionService.defaultHttpHeaders
        });
    }

    public removePlanoAmostragem(id: string) {
        const route = `${this.basePath}/${id}`;

        return this.httpClient.delete(
            route,
            {
                headers: this.sessionService.defaultHttpHeaders
            }
        ).toPromise();
    }

    public criarPlanoAmostragem(input: PlanoAmostragem) {
        const route = `${this.basePath}`;

        return this.httpClient.post(route, input, {
            headers: this.sessionService.defaultHttpHeaders
        }).toPromise();
    }

    public atualizarPlanoAmostragem(id: string, input: PlanoAmostragem) {
        const route = `${this.basePath}/${id}`;

        return this.httpClient.put(route, input, {
            headers: this.sessionService.defaultHttpHeaders
        }).toPromise();
    }

}