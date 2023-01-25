import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupsListComponent } from './groups-list/groups-list.component';
import {GroupRoutingModule} from "./group-routing.module";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {CommonComponentsModule} from "../../common-components/common-components.module";



@NgModule({
  declarations: [
    GroupsListComponent
  ],
  imports: [
    CommonModule,
    GroupRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    CommonComponentsModule
  ]
})
export class GroupModule { }
