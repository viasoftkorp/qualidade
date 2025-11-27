import { Component, Input, OnChanges, OnDestroy, SimpleChanges } from '@angular/core';
import { VsFileUploadService } from '@viasoft/file-provider';
import { VsSubscriptionManager } from '@viasoft/common';
import { Subject } from 'rxjs';

@Component({
  selector: 'inspecao-entrada-arquivos-inspecao-entrada',
  templateUrl: './arquivos-inspecao-entrada.component.html',
  styleUrls: ['./arquivos-inspecao-entrada.component.css']
})
export class ArquivosInspecaoEntradaComponent implements OnDestroy, OnChanges {
  @Input() public idNotaFiscal: string;
  private subscriptionManager = new VsSubscriptionManager();
  public domain = '978125C2-A927-4205-A89B-08E995906EBD';
  public refresh = new Subject<void>();
  public processando = false;
  public gridId = "01A07FEE-C6C9-432B-9891-759C433A4915"

  public get subdomain(): string {
    if (this.idNotaFiscal == null) {
      return 'D6089464-D722-4131-9A0B-6113FE4CB3E0'; //Id Aleatório pra trazer a lista vazia, não fazer upload com ele
    }
    return this.idNotaFiscal;
  }

  constructor(
    private fileUploadService: VsFileUploadService
  ) {
  }

  public ngOnChanges(changes: SimpleChanges): void {
    if(changes.idNotaFiscal) {
      this.refresh.next();
    }
  }
  public ngOnDestroy(): void {
    this.subscriptionManager.clear();
  }

  public fileChanged(files: File[]):void {
    this.processando = true;
    this.fileUploadService.upload(files, this.domain, this.idNotaFiscal).subscribe(() => {
      this.processando = false;
      this.refresh.next();
    });
  }
}
