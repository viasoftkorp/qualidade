import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VsFileProviderModule } from '@viasoft/file-provider';
import { NaoConformidadesFilesComponent } from './nao-conformidades-files.component';
import { VsLayoutModule } from '@viasoft/components';

@NgModule({
  declarations: [NaoConformidadesFilesComponent],
  imports: [
    CommonModule,
    VsFileProviderModule,
    VsLayoutModule
  ],
  exports: [NaoConformidadesFilesComponent]
})
export class NaoConformidadesFilesModule { }
