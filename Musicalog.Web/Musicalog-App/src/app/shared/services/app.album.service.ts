import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import AlbumModel from '../models/AlbumModel';
import { Observable } from 'rxjs';
import AlbumListResult from '../models/AlbumListResult';

@Injectable()
export default class AlbumService {
  public BASE = 'https://localhost:44374/api/album';
  public ALBUM_LIST_ENDPOINT = `${this.BASE}/list`;
  public ALBUM_BY_ID_ENDPOINT = `${this.BASE}`;

  constructor(private http: HttpClient) { }

  getAll(pageSize: number, page: number): Observable<AlbumListResult> {
    return this.http.get<AlbumListResult>(`${this.ALBUM_LIST_ENDPOINT}?pageSize=${pageSize}&page=${page}`);
  }

  getById(id: number): Observable<AlbumModel> {
    return this.http.get<AlbumModel>(`${this.ALBUM_BY_ID_ENDPOINT}/${id}`);
  }

  updateAlbum(album: AlbumModel): Observable<AlbumModel> {
    return this.http.post<AlbumModel>(`${this.ALBUM_BY_ID_ENDPOINT}/${album.Id}`, album);
  }

  createAlbum(album: AlbumModel): Observable<AlbumModel> {
    return this.http.put<AlbumModel>(`${this.ALBUM_BY_ID_ENDPOINT}`, album);
  }

  deleteAlbum(id: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.ALBUM_BY_ID_ENDPOINT}/${id}`);
  }
}
