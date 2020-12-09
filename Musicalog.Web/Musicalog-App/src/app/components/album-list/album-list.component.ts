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
  sortBy: string = 'name';
  sortDirection: string = 'asc';

  constructor(private albumService: AlbumService,
    public dialog: MatDialog) { }

  ngOnInit() {
    this.loadAlbums(this.pageSize, this.page, this.sortBy, this.sortDirection);
  }

  loadAlbums(pageSize: number, pageNumber: number, sortBy: string, sortDirection: string) {
    this.albumService.getAll(pageSize, pageNumber + 1, sortBy, sortDirection).subscribe(data => {
      console.log(data);
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
            this.loadAlbums(this.pageSize, this.page, this.sortBy, this.sortDirection);
          }
        });
      }
    });
  }

  getAlbumsPage(event: any) {    
    this.pageSize = event.pageSize;
    this.page = event.pageIndex;
    this.loadAlbums(event.pageSize, event.pageIndex, this.sortBy, this.sortDirection);    
  }

  sortAlbums(sort: Sort) {
    if (sort.direction === '') {
      this.sortedAlbums = this.albums.slice();
    } else {

      this.sortBy = sort.active;
      this.sortDirection = sort.direction;
      this.loadAlbums(this.pageSize, this.page, this.sortBy, this.sortDirection);
    }
  }
}
