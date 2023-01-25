import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {RolesListComponent} from "./roles-list/roles-list.component";

const routes: Routes = [
  {
    path: 'list',
    component: RolesListComponent,
    title: 'Roles'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RoleRoutingModule {
}
