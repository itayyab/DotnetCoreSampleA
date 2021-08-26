import { Product } from "../products/Product";

export interface CartDetails {
  cD_id: number;
  cD_Pr_id: number;
  cD_Pr_Qty: number;
  cD_Pr_price: number;
  cD_Pr_Amnt: number;
  cartForeignKey: number;
  cart: string;
  productForeignKey: number;
  product: Product;
}
