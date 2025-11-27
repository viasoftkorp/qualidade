import { NgModule } from '@angular/core';
import { VsCommonModule } from '@viasoft/common';
import { PagesRoutingModule } from './pages-routing.module';

@NgModule({
  declarations: [],
  imports: [VsCommonModule.forChild(), PagesRoutingModule]
})
export class PagesModule {}
