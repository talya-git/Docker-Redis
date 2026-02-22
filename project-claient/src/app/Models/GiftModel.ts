import { CustomerDetails } from "./CustomerDetails";

export class giftModel {
    id!: number;
    name!: string;
    donorId: number = 0; 
    donorName?: string;
    priceCard: number = 0;
    giftImage?: string;    
    category?: string;     
    customerDatails?: CustomerDetails[];
    customerId?: number;   
    customerName?: string; 
}