import * as React from 'react';
import { hot } from 'react-hot-loader/root';
import { Router, Route, Switch } from 'react-router';
import { TodoContainer } from 'app/containers/TodoContainer';
import { RootStore } from './stores/RootStore';
import ProductList from './components/productList/ProductList';

export const RubbishContext = React.createContext(new RootStore());

// render react DOM
export const App = hot(({ history }) => {
  return (
    <RubbishContext.Provider value={new RootStore()}>
      <Router history={history}>
        <Switch>
          <Route path="/todo" component={TodoContainer} />
          <Route path="/productList" component={ProductList} />
        </Switch>
      </Router>
    </RubbishContext.Provider>
  );
});
