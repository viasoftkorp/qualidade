import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MessageService } from '@viasoft/common';
// eslint-disable-next-line max-len
import { OdfRetrabalhoProxyService } from '@viasoft/rnc/api-clients/Nao-Conformidades/Ordem-Retrabalho/api/odf-retrabalho-proxy.service';
// eslint-disable-next-line max-len
import { OrdemRetrabalhoInput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Ordem-Retrabalho/model/ordem-retrabalho-input';
// eslint-disable-next-line max-len
import { OrdemRetrabalhoOutput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Ordem-Retrabalho/model/ordem-retrabalho-output';
import { Observable, forkJoin, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ConfiguracaoGeralOutput } from '@viasoft/rnc/api-clients/Configuracoes/Gerais/configuracao-geral-output';
import { ConfiguracaoGeralService } from '@viasoft/rnc/app/services/configuracoes/configuracao-geral.service';
import { OrdemProducaoOutput } from '@viasoft/rnc/api-clients/Ordem-Producao/model';
import { OrdemProducaoService } from '@viasoft/rnc/app/pages/shared/ordens-producao/ordem-producao.service';
import { NaoConformidadesEditorService } from '../../../nao-conformidades-editor.service';

@Injectable()
export class GerarOdfRetrabalhoEditorModalService {
  public isLoading = true;
  public configuracaoGeral: ConfiguracaoGeralOutput
  private ordemProducao: OrdemProducaoOutput;

  constructor(
    private odfRetrabalhoProxyService: OdfRetrabalhoProxyService,
    private naoConformidadesEditorService: NaoConformidadesEditorService,
    private messageService: MessageService,
    private configuracaoGeralService: ConfiguracaoGeralService,
    private ordemProducaoService: OrdemProducaoService
  ) {}

  public get numeroOdfDestino():number {
    if (this.configuracaoGeral.utilizarReservaDePedidoNaLocalizacaoDeEstoque) {
      return this.ordemProducao.numeroOdfDestino;
    }
    return null;
  }

  public onInit():void {
    forkJoin({
      configuracao: this.configuracaoGeralService.get(),
      ordemProducao: this.getOrdemProducao()
    })
      .subscribe(({ configuracao, ordemProducao }) => {
        this.configuracaoGeral = configuracao;
        this.ordemProducao = ordemProducao;
        this.isLoading = false;
      });
  }

  public gerarOdf(input: OrdemRetrabalhoInput):Observable<OrdemRetrabalhoOutput> {
    return this.odfRetrabalhoProxyService.gerarOdf(input, this.naoConformidadesEditorService.idNaoConformidade)
      .pipe(catchError((error:HttpErrorResponse) => {
        if (error.status === 422) {
          const result = error.error as OrdemRetrabalhoOutput;
          this.messageService.warn(result.message);
        }
        return of({} as OrdemRetrabalhoOutput);
      }));
  }

  private getOrdemProducao(): Observable<OrdemProducaoOutput | null> {
    const numeroOdf = Number(this.naoConformidadesEditorService.naoConformidadeAtual.numeroOdf);

    if (!numeroOdf) {
      return of(null) as Observable<null>;
    }

    return this.ordemProducaoService.getByNumeroOdf(numeroOdf);
  }
}
