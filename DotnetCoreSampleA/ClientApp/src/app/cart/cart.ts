import { CartDetails } from "./cart-details";
export interface Cart {
  cart_id: number;
  userID: string;
  totalQty: number;
  totalAmount: number;
  status: string;
  cartDetails: CartDetails[];
}
