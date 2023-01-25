import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {GroupsListComponent} from "./groups-list/groups-list.component";

const routes: Routes = [
  {
    path: 'list',
    component: GroupsListComponent,
    //canActivate: [AuthGuard],
    title: 'Groups',
    //data: { isAuth: true, roles: ['User'] },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GroupRoutingModule {
}
