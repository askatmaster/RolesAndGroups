import {Component, ElementRef, Inject, OnInit, Renderer2, ViewChild} from '@angular/core';

@Component({
  selector: 'app-sidebar',
  template: require('./sidebar.component.html').default,
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  isOpen : boolean = false
  constructor() {
  }

  ngOnInit(): void {
  }

  toggleSideBar() {
    this.isOpen = !this.isOpen
  }
}
