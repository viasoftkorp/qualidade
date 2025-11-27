import { Injectable } from '@angular/core';
// eslint-disable-next-line max-len
import { GerarOrdemRetrabalhoValidationResult } from '@viasoft/rnc/api-clients/Nao-Conformidades/Ordem-Retrabalho/model/gerar-ordem-retrabalho-validation-result';
import { Observable, Subject } from 'rxjs';
import { tap } from 'rxjs/operators';
// eslint-disable-next-line max-len
import { OdfRetrabalhoProxyService } from '@viasoft/rnc/api-clients/Nao-Conformidades/Ordem-Retrabalho/api/odf-retrabalho-proxy.service';
import { OrdemProducaoOutput } from '@viasoft/rnc/api-clients/Ordem-Producao/model';
import { StatusNaoConformidade } from '@viasoft/rnc/api-clients/Nao-Conformidades/model/status-nao-conformidades';
import { NaoConformidadesEditorService } from '../nao-conformidades-editor.service';

@Injectable()
export class RetrabalhoNaoConformidadesService {
  public getCurrentButtonFromOdfCommand = new Subject<OrdemProducaoOutput>();
  public canGerarOdfRetrabalhoValidationResult:GerarOrdemRetrabalhoValidationResult;

  public get canGerarOdfRetrabalho():boolean {
    return this.canGerarOdfRetrabalhoValidationResult === GerarOrdemRetrabalhoValidationResult.Ok;
  }
  public get hasOdfRetrabalhoGerada():boolean {
    return this.canGerarOdfRetrabalhoValidationResult === GerarOrdemRetrabalhoValidationResult.OdfRetrabalhoJaGerada;
  }

  public get showGerarOdfRetrabalho():boolean {
    const validationResult = this.canGerarOdfRetrabalhoValidationResult;
    if (validationResult == null) {
      return false;
    }
    return validationResult !== GerarOrdemRetrabalhoValidationResult.OdfObrigatorio
    && validationResult !== GerarOrdemRetrabalhoValidationResult.OdfNaoFinalizada
    && validationResult !== GerarOrdemRetrabalhoValidationResult.OdfNaoApontada;
  }

  public get odfNaoFinalizada():boolean {
    const validationResult = this.canGerarOdfRetrabalhoValidationResult;
    return validationResult === GerarOrdemRetrabalhoValidationResult.OdfNaoFinalizada;
  }

  constructor(private naoConformidadesEditorService: NaoConformidadesEditorService,
    private odfRetrabalhoProxyService: OdfRetrabalhoProxyService) {}

  public onDestroy():void {
    this.canGerarOdfRetrabalhoValidationResult = null;
  }
  public getCanGerarOdfRetrabalho(isFullValidation: boolean): Observable<GerarOrdemRetrabalhoValidationResult> {
    return this.odfRetrabalhoProxyService
      .canGenerateOdf(this.naoConformidadesEditorService.idNaoConformidade, isFullValidation)
      .pipe(
        tap((result: GerarOrdemRetrabalhoValidationResult) => {
          this.canGerarOdfRetrabalhoValidationResult = result;
        })
      );
  }
}
