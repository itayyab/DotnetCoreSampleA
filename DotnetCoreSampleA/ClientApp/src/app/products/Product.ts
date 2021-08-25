import { Categories } from "../categories/Categories";
export interface Product {
  pr_id: number;
  pr_name: string;
  pr_desc: string;
  pr_Picture: string;
  pr_price: number;
  categoryForeignKey: number;
  category: Categories;
}

