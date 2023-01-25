import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ModalWindowComponent} from "./modal-window/modal-window.component";
import {LoadSpinnerComponent} from "./load-spinner/load-spinner.component";


@NgModule({
  declarations: [
    ModalWindowComponent,
    LoadSpinnerComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  exports: [ModalWindowComponent,
    LoadSpinnerComponent]
})
export class CommonComponentsModule {
}
