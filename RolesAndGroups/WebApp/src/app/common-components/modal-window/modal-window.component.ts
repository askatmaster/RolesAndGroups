import {Component, EventEmitter, inject, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup} from "@angular/forms";
import {Subject} from "rxjs";

@Component({
  selector: 'app-modal-window',
  template: require('./modal-window.component.html').default,
  styleUrls: ['./modal-window.component.css']
})
export class ModalWindowComponent implements OnInit {
  @Input() buttonText: string | undefined;
  @Input() modalText: string | undefined;
  @Input() modalFormGroup: any;
  @Output() submited = new EventEmitter<any>();
  errorSbj = new Subject<string>()
  formBuilder: FormBuilder
  dataForm: FormGroup | undefined;

  constructor() {
    this.formBuilder = inject(FormBuilder);
  }

  ngOnInit(): void {
    this.dataForm = this.formBuilder.group(this.modalFormGroup.formControls);
  }

  submit(): void {
    if(this.dataForm?.invalid){
      this.errorSbj.next("Введите корректные данные")
      return;
    }

    this.submited.emit(this.dataForm?.value);
  }
}
