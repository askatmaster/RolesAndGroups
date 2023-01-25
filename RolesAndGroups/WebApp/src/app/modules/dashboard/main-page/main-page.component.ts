import {Component, OnInit} from '@angular/core';
import {DestroyComponent} from "../../../common-components/destroy/destroy.component";

@Component({
  selector: 'app-main-page',
  template: require('./main-page.component.html').default,
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent extends DestroyComponent implements OnInit {

  constructor() {
    super();
  }

  ngOnInit(): void {
  }

}
