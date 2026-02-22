import { giftModel } from "./GiftModel";

export class modorModel {
   id: number = 0;
   firstName!: string; 
   lastName!: string;  
   address!: string;   
   phone!: string;
   email!: string;
   gifts: giftModel[] = [];

}