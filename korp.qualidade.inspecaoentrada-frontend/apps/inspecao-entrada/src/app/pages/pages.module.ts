import { NgModule } from '@angular/core';
import { VsCommonModule } from '@viasoft/common';
import { PagesRoutingModule } from '@viasoft/inspecao-entrada/app/pages/pages-routing.module';

@NgModule({
  declarations: [],
  imports: [VsCommonModule.forChild(), PagesRoutingModule]
})
export class PagesModule {}
