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

  searchQuery: string = '';

  constructor(rootStore: RootStore) {
    makeAutoObservable(this);
    this.rootStore = rootStore;
  }

  updateSearchQuery = (v: string) => {
    this.searchQuery = v;
    console.log(v);
  };

  async get() {
    this.isLoading = true;
    await delay(2000);

    console.log({
      start: this.productList.length,
      length: 10,
      Category: this.CateList.map((c) => c.id),
      KeyWord: this.searchQuery,
    });

    const response = await axios({
      method: 'get',
      // /products
      // datat {}
      url: 'https://jsonplaceholder.typicode.com/users',
      data: {
        start: this.productList.length,
        length: 10,
        Category: this.CateList.map((c) => c.id),
        KeyWord: this.searchQuery,
      },
    });

    response.data.forEach((element) => {
      let product = new Product();
      product.name = element.name;
      // product.location = element.address.stringify();
      this.productList.push(product);
    });
    this.isLoading = false;
  }

  async updateList() {
    this.isLoading = true;
    await delay(2000);

    console.log({
      Category: this.CateList.filter((c) => c.isSelected).map((c) => c.id),
      KeyWord: this.searchQuery,
      start: 0,
      length: 10,
    });

    const response = await axios({
      method: 'get',
      // /products
      url: 'https://jsonplaceholder.typicode.com/users',
      data: {
        Category: this.CateList.filter((c) => c.isSelected).map((c) => c.id),
        KeyWord: this.searchQuery,
        start: 0,
        length: 10,
      },
    });

    this.productList.length = 0;

    response.data.forEach((element) => {
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

  getMyList = async () => {
    this.isLoading = true;
    const response = await axios.get(
      'https://jsonplaceholder.typicode.com/comments?postId=1'
    );
    await delay(3000);

    response.data.forEach((element) => {
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
  upVoteBtnActive: boolean = false;
  downVoteBtnActive: boolean = false;

  constructor() {
    makeAutoObservable(this);
  }

  updateVote(statusClicked: VoteStatus) {
    if (statusClicked === VoteStatus.UpVote) {
      switch (this.voteStatus) {
        case VoteStatus.Default:
          this.upVoteBtnActive = true;
          this.upVote = ++this.upVote;
          this.voteStatus = VoteStatus.UpVote;
          break;
        case VoteStatus.UpVote:
          this.upVoteBtnActive = false;
          this.upVote = --this.upVote;
          this.voteStatus = VoteStatus.Default;
          break;

        case VoteStatus.DownVote:
          this.upVoteBtnActive = true;
          this.downVoteBtnActive = false;
          this.upVote = ++this.upVote;
          this.downVote = --this.downVote;
          this.voteStatus = VoteStatus.UpVote;
          break;
      }
    }

    if (statusClicked === VoteStatus.DownVote) {
      switch (this.voteStatus) {
        case VoteStatus.Default:
          this.downVoteBtnActive = true;
          this.downVote = ++this.downVote;
          this.voteStatus = VoteStatus.DownVote;
          break;

        case VoteStatus.UpVote:
          this.downVoteBtnActive = true;
          this.upVoteBtnActive = false;
          this.downVote = ++this.downVote;
          this.upVote = --this.upVote;
          this.voteStatus = VoteStatus.DownVote;
          break;

        case VoteStatus.DownVote:
          this.downVoteBtnActive = false;
          this.downVote = --this.downVote;
          this.voteStatus = VoteStatus.Default;
          break;
      }
    }
    // post to server
    // upvote
    // /product/{id}/up
    // downvote
    // /product/{id}/down
  }
}

export enum VoteStatus {
  UpVote,
  DownVote,
  Default,
}
