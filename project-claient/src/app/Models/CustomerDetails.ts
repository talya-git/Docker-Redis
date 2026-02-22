import { customerModel } from "./customerModel";
import { giftModel } from "./GiftModel";

export class CustomerDetails{
    id!: number;
    customerId!: number;
    customer!: customerModel; 
    giftId!: number;
    gift!: giftModel; 
    quantity!: number; 
    status!: string;
    address!:string
}