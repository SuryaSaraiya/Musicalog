import AlbumModel from './AlbumModel';

export default interface AlbumListResult {
    Albums: AlbumModel[];
    Total: number;
}
