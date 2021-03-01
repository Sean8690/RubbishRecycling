import axios from 'axios';
import { makeAutoObservable } from 'mobx';
import { RootStore } from './RootStore';

class Category {
  id: number;
  name: string;
  isSelected: boolean = false;

  constructor(id: number, name: string) {
    this.id = id;
    this.name = name;
    makeAutoObservable(this);
  }

  onClick = () => {
    this.isSelected = !this.isSelected;
  };
}

const CateStrList: string[] = ['👕', '🪑', '📱', '📺', '🐾', '🏗️', '📚', '🏋️'];

export enum AppState {
  ProductList,
  ProductDetail,
  AddProduct,
  ViewMine,
}

export class Products {
  // categories: categories;
  productList: Product[] = [];
  myList: Product[] = [];
  rootStore: RootStore = null;
  isLoading: boolean = false;
  isAdding: boolean = false;
  isViewingMine: boolean = false;
  selectedProductId?: number = null;
  appState: AppState = AppState.ProductList;

  CateList: Category[] = CateStrList.map((i, c) => new Category(c, i));
  tempCate = [];

  constructor(rootStore: RootStore) {
    makeAutoObservable(this);
    this.rootStore = rootStore;
  }

  async get() {
    this.isLoading = true;
    await delay(2000);
    const response = await axios.get(
      'https://jsonplaceholder.typicode.com/users'
    );
    response.data.forEach((element) => {
      console.log(element);
      let product = new Product();
      product.name = element.name;
      // product.location = element.address.stringify();
      this.productList.push(product);
    });
    this.isLoading = false;
  }

  setAppState = (state: AppState) => {
    if (state === this.appState) {
      return;
    }

    if (this.appState === AppState.ProductList) {
      this.tempCate = this.CateList.filter((c) => c.isSelected).map(
        (c) => c.id
      );
      this.CateList.forEach((c) => (c.isSelected = false));
    }

    if (this.appState === AppState.ProductDetail) {
      this.selectedProductId = null;
    }

    this.appState = state;

    if (this.appState === AppState.ProductList) {
      this.CateList.forEach((c) =>
        this.tempCate.indexOf(c.id) !== -1
          ? (c.isSelected = true)
          : (c.isSelected = false)
      );
    }
  };

  // toggleAdding = () => {
  //   this.isAdding = !this.isAdding;
  //   if (this.isAdding) {
  //     this.isViewingMine = false;
  //     this.tempCate = this.CateList.filter((c) => c.isSelected).map(
  //       (c) => c.id
  //     );
  //     this.CateList.forEach((c) => (c.isSelected = false));
  //   }

  //   if (!this.isAdding) {
  //     this.CateList.forEach((c) =>
  //       this.tempCate.indexOf(c.id) !== -1
  //         ? (c.isSelected = true)
  //         : (c.isSelected = false)
  //     );
  //   }
  // };

  // toggleViewingMine = () => {
  //   this.isViewingMine = !this.isViewingMine;
  //   if (this.isViewingMine) {
  //     this.isAdding = false;
  //   }
  // };

  getMyList = async () => {
    this.isLoading = true;
    const response = await axios.get(
      'https://jsonplaceholder.typicode.com/comments?postId=1'
    );
    await delay(3000);

    response.data.forEach((element) => {
      console.log(element);
      let product = new Product();
      product.name = element.name;
      // product.location = element.address.stringify();
      this.myList.push(product);
    });
    this.isLoading = false;
  };

  selectProduct = (id: number) => {
    this.selectedProductId = id;
    this.setAppState(AppState.ProductDetail);
  };
}

const delay = (ms) => new Promise((res) => setTimeout(res, ms));

class Product {
  id: number;
  name: string = '';
  location: string = '';
  images: string[] = [];
  upVote: number = 0;
  downVote: number = 0;
  postTimeFromNow: string = '4h';
  distance: string = '5km';
  voteStatus: VoteStatus = VoteStatus.Default;

  constructor() {
    makeAutoObservable(this);
  }

  updateVote(statusClicked: VoteStatus) {
    if ((statusClicked = this.voteStatus)) {
      this.voteStatus = VoteStatus.Default;
    }else {
      this.voteStatus = statusClicked;
    }
    
  }
}

enum VoteStatus {
  UpVote,
  DownVote,
  Default,
}
