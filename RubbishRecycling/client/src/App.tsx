/**@jsx jsx */
import { BrowserRouter, Switch } from 'react-router-dom';
import { jsx } from 'theme-ui';
import { GlobalStyles } from './components';
import { ROUTES } from './constants';
import { AppContextProvider } from './contexts';
import { useAppInitialization } from './hooks';
import {
  CddLookupResult,
  CddSearch,
  DowJonesSearch,
  DowJonesLookupResult
} from './pages';
import { InfotrackAnalytics } from './utils';
import { getRouterBaseName } from './utils/getRouterBaseName';

const Route = InfotrackAnalytics.getRoute(getRouterBaseName());

export const App = () => {
  useAppInitialization();
  return (
    <AppContextProvider>
      <GlobalStyles />
      <BrowserRouter basename={getRouterBaseName()}>
        <Switch>
          <Route
            exact
            path={[ROUTES.CDD_SEARCH, ROUTES.CDD_SEARCH_BACK]}
            component={CddSearch}
          />
          <Route exact path={ROUTES.CDD_LOOKUP_RESULT} component={CddLookupResult} />
          <Route exact path={ROUTES.AML_SEARCH} component={DowJonesSearch} />
          <Route exact path={ROUTES.AML_LOOKUP_RESULT} component={DowJonesLookupResult} />
        </Switch>
      </BrowserRouter>
    </AppContextProvider>
  );
};
