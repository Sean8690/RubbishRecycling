import { ReactNode } from 'react';
import { DowJonesContextProvider } from './dowJonesContext';
import React from 'react';

interface AppContextProviderProps {
  children: ReactNode;
}
/** ### State Management
 *  As it is not possible to render other components than `<Route>` as children of `<Switch>`,
 *  we are not able to 'scope' the contexts only in the pages we need, instead we can use this
 *  application level context and add our providers here as we need them.
 *  This will not cause any performance issue.
 */
export function AppContextProvider(props: AppContextProviderProps) {
  const { children } = props;
  return <DowJonesContextProvider>{children}</DowJonesContextProvider>;
}
