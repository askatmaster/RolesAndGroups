import {ErrorHandler, Inject, NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {AppComponent} from './app.component';
import {NotFoundPageComponent} from "./common-components/not-found-page/not-found-page.component";
import {AppRoutingModule} from "./app-routing.module";
import {API_BASE_URL} from "./api/client-api";
import {environment} from "../environments/environment";
import {HttpClientModule} from "@angular/common/http";
import {FormsModule} from "@angular/forms";
import {SidebarComponent} from './common-components/sidebar/sidebar.component';
import {GlobalErrorHandlerService} from "./services/global-error-handler.service";

@NgModule({
  declarations: [
    AppComponent,
    NotFoundPageComponent,
    SidebarComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    BrowserModule,
    AppRoutingModule
  ],
  providers: [{provide: API_BASE_URL, useValue: environment.api.clientApi}, {
    provide: ErrorHandler,
    useClass: GlobalErrorHandlerService
  },],
  bootstrap: [AppComponent]
})
export class AppModule {
}
