import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ensureTrailingSlash } from '@viasoft/http';
import { HttpClient } from '@angular/common/http';
import { SessionService } from '../../../../../services/session.service';
import {
  GetPedidoVendaLoteDto,
  PedidoVendaLoteDto
} from '../../../../../tokens/interfaces/pedido-venda-lote-interface-dto';
import { tap } from 'rxjs/operators';
import { GerarNumeroLoteInput } from '../../../../../tokens';

@Injectable()
export class QuebraLoteService {
  public lotes:Array<PedidoVendaLoteDto>;
  public utilizaReserva: boolean;

  private get baseUrl(): string {
    return `${ensureTrailingSlash(this.sessionService.currentBaseUrl)}`;
  }

  constructor(private httpClient: HttpClient,
              private sessionService: SessionService) {
  }
  public onInit() {
    if(!this.utilizaReserva) {
      this.lotes = [];
    }
  }
  public onDestroy() {
    this.lotes = null;
  }

  public addLote(input: PedidoVendaLoteDto) {
    this.lotes.push(input);
  }
  public updateLote(input: PedidoVendaLoteDto) {
    const indexToUpdate = this.lotes.findIndex(e => e.id == input.id);
    if(indexToUpdate >= 0) {
      this.lotes[indexToUpdate] = input;
    }
  }

  public deleteLote(input: PedidoVendaLoteDto) {
    const indexToDelete = this.lotes.findIndex(e => e.id == input.id);
    if(indexToDelete >= 0) {
      this.lotes.splice(indexToDelete, 1);
    }
  }

  public getLotes(recnoInspecao: number | null, id: string | null ): Observable<GetPedidoVendaLoteDto> {
    if(this.lotes || !this.utilizaReserva) {
      const output = {
        items: this.lotes,
        totalCount: this.lotes.length,
      } as GetPedidoVendaLoteDto

      return of(output);
    }
    return this.getAll(recnoInspecao, id);
  }
  private getAll(recnoInspecao: number, id: string): Observable<GetPedidoVendaLoteDto> {
    const route = `${this.baseUrl}inspecoes/${recnoInspecao}/pedidos-venda/${id}`;

    return this.httpClient.get<GetPedidoVendaLoteDto>
    (route, { headers: this.sessionService.defaultHttpHeaders }).pipe(tap((lotes: GetPedidoVendaLoteDto) => {
      this.lotes = lotes.items;
    }))
  }

  public gerarNumeroLote(input: GerarNumeroLoteInput): Observable<string> {
    const route = `${this.baseUrl}lotes/gerar-numero`;

    return this.httpClient.post<string>(route, input, {
      headers: this.sessionService.defaultHttpHeaders
    });
  }
}
