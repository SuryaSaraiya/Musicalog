import ArtistModel from './ArtistModel';
import InventoryModel from './InventoryModel';

export default interface AlbumModel {
  Id: number;
  Name: string;
  Artists: ArtistModel[];
  Type: string;
  Inventory: InventoryModel;
}


