import { Injectable } from '@angular/core';
import { UUID } from 'angular2-uuid';

import { Observable, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';

import { IPagedResultOutputDto, MessageService } from '@viasoft/common';
import { VsDialog } from '@viasoft/components';

import {
  NaoConformidadeAgregacaoOutput,
  NaoConformidadeInput,
  ReclamacoesNaoConformidadesInput
} from '../../../api-clients/Nao-Conformidades';
import { NaturezaModel } from '../../../api-clients/Naturezas';
import { NaturezaService } from './natureza.service';
import { RncEditorModalComponent } from './rnc-editor-modal.component';
import { NaoConformidadesService } from './nao-conformidades.service';
import { NaoConformidadesEditorService } from './nao-conformidades-editor.service';

@Injectable()
export class RncEditorModalService {

  constructor(private vsDialog: VsDialog, private naoConformidadeService: NaoConformidadesService,
              private naoConformidadesEditorService: NaoConformidadesEditorService,
              private naturezaService: NaturezaService, private messageService: MessageService) {
  }

  public openCreateRncModal(naoConformidadeInput: NaoConformidadeInput): Observable<NaoConformidadeAgregacaoOutput> {
    this.naoConformidadesEditorService.somenteLeitura = false;

    if (!naoConformidadeInput?.id) {
      naoConformidadeInput.id = UUID.UUID();
    }

    if (!naoConformidadeInput?.idNatureza) {
      return this.naturezaService.getList({
        sorting: 'codigo',
        maxResultCount: 1,
        skipCount: 0,
      } as any)
        .pipe(
          switchMap((pagedNatureza: IPagedResultOutputDto<NaturezaModel>) => {
            if (pagedNatureza.items.length === 0) {
              this.messageService.warn('Rnc.NaoPossuiNaturezaCadastrada', 'Rnc.Atencao');
              return of(null);
            } else {
              naoConformidadeInput.idNatureza = pagedNatureza.items[0].id;
              return this.openAndUpdateRncModalObservable(naoConformidadeInput);
            }
          })
        )
    }

    return this.openAndUpdateRncModalObservable(naoConformidadeInput);
  }

  public openReadRncModal(idNaoConformidade: string) {
    this.naoConformidadesEditorService.somenteLeitura = true;
    return this.vsDialog.open(RncEditorModalComponent, idNaoConformidade).afterClosed();
  }

  private openAndUpdateRncModalObservable(input: NaoConformidadeInput): Observable<NaoConformidadeAgregacaoOutput> {
    input.incompleta = true;
    return this.naoConformidadeService.create(input)
      .pipe(
        switchMap(() => {
          const reclamacao = {
            id: UUID.UUID(),
            idNaoConformidade: input.id,
            procedentes: 0,
            improcedentes: 0,
            quantidadeLote: 0,
            quantidadeNaoConformidade: 0,
            disposicaoProdutosAprovados: 0,
            disposicaoProdutosConcessao: 0,
            rejeitado: 0,
            retrabalho: 0
          } as ReclamacoesNaoConformidadesInput;

          this.naoConformidadesEditorService.createReclamacao(reclamacao.idNaoConformidade, reclamacao).subscribe();

          return this.vsDialog.open(RncEditorModalComponent, input.id).afterClosed()
        }),
        switchMap((naoConformidade: NaoConformidadeAgregacaoOutput) => {
          if (!naoConformidade) {
            return this.naoConformidadeService.delete(input.id)
          }

          return of(naoConformidade);
        }),
        map((naoConformidade: NaoConformidadeAgregacaoOutput) => {
          if (!naoConformidade) {
            return null;
          }

          return naoConformidade;
        })
      );
  }
}
