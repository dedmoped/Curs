import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-image-loader',
  templateUrl: './image-loader.component.html',
  styleUrls: ['./image-loader.component.scss']
})
export class ImageLoaderComponent implements OnInit {

  constructor() { }
  loaded: boolean = false;
  @Input() image: boolean;
  ngOnInit(): void {
  }

  imgload() {
    this.loaded = true;
  }
}
