import { Type } from '@angular/core';
import { AfterViewInit, Component, ViewChild, ComponentFactoryResolver, ViewContainerRef, ElementRef } from '@angular/core';
import { AlbumDetail } from './components/album-details/album-details.component';
import { AlbumList } from './components/album-list/album-list.component';

@Component({
  selector: 'app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class App implements AfterViewInit {

  view: string = '';
  @ViewChild('dynamicComponent', { read: ViewContainerRef }) dynamicComponent: ViewContainerRef;

  constructor(private componentFactoryResolver: ComponentFactoryResolver,
    private elementRef: ElementRef) {
    this.view = this.elementRef.nativeElement.getAttribute('view');
  }

  ngAfterViewInit() {
    switch (this.view) {
      case 'list':
        this.loadAlbumListComponent();
        break;
      case 'detail':
        this.loadAlbumDetailComponent();
        break;
    }
  }

  private loadAlbumListComponent() {
    const pagesize: number = this.elementRef.nativeElement.getAttribute('album-list-page-size');    
    const component = this.initialiseAndGetComponent(AlbumList);
    component.pageSize = pagesize;
  }

  private loadAlbumDetailComponent() {
    const albumId: number = this.elementRef.nativeElement.getAttribute('view-data-album-id');
    const component = this.initialiseAndGetComponent(AlbumDetail);
    component.albumId = albumId;
  }

  private initialiseAndGetComponent<T>(type: Type<T>): T {
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(type);
    this.dynamicComponent.clear();
    const component = <T>this.dynamicComponent.createComponent(componentFactory).instance;
    return component;
  }
}
