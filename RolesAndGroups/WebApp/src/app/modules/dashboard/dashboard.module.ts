import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {MainPageComponent} from './main-page/main-page.component';
import {DashboardRoutingModule} from "./dashboard-routing.module";

@NgModule({
  declarations: [
    MainPageComponent
  ],
  imports: [
    DashboardRoutingModule,
    CommonModule
  ]
})
export class DashboardModule {
}
