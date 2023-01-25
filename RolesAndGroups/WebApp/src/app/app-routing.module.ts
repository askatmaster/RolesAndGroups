import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {NotFoundPageComponent} from "./common-components/not-found-page/not-found-page.component";

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./modules/dashboard/dashboard.module').then(m => m.DashboardModule)
  },
  {
    path: 'groups',
    loadChildren: () => import('./modules/group/group.module').then(m => m.GroupModule)
  },
  {
    path: 'roles',
    loadChildren: () => import('./modules/role/role.module').then(m => m.RoleModule)
  },
  {
    path: '**',
    component: NotFoundPageComponent,
    title: 'Not Found'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
