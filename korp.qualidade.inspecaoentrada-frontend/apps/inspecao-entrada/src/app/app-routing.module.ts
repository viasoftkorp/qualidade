import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MustBeLoggedAuthGuard } from '@viasoft/common';
import { authorizationRoutes } from '@viasoft/authorization-management';
import { IS_ON_PREMISE } from '../environments/is-on-premise.const';

const ROUTES: Routes = [
  {
    path: '',
    loadChildren: () => import('./pages/pages.module').then((m) => m.PagesModule),
    canActivate: [MustBeLoggedAuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(IS_ON_PREMISE ? ROUTES : [...authorizationRoutes, ...ROUTES])],
  exports: [RouterModule]
})
export class AppRoutingModule {}
