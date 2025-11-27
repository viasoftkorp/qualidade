import { Injectable } from '@angular/core';
// eslint-disable-next-line max-len
import { OperacaoRetrabalhoProxyService } from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/api/operacao-retrabalho-proxy.service';
// eslint-disable-next-line max-len
import { OperacaoRetrabalhoInput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/operacao-retrabalho-input';
// eslint-disable-next-line max-len
import { OperacaoRetrabalhoOutput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/operacao-retrabalho-output';
import { Observable } from 'rxjs';
import { NaoConformidadesEditorService } from '../../../nao-conformidades-editor.service';

@Injectable({
  providedIn: 'root'
})
export class GerarOperacaoRetrabalhoEditorModalService {
  constructor(private operacaoRetrabalhoProxyService: OperacaoRetrabalhoProxyService,
    private naoConformidadesEditorService: NaoConformidadesEditorService) { }

  public gerarOperacao(operacaoRetrabalhoInput: OperacaoRetrabalhoInput):Observable<OperacaoRetrabalhoOutput> {
    return this.operacaoRetrabalhoProxyService
      .gerarOperacaoRetrabalho(operacaoRetrabalhoInput, this.naoConformidadesEditorService.idNaoConformidade);
  }
}
