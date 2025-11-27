import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArquivosInspecaoSaidaComponent } from './arquivos-inspecao-saida.component';
import { VsFileProviderModule } from '@viasoft/file-provider';
import { VsLayoutModule } from '@viasoft/components';

@NgModule({
  declarations: [ArquivosInspecaoSaidaComponent],
  imports: [
    CommonModule,
    VsFileProviderModule,
    VsLayoutModule
  ],
  exports: [ArquivosInspecaoSaidaComponent]
})
export class ArquivosInspecaoSaidaModule { }
