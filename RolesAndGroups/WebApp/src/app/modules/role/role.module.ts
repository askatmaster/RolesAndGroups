import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RolesListComponent} from './roles-list/roles-list.component';
import {RoleRoutingModule} from "./role-routing.module";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {CommonComponentsModule} from "../../common-components/common-components.module";


@NgModule({
  declarations: [
    RolesListComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    CommonComponentsModule,
    ReactiveFormsModule,
    RoleRoutingModule
  ]
})
export class RoleModule {
}
