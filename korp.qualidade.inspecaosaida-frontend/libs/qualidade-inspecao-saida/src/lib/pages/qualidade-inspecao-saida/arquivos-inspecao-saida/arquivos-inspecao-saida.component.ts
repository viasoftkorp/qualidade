import { Component, Input, OnChanges, OnDestroy, SimpleChanges } from '@angular/core';
import { VsFileUploadService } from '@viasoft/file-provider';
import { VsSubscriptionManager } from '@viasoft/common';
import { Subject } from 'rxjs';

@Component({
  selector: 'inspecao-saida-arquivos-inspecao-saida',
  templateUrl: './arquivos-inspecao-saida.component.html',
  styleUrls: ['./arquivos-inspecao-saida.component.css']
})
export class ArquivosInspecaoSaidaComponent implements OnDestroy, OnChanges {
  @Input() public idInspecao: string;

  // Usado para dar tempo da grid calcular tamanho das colunas corretamente, não usado lazy load pois precisa ajustar eventos
  @Input() public tabIndex: number;
  @Input() public selectedTab: number;

  public domainId = 'C7E88D51-827F-4CFB-916B-7F27675E8FE3';
  public customSubdomainId = 'C1BF8795-874E-490B-8A43-7E8176AC93A7';
  public gridId = 'EB47AD6D-BEBF-4757-99F7-F846C0467D4E'

  public refresh = new Subject<void>();
  public loaded = false;
  public processando = false;
  private subscriptionManager = new VsSubscriptionManager();

  public get subdomainId(): string {
    return this.idInspecao ?? this.customSubdomainId;
  }

  constructor(
    private fileUploadService: VsFileUploadService
  ) { }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes.selectedTab) {
      if (this.selectedTab === this.tabIndex) {
        this.loaded = true;
      }
    }
  }

  public ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public fileChanged(files: File[]):void {
    this.processando = true;
    this.fileUploadService.upload(files, this.domainId, this.idInspecao).subscribe(() => {
      this.processando = false;
      this.refresh.next();
    });
  }
}
