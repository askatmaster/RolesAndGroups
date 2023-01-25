import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-not-found-page',
  template: require('./not-found-page.component.html').default,
  styleUrls: ['./not-found-page.component.css']
})
export class NotFoundPageComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
}
