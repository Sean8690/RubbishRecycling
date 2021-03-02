import { RubbishContext } from "app/index";
import * as React from 'react';
import { observer } from "mobx-react";
import useInfiniteScroll from "react-infinite-scroll-hook";
import { RootStore } from '../../stores/RootStore';
import styled from 'styled-components'
import { Avatar, Button, Card, Carousel, Input, Row, Slider, Spin } from "antd";
import Meta from "antd/lib/card/Meta";
import 'antd/dist/antd.css'; // or 'antd/dist/antd.less'
import { LoadingOutlined, EnvironmentOutlined, PhoneOutlined, UpOutlined, DownOutlined } from '@ant-design/icons';
import { SearchOutlined } from '@ant-design/icons';
import './ProductList.css'
import * as classNames from 'classnames';
import { AppState, Products, VoteStatus } from '../../stores/ProductListStore';
import {
    Form,
    Radio,
    Select,
    Cascader,
    DatePicker,
    InputNumber,
    TreeSelect,
    Switch,
} from 'antd';
import TextArea from "antd/lib/input/TextArea";
import { PicturesWall } from "./PicturesWall";

const { Search } = Input;

const List = styled.ul`
  list-style: none;
  font-size: 16px;

  padding: 0;
`;

const ListItem = styled.li`
  margin-left: 0rem;
`;


const antIcon = <LoadingOutlined style={{ fontSize: 24 }} spin />;

const SideNav = observer((props: { store: Products }) => {
    switch (props.store.appState) {
        case AppState.ViewMine:
        case AppState.AddProduct:
        case AppState.ProductList:
            return (
                <div style={{ position: "fixed", top: "0", zIndex: 99, width: "95%" }}>
                    <Search
                        style={{ marginTop: "0.5rem" }}
                        placeholder="input search text"
                        value={props.store.searchQuery}
                        allowClear
                        enterButton="Search"
                        size="large"
                        onChange={(e) => props.store.updateSearchQuery(e.target.value)}
                        onSearch={() => props.store.updateList()}
                    />
                    <div className="categoryList" style={{ position: "fixed", display: "flex", flexDirection: "column" }}>
                        {
                            props.store.CateList.map(c =>
                                <Button onClick={() => { c.onClick(); props.store.updateList() }} type={c.isSelected ? "primary" : "default"} size="large" shape="circle" > {c.name} </Button>
                            )
                        }
                        <Button onClick={() => props.store.setAppState(props.store.appState === AppState.AddProduct ? AppState.ProductList : AppState.AddProduct)} type={props.store.appState === AppState.AddProduct ? "primary" : "default"} size="large" shape="circle" > ➕ </Button>
                        <Button onClick={() => props.store.setAppState(props.store.appState === AppState.ViewMine ? AppState.ProductList : AppState.ViewMine)} type={props.store.appState === AppState.ViewMine ? "primary" : "default"} size="large" shape="circle" > 🧍 </Button>
                    </div >
                </div>
            );
        case AppState.ProductDetail:
            return (
                <div style={{ position: "fixed", top: "0", zIndex: 99, width: "95%" }}>
                    <Search
                        style={{ marginTop: "0.5rem" }}
                        placeholder="input search text"
                        allowClear
                        enterButton="Search"
                        size="large"
                        onSearch={() => { console.log("lol") }}
                    />
                    <div className="categoryList" style={{ position: "fixed", display: "flex", flexDirection: "column" }}>
                        <Button onClick={() => props.store.setAppState(AppState.ProductList)} size="large" shape="circle" > 🔙 </Button>
                        <Button onClick={() => { }} size="large" shape="circle" > ❤️ </Button>
                    </div >
                </div>
            )
    }
});

const contentStyle = {
    height: '160px',
    color: '#fff',
    lineHeight: '160px',
    background: '#364d79',
};

const ProductDetail = observer((props: { store: Products }) => {

    const product = props.store.productList.filter(p => p.id === props.store.selectedProductId)[0]
    return (
        <div>
            <Carousel>
                <div>
                    <img className="gm" src="https://loremflickr.com/320/240/product" />
                </div>
                <div>
                    <img className="gm" src="https://loremflickr.com/320/240/product?random=1" />
                </div>
                <div>
                    <img className="gm" src="https://loremflickr.com/320/240/product?random=2" />
                </div>
                <div>
                    <img className="gm" src="https://loremflickr.com/320/240/product?random=3" />
                </div>
            </Carousel>
            <h1>{product.name}</h1>
            <p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p>
            <img className="gm" src="https://i.ibb.co/X2qq8Ts/gmap.png" />
        </div>
    )
});

function NewItemForm() {
    return (<Form
        labelCol={{ xs: 10 }}
        wrapperCol={{ xs: 14 }}
        layout="horizontal"
        onValuesChange={() => { }}
    >
        <PicturesWall />
        <Form.Item >
            <Input placeholder="What do you list" />
        </Form.Item>
        <Form.Item label="Reusability:">
            <Slider min={1} max={5} defaultValue={3} tooltipVisible />
        </Form.Item>
        <Form.Item >
            <TextArea showCount maxLength={100} placeholder="Tell us about it" />
        </Form.Item>
        <Form.Item>
            <Input placeholder="Mobile or Email" />
        </Form.Item>
        <Form.Item>
            <Input placeholder="How do your friend call you" />
        </Form.Item>
        <Form.Item>
            <Input prefix={<EnvironmentOutlined />} placeholder="Address" />
        </Form.Item>
        {/* <Form.Item label="TreeSelect">
            <TreeSelect
                treeData={[
                    { title: 'Light', value: 'light', children: [{ title: 'Bamboo', value: 'bamboo' }] },
                ]}
            />
        </Form.Item> */}
    </Form>);
}

function ProductList() {
    const rootStore: RootStore = React.useContext(RubbishContext);
    let products = rootStore.products;

    if (products === null) {
        return <h1>initializing</h1>;
    }

    return (
        <div>
            <SideNav store={products} />
            <div className="mainContent">
                <Content store={products} />
            </div>
        </div>
    );
};

const Content = observer((props: { store: Products }) => {
    let products = props.store;

    function load() {
        if (products === null) {
            console.log('tryLoad');
        } else {
            products.appState === AppState.ViewMine ? products.getMyList() : products.get();
        }
    }

    const infiniteRef = useInfiniteScroll({
        loading: products.isLoading,
        hasNextPage: true,
        onLoadMore: load,
        scrollContainer: 'window',
    });

    const productList = products.appState === AppState.ViewMine ? products.myList : products.productList;

    switch (props.store.appState) {
        case AppState.ProductList:
        case AppState.ViewMine:
            return <List ref={infiniteRef}>
                {productList.map((i, item) => (
                    <ListItem key={item}>
                        <Card onClick={() => products.selectProduct(i.id)} style={{ "width": "100%", marginTop: 10 }}>
                            <div className="cardContent">
                                <div>
                                    <img src={`https://loremflickr.com/240/240/product?random=${item}`} />
                                </div>
                                <div className="cardRightSection">
                                    <h3>{i.name}</h3>
                                    <p>This is the description</p>
                                </div>
                                <div className="voteBtn">
                                    <span className="ww">{i.postTimeFromNow}/{i.distance}</span>

                                    <Button onClick={(e) => { e.stopPropagation(); i.updateVote(VoteStatus.UpVote) }} type={i.voteStatus === VoteStatus.UpVote ? "primary" : "default"} size="small" >+ {i.upVote}</Button>
                                    <Button onClick={(e) => { e.stopPropagation(); i.updateVote(VoteStatus.DownVote) }} type={i.voteStatus === VoteStatus.DownVote ? "primary" : "default"} size="small" >- {i.downVote}</Button>
                                </div>
                            </div>
                        </Card>
                    </ListItem>
                ))}
                {products.isLoading && <Spin indicator={antIcon} />}
            </List>;
        case AppState.AddProduct:
            return <NewItemForm />;
        case AppState.ProductDetail:
            return <ProductDetail store={props.store} />;
    }
})

export default observer(ProductList);