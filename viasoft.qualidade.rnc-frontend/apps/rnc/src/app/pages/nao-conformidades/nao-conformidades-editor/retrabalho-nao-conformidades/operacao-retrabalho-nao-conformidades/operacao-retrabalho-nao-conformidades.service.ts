import { Injectable } from '@angular/core';
// eslint-disable-next-line max-len
import { OperacaoRetrabalhoProxyService } from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/api/operacao-retrabalho-proxy.service';
// eslint-disable-next-line max-len
import { Observable } from 'rxjs';
import { IPagedResultOutputDto } from '@viasoft/common';
// eslint-disable-next-line max-len
import { OperacaoViewOutput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/operacao-view-output';
import { VsGridGetInput } from '@viasoft/components';
import {
  OperacaoSaldoOutput
} from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/operacao-saldo-output';
import { NaoConformidadesEditorService } from '../../nao-conformidades-editor.service';

@Injectable({
  providedIn: 'root'
})
export class OperacaoRetrabalhoNaoConformidadesService {
  constructor(
    private operacaoRetrabalhoProxyService: OperacaoRetrabalhoProxyService,
    private naoConformidadesEditorService: NaoConformidadesEditorService
  ) { }

  public getOperacoesView(input: VsGridGetInput):Observable<IPagedResultOutputDto<OperacaoViewOutput>> {
    if (!this.naoConformidadesEditorService.operacaoRetrabalho) {
      this.naoConformidadesEditorService.getOperacaoRetrabalho().subscribe();
    }

    return this.operacaoRetrabalhoProxyService
      .getOperacoesView(this.naoConformidadesEditorService.idNaoConformidade, this.naoConformidadesEditorService.operacaoRetrabalho.id, input);
  }

  public getOperacaoSaldo(legacyIdOperacao: number): Observable<OperacaoSaldoOutput> {
    return this.operacaoRetrabalhoProxyService.getOperacaoSaldo(legacyIdOperacao);
  }
}
