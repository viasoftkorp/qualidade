import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { MessageService } from '@viasoft/common';
import { ensureTrailingSlash } from '@viasoft/http';
import { ExportarRelatorioNaoConformidade } from '@viasoft/rnc/api-clients/Nao-Conformidades/Relatorios-Nao-Conformidades';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RelatorioNaoConformidadesService {
   private readonly endpoint: string;

   constructor(@Inject(VS_BACKEND_URL) protected gateway: string, protected httpClient: HttpClient,
              private message: MessageService) {
     this.endpoint = `${ensureTrailingSlash(gateway)}qualidade/rnc/gateway/nao-conformidades`;
   }

   public imprimirRelatorio(id: string) {
     return this.httpClient.get(`${this.endpoint}/${id}/relatorio`)
       .pipe(
         tap((output: ExportarRelatorioNaoConformidade) => {
           if (!output.success) {
             this.message.error(output.message, 'NaoConformidades.NaoConformidadesEditor.Relatorio.WarnTitle');
             return;
           }

           const byteArray = new Uint8Array(atob(output.fileBytes).split('').map((char) => char.charCodeAt(0)));
           const blob = new Blob([byteArray], { type: 'application/pdf' });
           const url = URL.createObjectURL(blob);
           window.open(url, '_blank', 'noopener');
         })
       );
   }
}
