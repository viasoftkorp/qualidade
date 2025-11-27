import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArquivosInspecaoEntradaComponent } from './arquivos-inspecao-entrada.component';
import { VsFileProviderModule } from '@viasoft/file-provider';
import { VsLayoutModule } from '@viasoft/components';



@NgModule({
  declarations: [ArquivosInspecaoEntradaComponent],
  imports: [
    CommonModule,
    VsFileProviderModule,
    VsLayoutModule
  ],
  exports: [ArquivosInspecaoEntradaComponent]
})
export class ArquivosInspecaoEntradaModule { }
