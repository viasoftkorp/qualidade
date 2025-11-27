import {
  Component, Input, OnDestroy, OnInit
} from '@angular/core';
import { VsSubscriptionManager } from '@viasoft/common';
import { Subject } from 'rxjs';
import { NaoConformidadesEditorService } from '../rnc-editor-modal/nao-conformidades-editor.service';
import { VsFileUploadService } from '@viasoft/file-provider';
import { APP_ID } from '../../../tokens/consts/app-consts.const';

@Component({
  selector: 'rnc-nao-conformidades-files',
  templateUrl: './nao-conformidades-files.component.html',
  styleUrls: ['./nao-conformidades-files.component.scss']
})
export class NaoConformidadesFilesComponent implements OnInit, OnDestroy {
  @Input() public idNaoConformidade: string;
  private subscriptionManager = new VsSubscriptionManager();
  public appId = APP_ID;
  public loaded = true;
  public refresh = new Subject<void>();
  public canEditArquivosNaoConformidade = true;
  public processando = false;
  constructor(
    private naoConformidadesEditorService: NaoConformidadesEditorService,
    private fileUploadService: VsFileUploadService,
  ) {
  }

  ngOnInit(): void {
    this.subscriptionManager.add('arquivoInserido', this.naoConformidadesEditorService.arquivoInserido.subscribe(() => {
      this.refresh.next();
    }));
    this.subscribeBloquearAtualizacaoNaoConformidade();
  }

  ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public fileChanged(files: File[]) {
    this.processando = true;
    this.fileUploadService.upload(files, this.appId, this.idNaoConformidade).subscribe(() => {
      this.processando = false;
      this.naoConformidadesEditorService.arquivoInserido.next();
    });
  }

  private subscribeBloquearAtualizacaoNaoConformidade() {
    this.subscriptionManager.add(
      'bloquearAtualizacaoNaoConformidade',
      this.naoConformidadesEditorService.validarAtualizacaoNaoConformidade.subscribe(() => {
        if (this.naoConformidadesEditorService.somenteLeitura) {
          this.canEditArquivosNaoConformidade = false;
        } else {
          this.canEditArquivosNaoConformidade = this.naoConformidadesEditorService.naoConformidadeNaoConcluida;
        }

        this.refresh.next();
      })
    );
  }
}
