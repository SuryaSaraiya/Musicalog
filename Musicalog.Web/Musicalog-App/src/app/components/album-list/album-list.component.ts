import { Component, Input, OnInit } from '@angular/core';
import AlbumModel from '../../shared/models/AlbumModel';
import AlbumService from '../../shared/services/app.album.service';
import { Sort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialog } from '../confirm-dialog/confirm-dialog.component';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'album-list',
  templateUrl: './album-list.component.html',
  styleUrls: ['./album-list.component.scss']
})
export class AlbumList implements OnInit {
  title = 'Musicalog - Album List';
  albums: Array<AlbumModel>;
  totalAlbums: number;
  sortedAlbums: AlbumModel[];
  pageEvent: PageEvent;

  @Input("pagesize") pageSize: number;
  page: number = 0;

  constructor(private albumService: AlbumService,
    public dialog: MatDialog) { }

  ngOnInit() {
    this.loadAlbums(this.pageSize, this.page);
  }

  loadAlbums(pageSize: number, pageNumber: number) {
    this.albumService.getAll(pageSize, pageNumber + 1).subscribe(data => {
      this.albums = data.Albums;
      this.totalAlbums = data.Total;
      this.sortedAlbums = this.albums.slice();
    });
  }

  deleteAlbum(album: AlbumModel) {
    console.log(album);
    let diagRef = this.dialog.open(ConfirmDialog);

    diagRef.afterClosed().subscribe(response => {
      if (response) {
        this.albumService.deleteAlbum(album.Id).subscribe(data => {
          if (data) {
            this.loadAlbums(this.pageSize, this.page);
          }
        });
      }
    });
  }

  getAlbumsPage(event: any) {
    console.log(event);
    this.pageSize = event.pageSize;
    this.page = event.pageIndex;
    this.loadAlbums(event.pageSize, event.pageIndex);    
  }

  sortAlbums(sort: Sort) {

    if (sort.direction === '') {
      this.sortedAlbums = this.albums.slice();
    } else {
      this.sortedAlbums = this.albums.slice().sort((item1, item2) => {
        switch (sort.active) {
          case 'name':
            return compareBy(item1.Name, item2.Name, sort.direction);
          case 'type':
            return compareBy(item1.Type, item2.Type, sort.direction);
          case 'artist':
            return compareBy(item1.Artists[0].Name, item2.Artists[0].Name, sort.direction);
          case 'stock':
            return compareBy(item1.Inventory.Stock, item2.Inventory.Stock, sort.direction);
          default:
            return 0;
        }
      });

      function compareBy(item1: any, item2: any, direction: string) {
        let asc = direction === 'asc' ? true : false;
        return (item2 >= item1 ? 1 : -1) * (asc ? 1 : -1);
      }
    }
  }
}
