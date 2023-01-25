import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-load-spinner',
  template: require('./load-spinner.component.html').default,
  styleUrls: ['./load-spinner.component.css']
})
export class LoadSpinnerComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
