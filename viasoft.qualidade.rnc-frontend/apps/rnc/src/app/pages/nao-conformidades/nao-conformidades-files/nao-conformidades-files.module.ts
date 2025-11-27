import { Input, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VsFileProviderModule } from '@viasoft/file-provider';
import { NaoConformidadesFilesComponent } from './nao-conformidades-files.component';
import { VsButtonModule, VsLayoutModule } from '@viasoft/components';

@NgModule({
  declarations: [NaoConformidadesFilesComponent],
  imports: [
    CommonModule,
    VsFileProviderModule,
    VsLayoutModule,
    VsButtonModule
  ],
  exports: [NaoConformidadesFilesComponent]
})
export class NaoConformidadesFilesModule {
}
