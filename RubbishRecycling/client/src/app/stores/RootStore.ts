import { Products } from './ProductListStore';
export class RootStore {
    products: Products = null;

    constructor() {
        this.products = new Products(this);
    }
}
