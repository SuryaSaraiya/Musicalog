import { Component, Input, OnInit, ElementRef, ChangeDetectorRef } from '@angular/core';
import { Location } from '@angular/common';
import { FormBuilder } from '@angular/forms';
import { Observable } from 'rxjs';
import { AlbumType } from '../../shared/enums/AlbumType';
import AlbumModel from '../../shared/models/AlbumModel';
import ArtistModel from '../../shared/models/ArtistModel';
import InventoryModel from '../../shared/models/InventoryModel';
import AlbumService from '../../shared/services/app.album.service';

enum Mode {
  View,
  Edit
}

@Component({
  selector: 'album-detail',
  templateUrl: './album-details.component.html',
  styleUrls: ['./album-details.component.scss']
})
export class AlbumDetail implements OnInit {

  @Input("mode") mode: Mode = Mode.View;
  @Input("albumId") albumId: number;

  title = 'Musicalog - Album Detail';
  album: AlbumModel;
  albumForm: any;
  albumTypes = AlbumType;
  albumTypeKeys: string[] = Object.keys(AlbumType).filter(k => !isNaN(Number(k)));

  constructor(private albumService: AlbumService,
    private elementRef: ElementRef,
    private formBuilder: FormBuilder,
    private location: Location) { }

  ngOnInit() {
    if (this.albumId > 0) {
      this.albumService.getById(this.albumId).subscribe(data => {
        this.album = data;
      });
    } else {
      this.album = {
        Id: 0,
        Name: 'New Album'
      } as AlbumModel;
      this.editAlbum();
    }
  }

  editing(): boolean {
    return this.mode == Mode.Edit ? true : false;
  }

  editAlbum() {
    this.mode = Mode.Edit;
    this.albumForm = this.formBuilder.group({
      Name: this.album.Name,
      Type: this.album.Type,
      Artist: this.album.Artists != undefined ? this.album.Artists[0].Name : '',
      Stock: this.album.Inventory != undefined ? this.album.Inventory.Stock : 0
    });
  }

  cancelEdit() {
    if (this.album.Id === 0) {
      window.location.href = '/Album/List';
    } else {
      this.mode = Mode.View;
    }
  }

  onAlbumFormSubmit(albumData: any) {
    if (this.album.Id == 0) {
      this.createAlbum(albumData);
    }
    else {
      this.saveAlbum(albumData);
    }
  }

  saveAlbum(albumData: any) {
    this.album.Name = albumData.Name;
    this.album.Type = albumData.Type;
    this.album.Artists[0].Name = albumData.Artist;
    this.album.Inventory.Stock = albumData.Stock;

    this.albumService.updateAlbum(this.album).subscribe(data => {
      console.log('Update Album Response');
      console.log(data);
      this.mode = Mode.View;
    });
  }

  createAlbum(albumData: any) {

    this.album.Name = albumData.Name;
    this.album.Type = albumData.Type;
    this.album.Artists = [{ Id: 0, Name: albumData.Artist }] as ArtistModel[];
    this.album.Inventory = { Stock: albumData.Stock } as InventoryModel;

    this.albumService.createAlbum(this.album).subscribe(data => {
      console.log('Create Album Response');
      console.log(data);
      this.location.replaceState(`/Album/Detail/${data.Id}`);
      this.mode = Mode.View;
    });
  }
}
