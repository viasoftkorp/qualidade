import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {VS_BACKEND_URL} from "@viasoft/client-core";
import {ensureTrailingSlash, VS_API_PREFIX} from "@viasoft/http";
import {SessionService} from "../../services/session.service";
import {Observable, of} from "rxjs";
import {ConfiguracoesDto} from "./configuracoes.component";
import {tap} from "rxjs/operators";

@Injectable({
  providedIn: "root"
})
export class ConfiguracoesService {
  private configuracaoCacheada: ConfiguracoesDto;

  constructor(protected httpClient: HttpClient, @Inject(VS_BACKEND_URL) private apiBaseUrl: string,
              @Inject(VS_API_PREFIX) private apiPrefix: string, private sessionService: SessionService) {
  }

  public getConfiguracao(ignorarCache = false): Observable<ConfiguracoesDto> {
    if (Boolean(this.configuracaoCacheada) && !ignorarCache) {
      return of(this.configuracaoCacheada);
    }

    return this.httpClient.get<ConfiguracoesDto>(this.basePath(), {
      headers: this.sessionService.defaultHttpHeaders
    })
      .pipe(
        tap((configuracao: ConfiguracoesDto) => {
          this.configuracaoCacheada = configuracao;
        })
      );
  }

  public updateConfiguracao(configuracoes: ConfiguracoesDto): Observable<unknown> {
    return this.httpClient.put<unknown>(`${this.basePath()}/${configuracoes.id}`, configuracoes,{
      headers: this.sessionService.defaultHttpHeaders
    })
      .pipe(tap(() => {
        this.configuracaoCacheada = null;
      }));
  }

  private basePath(): string {
    return `${ensureTrailingSlash(this.apiBaseUrl)}${this.apiPrefix}/configuracoes`;
  }
}
