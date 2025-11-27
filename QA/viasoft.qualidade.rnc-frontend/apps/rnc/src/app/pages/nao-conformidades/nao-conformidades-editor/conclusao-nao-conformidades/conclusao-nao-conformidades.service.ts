import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { ensureTrailingSlash } from '@viasoft/http';
import {
  ConclusoesNaoConformidadesInput,
  ConclusoesNaoConformidadesOutput,
} from '@viasoft/rnc/api-clients/Nao-Conformidades';
import { Observable } from 'rxjs';
import { NaoConformidadesEditorService } from '../nao-conformidades-editor.service';

@Injectable()
export class ConclusaoNaoConformidadesService {
  private get basePath():string {
    const idNaoConformidade = this.naoConformidadeEditorService.naoConformidadeAtual.id;
    return `${ensureTrailingSlash(this.gateway)}qualidade/rnc/gateway/nao-conformidades/${idNaoConformidade}/conclusao`;
  }

  constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient,
  private naoConformidadeEditorService: NaoConformidadesEditorService) {
  }

  public concluir(input: ConclusoesNaoConformidadesInput): Observable<never> {
    return this.httpClient.post<never>(this.basePath, input);
  }

  public estornarConclusao(): Observable<never> {
    return this.httpClient.delete<never>(`${this.basePath}`);
  }

  public getConclusao(): Observable<ConclusoesNaoConformidadesOutput> {
    return this.httpClient.get<ConclusoesNaoConformidadesOutput>(this.basePath);
  }

  public calcularCicloTempo(): Observable<number> {
    return this.httpClient.get<number>(`${this.basePath}/calcular-ciclo-tempo`);
  }
}
